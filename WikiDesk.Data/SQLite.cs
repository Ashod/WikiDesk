//
// Copyright (c) 2009-2010 Krueger Systems, Inc.
// Copyright (c) 2011 Ashod Nakashian.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
namespace SQLite
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;

    public class SQLiteException : Exception
    {
        public SQLite3.Result Result { get; private set; }

        protected SQLiteException(SQLite3.Result r,string message) : base(message)
        {
            Result = r;
        }

        public static SQLiteException New(SQLite3.Result r, string message)
        {
            return new SQLiteException(r, message);
        }
    }

    /// <summary>
    /// Represents an open connection to a SQLite database.
    /// </summary>
    public class SQLiteConnection : IDisposable
    {
        private bool _open;
        private TimeSpan _busyTimeout;
        private Dictionary<string, TableMapping> _mappings = null;
        private Dictionary<string, TableMapping> _tables = null;
        private System.Diagnostics.Stopwatch _sw;
        private long _elapsedMilliseconds = 0;

        public IntPtr Handle { get; private set; }

        public string DatabasePath { get; private set; }

        public bool TimeExecution { get; set; }

        public bool Trace { get; set; }

        /// <summary>
        /// Constructs a new SQLiteConnection and opens a SQLite database specified by databasePath.
        /// </summary>
        /// <param name="databasePath">
        /// Specifies the path to the database file.
        /// </param>
        public SQLiteConnection (string databasePath)
        {
            //SQLite3.Compress(0, 9, 8 * 64 * 1024);

            DatabasePath = databasePath;
            IntPtr handle;
            var r = (SQLite3.Result)SQLite3.Open(DatabasePath, out handle);
            Handle = handle;
            if (r != SQLite3.Result.OK) {
                throw SQLiteException.New (r, "Could not open database file: " + DatabasePath);
            }
            _open = true;

            BusyTimeout = TimeSpan.FromSeconds (0.1);
        }

        /// <summary>
        /// Sets a busy handler to sleep the specified amount of time when a table is locked.
        /// The handler will sleep multiple times until a total time of <see cref="BusyTimeout"/> has accumulated.
        /// </summary>
        public TimeSpan BusyTimeout {
            get { return _busyTimeout; }
            set {
                _busyTimeout = value;
                if (Handle != IntPtr.Zero) {
                    SQLite3.BusyTimeout(Handle, (int)_busyTimeout.TotalMilliseconds);
                }
            }
        }

        /// <summary>
        /// Returns the mappings from types to tables that the connection
        /// currently understands.
        /// </summary>
        public IEnumerable<TableMapping> TableMappings {
            get {
                if (_tables == null) {
                    return Enumerable.Empty<TableMapping> ();
                } else {
                    return _tables.Values;
                }
            }
        }

        /// <summary>
        /// Retrieves the mapping that is automatically generated for the given type.
        /// </summary>
        /// <param name="type">
        /// The type whose mapping to the database is returned.
        /// </param>
        /// <returns>
        /// The mapping represents the schema of the columns of the database and contains
        /// methods to set and get properties of objects.
        /// </returns>
        public TableMapping GetMapping(Type type)
        {
            if (_mappings == null) {
                _mappings = new Dictionary<string, TableMapping> ();
            }
            TableMapping map;
            if (!_mappings.TryGetValue (type.FullName, out map)) {
                map = new TableMapping (type);
                _mappings [type.FullName] = map;
            }
            return map;
        }

        public int CreateTable<T>()
        {
            return CreateTable<T>(false);
        }

        /// <summary>
        /// Executes a "create table if not exists" on the database. It also
        /// creates any specified indexes on the columns of the table. It uses
        /// a schema automatically generated from the specified type. You can
        /// later access this schema by calling GetMapping.
        /// </summary>
        /// <returns>
        /// The number of entries added to the database schema.
        /// </returns>
        public int CreateTable<T>(bool fts)
        {
            var ty = typeof(T);

            if (_tables == null) {
                _tables = new Dictionary<string, TableMapping> ();
            }

            TableMapping map;
            if (!_tables.TryGetValue (ty.FullName, out map)) {
                map = GetMapping (ty);
                _tables.Add(ty.FullName, map);
            }

            string query;

            if (!fts)
            {
                query = "create table if not exists \"" + map.TableName + "\"(\n";
            }
            else
            {
                query = "create virtual table \"" + map.TableName + "\" using fts4(\n";
            }

            var decls = map.Columns.Select (p => Orm.SqlDecl(p));
            var decl = string.Join(",\n", decls.ToArray());
            query += decl;

            // Multi-column Unique Constraints.
            var uniqueAttributes = ty.GetCustomAttributes(typeof(UniqueAttribute), false);
            foreach (UniqueAttribute uniqueAttribute in uniqueAttributes)
            {
                string[] uniqColNames = uniqueAttribute.Fields;
                if (uniqColNames != null)
                {
                    query += string.Format(
                                ",\nCONSTRAINT uc{0}_{1} UNIQUE ({2})\n",
                                map.TableName,
                                string.Join("_", uniqColNames),
                                string.Join(", ", uniqColNames));
                }
            }

            // Single-column Unique Constraints.
            foreach (var u in map.Columns.Where(x => x.IsUnique))
            {
                query += string.Format(
                            ",\nCONSTRAINT uc{0}_{1} UNIQUE ({1})\n",
                            map.TableName,
                            u.Name);
            }

            // Primary Key Constraint.
            var primarkeyAttributes = ty.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
            if (primarkeyAttributes.Length > 1)
            {
                throw new InvalidOperationException("Only one primary key may be added to a table.");
            }

            string[] pkColNames = (primarkeyAttributes.Length > 0) ? ((PrimaryKeyAttribute)primarkeyAttributes[0]).Fields : null;
            if (pkColNames != null)
            {
                query += string.Format(
                            ",\nCONSTRAINT pk{0}_{1} PRIMARY KEY({2})\n",
                            map.TableName,
                            string.Join("_", pkColNames),
                            string.Join(", ", pkColNames));
            }
            query += ")";

            var count = Execute(query);

            if (count == 0) { //Possible bug: This always seems to return 0?
                // Table already exists, migrate it
                MigrateTable(map);
            }

            foreach (var p in map.Columns.Where(x => x.IsIndexed))
            {
                var indexName = map.TableName + "_" + p.Name;
                var q = string.Format("create index if not exists \"{0}\" on \"{1}\"(\"{2}\")", indexName, map.TableName, p.Name);
                count += Execute(q);
            }

            return count;
        }

        class TableInfo
        {
            public int cid { get; set; }

            public string name { get; set; }

            public string type { get; set; }

            public int notnull { get; set; }

            public string dflt_value { get; set; }

            public int pk { get; set; }
        }

        void MigrateTable (TableMapping map)
        {
            var query = "pragma table_info(\"" + map.TableName + "\")";

            var existingCols = Query<TableInfo> (query);

            var toBeAdded = new List<TableMapping.Column> ();

            foreach (var p in map.Columns) {
                var found = false;
                foreach (var c in existingCols) {
                    found = p.Name == c.name;
                    if (found)
                        break;
                }
                if (!found) {
                    toBeAdded.Add (p);
                }
            }

            foreach (var p in toBeAdded) {
                var addCol = "alter table \"" + map.TableName + "\" add column " + Orm.SqlDecl(p);
                Execute (addCol);
            }
        }

        /// <summary>
        /// Creates a new SQLiteCommand given the command text with arguments. Place a '?'
        /// in the command text for each of the arguments.
        /// </summary>
        /// <param name="cmdText">
        /// The fully escaped SQL.
        /// </param>
        /// <param name="args">
        /// Arguments to substitute for the occurrences of '?' in the command text.
        /// </param>
        /// <returns>
        /// A <see cref="SQLiteCommand"/>
        /// </returns>
        public SQLiteCommand CreateCommand(string cmdText, params object[] ps)
        {
            if (!_open)
            {
                throw SQLiteException.New (SQLite3.Result.Error, "Cannot create commands from unopened database");
            }
            else
            {
                var cmd = new SQLiteCommand(this);
                cmd.CommandText = cmdText;
                foreach (var o in ps)
                {
                    cmd.Bind(o);
                }

                return cmd;
            }
        }

        /// <summary>
        /// Creates an SQLiteCommand given the command text (SQL) with arguments. Place a '?'
        /// in the command text for each of the arguments and then executes that command.
        /// Use this method instead of Query when you don't expect rows back. Such cases include
        /// INSERTs, UPDATEs, and DELETEs.
        /// You can set the Trace or TimeExecution properties of the connection
        /// to profile execution.
        /// </summary>
        /// <param name="query">
        /// The fully escaped SQL.
        /// </param>
        /// <param name="args">
        /// Arguments to substitute for the occurences of '?' in the query.
        /// </param>
        /// <returns>
        /// The number of rows modified in the database as a result of this execution.
        /// </returns>
        public int Execute(string query, params object[] args)
        {
            var cmd = CreateCommand(query, args);

            if (TimeExecution) {
                if (_sw == null) {
                    _sw = new System.Diagnostics.Stopwatch ();
                }
                _sw.Reset ();
                _sw.Start ();
            }

            int r = cmd.ExecuteNonQuery();

            if (TimeExecution) {
                _sw.Stop ();
                _elapsedMilliseconds += _sw.ElapsedMilliseconds;
                Console.WriteLine ("Finished in {0} ms ({1:0.0} s total)", _sw.ElapsedMilliseconds, _elapsedMilliseconds / 1000.0);
            }

            return r;
        }

        /// <summary>
        /// Creates an SQLiteCommand given the command text (SQL) with arguments. Place a '?'
        /// in the command text for each of the arguments and then executes that command
        /// for each argument.
        /// You can set the Trace or TimeExecution properties of the connection
        /// to profile execution.
        /// </summary>
        /// <param name="query">
        /// The fully escaped SQL.
        /// </param>
        /// <param name="argsCollection">
        /// Arguments to substitute for the occurences of '?' in the query.
        /// </param>
        /// <returns>
        /// The number of rows modified in the database as a result of this execution.
        /// </returns>
        public int ExecuteNonQuery(string query, IEnumerable<object[]> argsCollection)
        {
            if (!_open)
            {
                throw SQLiteException.New(SQLite3.Result.Error, "Cannot create commands from unopened database");
            }

            if (TimeExecution)
            {
                if (_sw == null)
                {
                    _sw = new System.Diagnostics.Stopwatch();
                }

                _sw.Reset();
                _sw.Start();
            }

            var cmd = new SQLiteCommand(this);
            cmd.CommandText = query;

            int rows = 0;
            foreach (object[] args in argsCollection)
            {
                foreach (var o in args)
                {
                    cmd.Bind(o);
                }

                rows += cmd.ExecuteNonQuery();
            }

            if (TimeExecution)
            {
                _sw.Stop();
                _elapsedMilliseconds += _sw.ElapsedMilliseconds;
                Console.WriteLine("Finished in {0} ms ({1:0.0} s total)", _sw.ElapsedMilliseconds, _elapsedMilliseconds / 1000.0);
            }

            return rows;
        }

        /// <summary>
        /// Creates a SQLiteCommand given the command text (SQL) with arguments. Place a '?'
        /// in the command text for each of the arguments and then executes that command.
        /// Use this method instead of Query when you don't expect rows back. Such cases include
        /// INSERTs, UPDATEs, and DELETEs.
        /// You can set the Trace or TimeExecution properties of the connection
        /// to profile execution.
        /// </summary>
        /// <param name="query">
        /// The fully escaped SQL.
        /// </param>
        /// <param name="args">
        /// Arguments to substitute for the occurences of '?' in the query.
        /// </param>
        /// <returns>
        /// The number of rows modified in the database as a result of this execution.
        /// </returns>
        public long? ExecuteScalar(string query, params object[] args)
        {
            var cmd = CreateCommand(query, args);

            if (TimeExecution)
            {
                if (_sw == null)
                {
                    _sw = new System.Diagnostics.Stopwatch();
                }
                _sw.Reset();
                _sw.Start();
            }

            long? r = cmd.ExecuteScalar();

            if (TimeExecution)
            {
                _sw.Stop();
                _elapsedMilliseconds += _sw.ElapsedMilliseconds;
                Console.WriteLine("Finished in {0} ms ({1:0.0} s total)", _sw.ElapsedMilliseconds, _elapsedMilliseconds / 1000.0);
            }

            return r;
        }

        /// <summary>
        /// Creates a SQLiteCommand given the command text (SQL) with arguments. Place a '?'
        /// in the command text for each of the arguments and then executes that command.
        /// It returns each row of the result using the mapping automatically generated for
        /// the given type.
        /// </summary>
        /// <param name="query">
        /// The fully escaped SQL.
        /// </param>
        /// <param name="args">
        /// Arguments to substitute for the occurences of '?' in the query.
        /// </param>
        /// <returns>
        /// An enumerable with one result for each row returned by the query.
        /// </returns>
        public List<T> Query<T> (string query, params object[] args) where T : new()
        {
            var cmd = CreateCommand (query, args);
            return cmd.ExecuteQuery<T> ();
        }

        /// <summary>
        /// Creates a SQLiteCommand given the command text (SQL) with arguments. Place a '?'
        /// in the command text for each of the arguments and then executes that command.
        /// It returns each row of the result using the specified mapping. This function is
        /// only used by libraries in order to query the database via introspection. It is
        /// normally not used.
        /// </summary>
        /// <param name="map">
        /// A <see cref="TableMapping"/> to use to convert the resulting rows
        /// into objects.
        /// </param>
        /// <param name="query">
        /// The fully escaped SQL.
        /// </param>
        /// <param name="args">
        /// Arguments to substitute for the occurences of '?' in the query.
        /// </param>
        /// <returns>
        /// An enumerable with one result for each row returned by the query.
        /// </returns>
        public List<object> Query (TableMapping map, string query, params object[] args)
        {
            var cmd = CreateCommand (query, args);
            return cmd.ExecuteQuery<object> (map);
        }

        /// <summary>
        /// Returns a queryable interface to the table represented by the given type.
        /// </summary>
        /// <returns>
        /// A queryable object that is able to translate Where, OrderBy, and Take
        /// queries into native SQL.
        /// </returns>
        public TableQuery<T> Table<T> () where T : new()
        {
            return new TableQuery<T> (this);
        }

        /// <summary>
        /// Attempts to retrieve an object with the given primary key from the table
        /// associated with the specified type. Use of this method requires that
        /// the given type have a designated PrimaryKey (using the PrimaryKeyAttribute).
        /// </summary>
        /// <param name="pk">
        /// The primary key.
        /// </param>
        /// <returns>
        /// The object with the given primary key. Throws a not found exception
        /// if the object is not found.
        /// </returns>
        public T Get<T> (object pk) where T : new()
        {
            var map = GetMapping (typeof(T));
            string query = string.Format("select * from \"{0}\" where \"{1}\" = ?", map.TableName, map.PK.Name);
            return Query<T> (query, pk).First ();
        }

        /// <summary>
        /// Whether <see cref="BeginTransaction"/> has been called and the database is waiting for a <see cref="Commit"/>.
        /// </summary>
        public bool IsInTransaction { get; private set; }

        /// <summary>
        /// Begins a new transaction. Call <see cref="Commit"/> to end the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (!IsInTransaction) {
                Execute ("begin transaction");
                IsInTransaction = true;
            }
        }

        /// <summary>
        /// Rolls back the transaction that was begun by <see cref="BeginTransaction"/>.
        /// </summary>
        public void Rollback()
        {
            if (IsInTransaction) {
                Execute ("rollback");
                IsInTransaction = false;
            }
        }

        /// <summary>
        /// Commits the transaction that was begun by <see cref="BeginTransaction"/>.
        /// </summary>
        public void Commit()
        {
            if (IsInTransaction) {
                Execute ("commit");
                IsInTransaction = false;
            }
        }

        /// <summary>
        /// Executes <param name="action"> within a transaction and automatically rollsback the transaction
        /// if an exception occurs. The exception is rethrown.
        /// </summary>
        /// <param name="action">
        /// The <see cref="Action"/> to perform within a transaction. <param name="action"> can contain any number
        /// of operations on the connection but should never call <see cref="BeginTransaction"/>,
        /// <see cref="Rollback"/>, or <see cref="Commit"/>.
        /// </param>
        public void RunInTransaction(Action action)
        {
            if (IsInTransaction) {
                throw new InvalidOperationException ("The connection must not already be in a transaction when RunInTransaction is called");
            }
            try {
                BeginTransaction();
                action();
                Commit();
            } catch (Exception) {
                Rollback();
                throw;
            }
        }

        /// <summary>
        /// Inserts all specified objects.
        /// </summary>
        /// <param name="objects">
        /// An <see cref="IEnumerable"/> of the objects to insert.
        /// </param>
        /// <returns>
        /// The number of rows added to the table.
        /// </returns>
        public int InsertAll(System.Collections.IEnumerable objects)
        {
            BeginTransaction();
            var c = 0;
            foreach(var r in objects) {
                c += Insert(r);
            }
            Commit();
            return c;
        }

        /// <summary>
        /// Inserts all specified objects of type T
        /// in a transaction, if one doesn't exists.
        /// Commits the transaction only if it creates it.
        /// </summary>
        /// <param name="items">
        /// An <see cref="IEnumerable"/> of the objects to insert.
        /// </param>
        /// <typeparam name="T">The type of the objects to insert.</typeparam>
        /// <returns>
        /// The number of rows added to the table.
        /// </returns>
        public int InsertAll<T>(IEnumerable<T> items) where T : class
        {
            Type objType = typeof(T);
            var map = GetMapping(objType);
            var cols = map.InsertColumns;
            var vals = new object[cols.Length];
            var insertCmd = map.GetInsertCommand(this, string.Empty);

            bool ownTransaction = false;
            if (!IsInTransaction)
            {
                BeginTransaction();
                ownTransaction = true;
            }

            try
            {
                int c = 0;
                foreach (T item in items)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    for (var i = 0; i < vals.Length; i++)
                    {
                        vals[i] = cols[i].GetValue(item);
                    }

                    c += insertCmd.ExecuteNonQuery(vals);

                    if (map.HasAutoIncPK)
                    {
                        var id = SQLite3.LastInsertRowid(Handle);
                        map.SetAutoIncPK(item, id);
                    }
                }

                if (ownTransaction)
                {
                    Commit();
                }

                return c;
            }
            catch (Exception)
            {
                if (ownTransaction)
                {
                    Rollback();
                }

                throw;
            }
        }

        /// <summary>
        /// Upserts (update or insert) all specified objects of type T
        /// in a transaction, if one doesn't exists.
        /// Commits the transaction only if it creates it.
        /// </summary>
        /// <param name="items">
        /// An <see cref="IEnumerable"/> of the objects to upsert.
        /// </param>
        /// <typeparam name="T">The type of the objects to upsert.</typeparam>
        /// <returns>
        /// The number of rows added to the table.
        /// </returns>
        public int UpsertAll<T>(IEnumerable<T> items) where T : class
        {
            Type objType = typeof(T);
            var map = GetMapping(objType);
            var cols = map.InsertColumns;
            var vals = new object[cols.Length];
            var upsertCmd = map.GetUpsertCommand(this, string.Empty);

            bool ownTransaction = false;
            if (!IsInTransaction)
            {
                BeginTransaction();
                ownTransaction = true;
            }

            try
            {
                int c = 0;
                foreach (T item in items)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    for (var i = 0; i < vals.Length; i++)
                    {
                        vals[i] = cols[i].GetValue(item);
                    }

                    c += upsertCmd.ExecuteNonQuery(vals);

                    if (map.HasAutoIncPK)
                    {
                        var id = SQLite3.LastInsertRowid(Handle);
                        map.SetAutoIncPK(item, id);
                    }
                }

                if (ownTransaction)
                {
                    Commit();
                }

                return c;
            }
            catch (Exception)
            {
                if (ownTransaction)
                {
                    Rollback();
                }

                throw;
            }
        }

        /// <summary>
        /// Inserts the given object and retrieves its
        /// auto incremented primary key if it has one.
        /// </summary>
        /// <param name="obj">
        /// The object to insert.
        /// </param>
        /// <returns>
        /// The number of rows added to the table.
        /// </returns>
        public int Insert(object obj)
        {
            if (obj == null) {
                return 0;
            }
            return Insert(obj, string.Empty, obj.GetType());
        }

        public int Insert(object obj, Type objType)
        {
            return Insert(obj, string.Empty, objType);
        }

        public int Insert(object obj, string extra)
        {
            if (obj == null) {
                return 0;
            }
            return Insert(obj, extra, obj.GetType());
        }

        /// <summary>
        /// Inserts the given object and retrieves its
        /// auto incremented primary key if it has one.
        /// </summary>
        /// <param name="obj">
        /// The object to insert.
        /// </param>
        /// <param name="extra">
        /// Literal SQL code that gets placed into the command. INSERT {extra} INTO ...
        /// </param>
        /// <returns>
        /// The number of rows added to the table.
        /// </returns>
        public int Insert(object obj, string extra, Type objType)
        {
            if (obj == null || objType == null) {
                return 0;
            }

            var map = GetMapping(objType);

            var cols = map.InsertColumns;
            var vals = new object[cols.Length];
            for (var i = 0; i < vals.Length; i++) {
                vals [i] = cols [i].GetValue (obj);
            }

            var insertCmd = map.GetInsertCommand(this, extra);
            var count = insertCmd.ExecuteNonQuery(vals);

            if (map.HasAutoIncPK) {
                var id = SQLite3.LastInsertRowid(Handle);
                map.SetAutoIncPK (obj, id);
            }

            return count;
        }

        /// <summary>
        /// Updates all of the columns of a table using the specified object
        /// except for its primary key.
        /// The object is required to have a primary key.
        /// </summary>
        /// <param name="obj">
        /// The object to update. It must have a primary key designated using the PrimaryKeyAttribute.
        /// </param>
        /// <returns>
        /// The number of rows updated.
        /// </returns>
        public int Update(object obj)
        {
            if (obj == null) {
                return 0;
            }
            return Update (obj, obj.GetType ());
        }

        public int Update(object obj, Type objType)
        {
            if (obj == null || objType == null) {
                return 0;
            }

            var map = GetMapping (objType);

            var pk = map.PK;

            if (pk == null) {
                throw new NotSupportedException ("Cannot update " + map.TableName + ": it has no PK");
            }

            var cols = from p in map.Columns
                where p != pk
                select p;
            var vals = from c in cols
                select c.GetValue (obj);
            var ps = new List<object> (vals);
            ps.Add (pk.GetValue (obj));
            var q = string.Format("update \"{0}\" set {1} where {2} = ? ", map.TableName, string.Join (",", (from c in cols
                select "\"" + c.Name + "\" = ? ").ToArray ()), pk.Name);
            return Execute(q, ps.ToArray());
        }

        /// <summary>
        /// Deletes the given object from the database using its primary key.
        /// </summary>
        /// <param name="obj">
        /// The object to delete. It must have a primary key designated using the PrimaryKeyAttribute.
        /// </param>
        /// <returns>
        /// The number of rows deleted.
        /// </returns>
        public int Delete<T>(T obj) where T : class
        {
            if (obj == null)
            {
                return 0;
            }

            var map = GetMapping(obj.GetType());
            var pk = map.PK;
            if (pk == null) {
                throw new NotSupportedException ("Cannot delete " + map.TableName + ": it has no PK");
            }
            var q = string.Format("delete from \"{0}\" where \"{1}\" = ?", map.TableName, pk.Name);
            return Execute(q, pk.GetValue(obj));
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            if (_open && Handle != IntPtr.Zero) {
                SQLite3.Close (Handle);
                Handle = IntPtr.Zero;
                _open = false;
            }
        }
    }

    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(params string[] fields)
        {
            Fields = fields;
        }

        public string[] Fields { get; private set; }
    }

    public class AutoIncrementAttribute : Attribute
    {
    }

    public class IndexedAttribute : Attribute
    {
    }

    public class UniqueAttribute : Attribute
    {
        public UniqueAttribute(params string[] fields)
        {
            Fields = fields;
        }

        public string[] Fields { get; private set; }
    }

    public class IgnoreAttribute : Attribute
    {
    }

    public class MaxLengthAttribute : Attribute
    {
        public int Value { get; private set; }

        public MaxLengthAttribute(int length)
        {
            Value = length;
        }
    }

    public class CollationAttribute: Attribute
    {
        public string Value { get; private set; }

        public CollationAttribute (string collation)
        {
            Value = collation;
        }
    }

    public class TableMapping
    {
        public Type MappedType { get; private set; }

        public string TableName { get; private set; }

        public Column[] Columns { get; private set; }

        public Column PK { get; private set; }

        Column _autoPk = null;
        Column[] _insertColumns = null;
        string _insertSql = null;
        string _upsertSql = null;

        public TableMapping(Type type)
        {
            MappedType = type;
            TableName = MappedType.Name;
            var props = MappedType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            var cols = new List<Column> ();
            foreach (var p in props) {
                var ignore = p.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0;
                if (p.CanWrite && !ignore) {
                    cols.Add (new PropColumn (p));
                }
            }
            Columns = cols.ToArray ();
            foreach (var c in Columns) {
                if (c.IsAutoInc && c.IsPK) {
                    _autoPk = c;
                }
                if (c.IsPK) {
                    PK = c;
                }
            }

            HasAutoIncPK = _autoPk != null;
        }

        public bool HasAutoIncPK { get; private set; }

        public void SetAutoIncPK(object obj, long id)
        {
            if (_autoPk != null) {
                _autoPk.SetValue(obj, Convert.ChangeType(id, _autoPk.ColumnType));
            }
        }

        public Column[] InsertColumns {
            get {
                if (_insertColumns == null) {
                    _insertColumns = Columns.Where (c => !c.IsAutoInc).ToArray ();
                }
                return _insertColumns;
            }
        }

        public Column FindColumn (string name)
        {
            var exact = Columns.Where (c => c.Name == name).FirstOrDefault ();
            return exact;
        }

        public string InsertSql(string extra)
        {
            if (_insertSql == null)
            {
                var cols = InsertColumns;
                _insertSql = string.Format(
                    "INSERT {0} into \"{1}\" ({2}) values ({3})",
                    extra,
                    TableName,
                    string.Join(",", (from c in cols select "\"" + c.Name + "\"").ToArray()),
                    string.Join(",", (from c in cols select "?").ToArray()));
            }

            return _insertSql;
        }

        public string UpsertSql(string extra)
        {
            if (_upsertSql == null)
            {
                var cols = InsertColumns;
                _upsertSql = string.Format(
                    "INSERT OR REPLACE {0} into \"{1}\" ({2}) values ({3})",
                    extra,
                    TableName,
                    string.Join(",", (from c in cols select "\"" + c.Name + "\"").ToArray()),
                    string.Join(",", (from c in cols select "?").ToArray()));
            }

            return _upsertSql;
        }

        PreparedSqlLiteInsertCommand _insertCommand;
        string _insertCommandExtra = null;
        PreparedSqlLiteInsertCommand _upsertCommand;
        string _upsertCommandExtra = null;

        public PreparedSqlLiteInsertCommand GetInsertCommand(SQLiteConnection conn, string extra)
        {
            if (_insertCommand == null || _insertCommandExtra != extra)
            {
                var insertSql = InsertSql(extra);
                _insertCommand = new PreparedSqlLiteInsertCommand(conn);
                _insertCommand.CommandText = insertSql;
                _insertCommandExtra = extra;
            }

            return _insertCommand;
        }

        public PreparedSqlLiteInsertCommand GetUpsertCommand(SQLiteConnection conn, string extra)
        {
            if (_upsertCommand == null || _upsertCommandExtra != extra)
            {
                var upsertSql = UpsertSql(extra);
                _upsertCommand = new PreparedSqlLiteInsertCommand(conn);
                _upsertCommand.CommandText = upsertSql;
                _upsertCommandExtra = extra;
            }

            return _upsertCommand;
        }

        public abstract class Column
        {
            public string Name { get; protected set; }

            public Type ColumnType { get; protected set; }

            public string Collation { get; protected set; }

            public bool IsAutoInc { get; protected set; }

            public bool IsPK { get; protected set; }

            public bool IsIndexed { get; protected set; }

            public bool IsUnique { get; protected set; }

            public bool IsNullable { get; protected set; }

            public int MaxStringLength { get; protected set; }

            public abstract void SetValue (object obj, object val);

            public abstract object GetValue (object obj);
        }

        public class PropColumn : Column
        {
            PropertyInfo _prop;

            public PropColumn (PropertyInfo prop)
            {
                _prop = prop;
                Name = prop.Name;
                //If this type is Nullable<T> then Nullable.GetUnderlyingType returns the T, otherwise it returns null, so get the the actual type instead
                ColumnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                Collation = Orm.Collation (prop);
                IsAutoInc = Orm.IsAutoInc (prop);
                IsPK = Orm.IsPK (prop);
                IsIndexed = Orm.IsIndexed (prop);
                IsUnique = Orm.IsUnique(prop);
                IsNullable = !IsPK;
                MaxStringLength = Orm.MaxStringLength (prop);
            }

            public override void SetValue (object obj, object val)
            {
                _prop.SetValue (obj, val, null);
            }

            public override object GetValue (object obj)
            {
                return _prop.GetValue (obj, null);
            }
        }
    }

    public static class Orm
    {
        public const int DefaultMaxStringLength = 140;

        public static string SqlDecl(TableMapping.Column p)
        {
            string decl = "\"" + p.Name + "\" " + SqlType(p) + " ";

            if (p.IsPK) {
                decl += "primary key ";
            }
            if (p.IsAutoInc) {
                decl += "autoincrement ";
            }
            if (!p.IsNullable) {
                decl += "not null ";
            }
            if (!string.IsNullOrEmpty (p.Collation)) {
                decl += "collate " + p.Collation + " ";
            }

            return decl;
        }

        public static string SqlType(TableMapping.Column p)
        {
            var clrType = p.ColumnType;
            if (clrType == typeof(Boolean) || clrType == typeof(Byte) || clrType == typeof(UInt16) || clrType == typeof(SByte) || clrType == typeof(Int16) || clrType == typeof(Int32)) {
                return "integer";
            } else if (clrType == typeof(UInt32) || clrType == typeof(Int64)) {
                return "bigint";
            } else if (clrType == typeof(Single) || clrType == typeof(Double) || clrType == typeof(Decimal)) {
                return "float";
            } else if (clrType == typeof(String)) {
                 int len = p.MaxStringLength;
                if (len > 0)
                {
                    return "nvarchar(" + len + ")";
                }

                return "text";
            }
            else if (clrType == typeof(DateTime))
            {
                return "datetime";
            } else if (clrType.IsEnum) {
                return "integer";
            } else if (clrType == typeof(byte[])) {
                return "blob";
            } else {
                throw new NotSupportedException ("Don't know about " + clrType);
            }
        }

        public static bool IsPK(MemberInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            return attrs.Length > 0;
        }

        public static string Collation(MemberInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof(CollationAttribute), true);
            return attrs.Length > 0 ? ((CollationAttribute)attrs [0]).Value : string.Empty;
        }

        public static bool IsAutoInc(MemberInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof(AutoIncrementAttribute), true);
            return attrs.Length > 0;
        }

        public static bool IsIndexed(MemberInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof(IndexedAttribute), true);
            return attrs.Length > 0;
        }

        public static bool IsUnique(MemberInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof(UniqueAttribute), true);
            return attrs.Length > 0;
        }

        public static int MaxStringLength(PropertyInfo p)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }

            var attrs = p.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            return attrs.Length > 0 ? ((MaxLengthAttribute)attrs[0]).Value : DefaultMaxStringLength;
        }
    }

    public class SQLiteCommand
    {
        SQLiteConnection _conn;
        private List<Binding> _bindings;

        public string CommandText { get; set; }

        internal SQLiteCommand (SQLiteConnection conn)
        {
            _conn = conn;
            _bindings = new List<Binding> ();
            CommandText = "";
        }

        public int ExecuteNonQuery()
        {
            if (_conn.Trace) {
                Console.WriteLine ("Executing: " + this);
            }

            var stmt = Prepare();
            SQLite3.Result r = SQLite3.Step(stmt);
            Finalize(stmt);

            if (r == SQLite3.Result.Done || r == SQLite3.Result.Row) {
                int rowsAffected = SQLite3.Changes(_conn.Handle);
                return rowsAffected;
            } else if (r == SQLite3.Result.Error) {
                string msg = SQLite3.GetErrmsg (_conn.Handle);
                throw SQLiteException.New (r, msg);
            } else {
                throw SQLiteException.New (r, r.ToString ());
            }
        }

        /// <summary>
        /// Executes a scalar query that returns a single integral value.
        /// If the query doesn't return any value, null is returned.
        /// </summary>
        /// <returns>The result of the query or null when none.</returns>
        public long? ExecuteScalar()
        {
            if (_conn.Trace)
            {
                Console.WriteLine("Executing Query: " + this);
            }

            var stmt = Prepare();
            SQLite3.Result result = SQLite3.Step(stmt);
            switch (result)
            {
                case SQLite3.Result.Done:
                    return null;

                case SQLite3.Result.Row:
                {
                    long val = SQLite3.ColumnInt64(stmt, 0);
                    Finalize(stmt);
                    return val;
                }

                case SQLite3.Result.Error:
                    throw SQLiteException.New(result, SQLite3.GetErrmsg(_conn.Handle));

                default:
                    throw SQLiteException.New(result, result.ToString());
            }
        }

        public IEnumerable<T> ExecuteQueryX<T>() where T : new()
        {
            if (_conn.Trace)
            {
                Console.WriteLine("Executing Query: " + this);
            }

            TableMapping map = _conn.GetMapping(typeof(T));

            var stmt = Prepare();

            var cols = new TableMapping.Column[SQLite3.ColumnCount(stmt)];

            for (int i = 0; i < cols.Length; i++)
            {
                var name = Marshal.PtrToStringUni(SQLite3.ColumnName16(stmt, i));
                cols[i] = map.FindColumn(name);
            }

            while (SQLite3.Step(stmt) == SQLite3.Result.Row)
            {
                var obj = Activator.CreateInstance(map.MappedType);
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i] == null)
                        continue;
                    var colType = SQLite3.ColumnType(stmt, i);
                    var val = ReadCol(stmt, i, colType, cols[i].ColumnType);
                    cols[i].SetValue(obj, val);
                }

                yield return (T)obj;
            }

            Finalize(stmt);
        }

        public List<T> ExecuteQuery<T>() where T : new()
        {
            return ExecuteQuery<T>(_conn.GetMapping(typeof(T)));
        }

        public List<T> ExecuteQuery<T>(TableMapping map)
        {
            if (_conn.Trace)
            {
                Console.WriteLine ("Executing Query: " + this);
            }

            var r = new List<T>();

            var stmt = Prepare();

            var cols = new TableMapping.Column[SQLite3.ColumnCount (stmt)];

            for (int i = 0; i < cols.Length; i++) {
                var name = Marshal.PtrToStringUni (SQLite3.ColumnName16 (stmt, i));
                cols [i] = map.FindColumn (name);
            }

            while (SQLite3.Step (stmt) == SQLite3.Result.Row) {
                var obj = Activator.CreateInstance (map.MappedType);
                for (int i = 0; i < cols.Length; i++) {
                    if (cols [i] == null)
                        continue;
                    var colType = SQLite3.ColumnType (stmt, i);
                    var val = ReadCol (stmt, i, colType, cols [i].ColumnType);
                    cols [i].SetValue (obj, val);
                }
                r.Add ((T)obj);
            }

            Finalize (stmt);
            return r;
        }

        public T ExecuteScalar<T>()
        {
            if (_conn.Trace) {
                Console.WriteLine ("Executing Query: " + this);
            }

            T val = default(T);

            var stmt = Prepare ();
            if (SQLite3.Step (stmt) == SQLite3.Result.Row) {
                var colType = SQLite3.ColumnType (stmt, 0);
                val = (T)ReadCol (stmt, 0, colType, typeof(T));
            }
            Finalize (stmt);

            return val;
        }

        public void Bind(string name, object val)
        {
            _bindings.Add(new Binding {
                Name = name,
                Value = val
            });
        }

        public void Bind(object val)
        {
            Bind(null, val);
        }

        public override string ToString()
        {
            var parts = new string[1 + _bindings.Count];
            parts [0] = CommandText;
            var i = 1;
            foreach (var b in _bindings) {
                parts [i] = string.Format ("  {0}: {1}", i - 1, b.Value);
                i++;
            }
            return string.Join(Environment.NewLine, parts);
        }

        IntPtr Prepare()
        {
            var stmt = SQLite3.Prepare2(_conn.Handle, CommandText);
            BindAll (stmt);
            return stmt;
        }

        void Finalize(IntPtr stmt)
        {
            SQLite3.Finalize(stmt);
        }

        void BindAll(IntPtr stmt)
        {
            int nextIdx = 1;
            foreach (var b in _bindings) {
                if (b.Name != null) {
                    b.Index = SQLite3.BindParameterIndex (stmt, b.Name);
                } else {
                    b.Index = nextIdx++;
                }
            }
            foreach (var b in _bindings) {
                BindParameter (stmt, b.Index, b.Value);
            }
        }

        internal static IntPtr NegativePointer = new IntPtr (-1);

        internal static void BindParameter(IntPtr stmt, int index, object value)
        {
            if (value == null) {
                SQLite3.BindNull (stmt, index);
            } else {
                if (value is Int32) {
                    SQLite3.BindInt (stmt, index, (int)value);
                } else if (value is String) {
                    SQLite3.BindText16 (stmt, index, (string)value, -1, NegativePointer);
                } else if (value is Byte || value is UInt16 || value is SByte || value is Int16) {
                    SQLite3.BindInt (stmt, index, Convert.ToInt32 (value));
                } else if (value is Boolean) {
                    SQLite3.BindInt (stmt, index, (bool)value ? 1 : 0);
                } else if (value is UInt32 || value is Int64) {
                    SQLite3.BindInt64 (stmt, index, Convert.ToInt64 (value));
                } else if (value is Single || value is Double || value is Decimal) {
                    SQLite3.BindDouble (stmt, index, Convert.ToDouble (value));
                } else if (value is DateTime) {
                    SQLite3.BindText (stmt, index, ((DateTime)value).ToString ("yyyy-MM-dd HH:mm:ss"), -1, NegativePointer);
                } else if (value.GetType ().IsEnum) {
                    SQLite3.BindInt (stmt, index, Convert.ToInt32 (value));
                } else if (value is byte[]) {
                    SQLite3.BindBlob (stmt, index, (byte[])value, ((byte[])value).Length, NegativePointer);
                } else {
                    throw new NotSupportedException ("Cannot store type: " + value.GetType ());
                }
            }
        }

        private class Binding
        {
            public string Name { get; set; }

            public object Value { get; set; }

            public int Index { get; set; }
        }

        private object ReadCol(IntPtr stmt, int index, SQLite3.ColType type, Type clrType)
        {
            if (type == SQLite3.ColType.Null) {
                return null;
            } else {
                if (clrType == typeof(String)) {
                    return SQLite3.ColumnString (stmt, index);
                } else if (clrType == typeof(Int32)) {
                    return (int)SQLite3.ColumnInt (stmt, index);
                } else if (clrType == typeof(Boolean)) {
                    return SQLite3.ColumnInt (stmt, index) == 1;
                } else if (clrType == typeof(double)) {
                    return SQLite3.ColumnDouble (stmt, index);
                } else if (clrType == typeof(float)) {
                    return (float)SQLite3.ColumnDouble (stmt, index);
                } else if (clrType == typeof(DateTime)) {
                    var text = SQLite3.ColumnString (stmt, index);
                    return DateTime.Parse (text);
                } else if (clrType.IsEnum) {
                    return SQLite3.ColumnInt (stmt, index);
                } else if (clrType == typeof(Int64)) {
                    return SQLite3.ColumnInt64 (stmt, index);
                } else if (clrType == typeof(UInt32)) {
                    return (uint)SQLite3.ColumnInt64 (stmt, index);
                } else if (clrType == typeof(decimal)) {
                    return (decimal)SQLite3.ColumnDouble (stmt, index);
                } else if (clrType == typeof(Byte)) {
                    return (byte)SQLite3.ColumnInt (stmt, index);
                } else if (clrType == typeof(UInt16)) {
                    return (ushort)SQLite3.ColumnInt (stmt, index);
                } else if (clrType == typeof(Int16)) {
                    return (short)SQLite3.ColumnInt (stmt, index);
                } else if (clrType == typeof(sbyte)) {
                    return (sbyte)SQLite3.ColumnInt (stmt, index);
                } else if (clrType == typeof(byte[])) {
                    return SQLite3.ColumnByteArray (stmt, index);
                } else {
                    throw new NotSupportedException ("Don't know how to read " + clrType);
                }
            }
        }
    }

    /// <summary>
    /// Since the insert never changed, we only need to prepare once.
    /// </summary>
    public class PreparedSqlLiteInsertCommand : IDisposable
    {
        public bool Initialized { get; set; }

        protected SQLiteConnection Connection { get; set; }

        public string CommandText { get; set; }

        protected IntPtr Statement { get; set; }

        internal PreparedSqlLiteInsertCommand (SQLiteConnection conn)
        {
            Connection = conn;
        }

        public int ExecuteNonQuery(object[] source)
        {
            if (Connection.Trace) {
                Console.WriteLine ("Executing: " + CommandText);
            }

            var r = SQLite3.Result.OK;

            if (!Initialized) {
                Statement = Prepare ();
                Initialized = true;
            }

            //bind the values.
            if (source != null) {
                for (int i = 0; i < source.Length; i++) {
                    SQLiteCommand.BindParameter (Statement, i + 1, source [i]);
                }
            }
            r = SQLite3.Step (Statement);

            if (r == SQLite3.Result.Done) {
                int rowsAffected = SQLite3.Changes (Connection.Handle);
                SQLite3.Reset (Statement);
                return rowsAffected;
            } else if (r == SQLite3.Result.Error) {
                string msg = SQLite3.GetErrmsg (Connection.Handle);
                SQLite3.Reset (Statement);
                throw SQLiteException.New (r, msg);
            } else {
                SQLite3.Reset (Statement);
                throw SQLiteException.New (r, r.ToString ());
            }
        }

        protected virtual IntPtr Prepare()
        {
            var stmt = SQLite3.Prepare2(Connection.Handle, CommandText);
            return stmt;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Statement != IntPtr.Zero) {
                try {
                    SQLite3.Finalize (Statement);
                } finally {
                    Statement = IntPtr.Zero;
                    Connection = null;
                }
            }
        }

        ~PreparedSqlLiteInsertCommand ()
        {
            Dispose (false);
        }
    }

    public class TableQuery<T> : IEnumerable<T> where T : new()
    {
        public SQLiteConnection Connection { get; private set; }

        public TableMapping Table { get; private set; }

        Expression _where;
        List<Ordering> _orderBys;
        int? _limit;
        int? _offset;

        class Ordering
        {
            public string ColumnName { get; set; }

            public bool Ascending { get; set; }
        }

        TableQuery (SQLiteConnection conn, TableMapping table)
        {
            Connection = conn;
            Table = table;
        }

        public TableQuery (SQLiteConnection conn)
        {
            Connection = conn;
            Table = Connection.GetMapping (typeof(T));
        }

        public TableQuery<T> Clone ()
        {
            var q = new TableQuery<T> (Connection, Table);
            q._where = _where;
            if (_orderBys != null) {
                q._orderBys = new List<Ordering> (_orderBys);
            }
            q._limit = _limit;
            q._offset = _offset;
            return q;
        }

        public TableQuery<T> Where (Expression<Func<T, bool>> predExpr)
        {
            if (predExpr.NodeType == ExpressionType.Lambda) {
                var lambda = (LambdaExpression)predExpr;
                var pred = lambda.Body;
                var q = Clone ();
                q.AddWhere (pred);
                return q;
            } else {
                throw new NotSupportedException ("Must be a predicate");
            }
        }

        public TableQuery<T> Take (int n)
        {
            var q = Clone ();
            q._limit = n;
            return q;
        }

        public TableQuery<T> Skip (int n)
        {
            var q = Clone ();
            q._offset = n;
            return q;
        }

        public TableQuery<T> OrderBy<U> (Expression<Func<T, U>> orderExpr)
        {
            return AddOrderBy<U> (orderExpr, true);
        }

        public TableQuery<T> OrderByDescending<U> (Expression<Func<T, U>> orderExpr)
        {
            return AddOrderBy<U> (orderExpr, false);
        }

        private TableQuery<T> AddOrderBy<U> (Expression<Func<T, U>> orderExpr, bool asc)
        {
            if (orderExpr.NodeType == ExpressionType.Lambda) {
                var lambda = (LambdaExpression)orderExpr;
                var mem = lambda.Body as MemberExpression;
                if (mem != null && (mem.Expression.NodeType == ExpressionType.Parameter)) {
                    var q = Clone ();
                    if (q._orderBys == null) {
                        q._orderBys = new List<Ordering> ();
                    }
                    q._orderBys.Add (new Ordering {
                        ColumnName = mem.Member.Name,
                        Ascending = asc
                    });
                    return q;
                } else {
                    throw new NotSupportedException ("Order By does not support: " + orderExpr);
                }
            } else {
                throw new NotSupportedException ("Must be a predicate");
            }
        }

        private void AddWhere (Expression pred)
        {
            if (_where == null) {
                _where = pred;
            } else {
                _where = Expression.AndAlso (_where, pred);
            }
        }

        private SQLiteCommand GenerateCommand (string selectionList)
        {
            var cmdText = "select " + selectionList + " from \"" + Table.TableName + "\"";
            var args = new List<object> ();
            if (_where != null) {
                var w = CompileExpr (_where, args);
                cmdText += " where " + w.CommandText;
            }
            if ((_orderBys != null) && (_orderBys.Count > 0)) {
                var t = string.Join (", ", _orderBys.Select (o => "\"" + o.ColumnName + "\"" + (o.Ascending ? "" : " desc")).ToArray ());
                cmdText += " order by " + t;
            }
            if (_limit.HasValue) {
                cmdText += " limit " + _limit.Value;
            }
            if (_offset.HasValue) {
                if (!_limit.HasValue) {
                    cmdText += " limit -1 ";
                }
                cmdText += " offset " + _offset.Value;
            }
            return Connection.CreateCommand (cmdText, args.ToArray ());
        }

        class CompileResult
        {
            public string CommandText { get; set; }

            public object Value { get; set; }
        }

        private CompileResult CompileExpr (Expression expr, List<object> queryArgs)
        {
            if (expr == null) {
                throw new NotSupportedException ("Expression is NULL");
            } else if (expr is BinaryExpression) {
                var bin = (BinaryExpression)expr;

                var leftr = CompileExpr (bin.Left, queryArgs);
                var rightr = CompileExpr (bin.Right, queryArgs);

                //If either side is a parameter and is null, then handle the other side specially (for "is null"/"is not null")
                string text;
                if (leftr.CommandText == "?" && leftr.Value == null)
                    text = CompileNullBinaryExpression(bin, rightr);
                else if (rightr.CommandText == "?" && rightr.Value == null)
                    text = CompileNullBinaryExpression(bin, leftr);
                else
                    text = "(" + leftr.CommandText + " " + GetSqlName(bin) + " " + rightr.CommandText + ")";
                return new CompileResult { CommandText = text };
            } else if (expr.NodeType == ExpressionType.Call) {

                var call = (MethodCallExpression)expr;
                var args = new CompileResult[call.Arguments.Count];

                for (var i = 0; i < args.Length; i++) {
                    args [i] = CompileExpr (call.Arguments [i], queryArgs);
                }

                var sqlCall = "";

                if (call.Method.Name == "Like" && args.Length == 2) {
                    sqlCall = "(" + args [0].CommandText + " like " + args [1].CommandText + ")";
                } else if (call.Method.Name == "Contains" && args.Length == 2) {
                    sqlCall = "(" + args [1].CommandText + " in " + args [0].CommandText + ")";
                } else {
                    sqlCall = call.Method.Name.ToLower () + "(" + string.Join (",", args.Select (a => a.CommandText).ToArray ()) + ")";
                }
                return new CompileResult { CommandText = sqlCall };

            } else if (expr.NodeType == ExpressionType.Constant) {
                var c = (ConstantExpression)expr;
                queryArgs.Add (c.Value);
                return new CompileResult {
                    CommandText = "?",
                    Value = c.Value
                };
            } else if (expr.NodeType == ExpressionType.Convert) {
                var u = (UnaryExpression)expr;
                var ty = u.Type;
                var valr = CompileExpr (u.Operand, queryArgs);
                return new CompileResult {
                    CommandText = valr.CommandText,
                    Value = valr.Value != null ? Convert.ChangeType (valr.Value, ty) : null
                };
            } else if (expr.NodeType == ExpressionType.MemberAccess) {
                var mem = (MemberExpression)expr;

                if (mem.Expression.NodeType == ExpressionType.Parameter) {
                    //
                    // This is a column of our table, output just the column name
                    //
                    return new CompileResult { CommandText = "\"" + mem.Member.Name + "\"" };
                } else {
                    object obj = null;
                    if (mem.Expression != null) {
                        var r = CompileExpr (mem.Expression, queryArgs);
                        if (r.Value == null) {
                            throw new NotSupportedException ("Member access failed to compile expression");
                        }
                        if (r.CommandText == "?") {
                            queryArgs.RemoveAt (queryArgs.Count - 1);
                        }
                        obj = r.Value;
                    }

                    //
                    // Get the member value
                    //
                    object val = null;

                    if (mem.Member.MemberType == MemberTypes.Property) {
                        var m = (PropertyInfo)mem.Member;
                        val = m.GetValue (obj, null);
                    } else if (mem.Member.MemberType == MemberTypes.Field) {
                        var m = (FieldInfo)mem.Member;
                        val = m.GetValue (obj);
                    } else {
                        throw new NotSupportedException ("MemberExpr: " + mem.Member.MemberType.ToString ());
                    }

                    //
                    // Work special magic for enumerables
                    //
                    if (val != null && val is System.Collections.IEnumerable && !(val is string)) {
                        var sb = new System.Text.StringBuilder();
                        sb.Append("(");
                        var head = "";
                        foreach (var a in (System.Collections.IEnumerable)val) {
                            queryArgs.Add(a);
                            sb.Append(head);
                            sb.Append("?");
                            head = ",";
                        }
                        sb.Append(")");
                        return new CompileResult {
                            CommandText = sb.ToString(),
                            Value = val
                        };
                    }
                    else {
                        queryArgs.Add (val);
                        return new CompileResult {
                            CommandText = "?",
                            Value = val
                        };
                    }
                }
            }
            throw new NotSupportedException ("Cannot compile: " + expr.NodeType.ToString ());
        }

        /// <summary>
        /// Compiles a BinaryExpression where one of the parameters is null.
        /// </summary>
        /// <param name="parameter">The non-null parameter</param>
        private string CompileNullBinaryExpression(BinaryExpression expression, CompileResult parameter)
        {
            if (expression.NodeType == ExpressionType.Equal)
                return "(" + parameter.CommandText + " is ?)";
            else if (expression.NodeType == ExpressionType.NotEqual)
                return "(" + parameter.CommandText + " is not ?)";
            else
                throw new NotSupportedException("Cannot compile Null-BinaryExpression with type " + expression.NodeType.ToString());
        }

        string GetSqlName (Expression expr)
        {
            var n = expr.NodeType;
            if (n == ExpressionType.GreaterThan)
                return ">"; else if (n == ExpressionType.GreaterThanOrEqual) {
                return ">=";
            } else if (n == ExpressionType.LessThan) {
                return "<";
            } else if (n == ExpressionType.LessThanOrEqual) {
                return "<=";
            } else if (n == ExpressionType.And) {
                return "and";
            } else if (n == ExpressionType.AndAlso) {
                return "and";
            } else if (n == ExpressionType.Or) {
                return "or";
            } else if (n == ExpressionType.OrElse) {
                return "or";
            } else if (n == ExpressionType.Equal) {
                return "=";
            } else if (n == ExpressionType.NotEqual) {
                return "!=";
            } else {
                throw new System.NotSupportedException ("Cannot get SQL for: " + n.ToString ());
            }
        }

        public int Count ()
        {
            return GenerateCommand("count(*)").ExecuteScalar<int> ();
        }

        public IEnumerator<T> GetEnumerator ()
        {
            return GenerateCommand("*").ExecuteQueryX<T>().GetEnumerator();
            //ExecuteQuery<T> ().GetEnumerator ();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }
    }

    public static class SQLite3
    {
        public enum Result : int
        {
            OK = 0,             /* Successful result */
            Error = 1,          /* SQL error or missing database */
            Internal = 2,       /* Internal logic error in SQLite */
            Perm = 3,           /* Access permission denied */
            Abort = 4,          /* Callback routine requested an abort */
            Busy = 5,           /* The database file is locked */
            Locked = 6,         /* A table in the database is locked */
            NoMem = 7,          /* A malloc() failed */
            ReadOnly = 8,       /* Attempt to write a readonly database */
            Interrupt = 9,      /* Operation terminated by sqlite3_interrupt()*/
            IOError = 10,       /* Some kind of disk I/O error occurred */
            Corrupt = 11,       /* The database disk image is malformed */
            NotFound = 12,      /* Unknown opcode in sqlite3_file_control() */

            FULL = 13,          /* Insertion failed because database is full */
            CANTOPEN = 14,      /* Unable to open the database file */
            PROTOCOL = 15,      /* Database lock protocol error */
            EMPTY = 16,         /* Database is empty */
            SCHEMA = 17,        /* The database schema changed */
            TooBig = 18,        /* String or BLOB exceeds size limit */
            Constraint = 19,    /* Abort due to constraint violation */
            MISMATCH = 20,      /* Data type mismatch */
            MISUSE = 21,        /* Library used incorrectly */
            NOLFS = 22,         /* Uses OS features not supported on host */
            AUTH = 23,          /* Authorization denied */
            FORMAT = 24,        /* Auxiliary database format error */
            RANGE = 25,         /* 2nd parameter to sqlite3_bind out of range */
            NOTADB = 26,        /* File opened that is not a database file */
            Row = 100,          /* sqlite3_step() has another row ready */
            Done = 101          /* sqlite3_step() has finished executing */
        }

        public enum ConfigOption : int
        {
            SingleThread = 1,
            MultiThread = 2,
            Serialized = 3
        }

        [DllImport("sqlite3", EntryPoint = "sqlite3_open", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Open(string filename, out IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Close(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Config(ConfigOption option);

        [DllImport("sqlite3", EntryPoint = "sqlite3_compress", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Compress(Int32 trace, Int32 compressionLevel, Int32 chunkSizeBytes);

        [DllImport("sqlite3", EntryPoint = "sqlite3_busy_timeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result BusyTimeout(IntPtr db, Int32 milliseconds);

        [DllImport("sqlite3", EntryPoint = "sqlite3_changes", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Changes(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Prepare2(IntPtr db, string sql, Int32 numBytes, out IntPtr stmt, IntPtr pzTail);

        public static IntPtr Prepare2(IntPtr db, string query)
        {
            IntPtr stmt;
            var r = Prepare2(db, query, query.Length, out stmt, IntPtr.Zero);
            if (r != Result.OK) {
                throw SQLiteException.New(r, GetErrmsg (db));
            }
            return stmt;
        }

        [DllImport("sqlite3", EntryPoint = "sqlite3_step", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Step(IntPtr stmt);

        [DllImport("sqlite3", EntryPoint = "sqlite3_reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Reset(IntPtr stmt);

        [DllImport("sqlite3", EntryPoint = "sqlite3_finalize", CallingConvention = CallingConvention.Cdecl)]
        public static extern Result Finalize(IntPtr stmt);

        [DllImport("sqlite3", EntryPoint = "sqlite3_last_insert_rowid", CallingConvention = CallingConvention.Cdecl)]
        public static extern long LastInsertRowid(IntPtr db);

        [DllImport("sqlite3", EntryPoint = "sqlite3_errmsg16", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Errmsg(IntPtr db);

        public static string GetErrmsg(IntPtr db)
        {
            return Marshal.PtrToStringUni (Errmsg (db));
        }

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_parameter_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindParameterIndex(IntPtr stmt, string name);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_null", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindNull(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindInt(IntPtr stmt, int index, int val);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindInt64(IntPtr stmt, int index, long val);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindDouble(IntPtr stmt, int index, double val);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_text", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindText(IntPtr stmt, int index, string val, int n, IntPtr free);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_text16", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindText16(IntPtr stmt, int index, string val, int n, IntPtr free);

        [DllImport("sqlite3", EntryPoint = "sqlite3_bind_blob", CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindBlob(IntPtr stmt, int index, byte[] val, int n, IntPtr free);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_count", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnCount(IntPtr stmt);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_name", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnName(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_name16", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnName16(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_type", CallingConvention = CallingConvention.Cdecl)]
        public static extern ColType ColumnType(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnInt(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern long ColumnInt64(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ColumnDouble(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_text", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnText(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_text16", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnText16(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_blob", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ColumnBlob(IntPtr stmt, int index);

        [DllImport("sqlite3", EntryPoint = "sqlite3_column_bytes", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ColumnBytes(IntPtr stmt, int index);


        public static string ColumnString(IntPtr stmt, int index)
        {
            return Marshal.PtrToStringUni(SQLite3.ColumnText16 (stmt, index));
        }

        public static byte[] ColumnByteArray(IntPtr stmt, int index)
        {
            int length = ColumnBytes (stmt, index);
            byte[] result = new byte[length];
            if (length > 0)
                Marshal.Copy (ColumnBlob (stmt, index), result, 0, length);
            return result;
        }

        public enum ColType : int
        {
            Integer = 1,
            Float = 2,
            Text = 3,
            Blob = 4,
            Null = 5
        }
    }

}

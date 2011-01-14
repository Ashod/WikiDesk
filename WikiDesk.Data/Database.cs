namespace WikiDesk.Data
{
    using System;

    using SQLite;

    /// <summary>
    /// The interface of all our DB record.
    /// Contains the primary key Id, necessary for quick updating.
    /// </summary>
    public interface IRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        int Id { get; set; }
    }

    public partial class Database : SQLiteConnection
    {
        public Database(string path)
            : base(path)
        {
            CreateTable<Page>();
            CreateTable<Language>();
            CreateTable<Domain>();
        }

        /// <summary>
        /// Updates an existing record or inserts a new one if missing.
        /// </summary>
        /// <param name="newRecord">A new record to add update or insert.</param>
        /// <param name="oldRecord">An old record, if any, otherwise null.</param>
        /// <returns>True if a new record was created, otherwise false.</returns>
        public bool UpdateInsert<T>(T newRecord, T oldRecord) where T : class, IRecord, IEquatable<T>
        {
            if (oldRecord != null)
            {
                // Make sure the primary key is set before updating.
                newRecord.Id = oldRecord.Id;
                if (!oldRecord.Equals(newRecord))
                {
                    Update(newRecord);
                }

                return false;
            }

            Insert(newRecord);
            return true;
        }
    }
}

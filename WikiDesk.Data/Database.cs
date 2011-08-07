// -----------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="ashodnakashian.com">
//
// This file is part of WikiDesk.
// Copyright (C) 2010, 2011 Ashod Nakashian
// https://github.com/Ashod/WikiDesk
//
//  WikiDesk is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  WikiDesk is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with WikiDesk. If not, see http://www.gnu.org/licenses/.
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The interface of all our DB record.
//   Contains the primary key Id, necessary for quick updating.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Data
{
    using System;

    using SQLite;

    public partial class Database : SQLiteConnection
    {
        public Database(string path)
            : this(path, new DatabaseConfig())
        {
        }

        public Database(string path, DatabaseConfig config)
            : base(path)
        {
            config_ = config;

            Execute(string.Format("PRAGMA cache_size = {0};", config_.CacheSizePages));
            Execute(string.Format("PRAGMA case_sensitive_like = {0};", config_.CaseSensitiveLike));
            Execute(string.Format("PRAGMA locking_mode = {0};", config_.LockMode.ToString().ToUpperInvariant()));
            Execute(string.Format("PRAGMA case_sensitive_like = {0};", config_.SyncMode.ToString().ToUpperInvariant()));

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

        #region representation

        /// <summary>
        /// The database configuration.
        /// </summary>
        private readonly DatabaseConfig config_;

        #endregion // representation
    }
}

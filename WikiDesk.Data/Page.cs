﻿// -----------------------------------------------------------------------------------------
// <copyright file="Page.cs" company="ashodnakashian.com">
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
//   A page in the database.
//   Wikimedia doesn't mix domains/languages, but we do.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq.SqlClient;
    using System.Linq;

    using SQLite;

    /// <summary>
    /// A page in the database.
    /// Wikimedia doesn't mix domains/languages, but we do.
    /// </summary>
    [Unique("Domain", "Language", "Title")]
    public class Page : IRecord, IComparer<Page>, IComparable<Page>, IEquatable<Page>
    {
        /// <summary>
        /// Gets or sets the page's unique ID.
        /// Used for updates as the SQLite.net library doesn't support
        /// multi-column PK for updates.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Domain ID. Reference into Domain table.
        /// </summary>
        public int Domain { get; set; }

        /// <summary>
        /// Gets or sets the Lanuage ID. Reference into Language table.
        /// </summary>
        public int Language { get; set; }

        /// <summary>
        /// Gets or sets the Page title.
        /// </summary>
        [MaxLength(256)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the date/time when the text was last updated from the web.
        /// If imported from a dump, this is the dump date/time.
        /// </summary>
        public DateTime LastUpdateDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the Page content.
        /// </summary>
        [MaxLength(0)]
        public string Text { get; set; }

        #region Implementation of IComparer<Page>

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Less than zero if <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero if <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public int Compare(Page x, Page y)
        {
            return x.CompareTo(y);
        }

        #endregion // Implementation of IComparer<Page>

        #region Implementation of IComparable<Page>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Less than zero if this object is less than the <paramref name="other"/> parameter.
        /// Zero if this object is equal to <paramref name="other"/>.
        /// Greater than zero if this object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(Page other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            int val = Domain.CompareTo(other.Domain);
            if (val != 0)
            {
                return val;
            }

            val = Language.CompareTo(other.Language);
            if (val != 0)
            {
                return val;
            }

            val = string.Compare(Title, other.Title);
            if (val != 0)
            {
                return val;
            }

            return string.Compare(Text, other.Text);
        }

        #endregion // Implementation of IComparable<Page>

        #region Equality

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(Page) && Equals((Page)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(Page other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Domain == Domain &&
                   other.Language == Language &&
                   other.LastUpdateDateUtc == LastUpdateDateUtc &&
                   Equals(other.Title, Title) &&
                   Equals(other.Text, Text);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = Domain;
                result = (result * 397) ^ Language;
                result = (result * 397) ^ LastUpdateDateUtc.GetHashCode();
                result = (result * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                result = (result * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(Page left, Page right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Page left, Page right)
        {
            return !Equals(left, right);
        }

        #endregion // Equality
    }

    public partial class Database
    {
        public IEnumerable<string> SelectPageTitles(long domainId, long languageId)
        {
            return (from s in Table<Page>()
                    where s.Domain == domainId &&
                          s.Language == languageId
                    select s.Title);
        }

        public Page SelectPage(long domainId, long languageId, string title)
        {
            return (from s in Table<Page>()
                    where s.Domain == domainId &&
                          s.Language == languageId &&
                          s.Title == title
                    select s).FirstOrDefault();
        }

        public int DeletePage(long domainId, long languageId, string title)
        {
            string query = string.Format(
                "DELETE FROM Page WHERE Domain={0} AND Language={1} AND Title='{2}'",
                domainId,
                languageId,
                title.Replace("'", "''"));
            return Execute(query);
        }

        /// <summary>
        /// Count the number of pages by domain and/or language, both optional.
        /// </summary>
        /// <param name="domainId">ID of a domain or 0 for all.</param>
        /// <param name="languageId">ID of a language or 0 for all.</param>
        /// <returns>The number of pages found.</returns>
        public long CountPages(long domainId, long languageId)
        {
            string query = "SELECT COUNT(*) FROM Page WHERE 1=1";
            if (domainId != 0)
            {
                query += " AND Domain=" + domainId;
            }

            if (languageId != 0)
            {
                query += " AND Language=" + languageId;
            }

            long? count = ExecuteScalar(query);
            if (count == null)
            {
                throw new ApplicationException("Unexpected result from SQLite. Query: " + query);
            }

            return (long)count;
        }

        public IList<string> SearchPages(long domainId, long languageId, string text)
        {
            return (from s in Table<Page>()
                    where s.Domain == domainId &&
                          s.Language == languageId &&
                          SqlMethods.Like(s.Text, text)
                    select s.Title).ToList();
        }
    }
}

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
    public class Page : IComparer<Page>, IComparable<Page>
    {
        /// <summary>
        /// Used for updates as the SQLite.net library doesn't support
        /// multi-column PK for updates.
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Reference into Domain table.
        /// </summary>
        public int Domain { get; set; }

        /// <summary>
        /// Reference into Language table.
        /// </summary>
        public int Language { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        /// <summary>
        /// The date/time when the text was last updated from the web.
        /// If imported from a dump, this is the dump date/time.
        /// </summary>
        public DateTime LastUpdateDateUtc { get; set; }

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
    }

    public partial class Database
    {
        /// <summary>
        /// Updates, replaces or inserts a new page record as necessary.
        /// </summary>
        /// <param name="page">The page record in question.</param>
        /// <returns>True if a new record was created.</returns>
        public bool UpdateReplacePage(Page page)
        {
            // Try to find this page.
            Page dbPage = SelectPage(page.Domain, page.Language, page.Title);
            if (dbPage == null)
            {
                // New page.
                Insert(page);
                return true;
            }

            page.Id = dbPage.Id;

            // Old page. See if there are any changes and avoid updating if none.
            if (dbPage.CompareTo(page) != 0)
            {
                Update(page);
            }

            return false;
        }

        public IList<string> SelectPageTitles(long domainId, long languageId)
        {
            return (from s in Table<Page>()
                    where s.Domain == domainId &&
                          s.Language == languageId
                    select s.Title).ToList();
        }

        public Page SelectPage(long domainId, long languageId, string title)
        {
            return (from s in Table<Page>()
                    where s.Domain == domainId &&
                          s.Language == languageId &&
                          s.Title == title
                    select s).FirstOrDefault();
        }

        public IList<string> SearchPages(string text)
        {
            return (from s in Table<Page>()
                    where SqlMethods.Like(s.Text, text)
                    select s.Title).ToList();
        }
    }
}

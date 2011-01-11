namespace WikiDesk.Data
{
    using System;
    using System.Collections.Generic;
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
        /// Reference into Domain table.
        /// </summary>
        [Indexed]
        public long Domain { get; set; }

        /// <summary>
        /// Reference into Language table.
        /// </summary>
        [Indexed]
        public long Language { get; set; }

        [Indexed]
        [MaxLength(256)]
        public string Title { get; set; }

        /// <summary>
        /// Reference into Revision table.
        /// </summary>
        public long LastRevisionId { get; set; }

        /// <summary>
        /// The date/time when the page was last updated from the web.
        /// If imported from a dump, this is the dump date/time.
        /// </summary>
        public DateTime LastUpdateDateUtc { get; set; }

        [Ignore]
        public Revision Revision { get; set; }

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

            val = Title.CompareTo(other.Title);
            if (val != 0)
            {
                return val;
            }

            val = LastRevisionId.CompareTo(other.LastRevisionId);
            if (val != 0)
            {
                return val;
            }

            return val;
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
            Page dbPage = QueryPage(page.Title, page.Domain, page.Language);
            if (dbPage == null)
            {
                // New page.
                Insert(page);
                return true;
            }

            // Old page. See if there are any changes and avoid updating if none.
            if (dbPage != page)
            {
                Update(page);
            }

            return false;
        }

        public IList<Page> GetPages()
        {
            return (from s in Table<Page>() select s).ToList();
        }

        public IList<Page> GetPages(long domainId)
        {
            return (from s in Table<Page>()
                    where s.Domain == domainId
                    select s).ToList();
        }

        public IList<Page> GetPages(long domainId, long languageId)
        {
            return (from s in Table<Page>()
                    where s.Domain == domainId &&
                          s.Language == languageId
                    select s).ToList();
        }

        public Page QueryPage(string title, long domainId, long languageId)
        {
            return (from s in Table<Page>()
                    where s.Title == title &&
                          s.Domain == domainId &&
                          s.Language == languageId
                    select s).FirstOrDefault();
        }

        public Page QueryPage(string title, long domainId, string languageCode)
        {
            Language language = GetLanguageByCode(languageCode);
            if (language == null)
            {
                return null;
            }

            return QueryPage(title, domainId, language.Id);
        }
    }
}

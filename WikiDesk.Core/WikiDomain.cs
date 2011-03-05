
namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WikiDomain : IComparer<WikiDomain>, IComparable<WikiDomain>
    {
        public string Name;

        /// <summary>
        /// The base URL for the wiki.
        /// A language code is prepended to this base-url.
        /// </summary>
        public string BaseUrl;

        /// <summary>
        /// The export-page URL for the wiki.
        /// A language code is prepended to this url.
        /// </summary>
        public string ExportUrl;

        public string EditUrl;

        #region Implementation of IComparer<WikiDomain>

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
        public int Compare(WikiDomain x, WikiDomain y)
        {
            return x.CompareTo(y);
        }

        #endregion

        #region Implementation of IComparable<WikiDomain>

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
        public int CompareTo(WikiDomain other)
        {
            int val = Name.CompareTo(other.Name);
            if (val != 0)
            {
                return val;
            }

            val = BaseUrl.CompareTo(other.BaseUrl);
            if (val != 0)
            {
                return val;
            }

            val = ExportUrl.CompareTo(other.ExportUrl);
            if (val != 0)
            {
                return val;
            }

            val = EditUrl.CompareTo(other.EditUrl);
            return val;
        }

        #endregion // Implementation of IComparable<WikiDomain>
    }
}


namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WikiLanguage : IComparer<WikiLanguage>, IComparable<WikiLanguage>
    {
        public string Code { get; set; }

        /// <summary>
        /// The language family ISO 639-1 code and sub-tag.
        /// Used when Code is not a language code (such as 'Simple).
        /// </summary>
        public string MimeCode { get; set; }

        public string Name { get; set; }

        public string LocalName { get; set; }

        public string Notes { get; set; }

        public bool RightToLeft { get; set; }

        public bool Disabled { get; set; }

        #region Implementation of IComparer<WikiLanguage>

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
        public int Compare(WikiLanguage x, WikiLanguage y)
        {
            return x.CompareTo(y);
        }

        #endregion

        #region Implementation of IComparable<WikiLanguage>

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
        public int CompareTo(WikiLanguage other)
        {
            int val = Code.CompareTo(other.Code);
            if (val != 0)
            {
                return val;
            }

            val = Name.CompareTo(other.Name);
            if (val != 0)
            {
                return val;
            }

            val = LocalName.CompareTo(other.LocalName);
            if (val != 0)
            {
                return val;
            }

            val = Notes.CompareTo(other.Notes);
            return val;
        }

        #endregion // Implementation of IComparable<WikiLanguage>
    }
}

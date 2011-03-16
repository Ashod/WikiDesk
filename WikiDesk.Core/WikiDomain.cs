
namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WikiDomain : IComparer<WikiDomain>, IComparable<WikiDomain>
    {
        public WikiDomain()
            : this(string.Empty)
        {
        }

        public WikiDomain(string name)
            : this(name, name + ".org")
        {
        }

        public WikiDomain(string name, string domain)
        {
            Name = name;
            Domain = domain;
            FiendlyPath = "/";
            FullPath = "/w/index.php?title=";
        }

        /// <summary>
        /// The name is typically the host name, without the domain.
        /// <example>Wikipedia</example>
        /// <example>Wikinews</example>
        /// </summary>
        public string Name;

        /// <summary>
        /// The Domain is the host name.
        /// <example>wikipedia.org</example>
        /// <example>wikinews.org</example>
        /// </summary>
        public string Domain;

        /// <summary>
        /// For editing, the full path must be provided, as opposed to the
        /// shortcut viewing path.
        /// <example>/w/index.php?title=</example>
        /// </summary>
        public string FullPath;

        /// <summary>
        /// User-friendly path to the pages. Can't take arguments. Not used for editing.
        /// <example>/wiki/</example>
        /// </summary>
        public string FiendlyPath;

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

            val = Domain.CompareTo(other.Domain);
            if (val != 0)
            {
                return val;
            }

            val = FiendlyPath.CompareTo(other.FiendlyPath);
            if (val != 0)
            {
                return val;
            }

            val = FullPath.CompareTo(other.FullPath);
            return val;
        }

        #endregion // Implementation of IComparable<WikiDomain>
    }
}

namespace WikiDesk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SQLite;

    public class Domain : IComparer<Domain>, IComparable<Domain>
    {
        [PrimaryKey]
        [AutoIncrement]
        [Indexed]
        public int Id { get; set; }

        [MaxLength(32)]
        [Indexed]
        public string Name { get; set; }

        #region Implementation of IComparer<Domain>

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
        public int Compare(Domain x, Domain y)
        {
            return x.CompareTo(y);
        }

        #endregion // Implementation of IComparer<Domain>

        #region Implementation of IComparable<Domain>

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
        public int CompareTo(Domain other)
        {
            return Name.CompareTo(other.Name);
        }

        #endregion // Implementation of IComparable<Domain>
    }

    public partial class Database
    {
        /// <summary>
        /// Updates an existing Domain record or inserts a new one if missing.
        /// </summary>
        /// <param name="domain">The domain record in question.</param>
        /// <returns>True if a new record was created.</returns>
        public bool UpdateInsertDomain(Domain domain)
        {
            Domain dbDomain = GetDomain(domain.Name);
            if (dbDomain != null)
            {
                if (dbDomain != domain)
                {
                    Update(domain);
                }

                return false;
            }

            Insert(domain);
            return true;
        }

        /// <summary>
        /// Selects all available domains from the DB.
        /// </summary>
        /// <returns>A list of all domains.</returns>
        public IList<Domain> GetDomains()
        {
            return (from d in Table<Domain>() select d).ToList();
        }

        /// <summary>
        /// Given a domain name, selects the relevant record from the DB.
        /// </summary>
        /// <param name="domainName">The domain name to select.</param>
        /// <returns>A domain record if one is found, otherwise null.</returns>
        public Domain GetDomain(string domainName)
        {
            return (from d in Table<Domain>()
                    where d.Name == domainName
                    select d).FirstOrDefault();
        }
    }
}

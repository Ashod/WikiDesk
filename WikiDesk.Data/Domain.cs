namespace WikiDesk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SQLite;

    public class Domain : IRecord, IComparer<Domain>, IComparable<Domain>, IEquatable<Domain>
    {
        [PrimaryKey]
        [AutoIncrement]
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

            if (obj.GetType() != typeof(Domain))
            {
                return false;
            }

            return Equals((Domain)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(Domain other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.Name, Name);
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
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(Domain left, Domain right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Domain left, Domain right)
        {
            return !Equals(left, right);
        }

        #endregion // Equality
    }

    public partial class Database
    {
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

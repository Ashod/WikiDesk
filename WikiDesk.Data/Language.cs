namespace WikiDesk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SQLite;

    public class Language : IComparer<Language>, IComparable<Language>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [MaxLength(16)]
        [Indexed]
        public string Code { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        #region Implementation of IComparer<Language>

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
        public int Compare(Language x, Language y)
        {
            return x.CompareTo(y);
        }

        #endregion // Implementation of IComparer<Language>

        #region Implementation of IComparable<Language>

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
        public int CompareTo(Language other)
        {
            int val = Code.CompareTo(other.Code);
            if (val != 0)
            {
                return val;
            }

            val = Name.CompareTo(other.Name);
            return val;
        }

        #endregion // Implementation of IComparable<Language>
    }

    public partial class Database
    {
        /// <summary>
        /// Updates an existing Language record or inserts a new one if missing.
        /// </summary>
        /// <param name="lang">The language record in question.</param>
        /// <returns>True if a new record was created.</returns>
        public bool UpdateInsertLanguage(Language lang)
        {
            Language language = GetLanguageByCode(lang.Code);
            if (language != null)
            {
                if (language != lang)
                {
                    Update(lang);
                }

                return false;
            }

            Insert(lang);
            return true;
        }

        /// <summary>
        /// Selects all available languages from the DB.
        /// </summary>
        /// <returns>A list of all languages.</returns>
        public IList<Language> GetLanguages()
        {
            return (from l in Table<Language>() select l).ToList();
        }

        /// <summary>
        /// Given a language code, selects the relevant record from the DB.
        /// </summary>
        /// <param name="languageCode">The language code to select.</param>
        /// <returns>A language record if one is found, otherwise null.</returns>
        public Language GetLanguageByCode(string languageCode)
        {
            return (from l in Table<Language>()
                    where l.Code == languageCode
                    select l).FirstOrDefault();
        }

        public Language GetLanguageByName(string languageName)
        {
            return (from l in Table<Language>()
                    where l.Name == languageName
                    select l).FirstOrDefault();
        }
    }
}

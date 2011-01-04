namespace WikiDesk.Data
{
    using System.Linq;

    using SQLite;

    public class Language
    {
        [PrimaryKey]
        [AutoIncrement]
        [Indexed]
        public int Id { get; set; }

        [MaxLength(16)]
        [Indexed]
        public string Code { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }
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
            Language language = GetLanguage(lang.Code);
            if (language != null)
            {
                Update(lang);
                return false;
            }

            Insert(lang);
            return true;
        }

        /// <summary>
        /// Given a language code, selects the relevant record from the DB.
        /// </summary>
        /// <param name="langCode">The language code to select.</param>
        /// <returns>A language record if one is found, otherwise null.</returns>
        private Language GetLanguage(string langCode)
        {
            return (from l in Table<Language>()
                    where l.Code == langCode
                    select l).FirstOrDefault();
        }
    }
}

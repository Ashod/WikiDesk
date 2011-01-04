
using System;

namespace WikiDesk
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    [Serializable]
    public class Language : IComparer<Language>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string LocalName { get; set; }

        public string Notes { get; set; }

        #region Implementation of IComparer<Language>

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// Value
        ///                     Condition
        ///                     Less than zero
        /// <paramref name="x"/> is less than <paramref name="y"/>.
        ///                     Zero
        /// <paramref name="x"/> equals <paramref name="y"/>.
        ///                     Greater than zero
        /// <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.
        ///                 </param><param name="y">The second object to compare.
        ///                 </param>
        public int Compare(Language x, Language y)
        {
            return x.Code.CompareTo(y.Code);
        }

        #endregion
    }

    [Serializable]
    public class LanguageCodes
    {
        public static LanguageCodes DefaultLanguageCodes()
        {
            // Add default languages.
            LanguageCodes codes = new LanguageCodes();
            codes.Languages.Add(new Language() { Code = "en", Name = "English" , LocalName = "English"});
            codes.Languages.Add(new Language() { Code = "de", Name = "German", LocalName = "Deutsch" });
            codes.Languages.Add(new Language() { Code = "fr", Name = "French", LocalName = "Français" });
            codes.Languages.Add(new Language() { Code = "pl", Name = "Polish", LocalName = "Polski" });
            codes.Languages.Add(new Language() { Code = "it", Name = "Italian", LocalName = "Italiano" });
            codes.Languages.Add(new Language() { Code = "ja", Name = "Japanese", LocalName = "日本語" });
            codes.Languages.Add(new Language() { Code = "es", Name = "Spanish", LocalName = "Español" });
            codes.Languages.Add(new Language() { Code = "nl", Name = "Dutch", LocalName = "Nederlands" });
            codes.Languages.Add(new Language() { Code = "pt", Name = "Portuguese", LocalName = "Português" });
            codes.Languages.Add(new Language() { Code = "ru", Name = "Russian", LocalName = "Русский" });
            codes.Languages.Add(new Language() { Code = "sv", Name = "Swedish", LocalName = "Svenska" });
            codes.Languages.Add(new Language() { Code = "zh", Name = "Chinese", LocalName = "中文" });
            codes.Languages.Add(new Language() { Code = "ca", Name = "Catalan", LocalName = "Català" });
            codes.Languages.Add(new Language() { Code = "no", Name = "Norwegian (Bokmål)", LocalName = "Norsk (Bokmål)" });
            codes.Languages.Add(new Language() { Code = "fi", Name = "Finnish", LocalName = "Suomi" });
            codes.Languages.Add(new Language() { Code = "uk", Name = "Ukrainian", LocalName = "Українська" });
            codes.Languages.Add(new Language() { Code = "hu", Name = "Hungarian", LocalName = "Magyar" });
            codes.Languages.Add(new Language() { Code = "cs", Name = "Czech", LocalName = "Čeština" });
            codes.Languages.Add(new Language() { Code = "ro", Name = "Romanian", LocalName = "Română" });
            codes.Languages.Add(new Language() { Code = "tr", Name = "Turkish", LocalName = "Türkçe" });
            return codes;
        }

        public static LanguageCodes Deserialize(string filename)
        {
            try
            {
                LanguageCodes codes;
                XmlSerializer serializer = new XmlSerializer(typeof(LanguageCodes));
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    codes = (LanguageCodes)serializer.Deserialize(fs);
                }

                if ((codes.Languages != null) && (codes.Languages.Count > 0))
                {
                    return codes;
                }
            }
            catch (Exception)
            {
            }

            return DefaultLanguageCodes();
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LanguageCodes));
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        public readonly List<Language> Languages = new List<Language>(32);
    }

    public class WikiArticleName
    {
        public WikiArticleName(string title, Language lang)
        {
            Name = title;
            if (lang != null)
            {
                LanguageCode = lang.Code;
                if (!string.IsNullOrEmpty(lang.Name))
                {
                    LanguageName = lang.Name;
                    return;
                }

                if (!string.IsNullOrEmpty(lang.LocalName))
                {
                    LanguageName = lang.LocalName;
                    return;
                }
            }

            LanguageCode = "??";
            LanguageName = "Unknown";
        }

        public string Name { get; set; }

        public string LanguageCode { get; set; }

        public string LanguageName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", LanguageName, Name);
        }
    }
}

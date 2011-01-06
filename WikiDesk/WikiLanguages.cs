
namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    [Serializable]
    public class WikiLanguage : IComparer<WikiLanguage>, IComparable<WikiLanguage>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string LocalName { get; set; }

        public string Notes { get; set; }

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

    [Serializable]
    public class LanguageCodes
    {
        public static LanguageCodes DefaultLanguageCodes()
        {
            // Add default languages.
            LanguageCodes codes = new LanguageCodes();
            codes.Languages.Add(new WikiLanguage() { Code = "en", Name = "English" , LocalName = "English"});
            codes.Languages.Add(new WikiLanguage() { Code = "de", Name = "German", LocalName = "Deutsch" });
            codes.Languages.Add(new WikiLanguage() { Code = "fr", Name = "French", LocalName = "Français" });
            codes.Languages.Add(new WikiLanguage() { Code = "pl", Name = "Polish", LocalName = "Polski" });
            codes.Languages.Add(new WikiLanguage() { Code = "it", Name = "Italian", LocalName = "Italiano" });
            codes.Languages.Add(new WikiLanguage() { Code = "ja", Name = "Japanese", LocalName = "日本語" });
            codes.Languages.Add(new WikiLanguage() { Code = "es", Name = "Spanish", LocalName = "Español" });
            codes.Languages.Add(new WikiLanguage() { Code = "nl", Name = "Dutch", LocalName = "Nederlands" });
            codes.Languages.Add(new WikiLanguage() { Code = "pt", Name = "Portuguese", LocalName = "Português" });
            codes.Languages.Add(new WikiLanguage() { Code = "ru", Name = "Russian", LocalName = "Русский" });
            codes.Languages.Add(new WikiLanguage() { Code = "sv", Name = "Swedish", LocalName = "Svenska" });
            codes.Languages.Add(new WikiLanguage() { Code = "zh", Name = "Chinese", LocalName = "中文" });
            codes.Languages.Add(new WikiLanguage() { Code = "ca", Name = "Catalan", LocalName = "Català" });
            codes.Languages.Add(new WikiLanguage() { Code = "no", Name = "Norwegian (Bokmål)", LocalName = "Norsk (Bokmål)" });
            codes.Languages.Add(new WikiLanguage() { Code = "fi", Name = "Finnish", LocalName = "Suomi" });
            codes.Languages.Add(new WikiLanguage() { Code = "uk", Name = "Ukrainian", LocalName = "Українська" });
            codes.Languages.Add(new WikiLanguage() { Code = "hu", Name = "Hungarian", LocalName = "Magyar" });
            codes.Languages.Add(new WikiLanguage() { Code = "cs", Name = "Czech", LocalName = "Čeština" });
            codes.Languages.Add(new WikiLanguage() { Code = "ro", Name = "Romanian", LocalName = "Română" });
            codes.Languages.Add(new WikiLanguage() { Code = "tr", Name = "Turkish", LocalName = "Türkçe" });
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

                if (codes.Languages != null && codes.Languages.Count > 0)
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

        public readonly List<WikiLanguage> Languages = new List<WikiLanguage>(32);
    }

    public class WikiArticleName
    {
        public WikiArticleName(string title, WikiLanguage lang)
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


using System;

namespace WikiDesk
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    [Serializable]
    public class Language
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string LocalName { get; set; }

        public string Notes { get; set; }
    }

    [Serializable]
    public class LanguageCodes
    {
        public LanguageCodes()
        {
            // Add default languages.
            Languages = new List<Language>(32);
            Languages.Add(new Language() { Code = "en", Name = "English" , LocalName = "English"});
            Languages.Add(new Language() { Code = "de", Name = "German", LocalName = "Deutsch" });
            Languages.Add(new Language() { Code = "fr", Name = "French", LocalName = "Français" });
            Languages.Add(new Language() { Code = "pl", Name = "Polish", LocalName = "Polski" });
            Languages.Add(new Language() { Code = "it", Name = "Italian", LocalName = "Italiano" });
            Languages.Add(new Language() { Code = "ja", Name = "Japanese", LocalName = "日本語" });
            Languages.Add(new Language() { Code = "es", Name = "Spanish", LocalName = "Español" });
            Languages.Add(new Language() { Code = "nl", Name = "Dutch", LocalName = "Nederlands" });
            Languages.Add(new Language() { Code = "pt", Name = "Portuguese", LocalName = "Português" });
            Languages.Add(new Language() { Code = "ru", Name = "Russian", LocalName = "Русский" });
            Languages.Add(new Language() { Code = "sv", Name = "Swedish", LocalName = "Svenska" });
            Languages.Add(new Language() { Code = "zh", Name = "Chinese", LocalName = "中文" });
            Languages.Add(new Language() { Code = "ca", Name = "Catalan", LocalName = "Català" });
            Languages.Add(new Language() { Code = "no", Name = "Norwegian (Bokmål)", LocalName = "Norsk (Bokmål)" });
            Languages.Add(new Language() { Code = "fi", Name = "Finnish", LocalName = "Suomi" });
            Languages.Add(new Language() { Code = "uk", Name = "Ukrainian", LocalName = "Українська" });
            Languages.Add(new Language() { Code = "hu", Name = "Hungarian", LocalName = "Magyar" });
            Languages.Add(new Language() { Code = "cs", Name = "Czech", LocalName = "Čeština" });
            Languages.Add(new Language() { Code = "ro", Name = "Romanian", LocalName = "Română" });
            Languages.Add(new Language() { Code = "tr", Name = "Turkish", LocalName = "Türkçe" });
        }

        public static LanguageCodes Deserialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LanguageCodes));
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (LanguageCodes)serializer.Deserialize(fs);
            }
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LanguageCodes));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        public readonly List<Language> Languages;
    }
}

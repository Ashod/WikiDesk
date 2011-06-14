
namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using WikiDesk.Core;

    [Serializable]
    public class LanguageCodes
    {
        public static LanguageCodes DefaultLanguageCodes()
        {
            // Add default languages.
            LanguageCodes codes = new LanguageCodes();
            codes.Languages.Add(new WikiLanguage("English", "en") { LocalName = "English" });
            codes.Languages.Add(new WikiLanguage("German", "de") { LocalName = "Deutsch" });
            codes.Languages.Add(new WikiLanguage("French", "fr") { LocalName = "Français" });
            codes.Languages.Add(new WikiLanguage("Polish",  "pl") { LocalName = "Polski" });
            codes.Languages.Add(new WikiLanguage("Italian", "it") { LocalName = "Italiano" });
            codes.Languages.Add(new WikiLanguage("Japanese", "ja") { LocalName = "日本語" });
            codes.Languages.Add(new WikiLanguage("Spanish", "es") { LocalName = "Español" });
            codes.Languages.Add(new WikiLanguage("Dutch", "nl") { LocalName = "Nederlands" });
            codes.Languages.Add(new WikiLanguage("Portugues", "pt") { LocalName = "Português" });
            codes.Languages.Add(new WikiLanguage("Russian", "ru") { LocalName = "Русский" });
            codes.Languages.Add(new WikiLanguage("Swedish", "sv") { LocalName = "Svenska" });
            codes.Languages.Add(new WikiLanguage("Chinese", "zh") { LocalName = "中文" });
            codes.Languages.Add(new WikiLanguage("Catalan", "ca") { LocalName = "Català" });
            codes.Languages.Add(new WikiLanguage("Norwegian", "no") { LocalName = "Norsk (Bokmål)" });
            codes.Languages.Add(new WikiLanguage("Finnish", "fi") { LocalName = "Suomi" });
            codes.Languages.Add(new WikiLanguage("Ukrainian", "uk") { LocalName = "Українська" });
            codes.Languages.Add(new WikiLanguage("Hungarian", "hu") { LocalName = "Magyar" });
            codes.Languages.Add(new WikiLanguage("Czech", "cs") { LocalName = "Čeština" });
            codes.Languages.Add(new WikiLanguage("Romanian", "ro") { LocalName = "Română" });
            codes.Languages.Add(new WikiLanguage("Turkish", "tr") { LocalName = "Türkçe" });
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
                    codes.Languages.Sort();
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
            if (!string.IsNullOrEmpty(Name))
            {
                return string.Format("{0} - {1}", LanguageName, Name);
            }

            return string.Format("{0} [{1}]", LanguageName, LanguageCode);
        }
    }
}

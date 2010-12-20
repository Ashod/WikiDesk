using System;

namespace WikiDesk
{
    using System.IO;
    using System.Xml.Serialization;

    [Serializable]
    public class Settings
    {
        public static Settings Deserialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (Settings)serializer.Deserialize(fs);
            }
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        public bool AutoUpdate;
        public bool AutoRetrieveMissing;

        /// <summary>
        /// The base URL for the wiki.
        /// A language code is prepended to this base-url.
        /// </summary>
        public string BaseUrl = ".wikipedia.org/wiki/";

        /// <summary>
        /// The export-page URL for the wiki.
        /// A language code is prepended to this url.
        /// </summary>
        public string ExportUrl = ".wikipedia.org/wiki/Special:Export/";

        /// <summary>
        /// The language codes configuration filename.
        /// </summary>
        public string LanguageCodesFilename = "LangCodes.xml";

        /// <summary>
        /// The current language code.
        /// </summary>
        public string CurrentLanguageCode = "en";

        public string CssFilename = "css\\default.css";

        public int ThumbnailWidthPixels = 220;

        public string FileCacheFolder = "Z:\\wikidesk_cache\\";

        /// <summary>
        /// The default database filename. Loaded at startup.
        /// </summary>
        public string DefaultDatabaseFilename = "Z:\\wikidesk.db";
    }
}

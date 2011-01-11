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
            Settings settings;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    settings = (Settings)serializer.Deserialize(fs);
                }

                if (settings != null)
                {
                    return settings;
                }
            }
            catch (Exception)
            {
            }

            settings = new Settings();
            return settings;
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        public bool AutoUpdate = true;
        public bool AutoRetrieveMissing = true;

        #region domain

        /// <summary>
        /// The wiki domains configuration filename.
        /// </summary>
        public string DomainsFilename = "WikiDomains.xml";

        /// <summary>
        /// The current domain name.
        /// </summary>
        public string CurrentDomainName = "wikipedia";

        /// <summary>
        /// The default domain name. When no domain is specified.
        /// </summary>
        public string DefaultDomainName = "wikipedia";

        #endregion // domain

        #region language

        /// <summary>
        /// The wiki languages configuration filename.
        /// </summary>
        public string LanguagesFilename = "WikiLanguages.xml";

        /// <summary>
        /// The current language code.
        /// </summary>
        public string CurrentLanguageCode = "en";

        /// <summary>
        /// The default language code. When no language is specified.
        /// </summary>
        public string DefaultLanguageCode = "en";

        #endregion // language

        public string SkinName = "simple";

        public int ThumbnailWidthPixels = 220;

        public string FileCacheFolder = "Z:\\wikidesk_cache\\";

        /// <summary>
        /// The default database filename. Loaded at startup.
        /// </summary>
        public string DefaultDatabaseFilename = "Z:\\wikidesk.db";
    }
}

namespace WikiDesk.Core
{
    public class Configuration
    {
        public Configuration(WikiSite wikiSite)
        {
            wikiSite_ = wikiSite;
        }

        public string InternalLinkPrefix = "wiki://";

        /// <summary>
        /// The base URL for the wiki.
        /// A language code is prepended to this base-url.
        /// </summary>
        public string BaseUrl
        {
            get { return wikiSite_.BaseUrl; }
        }

        /// <summary>
        /// The export-page URL for the wiki.
        /// A language code is prepended to this url.
        /// </summary>
        public string ExportUrl
        {
            get { return wikiSite_.ExportUrl; }
        }

        /// <summary>
        /// The current language code.
        /// </summary>
        public string CurrentLanguageCode
        {
            get { return wikiSite_.Language.Code; }
        }

        public string FileUrl
        {
            get { return FullUrl + "File:"; }
        }

        public string FullUrl
        {
            get { return "http://" + CurrentLanguageCode + BaseUrl; }
        }

        public string SkinsPath = string.Empty;

        /// <summary>
        /// The common-images path within the skins folder.
        /// </summary>
        public string CommonImagesPath
        {
            get { return "common\\images"; }
        }

        public int ThumbnailWidthPixels = 220;

        public string FileCacheFolder = "Z:\\wikidesk_cache\\";

        private readonly WikiSite wikiSite_;
    }
}

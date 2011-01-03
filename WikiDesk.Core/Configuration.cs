namespace WikiDesk.Core
{
    public class Configuration
    {
        public string InternalLinkPrefix = "wiki://";

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
        /// The current language code.
        /// </summary>
        public string CurrentLanguageCode = "en";

        public string FileUrl
        {
            get { return FullUrl + "File:"; }
        }

        public string FullUrl
        {
            get { return "http://" + CurrentLanguageCode + BaseUrl; }
        }

        public string CommonImagesPath
        {
            get { return "skins\\common\\images"; }
        }

        public int ThumbnailWidthPixels = 220;

        public string FileCacheFolder = "Z:\\wikidesk_cache\\";
    }
}

namespace WikiDesk.Core
{
    public class Configuration
    {
        public Configuration(WikiSite wikiSite)
        {
            wikiSite_ = wikiSite;
        }

        public WikiSite WikiSite
        {
            get { return wikiSite_; }
        }

        public bool AutoRedirect = false;

        /// <summary>
        /// The number of H1 headers to automatically add TOC.
        /// Default is 4.
        /// -ve number to never automatically add TOC.
        /// 0 number to always add TOC.
        /// +ve number to add when at least that many headers exist.
        /// </summary>
        public int AutoTocHeaderCount = 4;

        /// <summary>
        /// The maximum heading number to include in the TOC.
        /// Default is 2.
        /// </summary>
        public int MaxHeadingToIncludeInToc = 2;

        public string InternalLinkPrefix = "wiki://";

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

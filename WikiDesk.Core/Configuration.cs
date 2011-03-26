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

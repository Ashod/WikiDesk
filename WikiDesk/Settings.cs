using System;

namespace WikiDesk
{
    [Serializable]
    public class Settings
    {
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
    }
}

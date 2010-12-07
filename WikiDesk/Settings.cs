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
    }
}

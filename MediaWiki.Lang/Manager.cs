using System.Collections.Generic;

namespace MediaWiki.Lang
{
    using System.IO;

    public class Manager
    {
        public void LoadMessages(string path)
        {
            string[] filenames = Directory.GetFiles(path, "Messages*.dll");

            messagesMap_ = new Dictionary<string, Messages>(filenames.Length);
            foreach (string filename in filenames)
            {
                
            }
        }

        public void GetMessagesForLanguage(string langCode)
        {
            
        }

        #region representation

        /// <summary>
        /// Maps language codes to Messages instances.
        /// </summary>
        Dictionary<string, Messages> messagesMap_;

        #endregion // representation
    }
}

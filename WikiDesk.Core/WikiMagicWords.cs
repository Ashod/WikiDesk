namespace WikiDesk.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Maps magic words and their aliases, possibly in different languages.
    /// Each magic word has a canonical, English and typically lower-case versions.
    /// These canonical versions are the IDs and the aliases are the words.
    /// This class helps us lookup a magic word alias into its canonical version (ID).
    /// </summary>
    public class WikiMagicWords
    {
        /// <summary>
        /// Finds a magic word ID, if registered.
        /// </summary>
        /// <param name="word">A magic word alias in any language.</param>
        /// <returns>The magic word ID, if registered, otherwise null.</returns>
        public string FindId(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return null;
            }

            string id;

            //TODO: Should we really trim the colon at the end?

            // Assume case-sensitive.
            if (caseSensitiveWordsMap_.TryGetValue(word, out id) ||
                caseSensitiveWordsMap_.TryGetValue(word.TrimEnd(':'), out id))
            {
                return id;
            }

            // Try case-insensitive.
            word = word.ToUpperInvariant();
            if (caseInsensitiveWordsMap_.TryGetValue(word, out id) ||
                caseInsensitiveWordsMap_.TryGetValue(word.TrimEnd(':'), out id))
            {
                return id;
            }

            // No luck.
            return null;
        }

        /// <summary>
        /// Registers a magic word for a given ID.
        /// </summary>
        /// <param name="id">The ID of the magic word.</param>
        /// <param name="word">The magic word to register.</param>
        /// <param name="caseSensitive">Whether the word is case-sensitive or not.</param>
        public void RegisterWord(string id, string word, bool caseSensitive)
        {
            if (caseSensitive)
            {
                caseSensitiveWordsMap_[word] = id;
            }
            else
            {
                //TODO: Should this be converted by a language-specific converter?
                caseInsensitiveWordsMap_[word.ToUpperInvariant()] = id;
            }
        }

        #region representation

        private readonly Dictionary<string, string> caseSensitiveWordsMap_ = new Dictionary<string, string>(16);
        private readonly Dictionary<string, string> caseInsensitiveWordsMap_ = new Dictionary<string, string>(16);

        #endregion // representation
    }
}


namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using MediaWiki.Lang;

    /// <summary>
    /// Represents a Wiki Site with all its info and attributes.
    /// A Wiki Site is a unique language.domain pair, such as en.wikipedia,
    /// hy.wiktionary, simple.wikibooks, fr.wikiquote or sr.wikinews.
    /// Since each on-line site has its own settings, templates and magic words,
    /// here is where we define those things and bring them together.
    /// WikiSite emulates on-line wiki sites.
    /// </summary>
    public class WikiSite
    {
        #region construction

        public WikiSite(WikiDomain domain, WikiLanguage language, string folder)
        {
            domain_ = domain;
            language_ = language;

            string langCode = language_.MimeCode ?? language_.Code;
            string moduleFilePath = Path.Combine(folder, MESSAGES);
            string modulefullPath = Path.Combine(moduleFilePath, MESSAGES + langCode + DLL_EXTENTION);
            module_ = new Module(modulefullPath);

            // Languages may fall-back on others. The only Exception is the default language.
            if (string.Compare(language.Code, DEF_LANG_CODE, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                string fallbackLangCode = module_.GetStringField(FIELD_FALLBACK);
                if (string.IsNullOrEmpty(fallbackLangCode))
                {
                    fallbackLangCode = DEF_LANG_CODE;
                }

                modulefullPath = Path.Combine(moduleFilePath, MESSAGES + fallbackLangCode + DLL_EXTENTION);
                Module fallbackModule = new Module(modulefullPath);
                MergeNamespaces(fallbackModule.GetString2StringMapField(FIELD_NAMESPACE_NAMES));
                MergeSpecialPageAliases(fallbackModule.GetString2StringsMapField(FIELD_SPECIAL_PAGE_ALIASES));
                MergeMagicWords(fallbackModule.GetString2StringsMapField(FIELD_MAGIC_WORDS));
            }

            MergeNamespaces(module_.GetString2StringMapField(FIELD_NAMESPACE_NAMES));
            MergeSpecialPageAliases(module_.GetString2StringsMapField(FIELD_SPECIAL_PAGE_ALIASES));
            MergeMagicWords(module_.GetString2StringsMapField(FIELD_MAGIC_WORDS));

            CurrentNamespace = Namespace.Main;
        }

        #endregion // construction

        #region properties

        public WikiLanguage Language
        {
            get { return language_; }
        }

        public WikiDomain Domain
        {
            get { return domain_; }
        }

        public string BaseFullUrl
        {
            get { return string.Format(".{0}{1}", domain_.Domain, domain_.FullPath); }
        }

        public string BaseFriendlyUrl
        {
            get { return string.Format(".{0}{1}", domain_.Domain, domain_.FiendlyPath); }
        }

        public string ExportUrl
        {
            get
            {
                string[] aliases = GetSpecialPageAliases(EXPORT);
                string export = aliases != null ? aliases[0] : EXPORT;
                return string.Format("{0}{1}:{2}/", BaseFullUrl, GetNamespaceName(Namespace.Special), export);
            }
        }

        public ICollection<string> Namespaces
        {
            get { return namespaces_.Values; }
        }

        public WikiMagicWords MagicWords
        {
            get { return magicWords_; }
        }

        public Namespace CurrentNamespace { get; set; }

        #endregion // properties

        public string GetViewUrl(string title)
        {
            return string.Format("http://{0}{1}{2}", language_.Code, BaseFriendlyUrl, Title.Normalize(title));
        }

        public string GetFileUrl(string title)
        {
            return string.Format("http://{0}{1}File:{2}", language_.Code, BaseFriendlyUrl, Title.Normalize(title));
        }

        public string GetExportUrl(string title)
        {
            return string.Format("http://{0}{1}{2}", language_.Code, ExportUrl, Title.Normalize(title));
        }

        public string GetEditUrl(string title)
        {
            return GetFullUrl(title) + "&action=edit";
        }

        public string GetFullUrl(string title)
        {
            return string.Format("http://{0}{1}{2}", language_.Code, BaseFullUrl, Title.Normalize(title));
        }

        public enum Namespace
        {
            Media,
            Special,
            Main,
            Talk,
            User,
            User_Talk,
            Wikipedia,
            Wikipedia_Talk,
            Project_Talk,
            File,
            File_Talk,
            MediaWiki,
            MediaWiki_Talk,
            Tempalate,
            Template_Talk,
            Help,
            Help_Talk,
            Category,
            Category_Talk,
            Portal,
            Portal_Talk,
            Book,
            Book_Talk
        }

        /// <summary>
        /// Returns the namespace for the given key.
        /// </summary>
        /// <param name="key">The namespace key to lookup.</param>
        /// <returns>The namespace.</returns>
        /// <exception cref="ApplicationException">Can't find Namespace with key: {0}.</exception>
        public Namespace GetNamespace(int key)
        {
            Namespace ns;
            if (NamespaceKeys_.TryGetValue(key, out ns))
            {
                return ns;
            }

            throw new ApplicationException("Can't find Namespace with key: " + key);
        }

        /// <summary>
        /// Returns the namespace name for the given key.
        /// </summary>
        /// <param name="key">The namespace key to lookup its name.</param>
        /// <returns>The site-specific name of the namespace.</returns>
        /// <exception cref="ApplicationException">Can't find Namespace with key: {0}.</exception>
        public string GetNamespaceName(int key)
        {
            return GetNamespaceName(GetNamespace(key));
        }

        /// <summary>
        /// Returns the namespace name for this site.
        /// Will return string.Empty if not found.
        /// </summary>
        /// <param name="ns">The namespace to lookup its name.</param>
        /// <returns>The site-specific name of the namespace.</returns>
        public string GetNamespaceName(Namespace ns)
        {
            string value;
            switch (ns)
            {
                case Namespace.Media:
                    namespaces_.TryGetValue("NS_MEDIA", out value);
                    break;

                case Namespace.Special:
                    namespaces_.TryGetValue("NS_SPECIAL", out value);
                    break;

                case Namespace.Main:
                    namespaces_.TryGetValue("NS_MAIN", out value);
                    break;

                case Namespace.Talk:
                    namespaces_.TryGetValue("NS_TALK", out value);
                    break;

                case Namespace.User:
                    namespaces_.TryGetValue("NS_USER", out value);
                    break;

                case Namespace.User_Talk:
                    namespaces_.TryGetValue("NS_USER_TALK", out value);
                    break;

                case Namespace.Wikipedia:
                    namespaces_.TryGetValue("NS_WIKIPEDIA", out value);
                    break;

                case Namespace.Wikipedia_Talk:
                    namespaces_.TryGetValue("NS_WIKIPEDIA_TALK", out value);
                    break;

                case Namespace.Project_Talk:
                    namespaces_.TryGetValue("NS_PROJECT_TALK", out value);
                    break;

                case Namespace.File:
                    namespaces_.TryGetValue("NS_FILE", out value);
                    break;

                case Namespace.File_Talk:
                    namespaces_.TryGetValue("NS_FILE_TALK", out value);
                    break;

                case Namespace.MediaWiki:
                    namespaces_.TryGetValue("NS_MEDIAWIKI", out value);
                    break;

                case Namespace.MediaWiki_Talk:
                    namespaces_.TryGetValue("NS_MEDIAWIKI_TALK", out value);
                    break;

                case Namespace.Tempalate:
                    namespaces_.TryGetValue("NS_TEMPLATE", out value);
                    break;

                case Namespace.Template_Talk:
                    namespaces_.TryGetValue("NS_TEMPLATE_TALK", out value);
                    break;

                case Namespace.Help:
                    namespaces_.TryGetValue("NS_HELP", out value);
                    break;

                case Namespace.Help_Talk:
                    namespaces_.TryGetValue("NS_HELP_TALK", out value);
                    break;

                case Namespace.Category:
                    namespaces_.TryGetValue("NS_CATEGORY", out value);
                    break;

                case Namespace.Category_Talk:
                    namespaces_.TryGetValue("NS_CATEGORY_TALK", out value);
                    break;

                case Namespace.Portal:
                    namespaces_.TryGetValue("NS_PORTAL", out value);
                    break;

                case Namespace.Portal_Talk:
                    namespaces_.TryGetValue("NS_PORTAL_TALK", out value);
                    break;

                case Namespace.Book:
                    namespaces_.TryGetValue("NS_BOOK", out value);
                    break;

                case Namespace.Book_Talk:
                    namespaces_.TryGetValue("NS_BOOK_TALK", out value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        public string[] GetSpecialPageAliases(string pageName)
        {
            string[] aliases;
            if (specialPageAliases_.TryGetValue(pageName, out aliases))
            {
                return aliases;
            }

            return null;
        }

        #region implementation

        private void MergeNamespaces(Dictionary<string, string> namespaces)
        {
            foreach (KeyValuePair<string, string> pair in namespaces)
            {
                namespaces_[pair.Key] = pair.Value;
            }
        }

        private void MergeSpecialPageAliases(Dictionary<string, string[]> specialPageAliases)
        {
            foreach (KeyValuePair<string, string[]> pair in specialPageAliases)
            {
                specialPageAliases_[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Populate the MagicWords instance from the map provided by the MessagesXX.php.
        /// </summary>
        /// <param name="mapMagicWords"></param>
        private void MergeMagicWords(Dictionary<string, string[]> mapMagicWords)
        {
            foreach (KeyValuePair<string, string[]> pair in mapMagicWords)
            {
                if (pair.Value != null && pair.Value.Length >= 1)
                {
                    // The format of the value is: ['?', "name1", "name2", ..., "nameN" ]
                    // Where '?' is either 1 (case-sensitive) or 2 (case-insensitive).
                    bool caseSensitive = false;
                    if (pair.Value[0] == "1")
                    {
                        caseSensitive = true;
                    }
                    else
                    if (pair.Value[0] != "0")
                    {
                        continue;
                    }

                    // The Key can be used directly.
                    magicWords_.RegisterWord(pair.Key, pair.Key, caseSensitive);

                    // Add aliases as well, if any.
                    for (int i = 1; i < pair.Value.Length; ++i)
                    {
                        string word = pair.Value[i];
                        magicWords_.RegisterWord(pair.Key, word, caseSensitive);
                    }
                }
            }
        }

        #endregion // implementation

        #region representation

        private readonly Module module_;
        private readonly WikiLanguage language_;
        private readonly WikiDomain domain_;

        private readonly Dictionary<string, string> namespaces_ = new Dictionary<string, string>(32);
        private readonly Dictionary<string, string[]> specialPageAliases_ = new Dictionary<string, string[]>(64);
        private readonly WikiMagicWords magicWords_ = new WikiMagicWords();

        private static Dictionary<int, Namespace> NamespaceKeys_ = new Dictionary<int, Namespace>()
            {
                { -2, Namespace.Media },
                { -1, Namespace.Special },
                { 0, Namespace.Main },
                { 1, Namespace.Talk },
                { 2, Namespace.User },
                { 3, Namespace.User_Talk },
                { 4, Namespace.Wikipedia },
                { 5, Namespace.Wikipedia_Talk },
                { 6, Namespace.File },
                { 7, Namespace.File_Talk },
                { 8, Namespace.MediaWiki },
                { 9, Namespace.MediaWiki_Talk },
                { 10, Namespace.Tempalate },
                { 11, Namespace.Template_Talk },
                { 12, Namespace.Help },
                { 13, Namespace.Help_Talk },
                { 14, Namespace.Category },
                { 15, Namespace.Category_Talk },
                { 100, Namespace.Portal },
                { 101, Namespace.Portal_Talk },
                { 108, Namespace.Book },
                { 109, Namespace.Book_Talk }
            };

        #endregion // representation

        #region constants

        private const string DEF_LANG_CODE = "en";

        private const string DLL_EXTENTION = ".dll";

        private const string EXPORT = "Export";

        private const string MESSAGES = "Messages";

        private const string FIELD_FALLBACK = "fallback";

        private const string FIELD_NAMESPACE_NAMES = "namespaceNames";

        private const string FIELD_SPECIAL_PAGE_ALIASES = "specialPageAliases";

        private const string FIELD_MAGIC_WORDS = "magicWords";

        #endregion // constants

    }
}

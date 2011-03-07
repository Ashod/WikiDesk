
namespace WikiDesk.Core
{
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

        public WikiSite(WikiDomain domain, WikiLanguage language)
        {
            domain_ = domain;
            language_ = language;

            string moduleFilePath = Path.Combine("Messages", "Messages" + language_.Code + ".dll");

            module_ = new Module(moduleFilePath);
            namespaces_ = module_.GetStringMapField("namespaceNames");
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

        #endregion // properties

        public enum Namespace
        {
            Media,
            Special,
            Talk,
            User,
            User_Talk,
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
            Category_Talk
        }

        /// <summary>
        /// Returns the namespace name for this site.
        /// </summary>
        /// <param name="ns">The namespace to lookup its name.</param>
        /// <returns>The site-specific name of the namespace.</returns>
        public string GetNamespace(Namespace ns)
        {
            object value;
            switch (ns)
            {
                case Namespace.Media:
                    namespaces_.TryGetValue("NS_MEDIA", out value);
                    break;

                case Namespace.Special:
                    namespaces_.TryGetValue("NS_SPECIAL", out value);
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

                default:
                    throw new System.ArgumentOutOfRangeException();
            }

            return value as string;
        }

        #region representation

        private readonly Module module_;

        private readonly Dictionary<string, object> namespaces_;

        private WikiMessages messages_;

        private readonly WikiLanguage language_;

        private readonly WikiDomain domain_;

        #endregion // representation
    }
}

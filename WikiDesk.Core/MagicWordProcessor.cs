﻿
namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class MagicWordProcessor : VariableProcessor
    {
        public MagicWordProcessor()
        {
            RegisterHandlers();
        }

        public void SetContext(WikiSite wikiSite, string nameSpace, string pageTitle)
        {
            wikiSite_ = wikiSite;
            nameSpace_ = nameSpace;
            pageTitle_ = pageTitle;
        }

        #region implementation

        private void RegisterHandlers()
        {
            RegisterHandler("int",               DoNothing);
            RegisterHandler("ns",                DoNothing);
            RegisterHandler("nse",               DoNothing);
            RegisterHandler("urlencode",         UrlEncode);
            RegisterHandler("lcfirst",           LcFirst);
            RegisterHandler("ucfirst",           UcFirst);
            RegisterHandler("lc",                Lc);
            RegisterHandler("uc",                Uc);
            RegisterHandler("localurl",          LocalUrl);
            RegisterHandler("localurle",         LocalUrl);
            RegisterHandler("fullurl",           FullUrl);
            RegisterHandler("fullurle",          FullUrl);
            RegisterHandler("formatnum",         DoNothing);
            RegisterHandler("grammar",           DoNothing);
            RegisterHandler("gender",            DoNothing);
            RegisterHandler("plural",            DoNothing);
            RegisterHandler("numberofpages",     DoNothing);
            RegisterHandler("numberofusers",     DoNothing);
            RegisterHandler("numberofactiveusers", DoNothing);
            RegisterHandler("numberofarticles",  DoNothing);
            RegisterHandler("numberoffiles",     DoNothing);
            RegisterHandler("numberofadmins",    DoNothing);
            RegisterHandler("numberingroup",     DoNothing);
            RegisterHandler("numberofedits",     DoNothing);
            RegisterHandler("numberofviews",     DoNothing);
            RegisterHandler("language",          DoNothing);
            RegisterHandler("padleft",           DoNothing);
            RegisterHandler("padright",          DoNothing);
            RegisterHandler("anchorencode",      DoNothing);
            RegisterHandler("#special",          DoNothing);
            RegisterHandler("defaultsort",       DoNothing);
            RegisterHandler("filepath",          DoNothing);
            RegisterHandler("pagesincategory",   DoNothing);
            RegisterHandler("pagesize",          DoNothing);
            RegisterHandler("protectionlevel",   DoNothing);
            RegisterHandler("namespace",         DoNothing);
            RegisterHandler("namespacee",        DoNothing);
            RegisterHandler("talkspace",         DoNothing);
            RegisterHandler("talkspacee",        DoNothing);
            RegisterHandler("subjectspace",      DoNothing);
            RegisterHandler("subjectspacee",     DoNothing);
            RegisterHandler("pagename",          PageName);
            RegisterHandler("pagenamee",         PageName);
            RegisterHandler("fullpagename",      FullPageName);
            RegisterHandler("fullpagenamee",     FullPageName);
            RegisterHandler("basepagename",      BasePageName);
            RegisterHandler("basepagenamee",     BasePageName);
            RegisterHandler("subpagename",       SubPageName);
            RegisterHandler("subpagenamee",      SubPageName);
            RegisterHandler("talkpagename",      TalkPageName);
            RegisterHandler("talkpagenamee",     TalkPageName);
            RegisterHandler("subjectpagename",   SubjectPageName);
            RegisterHandler("subjectpagenamee",  SubjectPageName);
            RegisterHandler("tag",               Tag);
            RegisterHandler("#formatdate",       FormatDate);
        }

        /// <summary>
        /// Returns the page title.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words	-> Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result PageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            return Result.Found;
        }

        /// <summary>
        /// Returns the full page title, including namespace.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words	-> Help:Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result FullPageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = Title.FullPageName(nameSpace_, pageTitle_);
            return Result.Found;
        }

        /// <summary>
        /// Returns the page title excluding the current subpage and
        /// namespace ("Title/foo" on "Title/foo/bar").
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words	-> Magic words.
        /// </example>
        /// <remarks>
        /// For more complex splitting, use {{#titleparts:}} from ParserFunctions extension.
        /// </remarks>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result BasePageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            return Result.Found;
        }

        /// <summary>
        /// Returns the subpage title ("foo" on "Title/foo").
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words	-> Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result SubPageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            return Result.Found;
        }

        /// <summary>
        /// Returns namespace and title of the associated talk page.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words	-> Help talk:Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result TalkPageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            return Result.Found;
        }

        /// <summary>
        /// Returns the namespace and title of the associated content page.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words	-> Help:Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result SubjectPageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = Title.FullPageName(nameSpace_, pageTitle_);
            return Result.Found;
        }

        private Result Tag(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result FormatDate(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result UrlEncode(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args != null && args.Count > 0)
            {
                string arg = args[0].Value;
                if (!string.IsNullOrEmpty(arg))
                {
                    arg = arg.Trim(WhiteSpaceChars);
                    output = HttpUtility.UrlEncode(arg);
                    return Result.Found;
                }
            }

            output = string.Empty;
            return Result.Found;
        }

        private static Result Lc(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args != null && args.Count > 0)
            {
                string arg = args[0].Value;
                if (!string.IsNullOrEmpty(arg))
                {
                    output = arg.ToLowerInvariant();
                    return Result.Found;
                }
            }

            output = string.Empty;
            return Result.Found;
        }

        private static Result Uc(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args != null && args.Count > 0)
            {
                string arg = args[0].Value;
                if (!string.IsNullOrEmpty(arg))
                {
                    output = arg.ToUpperInvariant();
                    return Result.Found;
                }
            }

            output = string.Empty;
            return Result.Found;
        }

        private static Result UcFirst(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args != null && args.Count > 0)
            {
                string arg = args[0].Value;
                if (!string.IsNullOrEmpty(arg))
                {
                    output = arg.Substring(0, 1).ToUpperInvariant();
                    if (arg.Length > 1)
                    {
                        output += arg.Substring(1);
                    }

                    return Result.Found;
                }
            }

            output = string.Empty;
            return Result.Found;
        }

        private static Result LcFirst(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args != null && args.Count > 0)
            {
                string arg = args[0].Value;
                if (!string.IsNullOrEmpty(arg))
                {
                    output = arg.Substring(0, 1).ToLowerInvariant();
                    if (arg.Length > 1)
                    {
                        output += arg.Substring(1);
                    }

                    return Result.Found;
                }
            }

            output = string.Empty;
            return Result.Found;
        }

        /// <summary>
        /// Returns the relative path to the title.
        /// {{localurl:page name}}
        /// {{localurl:page name|query_string}}
        /// </summary>
        /// <example>
        /// {{localurl:MediaWiki}} → /wiki/MediaWiki
        /// {{localurl:MediaWiki|printable=yes}} → /w/index.php?title=MediaWiki&printable=yes
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result LocalUrl(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the absolute path to the title. This will also resolve Interwiki prefixes.
        /// {{fullurl:page name}}
        /// {{fullurl:page name|query_string}}
        /// {{fullurl:interwiki:remote page name|query_string}}
        /// </summary>
        /// <example>
        /// {{fullurl:Category:Top level}} → http://www.mediawiki.org/wiki/Category:Top_level
        /// {{fullurl:Category:Top level|action=edit}} → http://www.mediawiki.org/w/index.php?title=Category:Top_level&action=edit
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result FullUrl(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        #endregion // implementation

        #region representation

        private static readonly char[] WhiteSpaceChars = { ' ', '\n', '\r', '\t' };

        private WikiSite wikiSite_;
        private string nameSpace_;
        private string pageTitle_;

        #endregion // representation
    }
}

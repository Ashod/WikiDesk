// -----------------------------------------------------------------------------------------
// <copyright file="MagicWordProcessor.cs" company="ashodnakashian.com">
//
// This file is part of WikiDesk.
// Copyright (C) 2010, 2011 Ashod Nakashian
// https://github.com/Ashod/WikiDesk
//
//  WikiDesk is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  WikiDesk is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with WikiDesk. If not, see http://www.gnu.org/licenses/.
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the MagicWordProcessor type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System.Collections.Generic;
    using System.Web;

    public class MagicWordProcessor : VariableProcessor
    {
        public MagicWordProcessor()
            : this(null)
        {
        }

        public MagicWordProcessor(ProcessMagicWords processMagicWordsDel)
            : base(processMagicWordsDel)
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
            RegisterHandler("ns",                Ns);
            RegisterHandler("nse",               Nse);
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
            RegisterHandler("numberofpages",     BogusNumber);
            RegisterHandler("numberofusers",     BogusNumber);
            RegisterHandler("numberofactiveusers", BogusNumber);
            RegisterHandler("numberofarticles",  BogusNumber);
            RegisterHandler("numberoffiles",     BogusNumber);
            RegisterHandler("numberofadmins",    BogusNumber);
            RegisterHandler("numberingroup",     BogusNumber);
            RegisterHandler("numberofedits",     BogusNumber);
            RegisterHandler("numberofviews",     BogusNumber);
            RegisterHandler("language",          DoNothing);
            RegisterHandler("padleft",           DoNothing);
            RegisterHandler("padright",          DoNothing);
            RegisterHandler("anchorencode",      AnchorEncode);
            RegisterHandler("#special",          DoNothing);
            RegisterHandler("defaultsort",       DoNothing);
            RegisterHandler("filepath",          DoNothing);
            RegisterHandler("pagesincategory",   DoNothing);
            RegisterHandler("pagesize",          BogusNumber);
            RegisterHandler("protectionlevel",   DoNothing);
            RegisterHandler("namespace",         Namespace);
            RegisterHandler("namespacee",        Namespacee);
            RegisterHandler("talkspace",         Talkspace);
            RegisterHandler("talkspacee",        Talkspacee);
            RegisterHandler("subjectspace",      Subjectspace);
            RegisterHandler("subjectspacee",     Subjectspacee);
            RegisterHandler("pagename",          PageName);
            RegisterHandler("pagenamee",         PageNamee);
            RegisterHandler("fullpagename",      FullPageName);
            RegisterHandler("fullpagenamee",     FullPageNamee);
            RegisterHandler("basepagename",      BasePageName);
            RegisterHandler("basepagenamee",     BasePageNamee);
            RegisterHandler("subpagename",       SubPageName);
            RegisterHandler("subpagenamee",      SubPageNamee);
            RegisterHandler("talkpagename",      TalkPageName);
            RegisterHandler("talkpagenamee",     TalkPageNamee);
            RegisterHandler("subjectpagename",   SubjectPageName);
            RegisterHandler("subjectpagenamee",  SubjectPageNamee);
            RegisterHandler("#formatdate",       FormatDate);
        }

        private Result BogusNumber(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "1";
            return Result.Found;
        }

        /// <summary>
        /// Resolves the namespace denormalized given the namespace-key.
        /// </summary>
        /// <example>
        /// ns:-1 -> Special
        /// ns:3 -> User talk
        /// </example>
        /// <param name="args">The argument. Must be exactly one integer.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        protected Result Ns(List<KeyValuePair<string, string>> args, out string output)
        {
            output = string.Empty;
            if (args != null && args.Count > 0)
            {
                int key;
                if (int.TryParse(args[0].Value, out key))
                {
                    output = wikiSite_.GetNamespaceName(key);
                    output = Title.Denormalize(output);
                }
            }

            return Result.Found;
        }

        /// <summary>
        /// Resolves the namespace normalized given the namespace-key.
        /// </summary>
        /// <example>
        /// ns:-1 -> Special
        /// ns:3 -> User_talk
        /// </example>
        /// <param name="args">The argument. Must be exactly one integer.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        protected Result Nse(List<KeyValuePair<string, string>> args, out string output)
        {
            Ns(args, out output);
            output = Title.Normalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Resolves the namespace denormalized given the namespace-key.
        /// </summary>
        /// <param name="args">The argument. Must be exactly one integer.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        protected Result Namespace(List<KeyValuePair<string, string>> args, out string output)
        {
            output = string.Empty;
            if (args != null && args.Count > 0)
            {
                // TODO: Support any page.
            }
            else
            {
                output = nameSpace_;
            }

            output = Title.Denormalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Resolves the namespace normalized given the namespace-key.
        /// </summary>
        /// <param name="args">The argument. Must be exactly one integer.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        protected Result Namespacee(List<KeyValuePair<string, string>> args, out string output)
        {
            Namespace(args, out output);
            output = Title.Normalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Resolves the talkspace denormalized given the namespace-key.
        /// </summary>
        /// <param name="args">The argument. Must be exactly one integer.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        protected Result Talkspace(List<KeyValuePair<string, string>> args, out string output)
        {
            Namespace(args, out output);
            output = Title.Denormalize(output + " talk");
            return Result.Found;
        }

        /// <summary>
        /// Resolves the namespace normalized given the namespace-key.
        /// </summary>
        /// <param name="args">The argument. Must be exactly one integer.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        protected Result Talkspacee(List<KeyValuePair<string, string>> args, out string output)
        {
            Namespace(args, out output);
            output = Title.Normalize(output + " talk");
            return Result.Found;
        }

        /// <summary>
        /// Resolves the talkspace denormalized given the namespace-key.
        /// </summary>
        /// <param name="args">The argument. Must be exactly one integer.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        protected Result Subjectspace(List<KeyValuePair<string, string>> args, out string output)
        {
            Namespace(args, out output);
            return Result.Found;
        }

        /// <summary>
        /// Resolves the namespace normalized given the namespace-key.
        /// </summary>
        /// <param name="args">The argument. Must be exactly one integer.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        protected Result Subjectspacee(List<KeyValuePair<string, string>> args, out string output)
        {
            Namespacee(args, out output);
            return Result.Found;
        }

        /// <summary>
        /// Returns the page title denormalized.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result PageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = Title.Denormalize(pageTitle_);
            return Result.Found;
        }

        /// <summary>
        /// Returns the page title normalized.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Magic_words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result PageNamee(List<KeyValuePair<string, string>> args, out string output)
        {
            output = Title.Normalize(pageTitle_);
            return Result.Found;
        }

        /// <summary>
        /// Returns the full page title denormalized, including namespace.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Help:Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result FullPageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = Title.FullPageName(nameSpace_, pageTitle_);
            output = Title.Denormalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns the full page title normalized, including namespace.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Help:Magic_words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result FullPageNamee(List<KeyValuePair<string, string>> args, out string output)
        {
            FullPageName(args, out output);
            output = Title.Normalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns the page title denormalized excluding the current subpage and
        /// namespace ("Title/foo" on "Title/foo/bar").
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Magic words.
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
            output = Title.Denormalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns the page title normalized excluding the current subpage and
        /// namespace ("Title/foo" on "Title/foo/bar").
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Magic_words.
        /// </example>
        /// <remarks>
        /// For more complex splitting, use {{#titleparts:}} from ParserFunctions extension.
        /// </remarks>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result BasePageNamee(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            output = Title.Normalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns the subpage title denormalized ("foo" on "Title/foo").
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result SubPageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            output = Title.Denormalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns the subpage title normalized ("foo" on "Title/foo").
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Magic_words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result SubPageNamee(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            output = Title.Normalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns namespace and title denormalized of the associated talk page.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Help talk:Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result TalkPageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            output = Title.Denormalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns namespace and title normalized of the associated talk page.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Help talk:Magic_words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result TalkPageNamee(List<KeyValuePair<string, string>> args, out string output)
        {
            output = pageTitle_;
            output = Title.Normalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns the namespace and title denormalized of the associated content page.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Help:Magic words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result SubjectPageName(List<KeyValuePair<string, string>> args, out string output)
        {
            output = Title.FullPageName(nameSpace_, pageTitle_);
            output = Title.Denormalize(output);
            return Result.Found;
        }

        /// <summary>
        /// Returns the namespace and title normalized of the associated content page.
        /// </summary>
        /// <example>
        /// http://www.mediawiki.org/wiki/Help:Magic_words -> Help:Magic_words.
        /// </example>
        /// <param name="args">The arguments, if any.</param>
        /// <param name="output">The output text.</param>
        /// <returns>A Result type.</returns>
        private Result SubjectPageNamee(List<KeyValuePair<string, string>> args, out string output)
        {
            SubjectPageName(args, out output);
            output = Title.Normalize(output);
            return Result.Found;
        }

        private Result FormatDate(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "<strong style=\"color: red;\">formatdate</strong>";
            return Result.Found;
        }

        private static Result UrlEncode(List<KeyValuePair<string, string>> args, out string output)
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
            output = "<strong style=\"color: red;\">localurl</strong>";
            return Result.Found;
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
            if (args.Count == 0)
            {
                output = "<strong style=\"color: red;\">Error in fullurl!</strong>";
                return Result.Found;
            }

            string pageName = args[0].Value;
//             int indexOfInterwiki = pageName.IndexOf(':');
//             string interWiki = string.Empty;
//             if (indexOfInterwiki >= 0)
//             {
//                 interWiki = pageName.Substring(0, indexOfInterwiki);
//                 pageName = pageName.Substring(indexOfInterwiki + 1);
//             }

            //TODO: Process interwiki.

            if (args.Count > 1)
            {
                // With query.
                output = string.Format("{0}&{1}={2}", wikiSite_.GetFullUrl(pageName), args[1].Key, args[1].Value);
            }
            else
            {
                output = wikiSite_.GetViewUrl(pageName);
            }

            return Result.Found;
        }

        private static Result AnchorEncode(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args != null && args.Count > 0)
            {
                string arg = args[0].Value;
                if (!string.IsNullOrEmpty(arg))
                {
                    arg = arg.Trim(WhiteSpaceChars);
                    output = Title.NormalizeAnchor(arg);
                    return Result.Found;
                }
            }

            output = string.Empty;
            return Result.Found;
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

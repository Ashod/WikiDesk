
namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    using ScrewTurn.Wiki;
    using ScrewTurn.Wiki.PluginFramework;

    public class Wiki2Html
    {
        #region construction

        public Wiki2Html()
            : this("http://en.wikipedia.org/wiki/")
        {
        }

        public Wiki2Html(string baseUrl)
        {
            baseUrl = baseUrl.ToLowerInvariant();
            if (!baseUrl.StartsWith("http://") &&
                !baseUrl.StartsWith("https://"))
            {
                throw new ArgumentException("Invalid base URL.");
            }

            BaseUrl = baseUrl;
            FileUrl = BaseUrl + "File:";
            ThumbnailWidth = 200;
        }

        #endregion // construction

        #region properties

        public string BaseUrl { get; set; }

        public string FileUrl { get; set; }

        public int ThumbnailWidth { get; set; }

        #endregion // properties

        public string ConvertX(string wikicode)
        {
            wikicode = ConvertUnaryCode(RedirectRegex, Redirect, wikicode);

            wikicode = ConvertBinaryCode(ImageRegex, Image, wikicode);
//             wikicode = ConvertBinaryCode(LinkRegex, Link, wikicode);

            wikicode = ConvertBinaryCode(BoldItalicRegex, BoldItalic, wikicode);
            wikicode = ConvertBinaryCode(BoldRegex, Bold, wikicode);
            wikicode = ConvertBinaryCode(ItalicRegex, Italic, wikicode);

            wikicode = ConvertBinaryCode(H6Regex, H6, wikicode);
            wikicode = ConvertBinaryCode(H5Regex, H5, wikicode);
            wikicode = ConvertBinaryCode(H4Regex, H4, wikicode);
            wikicode = ConvertBinaryCode(H3Regex, H3, wikicode);
            wikicode = ConvertBinaryCode(H2Regex, H2, wikicode);
            wikicode = ConvertBinaryCode(H1Regex, H1, wikicode);

            return wikicode;
        }

        private string Redirect(Match match)
        {
            string value = match.Groups[1].Value;
            return string.Concat("<a href=\"", BaseUrl, value, "\" title=\"", value, "\">", value, "</a>");
        }

        private delegate string MatchedCodeHandler(Match match);

        private static string ConvertUnaryCode(Regex regex, MatchedCodeHandler handler, string wikicode)
        {
            Match match = regex.Match(wikicode);
            while (match.Success)
            {
                string text = handler(match);

                StringBuilder sb = new StringBuilder(wikicode.Length);
                sb.Append(wikicode.Substring(0, match.Index));
                sb.Append(text);
                sb.Append(wikicode.Substring(match.Index + match.Length));
                wikicode = sb.ToString();

                match = regex.Match(wikicode, match.Index);
            }

            return wikicode;
        }

        private static string ConvertBinaryCode(Regex regex, MatchedCodeHandler handler, string wikicode)
        {
            Match match = regex.Match(wikicode);
            while (match.Success)
            {
                string text = handler(match);

                StringBuilder sb = new StringBuilder(wikicode.Length);
                sb.Append(wikicode.Substring(0, match.Index));
                sb.Append(text);
                sb.Append(wikicode.Substring(match.Index + match.Length));
                wikicode = sb.ToString();

                match = regex.Match(wikicode, match.Index);
            }

            return wikicode;
        }

        private static string H1(Match match)
        {
            string value = match.Value.Substring(1, match.Length - 2);
            return string.Concat("<h1><span class=\"mw-headline\">", value, "</span></h1>");
        }

        private static string H2(Match match)
        {
            string value = match.Value.Substring(2, match.Length - 4);
            return string.Concat("<h2><span class=\"mw-headline\">", value, "</span></h2>");
        }

        private static string H3(Match match)
        {
            string value = match.Value.Substring(3, match.Length - 6);
            return string.Concat("<h3><span class=\"mw-headline\">", value, "</span></h3>");
        }

        private static string H4(Match match)
        {
            string value = match.Value.Substring(4, match.Length - 8);
            return string.Concat("<h4><span class=\"mw-headline\">", value, "</span></h4>");
        }

        private static string H5(Match match)
        {
            string value = match.Value.Substring(5, match.Length - 10);
            return string.Concat("<h5><span class=\"mw-headline\">", value, "</span></h5>");
        }

        private static string H6(Match match)
        {
            string value = match.Value.Substring(6, match.Length - 12);
            return string.Concat("<h6><span class=\"mw-headline\">", value, "</span></h6>");
        }

        private static string Bold(Match match)
        {
            string value = match.Value.Substring(3, match.Length - 6);
            return string.Concat("<b>", value, "</b>");
        }

        private static string Italic(Match match)
        {
            string value = match.Value.Substring(2, match.Length - 4);
            return string.Concat("<i>", value, "</i>");
        }

        private static string BoldItalic(Match match)
        {
            string value = match.Value.Substring(5, match.Length - 10);
            return string.Concat("<i><b>", value, "</b></i>");
        }

        private string Image(Match match)
        {
            string imageFileName = match.Groups[2].Value;
            string url = FileUrl + HttpUtility.UrlEncode(imageFileName);
            string imagePage = Download.DownloadPage(url);
            Match imageSourceMatch = ImageSourceRegex.Match(imagePage);
            if (!imageSourceMatch.Success ||
                (imageSourceMatch.Groups[1].Value != imageFileName))
            {
                return string.Empty;
            }

            // Thumb/Frame
            if (match.Groups[3].Success)
            {

            }

            string imageUrl = imageSourceMatch.Groups[2].Value;

            return string.Concat("<img alt=\"", imageFileName, "\" src=\"", imageUrl, "\"></a>");
        }

        private static string Link(Match match)
        {
            string baseUrl = "http://en.wikipedia.org/wiki/";
            string value = match.Groups[1].Value;
            string text = match.Groups[2].Value;
            return string.Concat("<a href=\"", baseUrl, value, "\" title=\"", value, "\">", text, "</a>");
        }

        private void SplitNoWiki(string wikicode)
        {

            Match match = NoWikiRegex.Match(wikicode);
            while (match.Success)
            {
//                 noWikiBegin.Add(match.Index);
//                 noWikiEnd.Add(match.Index + match.Length);
//                 match = NoWikiRegex.Match(text, match.Index + match.Length);
            }
        }

//         /// <summary>
//         /// Computes the positions of all NOWIKI tags.
//         /// </summary>
//         /// <param name="text">The input text.</param>
//         /// <param name="noWikiBegin">The output list of begin indexes of NOWIKI tags.</param>
//         /// <param name="noWikiEnd">The output list of end indexes of NOWIKI tags.</param>
//         private static void ComputeNoWiki(string text, ref List<int> noWikiBegin, ref List<int> noWikiEnd)
//         {
//             Match match;
//             noWikiBegin.Clear();
//             noWikiEnd.Clear();
//
//             match = NoWikiRegex.Match(text);
//             while (match.Success)
//             {
//                 noWikiBegin.Add(match.Index);
//                 noWikiEnd.Add(match.Index + match.Length);
//                 match = NoWikiRegex.Match(text, match.Index + match.Length);
//             }
//         }

        /// <summary>
        /// Extracts all the alternative articles in other languages.
        /// These are typically listed at the end of the wiki text.
        /// </summary>
        /// <param name="wikiText">The wiki-text to parse and strip.</param>
        /// <returns>A dictionary of language-code and the title of the article.</returns>
        public static Dictionary<string, string> ExtractLanguages(ref string wikiText)
        {
            List<string> lines = new List<string>(10240);
            using (StringReader sr = new StringReader(wikiText))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    lines.Add(line);
                }
            }

            Dictionary<string, string> languages = new Dictionary<string, string>(32);

            int lastLineIndx = lines.Count - 1;
            for (int i = lines.Count - 1; i >= 0; --i)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                if (line.StartsWith("[[") && line.EndsWith("]]"))
                {
                    lastLineIndx = i;
                    int split = line.IndexOf(':');
                    if (split >= 0)
                    {
                        string langCode = line.Substring(2, split - 2);
                        string langTitle = line.Substring(split + 1, line.Length - split - 2 - 1);
                        languages[langCode] = langTitle; //TODO: Check for duplicates
                    }
                }
            }

            StringBuilder sb = new StringBuilder(wikiText.Length);
            for (int i = 0; i < lastLineIndx; ++i)
            {
                sb.AppendLine(lines[i]);
            }

            wikiText = sb.ToString();
            return languages;
        }

        public static string Convert(string wikiText)
        {
            FormattingContext context = FormattingContext.PageContent;
            PageInfo currentPage = null;
            string[] linkedPages = null;
            string output = Formatter.Format(wikiText, false, context, currentPage, out linkedPages, false);
            return output;
        }

        #region representation

        #endregion // representation

        #region Regex

        //
        // Unary Operators
        //
        private static readonly Regex RedirectRegex = new Regex(@"^#REDIRECT \[\[(.+?)\]\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline);

        //
        // These can appear only at the start of a line.
        //
        private static readonly Regex H1Regex = new Regex(@"^=.+?=", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H2Regex = new Regex(@"^==.+?==", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H3Regex = new Regex(@"^===.+?===", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H4Regex = new Regex(@"^====.+?====", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H5Regex = new Regex(@"^=====.+?=====", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H6Regex = new Regex(@"^=====.+?=====", RegexOptions.Compiled | RegexOptions.Multiline);
        //
        // These can appear anywhere in a line.
        //
        private static readonly Regex ItalicRegex = new Regex(@"''.+?''", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex BoldRegex = new Regex(@"'''.+?'''", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex BoldItalicRegex = new Regex(@"'''''.+?'''''", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex LinkRegex = new Regex(@"\[\[(.+?)\|(.+?)\]\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex ImageRegex = new Regex(
                                    @"\[\[(Image|File)\:(.+?)" +
                                    @"(\|(thumb|thumbnail|frame|frameless))?" +
                                    @"(\|border)?" +
                                    @"(\|(right|left|center|none))?" +
                                    //@"\|alt=()?\|" +
                                    @"(\|.+?)?\]\]", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex ImageSourceRegex = new Regex("<img alt=\"File:(.+?)\" src=\"(.+?)\"");

        private static readonly Regex NoWikiRegex = new Regex(@"\<nowiki\>(.|\n|\r)+?\<\/nowiki\>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        #endregion // Regex
    }
}

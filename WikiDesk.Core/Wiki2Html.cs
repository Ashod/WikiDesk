
namespace WikiDesk.Core
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    using ScrewTurn.Wiki;
    using ScrewTurn.Wiki.PluginFramework;

    public class Wiki2Html
    {
        /// <summary>
        /// This delegate is used to resolve wiki links.
        /// This is typically used to decide if the link is internal or
        /// references another wiki.
        /// </summary>
        /// <param name="title">The title of the page to link to.</param>
        /// <param name="lanugageCode">The code if the target wiki language.</param>
        /// <returns>A valid full or relative URL.</returns>
        public delegate string ResolveWikiLink(string title, string lanugageCode);

        #region construction

        public Wiki2Html()
            : this(new Configuration())
        {
        }

        public Wiki2Html(Configuration config)
            : this(config, null, null)
        {
        }

        public Wiki2Html(Configuration config,
                         ResolveWikiLink resolveWikiLinkDel,
                         IFileCache fileCache)
        {
            config_ = config;
            resolveWikiLinkDel_ = resolveWikiLinkDel;
            fileCache_ = fileCache;

            currentFolder_ = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            commonImagesPath_ = Path.Combine(currentFolder_ ?? string.Empty, @"skins\common\images");
            commonImagesPath_ = commonImagesPath_.Replace('\\', '/');
            commonImagesPath_ = "file:///" + HttpUtility.HtmlEncode(commonImagesPath_);
            commonImagesPath_ = commonImagesPath_.TrimEnd('/') + '/';
        }

        #endregion // construction

        #region properties

        private string FullUrl
        {
            get { return config_.FullUrl; }
        }

        private string FileUrl
        {
            get { return config_.FileUrl; }
        }

        private int ThumbnailWidthPixels
        {
            get { return config_.ThumbnailWidthPixels; }
        }

        #endregion // properties

        public string ConvertX(string wikicode)
        {
            wikicode = ConvertUnaryCode(RedirectRegex, Redirect, wikicode);
            wikicode = ConvertListCode(wikicode);

            wikicode = ConvertBinaryCode(BoldItalicRegex, BoldItalic, wikicode);
            wikicode = ConvertBinaryCode(BoldRegex, Bold, wikicode);
            wikicode = ConvertBinaryCode(ItalicRegex, Italic, wikicode);

            wikicode = ConvertBinaryCode(H6Regex, H6, wikicode);
            wikicode = ConvertBinaryCode(H5Regex, H5, wikicode);
            wikicode = ConvertBinaryCode(H4Regex, H4, wikicode);
            wikicode = ConvertBinaryCode(H3Regex, H3, wikicode);
            wikicode = ConvertBinaryCode(H2Regex, H2, wikicode);
            wikicode = ConvertBinaryCode(H1Regex, H1, wikicode);

            wikicode = ConvertBinaryCode(TemplateRegex, Template, wikicode);

            wikicode = ConvertBinaryCode(WikiLinkRegex, WikiLink, wikicode);
            wikicode = ConvertBinaryCode(ImageRegex, Image, wikicode);
            wikicode = ConvertBinaryCode(ExtLinkRegex, ExtLink, wikicode);

            return wikicode;
        }

        private string Redirect(Match match)
        {
            string value = match.Groups[1].Value;

            //TODO: Consider language codes.
            string url = ResolveLink(value, config_.CurrentLanguageCode);
            return string.Concat("<a href=\"", url, "\" title=\"", value, "\">", value, "</a>");
        }

        private string List(Match match)
        {
            string value = match.Groups[1].Value;

            string url = ResolveLink(value, config_.CurrentLanguageCode);
            return string.Concat("<a href=\"", url, "\" title=\"", value, "\">", value, "</a>");
        }

        /// <summary>
        /// Handles conversion of a matching regex into HTML.
        /// May return null to skip conversion of the matching block.
        /// </summary>
        /// <param name="match">The regex match instance.</param>
        /// <returns>A replacement string, or null to skip.</returns>
        private delegate string MatchedCodeHandler(Match match);

        private static string ConvertListCode(string wikicode)
        {
            Match match = ListRegex.Match(wikicode);
            if (match.Success)
            {
                StringBuilder sb = new StringBuilder(wikicode.Length);
                sb.Append(wikicode.Substring(0, match.Index));
                sb.Append("<ul>");
                int index;
                int pos;

                do
                {
                    index = match.Index;
                    pos = index + match.Length;

                    sb.Append("<li>");
                    sb.Append(match.Groups[1].Value);
                    sb.Append("</li>");

                    match = ListRegex.Match(wikicode, pos);
                }
                while (match.Success);

                sb.Append("</ul>");
                sb.Append(wikicode.Substring(pos));
                wikicode = sb.ToString();
            }

            return wikicode;
        }

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

                //TODO: Optimize.
                match = regex.Match(wikicode, match.Index);
            }

            return wikicode;
        }

        private static string ConvertBinaryCode(Regex regex, MatchedCodeHandler handler, string wikicode)
        {
            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(wikicode.Length * 2);

            Match match = regex.Match(wikicode);
            while (match.Success && (lastIndex < wikicode.Length))
            {
                // Copy the skipped part.
                sb.Append(wikicode.Substring(lastIndex, match.Index - lastIndex));

                // Handle the match.
                string text = handler(match);

                // Either copy a replacement or the matched part as-is.
                sb.Append(text ?? match.Value);

                lastIndex = match.Index + match.Length;

                match = match.NextMatch();
            }

            // Copy the remaining bit.
            if (lastIndex == 0)
            {
                // There were no matches.
                Debug.Assert(sb.Length == 0, "Expected no matches.");
                return wikicode;
            }

            sb.Append(wikicode.Substring(lastIndex));
            return sb.ToString();
        }

        #region Headers

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

        #endregion // Headers

        #region Bold/Italic

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

        #endregion // Bold/Italic

        private string Image(Match match)
        {
            string imageFileName = match.Groups[2].Value;

            string imagePageUrl = FileUrl + HttpUtility.UrlEncode(imageFileName.Replace(' ', '_'));
            string imageSrcUrl = null;

            if (fileCache_ != null)
            {
                if (fileCache_.IsSourceCached(imageFileName, config_.CurrentLanguageCode))
                {
                    imageSrcUrl = fileCache_.ResolveSourceUrl(imageFileName, config_.CurrentLanguageCode);
                }
            }

            if (string.IsNullOrEmpty(imageSrcUrl))
            {
                string imagePage = Download.DownloadPage(imagePageUrl);
                Match imageSourceMatch = ImageSourceRegex.Match(imagePage);
                if (!imageSourceMatch.Success ||
                    (HttpUtility.HtmlDecode(imageSourceMatch.Groups[1].Value) != imageFileName))
                {
                    // File not found?
                    return string.Empty;
                }

                imageSrcUrl = imageSourceMatch.Groups[2].Value;

                if (fileCache_ != null)
                {
                    fileCache_.CacheMedia(imageFileName, config_.CurrentLanguageCode, imageSrcUrl);
                }
            }

            int width = -1;
            int height = -1;

            string options = match.Groups[3].Value;
            if (string.IsNullOrEmpty(options))
            {
                // No options - return default.
                return WikiImage2Html.Convert(
                                        imagePageUrl,
                                        imageSrcUrl,
                                        null,
                                        false,
                                        null,
                                        -1,
                                        -1,
                                        imageFileName,
                                        null,
                                        commonImagesPath_);
            }

            // Remove the first pipe to avoid an emtpy first token.
            options = options.TrimStart('|');

            WikiImage2Html.Type? type = null;
            WikiImage2Html.Location? location = null;
            bool haveBorder = false;
            string altText = null;
            string caption = null;

            foreach (string token in options.Split('|'))
            {
                string optionName = token.Trim().ToUpperInvariant();
                switch (optionName)
                {
                    // Thumbnail size. ThumbCaption. Magnify on caption.
                    case "THUMB":
                    case "THUMBNAIL":
                        type = WikiImage2Html.Type.Thumbnail;
                        width = (width < 0) ? config_.ThumbnailWidthPixels : width;
                        break;

                    // Full size. ThumbCaption.
                    case "FRAME":
                    case "FRAMED":
                        type = WikiImage2Html.Type.Framed;
                        break;

                    // Thumbnail size.
                    case "FRAMELESS":
                        type = WikiImage2Html.Type.Frameless;
                        width = (width < 0) ? config_.ThumbnailWidthPixels : width;
                        break;

                    case "BORDER":
                        haveBorder = true;
                        break;

                    case "RIGHT":
                        location = WikiImage2Html.Location.Right;
                        break;
                    case "LEFT":
                        location = WikiImage2Html.Location.Left;
                        break;
                    case "CENTER":
                        location = WikiImage2Html.Location.Center;
                        break;
                    case "NONE":
                        location = WikiImage2Html.Location.None;
                        break;

                    default:
                        if (optionName.StartsWith("ALT="))
                        {
                            altText = token.Substring(4);
                        }
                        else
                        if (optionName.StartsWith("UPRIGHT"))
                        {
                            string value = optionName.Substring("UPRIGHT".Length);
                            double factor = 0.75;
                            if (value.Length > 0)
                            {
                                value = value.Trim('=');
                                if (!double.TryParse(value, out factor))
                                {
                                    factor = 0.75;
                                }
                            }

                            // Round to the nearest 10 pixels.
                            int r = (int)(config_.ThumbnailWidthPixels * factor / 10 + 0.5);
                            width = r * 10;
                        }
                        else
                        if (optionName.EndsWith("PX"))
                        {
                            // Remove the PX suffix.
                            optionName = optionName.Substring(0, optionName.Length - 2);

                            // Width.
                            if (!optionName.StartsWith("X"))
                            {
                                string value = optionName;
                                int indexOfX = value.IndexOf("X");
                                if (indexOfX >= 0)
                                {
                                    value = value.Substring(0, indexOfX);
                                    optionName = optionName.Substring(indexOfX);
                                }

                                int w;
                                if (int.TryParse(value, out w))
                                {
                                    width = w;
                                }
                            }

                            // Height.
                            if (optionName.StartsWith("X"))
                            {
                                string value = optionName.TrimStart('X');

                                int h;
                                if (int.TryParse(value, out h))
                                {
                                    height = h;
                                }
                            }
                        }
                        else
                        {
                            // Caption.
                            caption = token;
                        }

                        break;
                }
            }

            if (altText == null)
            {
                if (caption != null)
                {
                    altText = caption;
                }
                else
                {
                    altText = imageFileName;
                }
            }

            return WikiImage2Html.Convert(
                                    imagePageUrl,
                                    imageSrcUrl,
                                    type,
                                    haveBorder,
                                    location,
                                    width,
                                    height,
                                    altText,
                                    caption,
                                    commonImagesPath_);
        }

        private string WikiLink(Match match)
        {
            string pageName = match.Groups[2].Value;

            // Embedded links need special treatment, skip them.)
            if (pageName.StartsWith("Image:") || pageName.StartsWith("File:"))
            {
                return null;
            }

            string text = match.Groups[4].Value;
            if (string.IsNullOrEmpty(text))
            {
                text = pageName;
            }

            string url = ResolveLink(pageName, config_.CurrentLanguageCode);
            return string.Concat("<a href=\"", url, "\" title=\"", pageName, "\" class=\"mw-redirect\">", text, "</a>");
        }

        private static string ExtLink(Match match)
        {
            string url = match.Groups[1].Value;

            string text = match.Groups[2].Value;
            if (string.IsNullOrEmpty(text))
            {
                text = url;
            }

            return string.Concat("<a href=\"", url, "\" title=\"", url, "\">", text, "</a>");
        }

        private string Template(Match match)
        {
            string name = match.Groups[1].Value;
            string nameUpper = name.ToUpperInvariant();
            string options = match.Groups[2].Value.Trim('|');

            if (nameUpper.StartsWith("LANG-"))
            {
                string lang = name.Substring("LANG-".Length);
                StringBuilder sb = new StringBuilder(128);
                sb.Append("<span lang=\"");
                sb.Append(lang);
                sb.Append("\" xml:lang=\"");
                sb.Append(lang);
                sb.Append("\">");
                sb.Append(options);
                sb.Append("</span>");
                return sb.ToString();
            }

            if (nameUpper == "MAIN")
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append("<div class=\"rellink relarticle mainarticle\">Main article: <a href=\"");
                string url = ResolveLink(options, config_.CurrentLanguageCode);
                sb.Append(url);
                sb.Append("\" title=\"");
                sb.Append(options);
                sb.Append("\">");
                sb.Append(options);
                sb.Append("</a></div>");
                return sb.ToString();
            }

            return null;
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

        #region implementation

        private string ResolveLink(string title, string languageCode)
        {
            if (resolveWikiLinkDel_ != null)
            {
                return resolveWikiLinkDel_(title, languageCode);
            }

            return FullUrl + title.Replace(' ', '_');
        }

        #endregion // implementation

        #region representation

        #endregion // representation

        private readonly Configuration config_;

        private readonly ResolveWikiLink resolveWikiLinkDel_;

        private readonly IFileCache fileCache_;

        #region Regex

        //
        // Unary Operators
        //
        private static readonly Regex RedirectRegex = new Regex(@"^#REDIRECT \[\[(.+?)\]\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline);
        private static readonly Regex ListRegex = new Regex(@"^\* (.+?)$", RegexOptions.Compiled | RegexOptions.Multiline);

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

        /// <summary>
        /// Wiki link regex: excludes images.
        /// </summary>
        private static readonly Regex WikiLinkRegex = new Regex(@"\[\[((?!Image\:)(?!File\:))(.+?)(\|(.+?))?\]\]", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// External links.
        /// </summary>
        private static readonly Regex ExtLinkRegex = new Regex(@"\[(.+?)\s+(.+?)\]", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Wiki image regex: matches all options as one group.
        /// </summary>
        private static readonly Regex ImageRegex = new Regex(
                                    @"\[\[(Image|File)\:(.+?)" +
                                    @"((\|.+?)*)\]\]", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex ImageSourceRegex = new Regex("<img alt=\"File:(.+?)\" src=\"(.+?)\"");

        private static readonly Regex NoWikiRegex = new Regex(@"\<nowiki\>(.|\n|\r)+?\<\/nowiki\>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private static readonly Regex TemplateRegex = new Regex(@"\{\{(.+?)(\|(.+?))?\}\}", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly string currentFolder_;

        private readonly string commonImagesPath_;

        #endregion // Regex
    }
}

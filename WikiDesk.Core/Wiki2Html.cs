﻿
namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    using Tracy;

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

        /// <summary>
        /// This delegate is used to resolve a magic word.
        /// This is typically used expand a template.
        /// The return value is recursively resolved.
        /// </summary>
        /// <param name="word">The magic word to resolve.</param>
        /// <param name="lanugageCode">The code if the target wiki language.</param>
        /// <returns>A valid full or relative URL.</returns>
        public delegate string ResolveTemplate(string word, string lanugageCode);

        #region construction

        public Wiki2Html(Configuration config)
            : this(config, null, null, null)
        {
        }

        public Wiki2Html(Configuration config,
                         ResolveWikiLink resolveWikiLinkDel,
                         ResolveTemplate resolveWikiTemplateDel,
                         IFileCache fileCache)
        {
            config_ = config;
            resolveWikiLinkDel_ = resolveWikiLinkDel;
            resolveWikiTemplateDel_ = resolveWikiTemplateDel;
            fileCache_ = fileCache;

            commonImagesPath_ = Path.Combine(config.SkinsPath, config.CommonImagesPath);
            commonImagesPath_ = commonImagesPath_.Replace('\\', '/');
            commonImagesPath_ = "file:///" + commonImagesPath_;
            commonImagesPath_ = commonImagesPath_.TrimEnd('/') + '/';

            magicWordProcessor_ = new MagicWordProcessor();
            parserFunctionsProcessor_ = new ParserFunctionProcessor();

            logger_ = LogManager.CreateLoger(typeof(Wiki2Html).FullName);
        }

        #endregion // construction

        #region properties

        private int ThumbnailWidthPixels
        {
            get { return config_.ThumbnailWidthPixels; }
        }

        #endregion // properties

        public string Convert(string nameSpace, string pageTitle, string wikiText)
        {
            logger_.Log(
                    Levels.Debug,
                    "Converting - NameSpace = {0}, PageTitle = {1}, WikiText = {2}.",
                    nameSpace,
                    pageTitle,
                    wikiText);

            nameSpace_ = nameSpace;
            pageTitle_ = pageTitle;

            // Process redirections first.
            string newTitle = Redirection(wikiText);
            if (newTitle != null)
            {
                logger_.Log(Levels.Info, "Redirection: " + newTitle);

                //TODO: Should download and process the new title.
                return Redirect(newTitle);
            }

            wikiText = ConvertListCode(wikiText);

            wikiText = ConvertBinaryCode(BoldItalicRegex, BoldItalic, wikiText);
            wikiText = ConvertBinaryCode(BoldRegex, Bold, wikiText);
            wikiText = ConvertBinaryCode(ItalicRegex, Italic, wikiText);

            wikiText = ConvertBinaryCode(H6Regex, H6, wikiText);
            wikiText = ConvertBinaryCode(H5Regex, H5, wikiText);
            wikiText = ConvertBinaryCode(H4Regex, H4, wikiText);
            wikiText = ConvertBinaryCode(H3Regex, H3, wikiText);
            wikiText = ConvertBinaryCode(H2Regex, H2, wikiText);
            wikiText = ConvertBinaryCode(H1Regex, H1, wikiText);

            wikiText = ProcessMagicWords(wikiText);

            wikiText = ConvertBinaryCode(WikiLinkRegex, WikiLink, wikiText);
            wikiText = ConvertBinaryCode(ImageRegex, Image, wikiText);
            wikiText = ConvertBinaryCode(ExtLinkRegex, ExtLink, wikiText);

            wikiText = ConvertParagraphs(wikiText);

            return wikiText;
        }

        private string Redirect(string newTitle)
        {
            //TODO: Consider language codes.
            string url = ResolveLink(newTitle, config_.WikiSite.Language.Code);

            StringBuilder sb = new StringBuilder(128);
            sb.Append("Redirected to <span class=\"redirectText\"><a href=\"");
            sb.Append(url);
            sb.Append("\" title=\"");
            sb.Append(newTitle);
            sb.Append("\">");
            sb.Append(newTitle);
            sb.Append("</a></span>");

            return sb.ToString();
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

                int pos = 0;
                string curDepth = match.Groups[1].Value;

                do
                {
                    string newDepth = match.Groups[1].Value;
                    if (newDepth.Length == 0)
                    {
                        break;
                    }

                    if (newDepth.Length > curDepth.Length)
                    {
                        sb.Append("<ul>");
                    }
                    else
                    if (newDepth.Length < curDepth.Length)
                    {
                        int close = curDepth.Length - newDepth.Length;
                        while (close-- > 0)
                        {
                            sb.Append("</ul>");
                        }
                    }

                    curDepth = newDepth;

                    sb.Append("<li>");
                    sb.Append(match.Groups[2].Value);
                    sb.Append("</li>");

                    pos = match.Index + match.Length;
                    match = ListRegex.Match(wikicode, pos);
                }
                while (match.Success);

                int depth = curDepth.Length;
                while (depth-- > 0)
                {
                    sb.Append("</ul>");
                }

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
            Match match = regex.Match(wikicode);
            if (!match.Success)
            {
                return wikicode;
            }

            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(wikicode.Length * 2);

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

        /// <summary>
        /// Checks if the wikiText is a redirection.
        /// If redirection is in place, it returns the new page title, otherwise null.
        /// </summary>
        /// <param name="wikicode">The wikiText to parse.</param>
        /// <returns>A new page title or null if no redirection.</returns>
        private static string Redirection(string wikicode)
        {
            Match match = RedirectRegex.Match(wikicode);
            if (match.Success)
            {
                return match.Groups[2].Value;
            }

            return null;
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

            string imagePageUrl = config_.WikiSite.GetFileUrl(imageFileName);
            string imageSrcUrl = null;

            if (fileCache_ != null)
            {
                if (fileCache_.IsSourceCached(imageFileName, config_.WikiSite.Language.Code))
                {
                    imageSrcUrl = fileCache_.ResolveSourceUrl(imageFileName, config_.WikiSite.Language.Code);
                }
            }

            if (string.IsNullOrEmpty(imageSrcUrl))
            {
                string imagePage = Download.DownloadPage(imagePageUrl);
                Match imageSourceMatch = ImageSourceRegex.Match(imagePage);
                if (!imageSourceMatch.Success ||
                    (imageSourceMatch.Groups[1].Value != imageFileName))
                {
                    // File not found?
                    return string.Empty;
                }

                imageSrcUrl = imageSourceMatch.Groups[2].Value;

                if (fileCache_ != null)
                {
                    fileCache_.CacheMedia(imageFileName, config_.WikiSite.Language.Code, imageSrcUrl);
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

            // Remove the first pipe to avoid an empty first token.
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
                        width = (width < 0) ? ThumbnailWidthPixels : width;
                        break;

                    // Full size. ThumbCaption.
                    case "FRAME":
                    case "FRAMED":
                        type = WikiImage2Html.Type.Framed;
                        break;

                    // Thumbnail size.
                    case "FRAMELESS":
                        type = WikiImage2Html.Type.Frameless;
                        width = (width < 0) ? ThumbnailWidthPixels : width;
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
                            int r = (int)(ThumbnailWidthPixels * factor / 10 + 0.5);
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
                altText = caption ?? imageFileName;
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
            string pageName = match.Groups[2].Value.Trim();
            if (pageName.Length == 0)
            {
                return match.Value;
            }

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

            string url = ResolveLink(pageName, config_.WikiSite.Language.Code);
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

        #region MagicWords, Functions and Templates

        private string ProcessMagicWords(string wikicode)
        {
            logger_.Log(Levels.Debug, "Processing Magic:{0}{1}", Environment.NewLine, wikicode);

            int endIndex;
            int startIndex = MagicParser.FindMagicBlock(wikicode, out endIndex);
            if (startIndex < 0)
            {
                return wikicode;
            }

            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(wikicode.Length * 16);

            while (startIndex >= 0 && lastIndex < wikicode.Length)
            {
                // Copy the skipped part.
                sb.Append(wikicode.Substring(lastIndex, startIndex - lastIndex));

                // Handle the match.
                string magic = wikicode.Substring(startIndex + 2, endIndex - startIndex - 4 + 1);
                string output;
                if (MagicWord(magic, out output) == VariableProcessor.Result.Found)
                {
                    // Recursively process.
                    output = ProcessMagicWords(output);
                }

                sb.Append(output);

                lastIndex = startIndex + (endIndex - startIndex + 1);
                startIndex = MagicParser.FindMagicBlock(wikicode, lastIndex, out endIndex);
            }

            sb.Append(wikicode.Substring(lastIndex));
            return sb.ToString();
        }

        private VariableProcessor.Result MagicWord(string magic, out string output)
        {
            logger_.Log(Levels.Debug, "Magic:{0}{1}", Environment.NewLine, magic);

            VariableProcessor.Result result = VariableProcessor.Result.Unknown;

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(magic, out args);
            if (string.IsNullOrEmpty(command))
            {
                logger_.Log(Levels.Debug, "Failed to find command and params in the magic: " + magic);
                output = string.Empty;
                return result;
            }

            logger_.Log(Levels.Debug, "Magic Variable: [{0}].", command);

            // If it's an explicit template, process now.
            if (command.TrimEnd(':') == config_.WikiSite.GetNamespace(WikiSite.Namespace.Tempalate))
            {
                if (args.Count > 0)
                {
                    command = args[0].Value;
                    args.RemoveAt(0);
                    return Template(command, args, out output);
                }
            }

            // Get the magic-word ID.
            string magicWordId = config_.WikiSite.MagicWords.FindId(command);
            if (!string.IsNullOrEmpty(magicWordId))
            {
                logger_.Log(Levels.Debug, "MagicWord ID for [{0}] is [{1}].", command, magicWordId);

                // Try processing.
                magicWordProcessor_.SetContext(config_.WikiSite, nameSpace_, pageTitle_);
                result = magicWordProcessor_.Execute(magicWordId, args, out output);
                if (result != VariableProcessor.Result.Unknown)
                {
                    logger_.Log(Levels.Debug, "MagicWord for [{0}] - {1}.", command, output);
                    return result;
                }
            }

            // Is it a parser function?
            result = parserFunctionsProcessor_.Execute(command, args, out output);
            if (result != VariableProcessor.Result.Unknown)
            {
                logger_.Log(Levels.Debug, "ParserFunction for command [{0}] - {1}.", command, output);
                return result;
            }

            // Assume it's a template.
            return Template(command, args, out output);
        }

        private VariableProcessor.Result Template(
                                            string name,
                                            List<KeyValuePair<string, string>> args,
                                            out string output)
        {
            if (resolveWikiTemplateDel_ == null)
            {
                output = string.Empty;
                return VariableProcessor.Result.Unknown;
            }

            logger_.Log(Levels.Debug, "Resolving template [{0}].", name);
            string template = resolveWikiTemplateDel_(name, config_.WikiSite.Language.Code);
            if (template == null)
            {
                logger_.Log(Levels.Debug, "Template [{0}] didn't resolve.", name);
                output = "~" + name + "~";
                return VariableProcessor.Result.Html;
            }

            logger_.Log(Levels.Debug, "Template for [{0}]:{1}{2}.", name, Environment.NewLine, template);

            // Redirection?
            string newName = Redirection(template);
            if (newName != null)
            {
                logger_.Log(Levels.Debug, "Template [{0}] redirects to [{1}].", template, newName);
                template = resolveWikiTemplateDel_(newName, config_.WikiSite.Language.Code);
                if (template == null)
                {
                    logger_.Log(Levels.Debug, "Template [{0}] didn't resolve.", newName);
                    output = "~" + name + "~";
                    return VariableProcessor.Result.Html;
                }

                newName = Redirection(template);
                if (newName != null)
                {
                    logger_.Log(Levels.Warn, "Template redirection loop on [{0}].", template);
                    output = "<b><em>Redirection Loop!</em></b>";
                    return VariableProcessor.Result.Html;
                }
            }

            template = RemoveComments(template);

            LogEntry logEntry = logger_.CreateEntry(
                                            Levels.Debug,
                                            "Processing template params for:{0}{1}",
                                            Environment.NewLine,
                                            template);
            if (args != null)
            {
                foreach (KeyValuePair<string, string> pair in args)
                {
                    if (pair.Key != null)
                    {
                        logEntry.Properties[pair.Key] = pair.Value ?? string.Empty;
                    }
                }
            }

            logger_.Log(logEntry);
            output = MagicParser.ProcessTemplateParams(template, args);
            return VariableProcessor.Result.Found;
        }

        #endregion // MagicWords, Functions and Templates

        private static string RemoveComments(string wikicode)
        {
            return StringUtils.RemoveBlocks(wikicode, "<!--", "-->");
        }

        private static string ConvertParagraphs(string wikicode)
        {
            StringBuilder sb = new StringBuilder(wikicode.Length * 2);
            using (StringReader sr = new StringReader(wikicode))
            {
                bool para = false;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 0 ||
                        line.StartsWith("<h") ||
                        line.StartsWith("<t") ||
                        line.StartsWith("<p") ||
                        line.StartsWith("<d") ||
                        line.StartsWith("<u") ||
                        line.StartsWith("<l"))
                    {
                        if (para)
                        {
                            sb.Append("</p>");
                            para = false;
                        }

                        sb.Append(line);
                    }
                    else
                    {
                        if (!para)
                        {
                            sb.Append("<p>");
                            para = true;
                        }

                        sb.Append(line);
                    }
                }

                if (para)
                {
                    sb.Append("</p>");
                }
            }

            return sb.ToString();
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
            List<string> lines = new List<string>(1024);
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
                        languages[langCode] = langTitle;
                    }
                }
            }

            if (lastLineIndx == 0)
            {
                // We found nothing. Probably it's a redirect page.
                return languages;
            }

            StringBuilder sb = new StringBuilder(wikiText.Length);
            for (int i = 0; i < lastLineIndx; ++i)
            {
                sb.AppendLine(lines[i]);
            }

            wikiText = sb.ToString();
            return languages;
        }

        #region implementation

        private string ResolveLink(string title, string languageCode)
        {
            if (resolveWikiLinkDel_ != null)
            {
                return resolveWikiLinkDel_(title, languageCode);
            }

            // Link to the online version.
            return config_.WikiSite.GetViewUrl(title);
        }

        #endregion // implementation

        #region representation

        private readonly Configuration config_;

        private readonly ResolveWikiLink resolveWikiLinkDel_;

        private readonly ResolveTemplate resolveWikiTemplateDel_;

        private readonly IFileCache fileCache_;

        private readonly string commonImagesPath_;

        private readonly MagicWordProcessor magicWordProcessor_;
        private readonly ParserFunctionProcessor parserFunctionsProcessor_;

        private readonly ILogger logger_;

        #endregion // representation

        #region Regex

        //
        // Unary Operators
        //
        private static readonly Regex RedirectRegex = new Regex(@"^#REDIRECT(\:?)\s+\[\[(.+?)\]\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline);
        private static readonly Regex ListRegex = new Regex(@"^(\*+)\s*(.+?)$", RegexOptions.Compiled | RegexOptions.Multiline);

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

        private static readonly Regex NoWikiRegex = new Regex(@"\<nowiki\>(.|\n|\r)+?\<\/nowiki\>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

//         private static readonly Regex MagicWordRegex = new Regex(@"(\{{2})(.+?)(?:\}{2,})", RegexOptions.Compiled | RegexOptions.Singleline);
//         private static readonly Regex ParserFunctionRegex = new Regex(@"((#)?(.+?))\:(.+?)", RegexOptions.Compiled | RegexOptions.Singleline);
//         private static readonly Regex TemplateRegex = new Regex(@"((Template\:)?(.+?))|((.+?)\|(.+?))", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex ImageSourceRegex = new Regex("<img alt=\"File:(.+?)\" src=\"(.+?)\"");

        private string nameSpace_;

        private string pageTitle_;

        #endregion // Regex
    }
}

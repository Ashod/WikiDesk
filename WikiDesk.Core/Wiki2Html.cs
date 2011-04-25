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
        private class TocEntry
        {
            public string Title = string.Empty;

            public string Link = string.Empty;

            public List<TocEntry> Children = new List<TocEntry>();
        }

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
        public delegate string RetrievePage(string word, string lanugageCode);

        #region construction

        public Wiki2Html(Configuration config)
            : this(config, null, null, null)
        {
        }

        public Wiki2Html(Configuration config,
                         ResolveWikiLink resolveWikiLinkDel,
                         RetrievePage retrievePageDel,
                         IFileCache fileCache)
        {
            config_ = config;
            resolveWikiLinkDel_ = resolveWikiLinkDel;
            retrievePageDel_ = retrievePageDel;
            fileCache_ = fileCache;

            commonImagesPath_ = Path.Combine(config.SkinsPath, config.CommonImagesPath);
            commonImagesPath_ = commonImagesPath_.Replace('\\', '/');
            commonImagesPath_ = "file:///" + commonImagesPath_;
            commonImagesPath_ = commonImagesPath_.TrimEnd('/') + '/';

            magicWordProcessor_ = new MagicWordProcessor(ProcessMagicWords);
            parserFunctionsProcessor_ = new ParserFunctionProcessor(ProcessMagicWords);

            logger_ = LogManager.CreateLoger(typeof(Wiki2Html).FullName);
        }

        #endregion // construction

        #region properties

        private int ThumbnailWidthPixels
        {
            get { return config_.ThumbnailWidthPixels; }
        }

        #endregion // properties

        #region operations

        public string Convert(ref string nameSpace, ref string pageTitle, string wikicode)
        {
            logger_.Log(
                    Levels.Debug,
                    "Converting - NameSpace = {0}, PageTitle = {1}, WikiText = {2}.",
                    nameSpace,
                    pageTitle,
                    wikicode);

            // Process redirections first.
            string newTitle = Redirection(wikicode);
            if (newTitle != null)
            {
                logger_.Log(Levels.Info, "Redirection: " + newTitle);

                if (config_.AutoRedirect && retrievePageDel_ != null)
                {
                    for (int i = 0; i < MAX_REDIRECTION_CHAIN_COUNT; ++i)
                    {
                        string text = retrievePageDel_(newTitle, config_.WikiSite.Language.Code);
                        if (text == null)
                        {
                            break;
                        }

                        pageTitle = newTitle;
                        newTitle = Redirection(text);
                        if (newTitle == null)
                        {
                            wikicode = text;
                            break;
                        }

                        logger_.Log(Levels.Info, "Redirection: " + newTitle);
                    }
                }

                if (!string.IsNullOrEmpty(newTitle))
                {
                    return Redirect(newTitle);
                }
            }

            nameSpace_ = nameSpace;
            pageTitle_ = pageTitle;

            wikicode = ConvertBinaryCode(wikicode, ConvertWikiCode, HeaderRegex, Header);

            return wikicode;
        }

        #endregion // operations

        private static string ProcessWikiCode(string wikicode)
        {
            if (string.IsNullOrEmpty(wikicode))
            {
                return string.Empty;
            }

            // Find the open tag.
            int startOffset = 0;
            int lastIndex;
            string tag = null;
            string attribs;
            string contents = StringUtils.ExtractTag(wikicode, ref startOffset, out lastIndex, ref tag, out attribs);
            if (string.IsNullOrEmpty(tag))
            {
                return ProcessWikiCode(wikicode);
            }

            StringBuilder sb = new StringBuilder(wikicode.Length * 16);

            int curWikiOffset = 0;
            while (lastIndex < wikicode.Length)
            {
                string wikiChunk = wikicode.Substring(curWikiOffset, startOffset - curWikiOffset);
                sb.Append(ProcessWikiCode(wikiChunk));
                contents = ProcessWikiCode(contents);
                sb.Append(contents);

                startOffset = lastIndex;
                curWikiOffset = lastIndex;
                tag = null;
                contents = StringUtils.ExtractTag(wikicode, ref startOffset, out lastIndex, ref tag, out attribs);
                if (string.IsNullOrEmpty(tag))
                {
                    if (lastIndex >= 0)
                    {
                        sb.Append(ProcessWikiCode(wikicode.Substring(lastIndex)));
                    }

                    break;
                }
            }

            return sb.ToString();
        }

        private string ConvertWikiCode(string wikicode)
        {
            wikicode = ProcessMagicWords(wikicode);

            wikicode = ConvertInlineCodes(wikicode);

            wikicode = ConvertListCode(wikicode);
            wikicode = ConvertTables(wikicode);
            wikicode = ConvertParagraphs(wikicode);
            return wikicode;
        }

        private string ConvertInlineCodes(string wikicode)
        {
            if (!string.IsNullOrEmpty(wikicode))
            {
                wikicode = ConvertBinaryCode(wikicode, x => x, BoldItalicRegex, BoldItalic);
                wikicode = ConvertBinaryCode(wikicode, x => x, '[', ']', Link);
            }

            return wikicode;
        }

        private string ProcessTag(string tag, string attribs, string contents)
        {
            // For now, just print out.
            if (contents == null)
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append("<").Append(tag).Append(" ").Append(attribs).Append(" />");
                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder(contents.Length * 2);
                sb.Append("<").Append(tag).Append(" ").Append(attribs).Append(">");
                sb.Append(contents);
                sb.Append("</").Append(tag).Append(">");
                return sb.ToString();
            }
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
        private delegate string MatchedRegexHandler(Match match);

        /// <summary>
        /// Handles conversion of a matching binary markers into HTML.
        /// May return null to skip conversion of the matching block.
        /// </summary>
        /// <param name="code">The code that is bound by the matched markers.</param>
        /// <returns>A replacement string, or null to skip.</returns>
        private delegate string MatchedCodeHandler(string code);

        /// <summary>
        /// Handles text that didn't match the current Regex.
        /// </summary>
        /// <param name="code">The code that didn't match.</param>
        /// <returns>A replacement string, or null to skip.</returns>
        private delegate string MismatchedCodeHandler(string code);

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

        private static string ConvertTables(string wikicode)
        {
            StringBuilder sb = new StringBuilder(wikicode.Length * 2);
            using (StringReader sr = new StringReader(wikicode))
            {
                string line;
                List<string> tableLines = new List<string>(32);
                bool table = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (table || line.Trim().StartsWith("{|"))
                    {
                        // This is the start of a table.
                        // Get all the table lines and pass to the generator.
                        tableLines.Add(line);
                        table = true;

                        if (line.Trim().StartsWith("|}"))
                        {
                            table = false;
                            ConvertTable(tableLines, sb);
                        }
                    }
                    else
                    {
                        sb.Append(line);
                    }
                }

                if (table)
                {
                    ConvertTable(tableLines, sb);
                }
            }

            return sb.ToString();
        }

        private static void ConvertTable(List<string> lines, StringBuilder sb)
        {
            string line = lines[0].Trim();
            if (line == "{|")
            {
                sb.AppendLine("<table>");
            }
            else
            {
                sb.Append("<table ");
                int indexOf = line.IndexOf("{|");
                if (indexOf < 0)
                {
                    // Invalid, and should never happen.
                    return;
                }

                sb.Append(line.Substring(indexOf + 2).Trim());
                sb.AppendLine(">");
            }

            int idx = 1;
            line = lines[idx].Trim();
            if (line.StartsWith("|+"))
            {
                sb.Append("<caption>");
                sb.Append(line.Substring(2).TrimStart());
                sb.AppendLine("</caption>");
                ++idx;
            }

            sb.AppendLine("<tbody>");

            bool row = false;
            for (; idx < lines.Count; ++idx)
            {
                line = lines[idx].Trim();
                if (line.StartsWith("|-"))
                {
                    if (row)
                    {
                        sb.AppendLine("</tr>");
                    }

                    sb.AppendLine("<tr>");
                    row = true;
                }
                else
                if (line.StartsWith("|}"))
                {
                    break;
                }
                else
                {
                    // Cell(s).
                    if (!row)
                    {
                        // Start a new row if it's missing.
                        sb.AppendLine("<tr>");
                        row = true;
                    }

                    line = line.Substring(1);
                    foreach (string cell in line.Split(new[]{ "||" }, StringSplitOptions.None))
                    {
                        string text;
                        string formatModifier = StringUtils.BreakAt(cell, '|', out text);
                        if (text == null)
                        {
                            text = formatModifier;
                            formatModifier = string.Empty;
                        }

                        if (formatModifier.IndexOf('=') < 0)
                        {
                            sb.Append("<td>");
                        }
                        else
                        {
                            sb.Append("<td ");
                            sb.Append(formatModifier.Trim());
                            sb.Append('>');
                        }

                        sb.Append(text.Trim());
                        sb.AppendLine("</td>");
                    }
                }
            }

            if (row)
            {
                sb.AppendLine("</tr>");
            }

            sb.Append("</tbody>");
            sb.AppendLine("</table>");
        }

        private static string ConvertUnaryCode(Regex regex, MatchedRegexHandler handler, string wikicode)
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

        private static string ConvertBinaryCode(
                                    string wikicode,
                                    MismatchedCodeHandler misHandler,
                                    Regex regex,
                                    MatchedRegexHandler hitHandler)
        {
            if (string.IsNullOrEmpty(wikicode))
            {
                return wikicode;
            }

            Match match = regex.Match(wikicode);
            if (!match.Success)
            {
                return misHandler(wikicode);
            }

            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(wikicode.Length * 2);

            while (match.Success && (lastIndex < wikicode.Length))
            {
                // Copy the skipped part.
                string code = wikicode.Substring(lastIndex, match.Index - lastIndex);
                if (!string.IsNullOrEmpty(code))
                {
                    sb.Append(misHandler(code));
                }

                // Handle the match. Either copy a replacement or the matched part as-is.
                code = hitHandler(match) ?? match.Value;
                sb.Append(code);

                lastIndex = match.Index + match.Length;
                match = match.NextMatch();
            }

            // Copy the remaining bit.
            if (lastIndex < wikicode.Length)
            {
                string code = wikicode.Substring(lastIndex);
                if (!string.IsNullOrEmpty(code))
                {
                    sb.Append(misHandler(code));
                }
            }

            return sb.ToString();
        }

        private static string ConvertBinaryCode(
                                    string wikicode,
                                    MismatchedCodeHandler misHandler,
                                    char startMarker,
                                    char endMarker,
                                    MatchedCodeHandler hitHandler)
        {
            if (string.IsNullOrEmpty(wikicode))
            {
                return string.Empty;
            }

            // Find the open tag.
            int startOffset = 0;
            int lastIndex;
            string contents = StringUtils.ExtractBlock(wikicode, startMarker, endMarker, ref startOffset, out lastIndex);
            if (string.IsNullOrEmpty(contents))
            {
                return wikicode;
            }

            StringBuilder sb = new StringBuilder(wikicode.Length * 16);

            int curWikiOffset = 0;
            while (lastIndex < wikicode.Length)
            {
                string wikiChunk = wikicode.Substring(curWikiOffset, startOffset - curWikiOffset);
                sb.Append(misHandler(wikiChunk));
                sb.Append(hitHandler(contents));

                ++lastIndex;
                startOffset = lastIndex;
                curWikiOffset = lastIndex;
                contents = StringUtils.ExtractBlock(wikicode, startMarker, endMarker, ref startOffset, out lastIndex);
                if (string.IsNullOrEmpty(contents))
                {
                    if (curWikiOffset >= 0 && curWikiOffset < wikicode.Length)
                    {
                        sb.Append(misHandler(wikicode.Substring(curWikiOffset)));
                    }

                    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks if the wikicode is a redirection.
        /// If redirection is in place, it returns the new page title, otherwise null.
        /// </summary>
        /// <param name="wikicode">The wikicode to parse.</param>
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

        /// <summary>
        /// Processes Header codes.
        /// </summary>
        /// <param name="match">The regex match.</param>
        /// <returns>HTML processed header tag.</returns>
        private string Header(Match match)
        {
            string left = match.Groups[1].ToString();
            string right = match.Groups[3].ToString();
            if (left != right)
            {
                // The number of '=' chars mismatch. Not a valid header.
                return ConvertInlineCodes(match.Value);
            }

            string value = match.Groups[2].ToString();
            value = ConvertInlineCodes(value);
            return string.Format(
                    "<h{0}><span class=\"mw-headline\" id=\"{1}\">{2}</span></h{0}>{3}",
                    left.Length,
                    Title.NormalizeAnchor(value),
                    value,
                    Environment.NewLine);
        }

        private string BoldItalic(Match match)
        {
            string left = match.Groups[1].ToString();
            string right = match.Groups[3].ToString();
            if (left == right)
            {
                string value = match.Groups[2].ToString();
                value = ConvertInlineCodes(value);
                switch (left.Length)
                {
                    case 2:
                        return string.Concat("<i>", value, "</i>");

                    case 3:
                        return string.Concat("<b>", value, "</b>");

                    case 5:
                        return string.Concat("<i><b>", value, "</b></i>");
                }
            }

            return match.Value;
        }

        private string Link(string code)
        {
            if (code.StartsWith("[") && code.EndsWith("]"))
            {
                code = code.TrimStart('[').TrimEnd(']');
                code = ConvertInlineCodes(code);

                // [[Image:blah.jpg | options]] internal link.
                string param;
                code = StringUtils.BreakAt(code, '|', out param);
                if (code.Length == 0)
                {
                    return string.Empty;
                }

                string upper = code.ToUpperInvariant();
                if (upper.StartsWith("IMAGE:") || upper.StartsWith("FILE:"))
                {
                    // Throw away the prefix.
                    StringUtils.BreakAt(code, ':', out code);
                    return Image(code, param);
                }

                // Internal Link. [[page_name | link_text]]
                string url = ResolveLink(code, config_.WikiSite.Language.Code);
                return string.Concat("<a href=\"", url, "\" title=\"", code, "\" class=\"mw-redirect\">", param ?? code, "</a>");
            }
            else
            {
                // [http://www.com name] external link.
                string text;
                string url = StringUtils.BreakAt(code, ' ', out text);
                return string.Concat("<a href=\"", url, "\" title=\"", url, "\">", text ?? url, "</a>");
            }
        }

        private string Image(string imageFileName, string options)
        {
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
                try
                {
                    string imagePage = Download.DownloadPage(imagePageUrl);
                    Match imageSourceMatch = ImageSourceRegex.Match(imagePage);
                    if (!imageSourceMatch.Success ||
                        (string.Compare(imageSourceMatch.Groups[1].Value, imageFileName, true) != 0))
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
                catch (Exception ex)
                {
                    logger_.Log(Levels.Error, "Failed to download image '{0}'. Error: {1}.", imagePageUrl, ex.Message);
                    imageSrcUrl = imagePageUrl;
                }
            }

            int width = -1;
            int height = -1;

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

        #region MagicWords, Functions and Templates

        private string ProcessMagicWords(string wikicode)
        {
            int endIndex;
            int startIndex = MagicParser.FindMagicBlock(wikicode, out endIndex);
            if (startIndex < 0)
            {
                return wikicode;
            }

            logger_.Log(Levels.Debug, "Processing Magic:{0}{1}", Environment.NewLine, wikicode);

            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(wikicode.Length * 16);

            while (startIndex >= 0 && lastIndex < wikicode.Length)
            {
                // Copy the skipped part.
                string text = wikicode.Substring(lastIndex, startIndex - lastIndex);
                sb.Append(ConvertInlineCodes(text));

                // Handle the match.
                string magic = wikicode.Substring(startIndex + 2, endIndex - startIndex - 4 + 1);
                if (magic.Length > 0)
                {
                    // Embedded links are processed as-is.
                    if (magic.StartsWith("["))
                    {
                        return ConvertInlineCodes(wikicode);
                    }

                    if (magic.StartsWith("<"))
                    {
                        sb.Append("{{").Append(magic).Append("}}");
                    }
                    else
                    {
                        magic = ProcessMagicWords(magic);

                        string output;
                        if (MagicWord(magic, out output) == VariableProcessor.Result.Found &&
                            !string.IsNullOrEmpty(output))
                        {
                            // Recursively process.
                            output = ProcessMagicWords(output);
                        }

                        sb.Append(output);
                    }
                }

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

            LogEntry logEntry = logger_.CreateEntry(Levels.Debug, "Magic Variable: [{0}].", command);
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

            // If it's an explicit template, process now.
            if (command.TrimEnd(':') == config_.WikiSite.GetNamespace(WikiSite.Namespace.Tempalate))
            {
                if (args != null && args.Count > 0)
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
                    logger_.Log(Levels.Debug, "MagicWord result [{0}]:{1}{2}.", command, Environment.NewLine, output);
                    return result;
                }
            }

            // Is it a parser function?
            result = parserFunctionsProcessor_.Execute(command, args, out output);
            if (result != VariableProcessor.Result.Unknown)
            {
                logger_.Log(Levels.Debug, "ParserFunction result [{0}]:{1}{2}.", command, Environment.NewLine, output);
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
            if (retrievePageDel_ == null)
            {
                output = string.Empty;
                return VariableProcessor.Result.Unknown;
            }

            string nameSpace;
            string title = Title.ParseFullPageName(name, out nameSpace);
            if (string.IsNullOrEmpty(nameSpace))
            {
                // Missing or invalid namespace, assume "Template".
                nameSpace = config_.WikiSite.GetNamespace(WikiSite.Namespace.Tempalate);
                name = Title.FullPageName(nameSpace, title);
            }

            string template = RetrieveTemplate(name);
            if (template == null)
            {
                logger_.Log(Levels.Debug, "Template [{0}] didn't resolve.", name);
                output = name; // Leave it as-is. Might be meaningful in a larger context.
                return VariableProcessor.Result.Html;
            }

            logger_.Log(Levels.Debug, "Template for [{0}]:{1}{2}.", name, Environment.NewLine, template);

            // Redirection?
            string newName = Redirection(template);
            if (newName != null)
            {
                logger_.Log(Levels.Debug, "Template [{0}] redirects to [{1}].", template, newName);
                template = RetrieveTemplate(newName);
                if (template == null)
                {
                    logger_.Log(Levels.Debug, "Template [{0}] didn't resolve.", newName);
                    output = name; // Leave it as-is. Might be meaningful in a larger context.
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

            logger_.Log(
                Levels.Debug,
                "Processing template params for:{0}{1}",
                Environment.NewLine,
                template);

            output = MagicParser.ProcessTemplateParams(template, args);
            return VariableProcessor.Result.Found;
        }

        private string RetrieveTemplate(string name)
        {
            Debug.Assert(retrievePageDel_ != null, "No RetrievePage delegate defined.");

            logger_.Log(Levels.Debug, "Resolving template [{0}].", name);

            string text = retrievePageDel_(name, config_.WikiSite.Language.Code);
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Remove documentation and other data.
            text = StringUtils.RemoveBlocks(text, "<noinclude>", "</noinclude>");

            //FIXME: These can be nested!
            // <noinclude>: the content will not be rendered there. These tags have no effect here.
            // <includeonly>: the content will render only there, and will not render here (like invisible ink made visible by means of transclusion).
            // <onlyinclude>: the content will render here and will render there, but it will only render there what is between these tags.
            // There can be several such section "elements". Also, they can be nested. All possible renderings are achievable. For example, to render there one or more sections of the page here use <onlyinclude> tags. To append text there, wrap the addition in <includeonly> tags above, within, or below the section. To omit portions of the section, nest <noinclude> tags within it.

            // Find an include block, if any.
            string template = StringUtils.ExtractBlock(text, "<onlyinclude>", "</onlyinclude>");
            if (template != null)
            {
                return template;
            }

            // Find an includeonly block, if any.
            template = StringUtils.ExtractBlock(text, "<includeonly>", "</includeonly>");
            if (template != null)
            {
                return template;
            }

            // Whatever is left after the noinclude is all there is.
            return text;
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
                int blankLines = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Trim().Length == 0)
                    {
                        ++blankLines;
                    }
                    else
                    if (line.StartsWith("<h") ||
                        line.StartsWith("<t") ||
                        line.StartsWith("<p") ||
                        line.StartsWith("<d") ||
                        line.StartsWith("<u") ||
                        line.StartsWith("<l") ||
                        line.StartsWith("</") ||
                        line.StartsWith("<c"))
                    {
                        if (para)
                        {
                            sb.AppendLine("</p>");
                            para = false;
                        }

                        if (blankLines == 1)
                        {
                            sb.AppendLine("<p>");
                            para = true;
                            blankLines = 0;
                        }
                        else
                        if (blankLines > 1)
                        {
                            sb.AppendLine("<p><br /></p>");
                            blankLines = 0;
                        }
                    }
                    else
                    {
                        if (!para)
                        {
                            if (blankLines > 1)
                            {
                                sb.AppendLine("<p><br /></p>");
                            }

                            sb.AppendLine("<p>");
                            para = true;
                            blankLines = 0;
                        }
                    }

                    sb.AppendLine(line);
                }

                if (para)
                {
                    sb.AppendLine("</p>");
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

        private readonly RetrievePage retrievePageDel_;

        private readonly IFileCache fileCache_;

        private readonly string commonImagesPath_;

        private readonly MagicWordProcessor magicWordProcessor_;
        private readonly ParserFunctionProcessor parserFunctionsProcessor_;

        private readonly ILogger logger_;

        private readonly List<TocEntry> tableOfContents_ = new List<TocEntry>(8);

        #endregion // representation

        #region Regex

        //
        // Unary Operators
        //
        private static readonly Regex RedirectRegex = new Regex(@"^#REDIRECT(\:?)\s*\[\[(.+?)\]\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline);
        private static readonly Regex ListRegex = new Regex(@"^(\*+)\s*(.+?)$", RegexOptions.Compiled | RegexOptions.Multiline);

        /// <summary>
        /// Headers (Can appear only at the start of a line.)
        /// </summary>
        private static readonly Regex HeaderRegex = new Regex(@"^(={1,6})(.+?)(={1,6})", RegexOptions.Compiled | RegexOptions.Multiline);

        /// <summary>
        /// Bold/Italic (Can appear anywhere in a line.)
        /// </summary>
        private static readonly Regex BoldItalicRegex = new Regex(@"('{2,5})(.+?)('{2,5})", RegexOptions.Compiled | RegexOptions.Multiline);

        /// <summary>
        /// Links: internal, external and image. (Can appear anywhere in a line.)
        /// </summary>
        private static readonly Regex LinkRegex = new Regex(@"\[(\[)?(.+?)(\])?\]", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex NoWikiRegex = new Regex(@"\<nowiki\>(.|\n|\r)+?\<\/nowiki\>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private static readonly Regex ImageSourceRegex = new Regex("<img alt=\"File:(.+?)\" src=\"(.+?)\"");

        /// <summary>The current Namespace.</summary>
        private string nameSpace_;

        /// <summary>The current page-title.</summary>
        private string pageTitle_;

        #endregion // Regex

        #region constants

        private const int MAX_REDIRECTION_CHAIN_COUNT = 3;

        #endregion // constants
    }
}

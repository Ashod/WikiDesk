
namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StringUtils
    {
        /// <summary>
        /// Removes all text between the start and end markers, if any.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="startMarker">The remove start marker.</param>
        /// <param name="endMarker">The remove end marker.</param>
        /// <returns>Returns the text without the removed blocks.</returns>
        public static string RemoveBlocks(string text, string startMarker, string endMarker)
        {
            int startIndex = text.IndexOf(startMarker);
            if (startIndex < 0)
            {
                return text;
            }

            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(text.Length);

            while (startIndex >= 0 && lastIndex < text.Length)
            {
                // Copy the good part.
                sb.Append(text.Substring(lastIndex, startIndex - lastIndex));

                // Skip over the match.
                int endIndex = text.IndexOf(endMarker, startIndex + startMarker.Length);
                if (endIndex < 0)
                {
                    // Missing end marker.
                    return sb.ToString();
                }

                lastIndex = endIndex + endMarker.Length;
                startIndex = text.IndexOf(startMarker, lastIndex);
            }

            sb.Append(text.Substring(lastIndex));
            return sb.ToString();
        }

        /// <summary>
        /// Extracts the text between the start and end markers, if any.
        /// Returns null if either marker were not found and offset is left as-is.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="startMarker">The start marker.</param>
        /// <param name="endMarker">The end marker.</param>
        /// <param name="startOffset">
        /// The offset to start searching from.
        /// Contains the start index, on success, -1 on failure.
        /// </param>
        /// <param name="endOffset">Contains the end index, on success, -1 on failure.</param>
        /// <returns>The text between the markers, if found, otherwise null.</returns>
        public static string ExtractBlock(
                                string text, 
                                string startMarker,
                                string endMarker,
                                ref int startOffset,
                                out int endOffset)
        {
            startOffset = text.IndexOf(startMarker, startOffset);
            if (startOffset < 0)
            {
                startOffset = -1;
                endOffset = -1;
                return null;
            }

            int startIndex = startOffset + 1;
            int endIndex = text.IndexOf(endMarker, startIndex);
            if (endIndex < 0)
            {
                startOffset = -1;
                endOffset = -1;
                return null;
            }

            endOffset = endIndex + endMarker.Length - 1;
            startIndex = startIndex + startMarker.Length - 1;
            return text.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Extracts the text between the start and end markers, if any.
        /// Returns null if either marker were not found and offset is left as-is.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="startMarker">The start marker.</param>
        /// <param name="endMarker">The end marker.</param>
        /// <param name="offset">
        /// The offset to start searching from. Contains the end index, of success.
        /// Contains -1 on failure.
        /// </param>
        /// <returns>The text between the markers, if found, otherwise null.</returns>
        public static string ExtractBlock(string text, string startMarker, string endMarker, ref int offset)
        {
            int endOffset;
            string res = ExtractBlock(text, startMarker, endMarker, ref offset, out endOffset);
            offset = Math.Max(offset, endOffset);
            return res;
        }

        /// <summary>
        /// Extracts the text between the start and end markers, if any.
        /// Returns null if either marker were not found.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="startMarker">The start marker.</param>
        /// <param name="endMarker">The end marker.</param>
        /// <returns>The text between the markers, if found, otherwise null.</returns>
        public static string ExtractBlock(string text, string startMarker, string endMarker)
        {
            int offset = 0;
            return ExtractBlock(text, startMarker, endMarker, ref offset);
        }

        /// <summary>
        /// Given the contents of an opening tag (without brackets), it extracts the
        /// name and attribs (if any) and states whether it's self-closing or not.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="attribs">The parsed attribs. Empty if none. Null if closing.</param>
        /// <param name="closing">True if the tag is closing, otherwise False.</param>
        /// <returns>The tag name.</returns>
        public static string ExtractTagName(string text, out string attribs, out bool closing)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Trim();
                closing = text.StartsWith("/");
                if (closing)
                {
                    attribs = null;
                    return text.TrimStart('/').Trim();
                }

                int index = text.IndexOf(' ');
                if (index >= 0)
                {
                    attribs = text.Substring(index + 1);
                    closing = attribs.EndsWith("/");
                    if (closing)
                    {
                        attribs = attribs.TrimEnd('/');
                    }

                    return text.Substring(0, index);
                }

                attribs = string.Empty;
                closing = text.EndsWith("/");
                return closing ? text.TrimEnd('/') : text;
            }

            attribs = null;
            closing = false;
            return string.Empty;
        }

        /// <summary>
        /// Extracts the contents of the first matching tag.
        /// Returns null if self-closing, empty when there are no contents.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="offset"></param>
        /// <param name="tag"></param>
        /// <param name="attribs"></param>
        /// <returns></returns>
        public static string ExtractTag(string text, ref int offset, ref string tag, ref string attribs)
        {
            // Find the open tag.
            int openIndex = offset;
            string openTag = ExtractBlock(text, "<", ">", ref openIndex);
            if (string.IsNullOrEmpty(openTag))
            {
                // No tags.
                attribs = null;
                return null;
            }

            bool closing;
            tag = ExtractTagName(openTag, out attribs, out closing);
            if (closing)
            {
                offset = openIndex;
                return null;
            }

            // Search for the close tag.
            int closeIndex = openIndex + openTag.Length;

            while (closeIndex >= 0 && closeIndex < text.Length)
            {
                int endIndex;
                string closeTag = ExtractBlock(text, "<", ">", ref closeIndex, out endIndex);
                if (closeIndex < 0)
                {
                    // No tags, process all of it.
                    attribs = null;
                    return null;
                }

                string dummy;
                string closingTagName = ExtractTagName(closeTag, out dummy, out closing);
                if (closing && string.Compare(tag, closingTagName, true) == 0)
                {
                    // Found.
                    offset = endIndex;
                    return text.Substring(openIndex + 1, closeIndex - openIndex - 1);
                }
            }

            offset = -1;
            return string.Empty;
        }

        /// <summary>
        /// Extracts the contents of the
        /// </summary>
        /// <param name="text"></param>
        /// <param name="offset"></param>
        /// <param name="attribs"></param>
        /// <returns></returns>
        public static string ExtractTagContents(string text, ref int offset, out List<KeyValuePair<string, string>> attribs)
        {
            // Find the open tag.
            int openIndex = offset;
            string openTag = ExtractBlock(text, "<", ">", ref openIndex);
            if (openIndex == 0)
            {
                // No tags.
                attribs = null;
                return null;
            }

            // Search for the close tag.
            int closeIndex = 0;
            string closeTag = ExtractBlock(text, "<", ">", ref closeIndex);
            if (closeIndex == 0)
            {
                // No tags, process all of it.
                attribs = null;
                return null;
            }

            attribs = null;
            return string.Empty;
        }
    }
}

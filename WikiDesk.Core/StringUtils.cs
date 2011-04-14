
namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StringUtils
    {
        /// <summary>
        /// Breaks a string into two parts at the first point the at param appears.
        /// Returns the first part, the second part is in
        /// </summary>
        /// <param name="text">The text to break.</param>
        /// <param name="at">The character to break at.</param>
        /// <param name="second">The part after the break character, exclusive.</param>
        /// <returns>The part before the break character, exclusive.</returns>
        public static string BreakAt(string text, char at, out string second)
        {
            if (text == null)
            {
                second = null;
                return null;
            }

            if (text.Length == 0)
            {
                second = null;
                return string.Empty;
            }

            int index = text.IndexOf(at);
            if (index >= 0)
            {
                second = text.Substring(index + 1);
                return text.Substring(0, index);
            }

            second = null;
            return text;
        }

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
        /// Returns the position of the first open marker if found, otherwise -1.
        /// </summary>
        /// <param name="text">A string to search within.</param>
        /// <param name="startOffset">The offset at which to start the search.</param>
        /// <param name="end">The end position of the block, if any. -1 if not found.</param>
        /// <param name="open">The opening character.</param>
        /// <param name="close">The closing character.</param>
        /// <param name="repeat">The repetition count, 0 for exact match.</param>
        /// <returns>The position of the first open marker if found, otherwise -1.</returns>
        public static int FindWrappedBlock(
                                string text,
                                int startOffset,
                                out int end,
                                char open,
                                char close,
                                int repeat)
        {
            // Find start of a wrapped block.
            int indexOfOpen = text.IndexOf(open, startOffset);

            while (repeat > 0 && indexOfOpen >= 0)
            {
                int count = CountRepetition(text, indexOfOpen);
                if (count == repeat)
                {
                    break;
                }

                indexOfOpen = text.IndexOf(open, indexOfOpen + count);
            }

            if (indexOfOpen < 0)
            {
                // No blocks found.
                end = -1;
                return -1;
            }

            // Find end of the magic block.
            int nesting = 0;
            int pos = indexOfOpen;
            while (pos < text.Length)
            {
                if (text[pos] == open)
                {
                    ++nesting;
                }
                else
                if (text[pos] == close)
                {
                    --nesting;
                }

                if (nesting == 0)
                {
                    break;
                }

                ++pos;
            }

            if (nesting == 0 && pos < text.Length)
            {
                end = pos;
                return indexOfOpen;
            }

            // No blocks found.
            end = -1;
            return -1;
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
                                char startMarker,
                                char endMarker,
                                ref int startOffset,
                                out int endOffset)
        {
            startOffset = FindWrappedBlock(text, startOffset, out endOffset, startMarker, endMarker, 0);
            if (startOffset < 0)
            {
                return null;
            }

            return text.Substring(startOffset + 1, endOffset - startOffset - 1);
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

            // At least one block found, now account for nesting
            // and find the matching end marker.
            int endIndex;
            int depth = 0;
            int startIndex = startOffset;
            do
            {
                endIndex = text.IndexOf(endMarker, startIndex);
                if (endIndex < 0)
                {
                    startOffset = -1;
                    endOffset = -1;
                    return null;
                }

                startIndex = text.IndexOf(startMarker, startIndex);
                if (startIndex < 0 || startIndex > endIndex)
                {
                    --depth;
                }
                else
                {
                    ++depth;
                    startIndex += startMarker.Length;
                }
            }
            while (depth > 0);

            endOffset = endIndex + endMarker.Length - 1;
            startIndex = startOffset + startMarker.Length;
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
            if (string.IsNullOrEmpty(text))
            {
                attribs = null;
                closing = false;
                return string.Empty;
            }

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

        /// <summary>
        /// Searches for a specific tag.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="startOffset">
        /// The offset where to start the search.
        /// Contains the index of the open tag, if found.
        /// </param>
        /// <param name="endOffset">The end index of the tag, if found, otherwise -1.</param>
        /// <param name="tag">The tag to search for.</param>
        /// <param name="closing">True if the tag is a auto-closing. Meaningful if found.</param>
        /// <param name="attribs">The parsed attribs. Empty if none. Null if closing.</param>
        /// <returns>The index where it's found, otherwise -1.</returns>
        public static int FindTag(
                            string text,
                            int startOffset,
                            out int endOffset,
                            string tag,
                            out bool closing,
                            out string attribs)
        {
            closing = false;
            if (string.IsNullOrEmpty(tag))
            {
                endOffset = -1;
                attribs = null;
                return -1;
            }

            string openTag;
            do
            {
                // Find the next tag, compare with the request one.
                int openEndIndex;
                openTag = ExtractBlock(text, "<", ">", ref startOffset, out openEndIndex);
                string curTagName = ExtractTagName(openTag, out attribs, out closing);
                if (string.Compare(tag, curTagName, true) == 0)
                {
                    // Found.
                    endOffset = openEndIndex;
                    return startOffset;
                }

                startOffset = openEndIndex;
            }
            while (!string.IsNullOrEmpty(openTag) && startOffset < text.Length);

            // Can't find the user's tag.
            endOffset = -1;
            attribs = null;
            return -1;
        }

        /// <summary>
        /// Extracts the contents of the first matching tag.
        /// Returns null if self-closing, empty when there are no contents.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="startOffset">
        /// The offset where to start the search.
        /// Contains the index of the open tag, if found.
        /// </param>
        /// <param name="endOffset">The end index of the close tag, if found, otherwise -1.</param>
        /// <param name="tag">The tag to search or the tag found.</param>
        /// <param name="attribs">The tag attributes, if any.</param>
        /// <returns></returns>
        public static string ExtractTag(
                                string text,
                                ref int startOffset,
                                out int endOffset,
                                ref string tag,
                                out string attribs)
        {
            // Find the open tag.
            int openEndIndex;
            string openTag = ExtractBlock(text, "<", ">", ref startOffset, out openEndIndex);
            if (string.IsNullOrEmpty(openTag))
            {
                // No tags.
                endOffset = -1;
                tag = null;
                attribs = null;
                return null;
            }

            if (!string.IsNullOrEmpty(tag))
            {
                // Find the tag requested.
                while (startOffset < text.Length &&
                       string.Compare(tag, openTag, true) != 0)
                {
                    startOffset = openEndIndex;
                    openTag = ExtractBlock(text, "<", ">", ref startOffset, out openEndIndex);
                    if (string.IsNullOrEmpty(openTag))
                    {
                        // Can't find the user's tag.
                        endOffset = -1;
                        tag = null;
                        attribs = null;
                        return null;
                    }
                }
            }

            bool closing;
            tag = ExtractTagName(openTag, out attribs, out closing);
            if (closing)
            {
                endOffset = openEndIndex;
                return null;
            }

            // Search for the close tag.
            int closeIndex = openEndIndex + 1;
            while (closeIndex >= 0 && closeIndex < text.Length)
            {
                string closeTag = ExtractBlock(text, "<", ">", ref closeIndex, out endOffset);
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
                    // Found. Extract the contents.
                    return text.Substring(openEndIndex + 1, closeIndex - openEndIndex - 1);
                }

                closeIndex = endOffset + 1;
            }

            endOffset = -1;
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

        private static int CountRepetition(string value, int pos)
        {
            int count = 1;
            char ch = value[pos];
            while (++pos < value.Length)
            {
                if (value[pos] == ch)
                {
                    ++count;
                }
                else
                {
                    break;
                }
            }

            return count;
        }
    }
}

﻿// -----------------------------------------------------------------------------------------
// <copyright file="StringUtils.cs" company="ashodnakashian.com">
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
//   Defines the StringUtils type.
// </summary>
// -----------------------------------------------------------------------------------------

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
        /// <param name="raw">The text to break.</param>
        /// <param name="at">The character to break at.</param>
        /// <param name="second">The part after the break character, exclusive.</param>
        /// <returns>The part before the break character, exclusive.</returns>
        public static string BreakAt(string raw, char at, out string second)
        {
            if (raw == null)
            {
                second = null;
                return null;
            }

            if (raw.Length == 0)
            {
                second = null;
                return string.Empty;
            }

            int index = raw.IndexOf(at);
            if (index >= 0)
            {
                second = raw.Substring(index + 1);
                return raw.Substring(0, index);
            }

            second = null;
            return raw;
        }

        /// <summary>
        /// Parses the string enclosed between two characters.
        /// </summary>
        /// <param name="raw">The source string.</param>
        /// <param name="leftChar">The left character.</param>
        /// <param name="rightChar">The right character.</param>
        /// <returns>The enclosed string if found, otherwise string.Empty.</returns>
        public static string ParseEnclosedString(string raw, char leftChar, char rightChar)
        {
            int first = raw.IndexOf(leftChar);
            if ((first >= 0) && ((first + 1) < raw.Length))
            {
                int second = raw.IndexOf(rightChar, first + 1);
                if (second >= 0)
                {
                    return raw.Substring(first + 1, second - first - 1);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Parses the string enclosed between two same-characters.
        /// </summary>
        /// <param name="raw">The source string.</param>
        /// <param name="ch">The delimiting character.</param>
        /// <returns>The enclosed string if found, otherwise string.Empty.</returns>
        public static string ParseEnclosedString(string raw, char ch)
        {
            return ParseEnclosedString(raw, ch, ch);
        }

        /// <summary>
        /// Same as string.IndexOf() but skips the contents of
        /// within some bounding chars, such as double-quotes.
        /// </summary>
        /// <returns>The index of the char in question, -1 if not found.</returns>
        public static int UnboundIndexOf(string raw, char ch, char leftCh, char rightCh)
        {
            char[] chars = { ch, leftCh };
            int len = raw.Length;

            int idx = raw.IndexOfAny(chars);
            while (idx >= 0)
            {
                // We found the target before the bounder, no chance of being nested.
                if (raw[idx] != leftCh)
                {
                    return idx;
                }

                if ((idx + 1) >= len)
                {
                    return -1; // Can't find it.
                }

                // May be quoted 'ch', try to find the second quote.
                idx = raw.IndexOf(rightCh, idx + 1);
                if ((idx + 1) < len)
                {
                    idx = raw.IndexOfAny(chars, idx + 1);
                }
            }

            return -1;
        }

        /// <summary>
        /// Same as string.IndexOfAny() but skips the contents of
        /// within some bounding chars, such as double-quotes.
        /// </summary>
        /// <returns>The index of the char in question, -1 if not found.</returns>
        public static int UnboundIndexOfAny(string raw, char[] ch, char leftCh, char rightCh)
        {
            char[] chars = new char[ch.Length + 1];
            ch.CopyTo(chars, 0);
            chars[ch.Length] = leftCh;

            int len = raw.Length;

            int idx = raw.IndexOfAny(chars);
            while (idx >= 0)
            {
                // We found the target before the bounder, no chance of being nested.
                if (raw[idx] != leftCh)
                {
                    return idx;
                }

                if ((idx + 1) >= len)
                {
                    return -1; // Can't find it.
                }

                // May be bounded 'ch', try to find the second quote.
                idx = raw.IndexOf(rightCh, idx + 1);
                if (idx < 0)
                {
                    // No closing char.
                    break;
                }

                if ((idx + 1) < len)
                {
                    idx = raw.IndexOfAny(chars, idx + 1);
                }
            }

            return -1;
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
        /// Returns the position of the first marker if found, otherwise -1.
        /// Used with single-marker wrapping, such as quotes.
        /// </summary>
        /// <param name="text">A string to search within.</param>
        /// <param name="startOffset">The offset at which to start the search.</param>
        /// <param name="end">The end position of the block, if any. -1 if not found.</param>
        /// <param name="marker">The marker character.</param>
        /// <param name="repeat">The repetition count, 0 or -ve for greedy.</param>
        /// <param name="symmetric">If true, the closing marker must repeat as many times as the opening.</param>
        /// <returns>The position of the first open marker if found, otherwise -1.</returns>
        public static int FindWrappedBlock(
                                string text,
                                int startOffset,
                                out int end,
                                char marker,
                                int repeat,
                                bool symmetric)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            // Find start of a wrapped block.
            int indexOfOpen = text.IndexOf(marker, startOffset);
            while (indexOfOpen >= 0)
            {
                int count = CountRepetition(text, indexOfOpen);
                if (repeat <= 0)
                {
                    repeat = count;
                    break;
                }

                if (count == repeat)
                {
                    break;
                }

                indexOfOpen = text.IndexOf(marker, indexOfOpen + count);
            }

            if (indexOfOpen < 0)
            {
                // No blocks found.
                end = -1;
                return -1;
            }

            // Find end of the wrapped block.
            int indexOfClose = text.IndexOf(marker, indexOfOpen + repeat);
            while (indexOfClose >= 0)
            {
                int count = CountRepetition(text, indexOfClose);
                if (!symmetric || count == repeat)
                {
                    end = indexOfClose + count - 1;
                    return indexOfOpen;
                }

                indexOfClose = text.IndexOf(marker, indexOfClose + count);
            }

            // No blocks found.
            end = -1;
            return -1;
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
        public static int FindWrappedBlock(string text, int startOffset, out int end, char open, char close, int repeat)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (startOffset < 0 || text.Length <= startOffset)
            {
                end = -1;
                return -1;
            }

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
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (startMarker == null)
            {
                throw new ArgumentNullException("startMarker");
            }

            if (endMarker == null)
            {
                throw new ArgumentNullException("endMarker");
            }

            if (text.Length < startMarker.Length ||
                text.Length < startMarker.Length + endMarker.Length)
            {
                // text must be at least as long as the markers.
                startOffset = -1;
                endOffset = -1;
                return null;
            }

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

        /// <summary>
        /// Counts the number of occurrences of the character at the given position in a row.
        /// </summary>
        /// <param name="text">The text to search within.</param>
        /// <param name="pos">The position to start searching.</param>
        /// <returns>The number of occurrences. A minimum of 1 is always returned.</returns>
        public static int CountRepetition(string text, int pos)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (pos >= text.Length || pos < 0)
            {
                throw new ArgumentOutOfRangeException("pos", pos, "Expected: pos < text.Length");
            }

            int count = 1;
            char ch = text[pos];
            while (++pos < text.Length)
            {
                if (text[pos] == ch)
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

        /// <summary>
        /// Reverse counts the number of occurrences of the character at the given position in a row.
        /// </summary>
        /// <param name="text">The text to search within.</param>
        /// <param name="pos">The position to start searching.</param>
        /// <returns>The number of occurrences. A minimum of 1 is always returned.</returns>
        public static int CountReverseRepetition(string text, int pos)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (pos >= text.Length || pos < 0)
            {
                throw new ArgumentOutOfRangeException("pos", pos, "Expected: pos < text.Length");
            }

            int count = 1;
            char ch = text[pos];
            while (--pos >= 0)
            {
                if (text[pos] == ch)
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

        /// <summary>
        /// Replaces a given character with another removing duplicates.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="bad">The character to replace.</param>
        /// <param name="good">The character to replace with.</param>
        /// <returns>The resulting text.</returns>
        public static string CollapseReplace(string text, char bad, char good)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(text.Length);
            int index = 0;
            while (index < text.Length)
            {
                if (text[index] == bad)
                {
                    sb.Append(good);
                    while (++index < text.Length && text[index] == bad)
                        ;
                }
                else
                {
                    sb.Append(text[index++]);
                }
            }

            return sb.ToString();
        }
    }
}

// -----------------------------------------------------------------------------------------
// <copyright file="WikiTokenizer.cs" company="ashodnakashian.com">
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
//   Defines the Wiki Tokenizer class.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System;
    using System.Text;

    internal class WikiTokenizer
    {
        public enum TokenType
        {
            None = 0,
            Text,
            NewLine,
            Brace,
            MarkupTag,
            Header
        }

        public WikiTokenizer(string wikicode)
        {
            if (wikicode == null)
            {
                throw new ArgumentNullException("wikicode", "Expected a valid string.");
            }

            wikicode_ = wikicode;
            currentIndex_ = 0;
            currentTokenType_ = TokenType.None;
            nextTokenType_ = TokenType.None;
        }

        public TokenType CurrentTokenType
        {
            get { return currentTokenType_; }
        }

        public int CurrentIndex
        {
            get { return currentIndex_; }
        }

        public string Tokenize(Func<string, string> parser)
        {
            StringBuilder sb = new StringBuilder(wikicode_.Length * 16);
            string token;
            while ((token = NextToken()) != null)
            {
                string parsed = parser(token);
                sb.Append(parsed);
            }

            return sb.ToString();
        }

        public string NextToken()
        {
            if (CurrentIndex >= wikicode_.Length)
            {
                // Complete.
                return null;
            }

            int tokenIndex = wikicode_.IndexOfAny(tokens_, CurrentIndex);
            if (tokenIndex < 0)
            {
                // Final token.
                string token = wikicode_.Substring(CurrentIndex);
                currentTokenType_ = TokenType.Text;
                currentIndex_ = wikicode_.Length;
                return token;
            }

            return string.Empty;
        }

        /// <summary>Tokenizes Wiki code and passes each token to the parser.</summary>
        /// <param name="wikicode">The wikicode to tokenize.</param>
        /// <param name="parser">The parser which takes each token in sequence and returns some string.</param>
        /// <returns>Returns the processed text.</returns>
        public static string Tokenize(string wikicode, Func<string, string> parser)
        {
            WikiTokenizer tokenizer = new WikiTokenizer(wikicode);
            return tokenizer.Tokenize(parser);
        }

        private static string Tokenize(string wikicode, ref int index)
        {
            /*
            // Process beginning-of-line markers.
            // Two types: Single-line and Multi-line.
            // Use single-line code to split the text.
            // Header, Indent and Hr.
            StringBuilder html = new StringBuilder(wikicode.Length * 16);
            using (StringReader sr = new StringReader(wikicode))
            {
                StringBuilder wiki = new StringBuilder(wikicode.Length * 16);

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 0)
                    {
                        wiki.AppendLine();
                        continue;
                    }

                    char firstChar = line[0];
                    switch (firstChar)
                    {
                        // Header.
                        case '=':
                            if (wiki.Length > 0)
                            {
                                html.Append(ConvertComplex(wiki.ToString()));
                                wiki.Remove(0, wiki.Length);
                            }

                            line = line.TrimEnd();
                            int left = StringUtils.CountRepetition(line, 0);
                            int right = StringUtils.CountReverseRepetition(line, line.Length - 1);
                            if (left != right || line[line.Length - 1] != '=')
                            {
                                // The number of '=' chars mismatch. Not a valid header.
                                line = ConvertInlineCodes(line);
                                html.Append(line);
                                continue;
                            }

                            string value = line.Substring(left, line.Length - left - right);
                            value = ConvertInlineCodes(value);
                            line = string.Format(
                                    "{3}<h{0}><span class=\"mw-headline\" id=\"{1}\">{2}</span></h{0}>",
                                    left,
                                    Title.CanonicalizeAnchor(value),
                                    value,
                                    Environment.NewLine);
                            html.Append(line);
                            continue;

                        // Indent.
                        case ':':
                            int indent = StringUtils.CountRepetition(line, 0);
                            line = line.Substring(indent).TrimEnd();
                            if (line.Length == 0)
                            {
                                break;
                            }

                            if (wiki.Length > 0)
                            {
                                html.Append(ConvertComplex(wiki.ToString()));
                                wiki.Remove(0, wiki.Length);
                            }

                            --indent;
                            html.Append("<dl>");
                            for (int i = 0; i < indent; ++i)
                            {
                                html.Append("\r\n<dd>\r\n<dl>");
                            }

                            html.Append("\r\n<dd>");
                            line = ConvertInlineCodes(line.Trim());
                            html.Append(line);
                            html.Append("</dd>");

                            for (int i = 0; i < indent; ++i)
                            {
                                html.Append("\r\n</dl>\r\n</dd>");
                            }

                            html.Append("\r\n</dl>");
                            continue;

                        // Hr.
                        case '-':
                            int dashes = StringUtils.CountRepetition(line, 0);
                            if (dashes < 4)
                            {
                                break;
                            }

                            if (wiki.Length > 0)
                            {
                                html.Append(ConvertComplex(wiki.ToString()));
                                wiki.Remove(0, wiki.Length);
                            }

                            html.Append("<hr>");
                            line = line.Substring(dashes).TrimEnd();
                            if (line.Length > 0)
                            {
                                html.Append(ConvertComplex(line));
                            }

                            continue;

                        // Pre.
                        case ' ':
                            if (wiki.Length > 0)
                            {
                                html.Append(ConvertComplex(wiki.ToString()));
                                wiki.Remove(0, wiki.Length);
                            }

                            html.Append(ConvertPreCode(line, sr));
                            continue;

                        // Unordered List.
                        case '*':
                            if (wiki.Length > 0)
                            {
                                html.Append(ConvertComplex(wiki.ToString()));
                                wiki.Remove(0, wiki.Length);
                            }

                            WikiList2Html list2Html = new WikiList2Html('*', "ul", "li", ConvertInlineCodes);
                            html.Append(list2Html.ConvertListCode(line, sr));
                            continue;

                        // Ordered List.
                        case '#':
                            if (wiki.Length > 0)
                            {
                                html.Append(ConvertComplex(wiki.ToString()));
                                wiki.Remove(0, wiki.Length);
                            }

                            WikiList2Html olist2Html = new WikiList2Html('#', "ol", "li", ConvertInlineCodes);
                            html.Append(olist2Html.ConvertListCode(line, sr));
                            continue;

                        default:
                            break;
                    }

                    wiki.AppendLine(line);
                }

                html.Append(ConvertComplex(wiki.ToString()));
            }

            return html.ToString();
             */
            return string.Empty;
        }

        #region representation

        private readonly string wikicode_;

        private int currentIndex_;

        private TokenType currentTokenType_;
        private TokenType nextTokenType_;

        private static char[] tokens_ = new[] { '\r', '\n', '{', '<', '=' };

        #endregion // representation
    }
}

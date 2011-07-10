// -----------------------------------------------------------------------------------------
// <copyright file="Title.cs" company="ashodnakashian.com">
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
//   Defines the Title type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Title canonicalization and related utilities.
    /// </summary>
    public static class Title
    {
        /// <summary>
        /// Given a full page name, parses the namespace and page title.
        /// </summary>
        /// <param name="title">The full page name to parse.</param>
        /// <param name="ns">The namespace, if any. Empty if not found.</param>
        /// <returns>The page title.</returns>
        public static string ParseFullTitle(string title, out string ns)
        {
            if (string.IsNullOrEmpty(title))
            {
                ns = string.Empty;
                return string.Empty;
            }

            int index = title.IndexOf(':');
            if (index < 0)
            {
                ns = string.Empty;
                return title;
            }

            ns = title.Substring(0, index).Trim();
            return title.Substring(index + 1).Trim();
        }

        /// <summary>
        /// Given a namespace and page title, return a full page name.
        /// </summary>
        /// <param name="ns">An optional namespace.</param>
        /// <param name="title">The page title.</param>
        /// <returns>Full page name.</returns>
        public static string FullTitleName(string ns, string title)
        {
            title = Canonicalize(title ?? string.Empty);

            if (ns != null)
            {
                ns = ns.Trim();
                if (ns.Length > 0)
                {
                    return ns + ':' + title;
                }
            }

            return title;
        }

        /// <summary>
        /// Converts a title such that the first character is in upper-case
        /// and all spaces are converted to underscores.
        /// </summary>
        /// <param name="title">The title to normalize.</param>
        /// <returns>Normalized title.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <c>null</c>.</exception>
        public static string Canonicalize(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            // Spaces and underscores are interchangeable.
            title = StringUtils.CollapseReplace(title, ' ', '_');
            title = StringUtils.CollapseReplace(title, '_', '_');
            if (title.Length == 0)
            {
                return string.Empty;
            }

            string ns;
            title = ParseFullTitle(title, out ns);

            // Spaces and underscores are superfluous if at either end.
            title = title.Trim(new[] { ' ', '_' });

            // Always make the first character upper for proper comparison.
            if (title.Length > 0)
            {
                title = title.Substring(0, 1).ToUpperInvariant() + title.Substring(1);
                title = title.Normalize();
            }

            // Spaces and underscores are superfluous if at either end.
            ns = ns.Trim(new[] { ' ', '_' });
            if (ns.Length == 0)
            {
                return title;
            }

            // Namespaces are case insensitive. We capitalize it for readability.
            ns = ns.Substring(0, 1).ToUpperInvariant() + ns.Substring(1).ToLowerInvariant();
            ns = ns.Normalize();

            return FullTitleName(ns, title);
        }

        /// <summary>
        /// Converts a title such that the first character is in upper-case
        /// and all underscores are converted to spaces.
        /// </summary>
        /// <param name="title">The title to decanonicalize.</param>
        /// <returns>Decanonicalized title.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <c>null</c>.</exception>
        public static string Decanonicalize(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            title = title.Trim();
            if (title.Length == 0)
            {
                return string.Empty;
            }

            // Spaces and underscores are interchangeable.
            title = title.Replace('_', ' ');

            // Always make the first character upper for proper comparison.
            title = title.Substring(0, 1).ToUpperInvariant() + title.Substring(1);
            return title.Normalize();
        }

        /// <summary>
        /// Encodes a title to be valid URL.
        /// </summary>
        /// <param name="title">The title to Encode.</param>
        /// <returns>Encoded title.</returns>
        public static string UrlCanonicalize(string title)
        {
            return HttpUtility.UrlEncode(Canonicalize(title));
        }

        /// <summary>
        /// Decodes a URL-encoded title to be human-readable.
        /// </summary>
        /// <param name="title">The title to decode.</param>
        /// <returns>Decoded title.</returns>
        public static string UrlDecanonicalize(string title)
        {
            return Decanonicalize(HttpUtility.UrlDecode(title));
        }

        /// <summary>
        /// Converts a title such that it's a valid anchor.
        /// </summary>
        /// <param name="title">The anchor title to normalize.</param>
        /// <returns>Normalized anchor title.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Anchors can't be empty.</exception>
        public static string CanonicalizeAnchor(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            title = title.Trim();
            if (title.Length == 0)
            {
                throw new ArgumentException("Anchors can't be empty.");
            }

            // May contain HTML, strip them.
            title = StringUtils.RemoveBlocks(title, "<", ">");
            title = title.Replace(' ', '_');
            title = AnchorEncode(title);

            return title.Normalize();
        }

        /// <summary>
        /// Encodes non-ASCII characters such that they become valid URL.
        /// Always normalizes the input title before encoding.
        /// </summary>
        /// <param name="title">The title to encode.</param>
        /// <returns>URL-safe title.</returns>
        /// <exception cref="System.ArgumentNullException">title is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">Page titles can't be empty.</exception>
        public static string EncodeNonAsciiCharacters(string title)
        {
            title = Canonicalize(title);

            StringBuilder sb = new StringBuilder(title.Length * 2);
            foreach (char c in title)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII.
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Decodes titles with non-ascii characters encoded to valid URL.
        /// Denormalizes decoded title.
        /// </summary>
        /// <param name="title">The title to decode.</param>
        /// <returns>Original title.</returns>
        public static string DecodeEncodedNonAsciiCharacters(string title)
        {
            string decoded = Regex.Replace(
                title,
                @"\\u([a-zA-Z0-9]{4})",
                m => ((char)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString());

            return Decanonicalize(decoded);
        }

        /// <summary>
        /// Encodes a title for in-page anchor ID. Not reversible.
        /// This name must be unique in a document.
        /// ID token must begin with a letter ([A-Za-z]) and may be followed by
        /// any number of letters, digits ([0-9]), hyphens ("-"),
        /// underscores ("_"), colons (":"), and periods (".").
        /// </summary>
        /// <param name="title">The title to encode.</param>
        /// <returns>Encoded title.</returns>
        private static string AnchorEncode(string title)
        {
            StringBuilder sb = new StringBuilder(title.Length * 2);
            sb.Append("a_");
            foreach (char ch in title)
            {
                char c = char.ToLowerInvariant(ch);
                if (c >= 'a' && c <= 'z')
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append(".");
                    sb.Append(((int)c).ToString("x"));
                }
            }

            return sb.ToString();
        }
    }
}

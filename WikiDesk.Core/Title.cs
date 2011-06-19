﻿namespace WikiDesk.Core
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public class Title
    {
        /// <summary>
        /// Given a full page name, parses the namespace and page title.
        /// </summary>
        /// <param name="pageName">The full page name to parse.</param>
        /// <param name="nameSpace">The namespace, if any. Empty if not found.</param>
        /// <returns>The page title.</returns>
        public static string ParseFullPageName(string pageName, out string nameSpace)
        {
            if (pageName == null)
            {
                nameSpace = string.Empty;
                return string.Empty;
            }

            pageName = pageName.Trim();
            int index = pageName.IndexOf(':');
            if (index >= 0)
            {
                nameSpace = pageName.Substring(0, index);
                return pageName.Substring(index + 1);
            }

            nameSpace = string.Empty;
            return pageName;
        }

        /// <summary>
        /// Given a namespace and page title, return a full page name.
        /// </summary>
        /// <param name="nameSpace">An optional namespace.</param>
        /// <param name="pageTitle">The page title.</param>
        /// <returns>Full page name.</returns>
        public static string FullPageName(string nameSpace, string pageTitle)
        {
            pageTitle = Normalize(pageTitle ?? string.Empty);

            if (nameSpace != null)
            {
                nameSpace = nameSpace.Trim();
                if (nameSpace.Length > 0)
                {
                    return nameSpace + ':' + pageTitle;
                }
            }

            return pageTitle;
        }

        /// <summary>
        /// Converts a title such that the first character is in upper-case
        /// and all spaces are converted to underscores.
        /// </summary>
        /// <param name="title">The title to normalize.</param>
        /// <returns>Normalized title.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <c>null</c>.</exception>
        public static string Normalize(string title)
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
            title = StringUtils.CollapseReplace(title, ' ', '_');

            // Always make the first character upper for proper comparison.
            title = title.Substring(0, 1).ToUpperInvariant() + title.Substring(1);
            return title.Normalize();
        }

        /// <summary>
        /// Converts a title such that the first character is in upper-case
        /// and all underscores are converted to spaces.
        /// </summary>
        /// <param name="title">The title to denormalize.</param>
        /// <returns>Denormalized title.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <c>null</c>.</exception>
        public static string Denormalize(string title)
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
        public static string UrlEncode(string title)
        {
            return HttpUtility.UrlEncode(title);
        }

        /// <summary>
        /// Decodes a URL-encoded title to be human-readable.
        /// </summary>
        /// <param name="title">The title to decode.</param>
        /// <returns>Decoded title.</returns>
        public static string UrlDecode(string title)
        {
            return HttpUtility.UrlDecode(title);
        }

        /// <summary>
        /// Converts a title such that it's a valid anchor.
        /// </summary>
        /// <param name="title">The anchor title to normalize.</param>
        /// <returns>Normalized anchor title.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Anchors can't be empty.</exception>
        public static string NormalizeAnchor(string title)
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
            title = Normalize(title);

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

            return Denormalize(decoded);
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

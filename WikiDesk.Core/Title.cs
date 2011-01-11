using System;
using System.Collections.Generic;
using System.Text;

namespace WikiDesk.Core
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    public class Title
    {
        /// <summary>
        /// Converts a title such that the first character is in upper-case
        /// and all spaces are converted to underscores.
        /// </summary>
        /// <param name="title">The title to normalize.</param>
        /// <returns>Normalized title.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="title" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Page titles can't be empty.</exception>
        public static string Normalize(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            title = title.Trim();
            if (title.Length == 0)
            {
                throw new ArgumentException("Page titles can't be empty.");
            }

            // Spaces and underscores are interchangeable.
            title = title.Replace(' ', '_');

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
        /// <exception cref="ArgumentException">Page titles can't be empty.</exception>
        public static string Denormalize(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            title = title.Trim();
            if (title.Length == 0)
            {
                throw new ArgumentException("Page titles can't be empty.");
            }

            // Spaces and underscores are interchangeable.
            title = title.Replace('_', ' ');

            // Always make the first character upper for proper comparison.
            title = title.Substring(0, 1).ToUpperInvariant() + title.Substring(1);
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

            StringBuilder sb = new StringBuilder();
            foreach (char c in title)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
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

    }
}

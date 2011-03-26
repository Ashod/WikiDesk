
namespace WikiDesk.Core
{
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
                int endIndex = text.IndexOf(endMarker, lastIndex);
                if (endIndex < 0)
                {
                    // Shouldn't happen!
                    break;
                }

                lastIndex = startIndex + (endIndex + 2 - startIndex + 1);
                startIndex = text.IndexOf(startMarker, lastIndex);
            }

            sb.Append(text.Substring(lastIndex));
            return sb.ToString();
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
            int startIndex = text.IndexOf(startMarker);
            if (startIndex < 0)
            {
                return null;
            }

            int endIndex = text.IndexOf(endMarker, startIndex + 1);
            if (endIndex < 0)
            {
                return null;
            }

            startIndex += startMarker.Length;
            return text.Substring(startIndex, endIndex - startIndex);
        }
    }
}

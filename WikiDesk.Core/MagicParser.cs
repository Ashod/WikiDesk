
namespace WikiDesk.Core
{
    using System.Collections.Generic;

    public class MagicParser
    {
        public MagicParser(string code, Dictionary<string, string> args)
        {
            code_ = code;
            args_ = args;
        }

        /// <summary>
        /// Returns a magic block or null if non is found.
        /// </summary>
        /// <param name="value">A string to search within.</param>
        /// <param name="end">The end position of the block, if any. -1 if not found.</param>
        /// <returns>A magic block, without braces, otherwise null.</returns>
        public static int FindMagicBlock(string value, out int end)
        {
            return FindMagicBlock(value, 0, out end);
        }

        /// <summary>
        /// Returns a magic block or null if non is found.
        /// </summary>
        /// <param name="value">A string to search within.</param>
        /// <param name="startOffset">The offset at which to start the search.</param>
        /// <param name="end">The end position of the block, if any. -1 if not found.</param>
        /// <returns>A magic block, without braces, otherwise null.</returns>
        public static int FindMagicBlock(string value, int startOffset, out int end)
        {
            // Find start of a magic block.
            int indexOfOpen = value.IndexOf('{', startOffset);
            while (indexOfOpen >= 0)
            {
                int count = CountRepetition(value, indexOfOpen);
                if (count == 2)
                {
                    break;
                }

                indexOfOpen = value.IndexOf('{', indexOfOpen + count);
            }

            if (indexOfOpen < 0)
            {
                // No blocks found.
                end = -1;
                return -1;
            }

            // Find end of the magic block.
            int braces = 0;
            int pos = indexOfOpen;
            while (pos < value.Length)
            {
                if (value[pos] == '{')
                {
                    ++braces;
                }
                else
                if (value[pos] == '}')
                {
                    --braces;
                }

                if (braces == 0)
                {
                    break;
                }

                ++pos;
            }

            if (braces == 0 && pos < value.Length)
            {
                end = pos;
                return indexOfOpen;
            }

            // No blocks found.
            end = -1;
            return -1;
        }

        /// <summary>
        /// Returns a magic block or null if non is found.
        /// </summary>
        /// <param name="value">A string to search within.</param>
        /// <returns>A magic block, without braces, otherwise null.</returns>
        public static string FindMagicBlock(string value)
        {
            int end;
            int indexOfOpen = FindMagicBlock(value, out end);

            if (indexOfOpen >= 0 && end > indexOfOpen)
            {
                // Strip the braces.
                indexOfOpen += 2;
                return value.Substring(indexOfOpen, end - indexOfOpen - 2 + 1);
            }

            return null;
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

        #region representation

        private readonly string code_;

        private readonly Dictionary<string, string> args_;

        #endregion // representation
    }
}

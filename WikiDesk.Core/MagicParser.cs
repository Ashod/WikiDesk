
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

        /// <summary>
        /// Gets the magic-word/variable/function-name/template-name and all params.
        /// </summary>
        /// <example>"#if:{{{lang|}}}|{{{{{lang}}}}}&nbsp;" should return
        /// "#if", {{{lang|}}}, {{{{{lang}}}}}&nbsp;
        /// </example>
        /// <example>"" should return
        ///
        /// </example>
        /// <param name="code">The code to parse.</param>
        /// <param name="args">The parameters, if any.</param>
        /// <returns>The command name.</returns>
        public static string GetMagicWordAndParams(string code, out List<KeyValuePair<string, string>> args)
        {
            int barIndex = code.IndexOf('|');
            if (barIndex < 0)
            {
                args = null;
                return code;
            }

            List<string> parameters = new List<string>(4);

            // If the magic word ends with a colon, parse it now.
            int colonIndex = code.IndexOf(':');
            if (colonIndex >= 0)
            {
                parameters.Add(code.Substring(0, colonIndex));
                code = code.Substring(colonIndex + 1);
            }

            int curParamStart = 0;
            while (true)
            {
                barIndex = FindUnwrapped(code, curParamStart, '|', '{', '}');
                if (barIndex >= 0)
                {
                    parameters.Add(code.Substring(curParamStart, barIndex - curParamStart));
                    curParamStart = barIndex + 1;
                }
                else
                {
                    // Last param.
                    parameters.Add(code.Substring(curParamStart));
                    break;
                }
            }

            if (parameters.Count == 0)
            {
                args = null;
                return null;
            }

            string command = parameters[0];
            parameters.RemoveAt(0);

            args = new List<KeyValuePair<string, string>>(parameters.Count);

            int paramNumber = 1;
            foreach (string parameter in parameters)
            {
                string name;
                string value;
                int indexOfAssignment = parameter.IndexOf('=');
                if (indexOfAssignment >= 0)
                {
                    name = parameter.Substring(0, indexOfAssignment);
                    value = parameter.Substring(indexOfAssignment + 1);
                }
                else
                {
                    name = paramNumber.ToString();
                    value = parameter;
                    ++paramNumber;
                }

                args.Add(new KeyValuePair<string, string>(name, value));
            }

            return command;
        }

        /// <summary>
        /// Find the index of the first occurrence of ch outside of the wrapper
        /// characters open and close.
        /// </summary>
        /// <param name="text">The text to search within.</param>
        /// <param name="offset">The offset where to start the search.</param>
        /// <param name="ch">The character to find.</param>
        /// <param name="open">The opening wrapper character.</param>
        /// <param name="close">The closing wrapper character.</param>
        /// <returns>The index of ch or -ve if not found.</returns>
        private static int FindUnwrapped(string text, int offset, char ch, char open, char close)
        {
            int nesting = 0;
            for (int pos = offset; pos < text.Length; ++pos)
            {
                char c = text[pos];
                if (c == open)
                {
                    ++nesting;
                }
                else
                if (c == close)
                {
                    --nesting;
                }
                else
                if (c == ch)
                {
                    if (nesting == 0)
                    {
                        return pos;
                    }
                }
            }

            return -1;
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

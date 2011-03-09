
namespace WikiDesk.Core
{
    using System.Collections.Generic;
    using System.Text;

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
            return FindWrappedBlock(value, startOffset, out end, '{', '}', 2);
        }

        /// <summary>
        /// Returns a magic block or null if non is found.
        /// </summary>
        /// <param name="text">A string to search within.</param>
        /// <param name="startOffset">The offset at which to start the search.</param>
        /// <param name="end">The end position of the block, if any. -1 if not found.</param>
        /// <param name="open">The opening character.</param>
        /// <param name="close">The closing character.</param>
        /// <param name="repeat">The repetition count.</param>
        /// <returns>A magic block, without braces, otherwise null.</returns>
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
            while (indexOfOpen >= 0)
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
        /// Processes the arguments to a template code.
        /// Arguments can be named or unnamed.
        /// </summary>
        /// <example>
        /// Define a template with the name t which contains a single numbered
        /// parameter 1 with a default of pqr. The only difference between the
        /// effects of t and t1demo appears when they are called with no parameter
        /// (and no "|"): start-{{{1|pqr}}}-end
        /// Calling the template named t with a parameter value of "a", i.e.,
        /// {{t|a}} -> "start-a-end".
        /// Calling the template named t with a parameter value of " ", i.e.,
        /// {{t| }} -> "start- -end".
        /// Calling the template named t with a parameter value of "", i.e.,
        /// {{t|}} -> "start--end".
        /// Calling the template named t with named parameter 1=, i.e.,
        /// {{t|1=no surprise}} -> "start-no surprise-end".
        /// Calling the template named t with 1= after an unnamed parameter, i.e.,
        /// {{t|no|1=surprise}} -> "start-surprise-end".
        /// Calling the template named t with 1= before an unnamed parameter, i.e.,
        /// {{t|1=no|surprise}} -> "start-surprise-end".
        /// Calling the template named t and no parameter at all, i.e.,
        /// {{t}} -> "start-pqr-end".
        /// Calling the template named t and no named or unnamed parameter 1, i.e.,
        /// {{t|2=two}} -> "start-pqr-end".</example>
        /// <param name="template">The template code to process.</param>
        /// <param name="args">The arguments, named and unnamed.</param>
        /// <returns>The processed result.</returns>
        public static string ProcessTemplateParams(string template, List<KeyValuePair<string, string>> args)
        {
            // Find the first argument to process.
            int endIndex;
            int startIndex = FindWrappedBlock(template, 0, out endIndex, '{', '}', 3);
            if (startIndex < 0)
            {
                return template;
            }

            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(template.Length * 16);

            while (startIndex >= 0 && lastIndex < template.Length)
            {
                // Copy the skipped part.
                sb.Append(template.Substring(lastIndex, startIndex - lastIndex));

                // Handle the match.
                string arg = template.Substring(startIndex, endIndex - startIndex + 1);

                // Get the argument name.
                int end = arg.IndexOfAny(new[] { '|', '}' });
                if (end < 0)
                {
                    // Should never happen.
                    break;
                }

                string argName = arg.Substring(3, end - 3);
                string value = null;
                if (args != null && args.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in args)
                    {
                        if (pair.Key == argName)
                        {
                            value = pair.Value;
                            break;
                        }
                    }
                }

                if (value != null)
                {
                    sb.Append(value);
                }
                else
                {
                    // See if there is a default value.
                    if (arg[end] == '|')
                    {
                        string def = arg.Substring(end + 1, arg.Length - end - 3 - 1);
                        sb.Append(def);
                    }
                }

                lastIndex = startIndex + (endIndex - startIndex + 1);
                startIndex = FindWrappedBlock(template, lastIndex, out endIndex, '{', '}', 3);
            }

            sb.Append(template.Substring(lastIndex));
            template = sb.ToString();
            startIndex = FindWrappedBlock(template, 0, out endIndex, '{', '}', 3);
            return startIndex >= 0 ? ProcessTemplateParams(template, args) : template;
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

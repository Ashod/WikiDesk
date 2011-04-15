
namespace WikiDesk.Core
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    public static class MagicParser
    {
        #region operations

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
            return StringUtils.FindWrappedBlock(value, startOffset, out end, '{', '}', 2);
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
        /// <example>
        /// "#if:{{{lang|}}}|{{{{{lang}}}}}&nbsp;" should return
        /// "#if", {{{lang|}}}, {{{{{lang}}}}}&nbsp;
        /// </example>
        /// <param name="code">The code to parse.</param>
        /// <param name="args">The parameters, if any.</param>
        /// <returns>The command name.</returns>
        public static string GetMagicWordAndParams(string code, out List<KeyValuePair<string, string>> args)
        {
            if (string.IsNullOrEmpty(code))
            {
                args = null;
                return null;
            }

            code = code.Trim(WhiteSpaceChars);

            int index = code.IndexOfAny(ParamDelimChars);
            if (index < 0)
            {
                args = null;
                return code;
            }

            List<string> parameters = new List<string>(4);

            if (code[index] == ':')
            {
                // If it's a colon, it's part of the command, include it.
                parameters.Add(code.Substring(0, index + 1).Trim(WhiteSpaceChars));
            }
            else
            {
                parameters.Add(code.Substring(0, index).Trim(WhiteSpaceChars));
            }

            code = code.Substring(index + 1).Trim(WhiteSpaceChars);

            int curParamStart = 0;
            while (true)
            {
                index = FindParam(code, curParamStart);

                if (index >= 0)
                {
                    string param = code.Substring(curParamStart, index - curParamStart);
                    parameters.Add(param.Trim(WhiteSpaceChars));
                    curParamStart = index + 1;
                }
                else
                {
                    // Last param.
                    string param = code.Substring(curParamStart);
                    parameters.Add(param.Trim(WhiteSpaceChars));
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

            foreach (string parameter in parameters)
            {
                string name;
                string value = parameter.Trim(WhiteSpaceChars);

                // If parameter is whitespace.
                if (value.Length == 0)
                {
                    args.Add(new KeyValuePair<string, string>(string.Empty, parameter));
                    continue;
                }

                // Avoid finding nested magic params.
                int indexOfAssignment = parameter.IndexOfAny(new[] { '{', '|', '=' });
                if (indexOfAssignment >= 0 && parameter[indexOfAssignment] == '=')
                {
                    name = parameter.Substring(0, indexOfAssignment);
                    value = parameter.Substring(indexOfAssignment + 1);
                }
                else
                {
                    name = string.Empty;
                    value = parameter;
                }

                name = name.Trim(WhiteSpaceChars);
                value = value.Trim(WhiteSpaceChars);
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
            Debug.Assert(!string.IsNullOrEmpty(template), "Invalid template.");

            // Find the first argument to process.
            int endIndex;
            int startIndex = StringUtils.FindWrappedBlock(template, 0, out endIndex, '{', '}', 3);
            if (startIndex < 0)
            {
                return template;
            }

            // Convert args to a map for faster query.
            Dictionary<string, string> mapArgs = null;
            if (args != null && args.Count > 0)
            {
                mapArgs = new Dictionary<string, string>(args.Count);
                foreach (KeyValuePair<string, string> pair in args)
                {
                    mapArgs[pair.Key] = pair.Value;
                }
            }

            // Process the params, passing the first match.
            return ProcessTemplateParams(template, mapArgs, startIndex, endIndex);
        }

        #endregion // operations

        #region implementation

        private static string ProcessTemplateParams(
                                    string template,
                                    Dictionary<string, string> args,
                                    int startIndex,
                                    int endIndex)
        {
            Debug.Assert(!string.IsNullOrEmpty(template), "Invalid template.");
            Debug.Assert(startIndex >= 0, "Invalid startIndex.");
            Debug.Assert(endIndex >= 0, "Invalid endIndex.");

            if (startIndex == 0 && endIndex == template.Length - 1)
            {
                // One argument only.
                return ProcessTemplateArg(template, args);
            }

            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(template.Length * 8);

            while (startIndex >= 0 && lastIndex < template.Length)
            {
                // Copy the skipped part.
                sb.Append(template.Substring(lastIndex, startIndex - lastIndex));

                // Handle the match.
                string arg = template.Substring(startIndex, endIndex - startIndex + 1);

                sb.Append(ProcessTemplateArg(arg, args));

                lastIndex = startIndex + (endIndex - startIndex + 1);
                startIndex = StringUtils.FindWrappedBlock(template, lastIndex, out endIndex, '{', '}', 3);
            }

            sb.Append(template.Substring(lastIndex));
            return sb.ToString();
        }

        private static string ProcessTemplateArg(string arg, Dictionary<string, string> args)
        {
            Debug.Assert(arg.Length > 6, "Invalid argument.");

            // Get the argument name.
            int end = arg.IndexOfAny(PipeOrEndParamChars, 3);
            Debug.Assert(end >= 0, "Invalid argument.");

            string argName = arg.Substring(3, end - 3);
            string value;
            if (args != null && args.TryGetValue(argName, out value))
            {
                return value;
            }

            // See if there is a default value.
            if (arg[end] == '|')
            {
                string def = arg.Substring(end + 1, arg.Length - end - 3 - 1);
                if (def.Length > 0)
                {
                    // Process the default value.
                    return ProcessTemplateParams(def, args);
                }
            }

            return string.Empty;
        }

        private static string ProcessTemplateParams(
                                    string template,
                                    Dictionary<string, string> args)
        {
            Debug.Assert(!string.IsNullOrEmpty(template), "Invalid template.");


            // Find the first argument to process.
            int endIndex;
            int startIndex = StringUtils.FindWrappedBlock(template, 0, out endIndex, '{', '}', 3);
            if (startIndex < 0)
            {
                return template;
            }

            // Process the params, passing the first match.
            return ProcessTemplateParams(template, args, startIndex, endIndex);
        }

        /// <summary>
        /// Gets the index of the next param start,
        /// the index that is both outside {} and [].
        /// Bars in braces are the argument default.
        /// Bars in brackets are wiki link names.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="startOffset"></param>
        /// <returns></returns>
        private static int FindParam(string code, int startOffset)
        {
            int nesting = 0;
            for (int pos = startOffset; pos < code.Length; ++pos)
            {
                char c = code[pos];
                if (c == '{' || c == '[')
                {
                    ++nesting;
                }
                else
                if (c == '}' || c == ']')
                {
                    --nesting;
                }
                else
                if (c == '|')
                {
                    if (nesting == 0)
                    {
                        return pos;
                    }
                }
            }

            return -1;
        }

        #endregion // implementation

        #region representation

        private static readonly char[] WhiteSpaceChars = { ' ', '\n', '\r', '\t' };
        private static readonly char[] ParamDelimChars = { ':', '|' };
        private static readonly char[] PipeOrEndParamChars = { '|', '}' };

        #endregion // representation
    }
}

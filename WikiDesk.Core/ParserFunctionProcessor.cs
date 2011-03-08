
namespace WikiDesk.Core
{
    using System.Collections.Generic;

    public class ParserFunctionProcessor
    {
        public ParserFunctionProcessor()
        {
            RegisterHandlers();
        }

        public Result Execute(string functionName, List<KeyValuePair<string, string>> args, out string output)
        {
            Handler func = FindHandler(functionName);
            if (func != null)
            {
                return func(args, out output);
            }

            output = null;
            return Result.Unknown;
        }

        public enum Result
        {
            /// <summary>
            /// Unknown function.
            /// </summary>
            Unknown,

            /// <summary>
            /// The text returned is valid, stop processing the template.
            /// </summary>
            Found,

            /// <summary>
            /// Wiki markup in the return value should be escaped.
            /// </summary>
            NoWiki,

            /// <summary>
            /// The returned text is HTML, armor it against wikitext transformation.
            /// </summary>
            Html
        }

        public delegate Result Handler(List<KeyValuePair<string, string>> args, out string output);

        public void RegisterHandler(string name, Handler func)
        {
            functionsMap_[name] = func;
        }

        private Handler FindHandler(string name)
        {
            Handler func;
            if (functionsMap_.TryGetValue(name, out func))
            {
                return func;
            }

            return null;
        }

        #region implementation

        private void RegisterHandlers()
        {
            RegisterHandler("#expr", DoNothing);
            RegisterHandler("#if", If);
            RegisterHandler("#ifexists", DoNothing);
        }

        /// <summary>
        /// {{#if: test string | value if true | value if false }}
        /// This function tests whether the first parameter is 'non-empty'. It evaluates to false if the test string is empty or contains only whitespace characters (spaces, newlines, etc).
        /// {{#if: | yes | no}} → no
        /// {{#if: string | yes | no}} → yes
        /// {{#if:      | yes | no}} → no
        /// {{#if:
        ///
        ///
        /// | yes | no}} → no
        /// The test string is always interpreted as pure text, so mathematical expressions are not evaluated:
        /// {{#if: 1==2 | yes | no }} → yes
        /// {{#if: 0 | yes | no }} → yes
        /// Either or both the return values may be omitted:
        /// {{#if: foo | yes }} → yes
        /// {{#if: | yes }} →
        /// {{#if: foo | | no}} →
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="output">The output string.</param>
        /// <returns>A result code.</returns>
        private static Result If(List<KeyValuePair<string, string>> args, out string output)
        {
            output = string.Empty;
            if (args == null || args.Count < 1)
            {
                return Result.Found;
            }

            string condition = args[0].Value.Trim();

            // Empty string -> No.
            if (condition.Length == 0)
            {
                if (args.Count >= 3)
                {
                    // Return the "No" result.
                    output = args[2].Value.Trim();
                }

                return Result.Found;
            }

            // Arguments should have been replaced before calling us.
            // Argument-references have the form "{{{arg|}}}", where "arg" is the
            // argument name.
            if (condition.StartsWith("{{{") && condition.EndsWith("|}}}"))
            {
                // Argument-references at this point means "not defined" -> No.
                if (args.Count >= 3)
                {
                    // Return the "No" result.
                    output = args[2].Value.Trim();
                }

                return Result.Found;
            }

            // Return Yes.
            if (args.Count >= 2)
            {
                output = args[1].Value.Trim();
            }

            return Result.Found;
        }

        private Result DoNothing(List<KeyValuePair<string, string>> args, out string output)
        {
            output = string.Empty;
            return Result.Html;
        }

        #endregion // implementation

        #region representation

        private readonly Dictionary<string, Handler> functionsMap_ = new Dictionary<string, Handler>(16);

        #endregion // representation
    }
}

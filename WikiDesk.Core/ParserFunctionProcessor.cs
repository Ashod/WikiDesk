
namespace WikiDesk.Core
{
    using System;
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
            RegisterHandler("#expr", Expr);
            RegisterHandler("#if", If);
            RegisterHandler("#ifeq", IfEq);
            RegisterHandler("#iferror", IfError);
            RegisterHandler("#ifexpr", IfExpr);
            RegisterHandler("#ifexists", IfExists);
            RegisterHandler("#rel2abs", Rel2Abs);
            RegisterHandler("#switch", Switch);
            RegisterHandler("#time", Time);
            RegisterHandler("#timel", TimeL);
            RegisterHandler("#titleparts", TitleParts);
        }

        private Result Expr(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "~EXPRESSION~";
            return Result.Found;
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

        /// <summary>
        /// This parser function compares two strings and determines whether they are identical.
        /// {{#ifeq: string 1 | string 2 | value if identical | value if different }}
        /// If both strings are valid numerical values, the strings are compared numerically:
        /// {{#ifeq: 01 | 1 | yes | no}} → yes
        /// {{#ifeq: 0 | -0 | yes | no}} → yes
        /// {{#ifeq: 1e3 | 1000 | yes | no}} → yes
        /// {{#ifeq: {{#expr:10^3}} | 1000 | yes | no}} → yes
        /// Otherwise the comparison is made as text; this comparison is case sensitive:
        /// {{#ifeq: foo | bar | yes | no}} → no
        /// {{#ifeq: foo | Foo | yes | no}} → no
        /// {{#ifeq: "01" | "1" | yes | no}} → no
        /// {{#ifeq: 10^3 | 1000 | yes | no}} → no
        /// Warning:	Numerical comparisons with #ifeq and #switch are not equivalent
        ///             with comparisons in expressions:
        /// {{#ifeq:12345678901234567|12345678901234568|1|0}} gives 0
        /// because PHP compares two numbers of type integer here; where as
        /// {{#ifexpr:12345678901234567=12345678901234568|1|0}} gives 1
        /// because MediaWiki converts literal numbers in expressions to type float,
        /// which, for large integers like these, involves rounding.
        /// Warning:	Content inside parser tags (such as &lt;nowiki&gt;) is temporarily
        /// replaced by a unique code. This affects comparisons:
        /// {{#ifeq: &lt;nowiki&gt;foo&lt;/nowiki&gt; | &lt;nowiki>foo&lt;/nowiki&gt; | yes | no}} → no
        /// {{#ifeq: &lt;math&gt;foo&lt;/math&gt; | &lt;math>foo&lt;/math&gt; | yes | no}} → no
        /// {{#ifeq: {{#tag:math|foo}} | {{#tag:math|foo}} | yes | no}} → no
        /// {{#ifeq: [[foo]] | [[foo]] | yes | no}} → yes
        /// It the strings to be compared are given as equal calls to the same
        /// template containing such tags then the condition is true, but in the case
        /// of two templates with identical content containing such tags it is false.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private Result IfEq(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result IfError(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result IfExpr(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result IfExists(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result Rel2Abs(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result Switch(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result Time(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result TimeL(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        private Result TitleParts(List<KeyValuePair<string, string>> args, out string output)
        {
            throw new NotImplementedException();
        }

        #endregion // implementation

        #region representation

        private readonly Dictionary<string, Handler> functionsMap_ = new Dictionary<string, Handler>(16);

        #endregion // representation
    }
}

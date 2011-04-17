
namespace WikiDesk.Core
{
    using System.Collections.Generic;
    using System.Text;

    public class ParserFunctionProcessor : VariableProcessor
    {
        public ParserFunctionProcessor()
            : this(null)
        {
        }

        public ParserFunctionProcessor(ProcessMagicWords processMagicWordsDel)
            : base(processMagicWordsDel)
        {
            RegisterHandlers();
        }

        #region implementation

        private void RegisterHandlers()
        {
            RegisterHandler("#expr:", Expr);
            RegisterHandler("#if:", If);
            RegisterHandler("#ifeq:", IfEq);
            RegisterHandler("#iferror:", IfError);
            RegisterHandler("#ifexpr:", IfExpr);
            RegisterHandler("#ifexist:", IfExist);
            RegisterHandler("#rel2abs:", Rel2Abs);
            RegisterHandler("#switch:", Switch);
            RegisterHandler("#time:", Time);
            RegisterHandler("#timel:", TimeL);
            RegisterHandler("#titleparts:", TitleParts);
            RegisterHandler("#tag:", Tag);
        }

        private Result Expr(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "<strong style=\"color: red;\">#expr</strong>";
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
        private Result If(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args == null || args.Count < 1)
            {
                output = "<strong style=\"color: red;\">Error in #if</strong>";
                return Result.Found;
            }

            string condition = args[0].Value.Trim();
            condition = ProcessMagicWord(condition);
            output = string.Empty;

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
        /// {{#ifeq: &lt;math&gt;foo&lt;/math&gt; | &lt;math&gt;foo&lt;/math&gt; | yes | no}} → no
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
            if (args.Count < 3)
            {
                output = "<strong style=\"color: red;\">Error in #ifeq!</strong>";
                return Result.Found;
            }

            string arg1 = args[0].Value;
            arg1 = ProcessMagicWord(arg1);
            string arg2 = args[1].Value;
            arg2 = ProcessMagicWord(arg2);
            string yes = args[2].Value;

            if (arg1 == arg2)
            {
                output = yes;
                return Result.Found;
            }

            // Try converting to numbers and compare again.
            long num1;
            long num2;
            if (long.TryParse(arg1, out num1) &&
                long.TryParse(arg2, out num2) &&
                num1 == num2)
            {
                output = yes;
            }
            else
            {
                output = args.Count > 3 ? args[3].Value : string.Empty;
            }

            return Result.Found;
        }

        private Result IfError(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "<strong style=\"color: red;\">#iferror</strong>";
            return Result.Found;
        }

        private Result IfExpr(List<KeyValuePair<string, string>> args, out string output)
        {
            // {{#ifexpr:12345678901234567=12345678901234568|1|0}} gives 1
            // because MediaWiki converts literal numbers in expressions to type float,
            // which, for large integers like these, involves rounding.
            output = "<strong style=\"color: red;\">#ifexpr</strong>";
            return Result.Found;
        }

        private Result IfExist(List<KeyValuePair<string, string>> args, out string output)
        {
            //TODO: Evaluate. For now we assume it always exists.
            output = args.Count > 1 ? args[1].Value : string.Empty;
            return Result.Found;
        }

        private Result Rel2Abs(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "<strong style=\"color: red;\">#rel2abs</strong>";
            return Result.Found;
        }

        /// <summary>
        /// This function compares one input value against several test cases,
        /// returning an associated string if a match is found.
        /// {{#switch: comparison string
        ///  | case = result
        ///  | case = result
        ///  | ...
        ///  | case = result
        ///  | default result
        /// }}
        /// Alternatively, the default result may be explicitly declared with a
        /// case string of "#default".
        /// {{#switch: comparison string
        ///  | case = result
        ///  | case = result
        ///  | ...
        ///  | case = result
        ///  | #default = default result
        /// }}
        /// </summary>
        /// <example>
        /// {{#switch: baz | foo = Foo | baz = Baz | Bar }} → Baz
        /// #switch allows an editor to add information in one template and this
        /// information will be visible in several other templates which all have
        /// different formatting.
        /// Default:
        /// The default result is returned if no case string matches the comparison string:
        /// {{#switch: test | foo = Foo | baz = Baz | Bar }} → Bar
        /// In this syntax, the default result must be the last parameter and
        /// must not contain a raw equals sign.
        /// {{#switch: test | Bar | foo = Foo | baz = Baz }} →
        /// {{#switch: test | foo = Foo | baz = Baz | B=ar }} →
        /// Alternatively, the default result may be explicitly declared with a
        /// case string of "#default".
        /// {{#switch: comparison string
        ///  | case = result
        ///  | case = result
        ///  | ...
        ///  | case = result
        ///  | #default = default result
        /// }}
        /// Default results declared in this way may be placed anywhere within the function:
        /// {{#switch: test | foo = Foo | #default = Bar | baz = Baz }} → Bar
        /// If the default parameter is omitted and no match is made, no result is returned:
        /// {{#switch: test | foo = Foo | baz = Baz }} →
        /// Grouping results:
        /// It is possible to have 'fall through' values, where several case
        /// strings return the same result string. This minimizes duplication.
        /// {{#switch: comparison string
        ///  | case1 = result1
        ///  | case2
        ///  | case3
        ///  | case4 = result2
        ///  | case5 = result3
        ///  | case6
        ///  | case7 = result4
        ///  | #default = default result
        /// }}
        /// Here cases 2, 3 and 4 all return result2; cases 6 and 7 both return result4
        /// Comparison behavior:
        /// As with #ifeq, the comparison is made numerically if both the
        /// comparison string and the case string being tested are numeric;
        /// or as a case-sensitive string otherwise:
        /// {{#switch: 0 + 1 | 1 = one | 2 = two | three}} → three
        /// {{#switch: {{#expr: 0 + 1}} | 1 = one | 2 = two | three}} → one
        /// {{#switch: a | a = A | b = B | C}} → A
        /// {{#switch: A | a = A | b = B | C}} → C
        /// A case string may be empty:
        /// {{#switch: | = Nothing | foo = Foo | Something }} → Nothing
        /// Once a match is found, subsequent cases are ignored:
        /// {{#switch: b | f = Foo | b = Bar | b = Baz | }} → Bar
        /// Warning:	Numerical comparisons with #switch and #ifeq are not
        /// equivalent with comparisons in expressions (see also above):
        /// {{#switch: 12345678901234567 | 12345678901234568 = A | B}} → B
        /// {{#ifexpr: 12345678901234567 = 12345678901234568 | A | B}} → A
        /// Raw equal signs:
        /// "Case" strings cannot contain raw equals signs. To work around this,
        /// create a template {{=}} containing a single equals sign: =.
        /// Example:
        /// {{#switch: 1=2
        ///  | 1=2 = raw
        ///  | 1&lt;nowiki&gt;=&lt;/nowiki&gt;2 = nowiki
        ///  | 1&#61;2 = html
        ///  | 1{{=}}2 = template
        ///  | default }} → template
        /// </example>
        /// <param name="args">The arguments.</param>
        /// <param name="output">The output string.</param>
        /// <returns>A result code.</returns>
        private Result Switch(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args == null || args.Count < 2)
            {
                output = "<strong style=\"color: red;\">Error in #switch!</strong>";
                return Result.Found;
            }

            string def = string.Empty;
            string key = args[0].Value.Trim();
            key = ProcessMagicWord(key);

            for (int i = 1; i < args.Count; ++i)
            {
                if (args[i].Key == key)
                {
                    output = args[i].Value;
                    return Result.Found;
                }

                if (args[i].Key == "#default")
                {
                    def = args[i].Value;
                }
            }

            // Not found, use default if any.
            if (def.Length > 0)
            {
                output = def;
            }
            else
            if (args[args.Count - 1].Key.Length == 0)
            {
                // The last one is the default, if no key.
                output = args[args.Count - 1].Value;
            }
            else
            {
                output = string.Empty;
            }

            return Result.Found;
        }

        private Result Time(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "<strong style=\"color: red;\">#time</strong>";
            return Result.Found;
        }

        private Result TimeL(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "<strong style=\"color: red;\">#timel</strong>";
            return Result.Found;
        }

        private Result TitleParts(List<KeyValuePair<string, string>> args, out string output)
        {
            output = "<strong style=\"color: red;\">#titleparts</strong>";
            return Result.Found;
        }

        private static Result Tag(List<KeyValuePair<string, string>> args, out string output)
        {
            if (args.Count < 1 ||
                string.IsNullOrEmpty(args[0].Value))
            {
                output = string.Empty;
                return Result.Found;
            }

            string tag = args[0].Value;

            if (args.Count == 1)
            {
                output = string.Format("<{0} />", tag);
                return Result.Found;
            }

            // The last param is the content.
            string contents = args[args.Count - 1].Value;
            if (args.Count == 2)
            {
                output = string.Format("<{0}>{1}</{0}>", tag, contents);
                return Result.Found;
            }

            StringBuilder sb = new StringBuilder(128);
            sb.Append('<').Append(tag);

            // Don't include the tag name or the contents.
            for (int i = 1; i < args.Count - 1; ++i)
            {
                if (!string.IsNullOrEmpty(args[i].Key) &&
                    !string.IsNullOrEmpty(args[i].Value))
                {
                    string attr = args[i].Key.Trim();
                    string val = args[i].Value.Trim();
                    if (attr.Length > 0 && val.Length > 0)
                    {
                        sb.Append(' ').Append(attr).Append('=').Append(val);
                    }
                }
            }

            sb.Append('>').Append(contents);
            sb.Append("</").Append(tag).Append('>');

            output = sb.ToString();
            return Result.Found;
        }

        #endregion // implementation
    }
}

namespace WikiDesk.Core.Test
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class WikiParserFunctionProcessorTest
    {
        #region #if

        [Test]
        public void IfNo()
        {
            TestFunction("#if: |Yes|No", "No");

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", string.Empty),
                    new KeyValuePair<string, string>("2", "Yes"),
                    new KeyValuePair<string, string>("3", "No")
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual("No", output);
        }

        [Test]
        public void IfYes()
        {
            TestFunction("#if: blah|Yes|No", "Yes");

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", "blah"),
                    new KeyValuePair<string, string>("2", "Yes"),
                    new KeyValuePair<string, string>("3", "No")
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual("Yes", output);
        }

        [Test]
        public void IfBlank()
        {
            TestFunction("#if:      |Yes|No", "No");

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", "    "),
                    new KeyValuePair<string, string>("2", "Yes"),
                    new KeyValuePair<string, string>("3", "No")
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual("No", output);
        }

        [Test]
        public void IfMissingNo()
        {
            TestFunction("#if: |Yes", string.Empty);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", string.Empty),
                    new KeyValuePair<string, string>("2", "Yes"),
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void IfMissingYes()
        {
            TestFunction("#if: ||No", "No");

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", string.Empty),
                    new KeyValuePair<string, string>("2", string.Empty),
                    new KeyValuePair<string, string>("3", "No")
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual("No", output);
        }

        [Test]
        public void IfArith()
        {
            TestFunction("#if: 1==2|Yes|No", "Yes");

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", "1==2"),
                    new KeyValuePair<string, string>("2", "Yes"),
                    new KeyValuePair<string, string>("3", "No")
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual("Yes", output);
        }

        [Test]
        public void If0()
        {
            TestFunction("#if: 0|Yes|No", "Yes");

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", "0"),
                    new KeyValuePair<string, string>("2", "Yes"),
                    new KeyValuePair<string, string>("3", "No")
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual("Yes", output);
        }

        [Test]
        public void IfEmpty()
        {
            TestFunction("#if: |Yes", string.Empty);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", string.Empty),
                    new KeyValuePair<string, string>("2", "Yes"),
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void IfMissingArg()
        {
            TestFunction("#if: {{{lang|}}}|Yes|No", "No");

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>(4)
                {
                    new KeyValuePair<string, string>("1", "{{{lang|}}}"),
                    new KeyValuePair<string, string>("2", "Yes"),
                    new KeyValuePair<string, string>("3", "No")
                };

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute("#if:", args, out output));
            Assert.AreEqual("No", output);
        }

        #endregion // #if

        #region #switch

        [Test]
        public void SwitchSimple1()
        {
            TestFunction("#switch: baz | foo = Foo | baz = Baz | Bar ", "Baz");
        }

        [Test]
        public void SwitchSimple2()
        {
            TestFunction("#switch:4|1=C41E3A|2=FFFFFF|3=000000|4=C41E3A", "C41E3A");
        }

        [Test]
        public void SwitchEmpty()
        {
            TestFunction("#switch: \n| \n=\n Nothing | foo = Foo\n | Something", "Nothing");
        }

        [Test]
        public void SwitchEmptyDefault()
        {
            TestFunction("#switch: b | f = Foo | b = Bar | b = Baz | ", "Bar");
        }

        [Test]
        public void SwitchNoMatch1()
        {
            TestFunction("#switch: test | Bar | foo = Foo | baz = Baz ", string.Empty);
        }

        [Test]
        public void SwitchNoMatch2()
        {
            TestFunction("#switch: test | foo = Foo | baz = Baz | B=ar ", string.Empty);        }

        [Test]
        public void NestedSwitch()
        {
            TestFunction("#switch: foo | foo = {{#switch:x|y=z|def}} | baz = Baz | B=ar ", "{{#switch:x|y=z|def}}");
        }

        [Test]
        public void IfedSwitchFalse()
        {
            TestFunction("#switch: {{#if:|foo|baz}} | foo = {{#switch:x|y=z|def}} | baz = Baz | B=ar ", "Baz");
        }

        [Test]
        public void IfedSwitchTrue()
        {
            TestFunction(
                "#switch: {{#if:x|foo|baz}} | foo = {{#switch:x|y=z|def}} | baz = Baz | B=ar ",
                "{{#switch:x|y=z|def}}");
        }

        #endregion // #switch

        #region #ifeq

        [Test]
        public void IfEqSimple1()
        {
            TestFunction("#ifeq: 01 | 1 | yes | no", "yes");
        }

        [Test]
        public void IfEqSimple2()
        {
            TestFunction("#ifeq: 1e3 | 1000 | yes | no", "no");
        }

        [Test]
        public void IfEqSimple3()
        {
            TestFunction("#ifeq: 0 | -0 | yes | no", "yes");
        }

        [Test]
        public void IfEqExponent()
        {
            TestFunction("#ifeq: 1e3 | 1000 | yes | no", "yes");
        }

        [Test]
        public void IfEqExpression()
        {
            TestFunction("#ifeq: {{#expr:10^3}} | 1000 | yes | no", "yes");
        }

        [Test]
        public void IfEqSimple6()
        {
            TestFunction("#ifeq: foo | bar | yes | no", "no");
        }

        [Test]
        public void IfEqSimple7()
        {
            TestFunction("#ifeq: foo | Foo | yes | no", "no");
        }

        [Test]
        public void IfEqSimple8()
        {
            TestFunction("#ifeq: \"01\" | \"1\" | yes | no", "no");
        }

        [Test]
        public void IfEqPower()
        {
            TestFunction("#ifeq: 10^3 | 1000 | yes | no", "no");
        }

        [Test]
        public void IfEqLong()
        {
            TestFunction("#ifeq: 12345678901234567 | 12345678901234568 | 1 | 0", "0");
        }

        [Test]
        public void IfEqNoWiki()
        {
            TestFunction("#ifeq: <nowiki>foo</nowiki> | <nowiki>foo</nowiki> | yes | no", "no");
        }

        [Test]
        public void IfEqMath()
        {
            TestFunction("#ifeq: <math>foo</math> | <math>foo</math> | yes | no", "no");
        }

        [Test]
        public void IfEqTag()
        {
            TestFunction("#ifeq: {{#tag:math|foo}} | {{#tag:math|foo}} | yes | no", "no");
        }

        [Test]
        public void IfEqWiki()
        {
            TestFunction("#ifeq: [[foo]] | [[foo]] | yes | no", "yes");
        }

        [Test]
        public void IfEq3Args()
        {
            TestFunction("#ifeq: autocollapse|plain|yes", string.Empty);
        }

        #endregion // #ifeq

        #region implementation

        private static void TestFunction(string input, string expected)
        {
            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(input, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor(ProcessMagicWord);
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual(expected, output);
        }

        private static string ProcessMagicWord(string wikicode)
        {
            int endIndex;
            int startIndex = MagicParser.FindMagicBlock(wikicode, out endIndex);
            if (startIndex < 0)
            {
                return wikicode;
            }

            wikicode = wikicode.Substring(startIndex + 2, endIndex - startIndex - 4 + 1);

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(wikicode, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor(ProcessMagicWord);
            string output;
            proc.Execute(command, args, out output);
            return output;
        }

        #endregion // implementation
    }
}

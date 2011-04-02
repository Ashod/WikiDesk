﻿namespace WikiDesk.Core.Test
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
            const string RAW = "#switch: baz | foo = Foo | baz = Baz | Bar ";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual("Baz", output);
        }

        [Test]
        public void SwitchSimple2()
        {
            const string RAW = "#switch:4|1=C41E3A|2=FFFFFF|3=000000|4=C41E3A";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual("C41E3A", output);
        }

        [Test]
        public void SwitchEmpty()
        {
            const string RAW = "#switch: \n| \n=\n Nothing | foo = Foo\n | Something";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual("Nothing", output);
        }

        [Test]
        public void SwitchEmptyDefault()
        {
            const string RAW = "#switch: b | f = Foo | b = Bar | b = Baz | ";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual("Bar", output);
        }

        [Test]
        public void SwitchNoMatch1()
        {
            const string RAW = "#switch: test | Bar | foo = Foo | baz = Baz ";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void SwitchNoMatch2()
        {
            const string RAW = "#switch: test | foo = Foo | baz = Baz | B=ar ";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void NestedSwitch()
        {
            const string RAW = "#switch: foo | foo = {{#switch:x|y=z|def}} | baz = Baz | B=ar ";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor();
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual("{{#switch:x|y=z|def}}", output);
        }

        [Test]
        public void IfedSwitchFalse()
        {
            const string RAW = "#switch: {{#if:|foo|baz}} | foo = {{#switch:x|y=z|def}} | baz = Baz | B=ar ";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor(ProcessMagicWord);
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual("Baz", output);
        }

        [Test]
        public void IfedSwitchTrue()
        {
            const string RAW = "#switch: {{#if:x|foo|baz}} | foo = {{#switch:x|y=z|def}} | baz = Baz | B=ar ";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            ParserFunctionProcessor proc = new ParserFunctionProcessor(ProcessMagicWord);
            string output;

            Assert.AreEqual(VariableProcessor.Result.Found, proc.Execute(command, args, out output));
            Assert.AreEqual("{{#switch:x|y=z|def}}", output);
        }

        #endregion // #switch

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
    }
}

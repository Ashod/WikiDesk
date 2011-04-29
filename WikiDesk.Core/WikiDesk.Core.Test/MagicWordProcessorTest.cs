namespace WikiDesk.Core.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    [TestFixture]
    public class WikiMagicWordProcessorTest
    {
        static WikiMagicWordProcessorTest()
        {
            WikiDomain wikiDomain = new WikiDomain("wikipedia");
            WikiLanguage wikiLanguage = new WikiLanguage("English", "en");
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WikiSite = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
        }

        [SetUp]
        public void Setup()
        {
            proc_ = new MagicWordProcessor();
            proc_.SetContext(WikiSite, string.Empty, "TestPage");
            args_ = new List<KeyValuePair<string, string>>(4);
        }

        #region tests

        #region Lc

        [Test]
        public void Lc()
        {
            TestProcessor("lc:KIKOS", "kikos");
        }

        [Test]
        public void LcA()
        {
            TestProcessor("lc:1=TEST", "test");

            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcB()
        {
            TestProcessor("lc:1=test", "test");

            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcSingleChar()
        {
            TestProcessor("lc:1=A", "a");

            args_.Add(new KeyValuePair<string, string>("1", "A"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("a", output);
        }

        [Test]
        public void LcEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcNullArgs()
        {
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // Lc

        #region Uc

        [Test]
        public void UcA()
        {
            TestProcessor("uc:1=test", "TEST");

            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcB()
        {
            TestProcessor("uc:1=TEST", "TEST");

            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcSingleChar()
        {
            TestProcessor("uc:1=a", "A");

            args_.Add(new KeyValuePair<string, string>("1", "a"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("A", output);
        }

        [Test]
        public void UcEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcNullArgs()
        {
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // Uc

        #region LcFirst

        [Test]
        public void LcFirstA()
        {
            TestProcessor("lcfirst:1=TEST", "tEST");

            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("tEST", output);
        }

        [Test]
        public void LcFirstB()
        {
            TestProcessor("lcfirst:1=test", "test");

            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcFirstSingleChar()
        {
            TestProcessor("lcfirst:1=A", "a");

            args_.Add(new KeyValuePair<string, string>("1", "A"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("a", output);
        }

        [Test]
        public void LcFirstEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcFirstNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcFirstNullArgs()
        {
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // LcFirst

        #region UcFirst

        [Test]
        public void UcFirstA()
        {
            TestProcessor("ucfirst:1=test", "Test");

            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("Test", output);
        }

        [Test]
        public void UcFirstB()
        {
            TestProcessor("ucfirst:1=TEST", "TEST");

            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcFirstSingleChar()
        {
            TestProcessor("ucfirst:1=a", "A");

            args_.Add(new KeyValuePair<string, string>("1", "a"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("A", output);
        }

        [Test]
        public void UcFirstEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcFirstNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcFirstNullArgs()
        {
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // UcFirst

        #region FullUrl

        [Test]
        public void FullUrlCategory()
        {
            TestProcessor("fullurl:Category:Top level", "http://en.wikipedia.org/wiki/Category:Top_level");
        }

        [Test]
        public void FullUrlQuery()
        {
            TestProcessor("fullurl:Category:Top level|action=edit", "http://en.wikipedia.org/w/index.php?title=Category:Top_level&action=edit");
        }

        #endregion // UcFirst

        [Test]
        public void Namespaces()
        {
            TestProcessor("TALKSPACE", string.Empty);
        }

        #endregion // tests

        public void TestProcessor(string input, string expected)
        {
            string command = MagicParser.GetMagicWordAndParams(input, out args_);

            string magicId = WikiSite.MagicWords.FindId(command);
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute(magicId, args_, out output));
            Assert.AreEqual(expected, output);
        }

        #region representation

        private MagicWordProcessor proc_;

        private List<KeyValuePair<string, string>> args_;

        private static readonly WikiSite WikiSite;

        #endregion // representation
    }
}

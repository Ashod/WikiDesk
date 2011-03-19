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
            wikiSite_ = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
        }

        [SetUp]
        public void Setup()
        {
            proc_ = new MagicWordProcessor(wikiSite_);
            proc_.SetContext(string.Empty, "TestPage");
            args_ = new List<KeyValuePair<string, string>>(4);
        }

        #region tests

        #region Lc

        [Test]
        public void LcA()
        {
            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcB()
        {
            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcSingleChar()
        {
            args_.Add(new KeyValuePair<string, string>("1", "A"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("a", output);
        }

        [Test]
        public void LcEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcNullArgs()
        {
            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lc", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // Lc

        #region Uc

        [Test]
        public void UcA()
        {
            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcB()
        {
            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcSingleChar()
        {
            args_.Add(new KeyValuePair<string, string>("1", "a"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("A", output);
        }

        [Test]
        public void UcEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcNullArgs()
        {
            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("uc", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // Uc

        #region LcFirst

        [Test]
        public void LcFirstA()
        {
            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("tEST", output);
        }

        [Test]
        public void LcFirstB()
        {
            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcFirstSingleChar()
        {
            args_.Add(new KeyValuePair<string, string>("1", "A"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("a", output);
        }

        [Test]
        public void LcFirstEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcFirstNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcFirstNullArgs()
        {
            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("lcfirst", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // LcFirst

        #region UcFirst

        [Test]
        public void UcFirstA()
        {
            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("Test", output);
        }

        [Test]
        public void UcFirstB()
        {
            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcFirstSingleChar()
        {
            args_.Add(new KeyValuePair<string, string>("1", "a"));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("A", output);
        }

        [Test]
        public void UcFirstEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcFirstNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcFirstNullArgs()
        {
            string output;
            Assert.AreEqual(MagicWordProcessor.Result.Found, proc_.Execute("ucfirst", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // UcFirst

        #endregion // tests

        #region representation

        private MagicWordProcessor proc_;

        private List<KeyValuePair<string, string>> args_;

        private static readonly WikiSite wikiSite_;

        #endregion // representation
    }
}

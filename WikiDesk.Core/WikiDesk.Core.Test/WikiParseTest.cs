namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class WikiParseTest
    {
        [Test]
        public void Header1()
        {
            const string WIKI_CODE = "=!=";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<h1><span class=\"mw-headline\">!</span></h1>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Header2()
        {
            const string WIKI_CODE = "==!==";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<h2><span class=\"mw-headline\">!</span></h2>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Header2NewLine()
        {
            const string WIKI_CODE = "==!==\n";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<h2><span class=\"mw-headline\">!</span></h2>\n";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Header2Pre()
        {
            const string WIKI_CODE = "blah blha \n==!==\n";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "blah blha \n<h2><span class=\"mw-headline\">!</span></h2>\n";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Header2PrePost()
        {
            const string WIKI_CODE = "blah blha \n==!==\nThe bigest mistkae.";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "blah blha \n<h2><span class=\"mw-headline\">!</span></h2>\nThe bigest mistkae.";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Italic()
        {
            const string WIKI_CODE = "''!''";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<i>!</i>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void BoldItalic()
        {
            const string WIKI_CODE = "'''''!'''''";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<i><b>!</b></i>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Bold()
        {
            const string WIKI_CODE = "'''!'''";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<b>!</b>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void BoldHeader3()
        {
            const string WIKI_CODE = "=='''!'''==";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<h2><span class=\"mw-headline\"><b>!</b></span></h2>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void ImageMinimal()
        {
            const string WIKI_CODE = "[[Image:Westminstpalace.jpg]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\"><img alt=\"Westminstpalace.jpg\" src=\"Westminstpalace.jpg\" width=\"400\" height=\"300\"></a>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Image()
        {
            const string WIKI_CODE = "[[Image:Westminstpalace.jpg|frame|none|alt=alt text|caption text]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">kiko</a>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Link()
        {
            const string WIKI_CODE = "[[Brazil|kiko]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">kiko</a>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void Redirect()
        {
            const string WIKI_CODE = "#REDIRECT [[Brazil]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">Brazil</a>";
            Assert.AreEqual(EXPECTED, html);
        }

    }
}

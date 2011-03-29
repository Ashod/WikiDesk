namespace WikiDesk.Core.Test
{
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    [TestFixture]
    public class WikiParseTest
    {
        static WikiParseTest()
        {
            WikiDomain wikiDomain = new WikiDomain("wikipedia");
            WikiLanguage wikiLanguage = new WikiLanguage("English", "en");
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WikiSite wikiSite = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
            config_ = new Configuration(wikiSite);
        }

        #region Header

        [Test]
        public void Header1()
        {
            TestConvert(
                    "=!=",
                    "<h1><span class=\"mw-headline\">!</span></h1>");
        }

        [Test]
        public void Header2()
        {
            TestConvert(
                    "==!==",
                    "<h2><span class=\"mw-headline\">!</span></h2>");
        }

        [Test]
        public void Header2NewLine()
        {
            TestConvert(
                    "==!==\n",
                    "<h2><span class=\"mw-headline\">!</span></h2>");
        }

        [Test]
        public void Header2Pre()
        {
            TestConvert(
                    "blah blha \n==!==\n",
                    "<p>blah blha </p><h2><span class=\"mw-headline\">!</span></h2>");
        }

        [Test]
        public void Header2PrePost()
        {
            TestConvert(
                    "blah blha \n==!==\nThe bigest mistkae.",
                    "<p>blah blha </p><h2><span class=\"mw-headline\">!</span></h2><p>The bigest mistkae.</p>");
        }

        #endregion // Header

        #region Bold/Italic

        [Test]
        public void Italic()
        {
            TestConvert(
                    "''!''",
                    "<p><i>!</i></p>");
        }

        [Test]
        public void BoldItalic()
        {
            TestConvert(
                    "'''''!'''''",
                    "<p><i><b>!</b></i></p>");
        }

        [Test]
        public void Bold()
        {
            TestConvert(
                    "'''!'''",
                    "<p><b>!</b></p>");
        }

        [Test]
        public void BoldHeader3()
        {
            TestConvert(
                    "=='''!'''==",
                    "<h2><span class=\"mw-headline\"><b>!</b></span></h2>");
        }

        #endregion // Bold/Italic

        #region Unordered Lists

        [Test]
        public void UnorderedListSimple()
        {
            TestConvert(
                    "* One list entry.",
                    "<ul><li>One list entry.</li></ul>");
        }

        [Test]
        public void UnorderedListSimple2()
        {
            TestConvert(
                    "* Two list entries.\n" +
                    "* Another One.",
                    "<ul><li>Two list entries.</li><li>Another One.</li></ul>");
        }

        [Test]
        public void UnorderedListWithSubList()
        {
            TestConvert(
                    "* One list entry.\n" +
                    "** With one sublist." +
                    "** Make that two.",
                    "<ul><li>One list entry.</li><ul><li>With one sublist.</li><li>Make that two.</li></ul></ul>");
        }

        [Test]
        public void UnorderedListsComplex()
        {
            TestConvert(
                    "# Numbered lists are:" +
                    "## Very organized" +
                    "## Easy to follow" +
                    "#: Previous item continues" +
                    "A new line marks the end of the list." +
                    "# New numbering starts with 1.",
                    "<ul><li><i>Unordered lists</i> are easy to do:<ul><li>Start every line with a star.<ul><li>More stars indicate a deeper level.</li></ul></li></ul><dl><dd>Previous item continues.</dd></dl><ul><li>A new line</li></ul></li><li>in a list</li></ul><p>marks the end of the list.</p><ul><li>Of course you can start again.</li></ul>");
        }

        #endregion // Unordered Lists

        #region Unordered Lists

        [Test]
        public void OrderedListSimple()
        {
            TestConvert(
                    "# One list entry.",
                    "<ol><li>One list entry.</li></ol>");
        }

        [Test]
        public void OrderedListSimple2()
        {
            TestConvert(
                    "# Two list entries.\n" +
                    "# Another One.",
                    "<ol><li>Two list entries.</li><li>Another One.</li></ol>");
        }

        [Test]
        public void OrderedListWithSubList()
        {
            TestConvert(
                    "# One list entry.\n" +
                    "## With one sublist." +
                    "## Make that two.",
                    "<ol><li>One list entry.</li><ol><li>With one sublist.</li><li>Make that two.</li></ol></ol>");
        }

        [Test]
        public void OrderedListsComplex()
        {
            TestConvert(
                    "* ''Unordered lists'' are easy to do:" +
                    "** Start every line with a star." +
                    "*** More stars indicate a deeper level." +
                    "*: Previous item continues." +
                    "** A new line" +
                    "* in a list" +
                    "marks the end of the list." +
                    "* Of course you can start again.",
                    "<ul><li><i>Unordered lists</i> are easy to do:<ul><li>Start every line with a star.<ul><li>More stars indicate a deeper level.</li></ul></li></ul><dl><dd>Previous item continues.</dd></dl><ul><li>A new line</li></ul></li><li>in a list</li></ul><p>marks the end of the list.</p><ul><li>Of course you can start again.</li></ul>");
        }

        #endregion // Ordered Lists

        [Test]
        public void ExtLink()
        {
            TestConvert("[http://www.wikipedia.org WikiPipi]",
                "<p><a href=\"http://www.wikipedia.org\" title=\"http://www.wikipedia.org\">WikiPipi</a></p>");
        }

        [Test]
        public void ParserFunctionSimple()
        {
            TestConvert("{{lc:KIKOS}}",
                "<p>kikos</p>");
        }

        [Test]
        public void Link()
        {
            TestConvert(
                    "[[Brazil|kiko]]",
                    "<p><a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\" class=\"mw-redirect\">kiko</a></p>");
        }

        [Test]
        public void Redirect()
        {
            TestConvert(
                    "#REDIRECT [[Brazil]]",
                    "Redirected to <span class=\"redirectText\"><a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">Brazil</a></span>");
        }

        [Test]
        public void RedirectColon()
        {
            TestConvert(
                    "#REDIRECT: [[Brazil]]",
                    "Redirected to <span class=\"redirectText\"><a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">Brazil</a></span>");
        }

        internal static void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html(config_);
            string html = converter.Convert(string.Empty, "TestPage", wikicode);
            Assert.AreEqual(expected, html);
        }

        #region representation

        private static readonly Configuration config_;

        #endregion // representation

    }
}

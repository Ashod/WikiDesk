namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class WikiParseTest
    {
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
        public void ParserTemplateExclaim()
        {
            TestConvert("{{!}}",
                "|");
        }

        [Test]
        public void ParserTemplateExclaimExplicit()
        {
            TestConvert("{{Template:!}}",
                "|");
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
                    "<p>Redirected to <span class=\"redirectText\"><a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">Brazil</a></span></p>");
        }

        [Test]
        public void RedirectColon()
        {
            TestConvert(
                    "#REDIRECT: [[Brazil]]",
                    "<p>Redirected to <span class=\"redirectText\"><a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">Brazil</a></span></p>");
        }

        internal static void TestConvert(string wikicode, string expected)
        {
            Configuration config = new Configuration(
                                        "en",
                                        ".wikipedia.org/wiki/",
                                        ".wikipedia.org/wiki/Special:Export");

            Wiki2Html converter = new Wiki2Html(config);
            string html = converter.Convert(wikicode);
            Assert.AreEqual(expected, html);
        }
    }
}

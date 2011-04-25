﻿namespace WikiDesk.Core.Test
{
    using System;
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
                    "<h1><span class=\"mw-headline\" id=\"a_.21\">!</span></h1>" + Environment.NewLine);
        }

        [Test]
        public void Header2()
        {
            TestConvert(
                    "==!==",
                    "<h2><span class=\"mw-headline\" id=\"a_.21\">!</span></h2>" + Environment.NewLine);
        }

        [Test]
        public void Header3()
        {
            TestConvert(
                    "===!===",
                    "<h3><span class=\"mw-headline\" id=\"a_.21\">!</span></h3>" + Environment.NewLine);
        }

        [Test]
        public void Header4()
        {
            TestConvert(
                    "====!====",
                    "<h4><span class=\"mw-headline\" id=\"a_.21\">!</span></h4>" + Environment.NewLine);
        }

        [Test]
        public void Header5()
        {
            TestConvert(
                    "=====!=====",
                    "<h5><span class=\"mw-headline\" id=\"a_.21\">!</span></h5>" + Environment.NewLine);
        }

        [Test]
        public void Header6()
        {
            TestConvert(
                    "======!======",
                    "<h6><span class=\"mw-headline\" id=\"a_.21\">!</span></h6>" + Environment.NewLine);
        }

        [Test]
        public void HeaderInvalid()
        {
            TestConvert(
                    "=!======",
                    "=!======");
        }

        [Test]
        public void Header2NewLine()
        {
            TestConvert(
                    "==!==\n",
                    "<h2><span class=\"mw-headline\" id=\"a_.21\">!</span></h2>" + Environment.NewLine);
        }

        [Test]
        public void Header2Pre()
        {
            TestConvert(
                    "blah blha \n==!==\n",
                    "<p>\r\nblah blha \r\n</p>\r\n<h2><span class=\"mw-headline\" id=\"a_.21\">!</span></h2>" + Environment.NewLine);
        }

        [Test]
        public void Header2PrePost()
        {
            TestConvert(
                    "blah blha \n==!==  \nThe bigest mistkae.",
                    "<p>\r\nblah blha \r\n</p>\r\n<h2><span class=\"mw-headline\" id=\"a_.21\">!</span></h2>\r\n<p>\r\n  The bigest mistkae.\r\n</p>\r\n");
        }

        #endregion // Header

        #region Bold/Italic

        [Test]
        public void Italic()
        {
            TestConvert(
                    "''!''",
                    "<p>\r\n<i>!</i>\r\n</p>\r\n");
        }

        [Test]
        public void BoldItalic()
        {
            TestConvert(
                    "'''''!'''''",
                    "<p>\r\n<i><b>!</b></i>\r\n</p>\r\n");
        }

        [Test]
        public void Bold()
        {
            TestConvert(
                    "'''!'''",
                    "<p>\r\n<b>!</b>\r\n</p>\r\n");
        }

        [Test]
        public void Nested1()
        {
            TestConvert(
                    "'''''The '''red''' fox.'''''",
                    "<p>\r\n<i><b>The</b> red <b>fox.</b></i>\r\n</p>\r\n");
        }

        [Test]
        public void Nested2()
        {
            TestConvert(
                    "'''''The '''red''",
                    "<p>\r\n<i><b>The</b> red</i>\r\n</p>\r\n");
        }

        [Test]
        public void BoldHeader3()
        {
            TestConvert(
                    "=='''!'''==",
                    "<h2><span class=\"mw-headline\" id=\"a_.21\"><b>!</b></span></h2>" + Environment.NewLine);
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
                    "<ul><li><i>Unordered lists</i> are easy to do:<ul><li>Start every line with a star.<ul><li>More stars indicate a deeper level.</li></ul></li></ul><dl><dd>Previous item continues.</dd></dl><ul><li>A new line</li></ul></li><li>in a list</li></ul><p>\r\nmarks the end of the list.</p><ul><li>Of course you can start again.</li></ul>");
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
                    "<ul><li><i>Unordered lists</i> are easy to do:<ul><li>Start every line with a star.<ul><li>More stars indicate a deeper level.</li></ul></li></ul><dl><dd>Previous item continues.</dd></dl><ul><li>A new line</li></ul></li><li>in a list</li></ul><p>\r\nmarks the end of the list.</p><ul><li>Of course you can start again.</li></ul>");
        }

        #endregion // Ordered Lists

        [Test]
        public void ExtLink()
        {
            TestConvert("[http://www.wikipedia.org WikiPipi]",
                "<p>\r\n<a href=\"http://www.wikipedia.org\" title=\"http://www.wikipedia.org\">WikiPipi</a>\r\n</p>\r\n");
        }

        [Test]
        public void ParserFunctionSimple()
        {
            TestConvert("{{lc:KIKOS}}",
                "<p>\r\nkikos\r\n</p>\r\n");
        }

        [Test]
        public void Link()
        {
            TestConvert(
                    "[[Brazil|kiko]]",
                    "<p>\r\n<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\" class=\"mw-redirect\">kiko</a>\r\n</p>\r\n");
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
            string nameSpace = string.Empty;
            string title = "TestPage";
            string html = converter.Convert(ref nameSpace, ref title, wikicode);
            Assert.AreEqual(expected, html);
        }

        #region representation

        private static readonly Configuration config_;

        #endregion // representation

    }
}

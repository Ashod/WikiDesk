﻿// -----------------------------------------------------------------------------------------
// <copyright file="WikiParseTest.cs" company="ashodnakashian.com">
//
// This file is part of WikiDesk.
// Copyright (C) 2010, 2011 Ashod Nakashian
// https://github.com/Ashod/WikiDesk
//
//  WikiDesk is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  WikiDesk is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with WikiDesk. If not, see http://www.gnu.org/licenses/.
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the WikiParseTest type.
// </summary>
// -----------------------------------------------------------------------------------------

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
                    "<h1><span class=\"mw-headline\" id=\"a_.21\">!</span></h1>");
        }

        [Test]
        public void Header2()
        {
            TestConvert(
                    "==!==",
                    "<h2><span class=\"mw-headline\" id=\"a_.21\">!</span></h2>");
        }

        [Test]
        public void Header3()
        {
            TestConvert(
                    "===!===",
                    "<h3><span class=\"mw-headline\" id=\"a_.21\">!</span></h3>");
        }

        [Test]
        public void Header4()
        {
            TestConvert(
                    "====!====",
                    "<h4><span class=\"mw-headline\" id=\"a_.21\">!</span></h4>");
        }

        [Test]
        public void Header5()
        {
            TestConvert(
                    "=====!=====",
                    "<h5><span class=\"mw-headline\" id=\"a_.21\">!</span></h5>");
        }

        [Test]
        public void Header6()
        {
            TestConvert(
                    "======!======",
                    "<h6><span class=\"mw-headline\" id=\"a_.21\">!</span></h6>");
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
                    "<h2><span class=\"mw-headline\" id=\"a_.21\">!</span></h2>");
        }

        [Test]
        public void Header2Pre()
        {
            TestConvert(
                    "blah blha \n==!==\n",
                    "<p>blah blha</p>\r\n<h2><span class=\"mw-headline\" id=\"a_.21\">!</span></h2>");
        }

        [Test]
        public void Header2PrePost()
        {
            TestConvert(
                    "blah blha \n==!==  \nThe bigest mistkae.",
                    "<p>blah blha</p>\r\n<h2><span class=\"mw-headline\" id=\"a_.21\">!</span></h2>\r\n<p>The bigest mistkae.</p>");
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
        public void Nested1()
        {
            TestConvert(
                    "'''''The '''red''' fox.'''''",
                    "<p><i><b>The</b> red <b>fox.</b></i></p>");
        }

        [Test]
        public void Nested2()
        {
            TestConvert(
                    "'''''The '''red''",
                    "<p><i><b>The</b> red</i></p>");
        }

        [Test]
        public void BoldHeader3()
        {
            TestConvert(
                    "=='''!'''==",
                    "<h2><span class=\"mw-headline\" id=\"a_.21\"><b>!</b></span></h2>");
        }

        #endregion // Bold/Italic

        #region Indent

        [Test]
        public void IndentSingle()
        {
            TestConvert(
                    ":blo",
                    "<dl>\r\n<dd>blo</dd>\r\n</dl>");
        }

        [Test]
        public void IndentDouble()
        {
            TestConvert(
                    "::blo",
                    "<dl>\r\n<dd>\r\n<dl>\r\n<dd>blo</dd>\r\n</dl>\r\n</dd>\r\n</dl>");
        }

        [Test]
        public void IndentTripple()
        {
            TestConvert(
                    ":::blo",
                    "<dl>\r\n<dd>\r\n<dl>\r\n<dd>\r\n<dl>\r\n<dd>blo</dd>\r\n</dl>\r\n</dd>\r\n</dl>\r\n</dd>\r\n</dl>");
        }

        [Test]
        public void IndentQuad()
        {
            TestConvert(
                    "::::blo",
                    "<dl>\r\n<dd>\r\n<dl>\r\n<dd>\r\n<dl>\r\n<dd>\r\n<dl>\r\n<dd>blo</dd>\r\n</dl>\r\n</dd>\r\n</dl>\r\n</dd>\r\n</dl>\r\n</dd>\r\n</dl>");
        }

        [Test]
        public void IndentKlongon()
        {
            TestConvert(
                    ":''This article is about the Klingon \"race\". For the \"language\" see [[Klingon language]].''",
                    "<dl>\r\n<dd><i>This article is about the Klingon \"race\". For the \"language\" see <a href=\"http://en.wikipedia.org/wiki/Klingon_language\" title=\"Klingon language\">Klingon language</a>.</i></dd>\r\n</dl>");
        }

        #endregion // Indent

        #region Unordered Lists

        [Test]
        public void UnorderedListSimple()
        {
            TestConvert(
                    "* One list entry.",
                    "<ul>\r\n<li>One list entry.</li>\r\n</ul>");
        }

        [Test]
        public void UnorderedListSimple2()
        {
            TestConvert(
                    "* Two list entries.\n" +
                    "* Another One.",
                    "<ul>\r\n<li>Two list entries.</li>\r\n<li>Another One.</li>\r\n</ul>");
        }

        [Test]
        public void UnorderedListSimple3()
        {
            TestConvert(
                    "* Two list entries.\n" +
                    "** Another One.",
                    "<ul>\r\n<li>Two list entries.\r\n<ul>\r\n<li>Another One.</li>\r\n</ul>\r\n</li>\r\n</ul>");
        }

        [Test]
        public void UnorderedListSimple4()
        {
            // As Wikipedia.
            TestConvert(
                "Blah blah List\n" +
                "* first.\n" +
                "* second.\n" +
                "* last.\n" +
                "Other text.",
                "<p>Blah blah List</p>\r\n<ul>\r\n<li>first.</li>\r\n<li>second.</li>\r\n<li>last.</li>\r\n</ul>\r\n<p>Other text.</p>");
        }

        [Test]
        public void UnorderedListWithSubList()
        {
            TestConvert(
                    "* One list entry.\n" +
                    "** With one sublist.\n" +
                    "** Make that two.",
@"<ul>
<li>One list entry.
<ul>
<li>With one sublist.</li>
<li>Make that two.</li>
</ul>
</li>
</ul>");
        }

        [Test]
        public void UnorderedListBad()
        {
            TestConvert(
                    "** one\n" +
                    "**two",
                    "<ul>\r\n<li>\r\n<ul>\r\n<li>one</li>\r\n<li>two</li>\r\n</ul>\r\n</li>\r\n</ul>");
        }

        [Test]
        public void UnorderedListBad2()
        {
            TestConvert(
                    "**** Four.\n" +
                    "* One.\n" +
                    "**** 4.",
@"<ul>
<li>
<ul>
<li>
<ul>
<li>
<ul>
<li>Four.</li>
</ul>
</li>
</ul>
</li>
</ul>
</li>
<li>One.
<ul>
<li>
<ul>
<li>
<ul>
<li>4.</li>
</ul>
</li>
</ul>
</li>
</ul>
</li>
</ul>");
        }

        [Test]
        public void UnorderedListMultiple()
        {
            // As Wikipedia.
            TestConvert(
                "Blah blah List\n" +
                "* first.\n" +
                "** one.\n" +
                "**two.\n" +
                "* second.\n" +
                "** 1.\n" +
                "***2.\n" +
                "* last.\n" +
                "Other text.",
@"<p>Blah blah List</p>
<ul>
<li>first.
<ul>
<li>one.</li>
<li>two.</li>
</ul>
</li>
<li>second.
<ul>
<li>1.
<ul>
<li>2.</li>
</ul>
</li>
</ul>
</li>
<li>last.</li>
</ul>
<p>Other text.</p>");
        }

        [Test]
        public void UnorderedListSkewed()
        {
            // As Wikipedia.
            TestConvert(
                "Blah blah List\n" +
                "* first.\n" +
                "** second.\n" +
                "*** third.\n" +
                "**** fourth.\n" +
                "***** fifth.",
@"<p>Blah blah List</p>
<ul>
<li>first.
<ul>
<li>second.
<ul>
<li>third.
<ul>
<li>fourth.
<ul>
<li>fifth.</li>
</ul>
</li>
</ul>
</li>
</ul>
</li>
</ul>
</li>
</ul>");
        }

        [Test]
        public void UnorderedListsComplex()
        {
            TestConvert(
                "* ''Unordered lists'' are easy to do:\n" +
                "** Start every line with a star.\n" +
                "*** More stars indicate a deeper level.\n" +
                "*: Previous item continues.\n" +
                "** A new line\n" +
                "* in a list\n" +
                "marks the end of the list.\n" +
                "* Of course you can start again.",
                "<ul>\r\n<li><i>Unordered lists</i> are easy to do:\r\n<ul>\r\n<li>Start every line with a star.\r\n<ul>\r\n<li>More stars indicate a deeper level.</li>\r\n</ul>\r\n</li>\r\n</ul>\r\n<dl>\r\n<dd>Previous item continues.</dd>\r\n</dl>\r\n<ul>\r\n<li>A new line</li>\r\n</ul>\r\n</li>\r\n<li>in a list</li>\r\n</ul>\r\n<p>marks the end of the list.</p>\r\n<ul>\r\n<li>Of course you can start again.</li>\r\n</ul>");
        }

        #endregion // Unordered Lists

        #region Ordered Lists

        [Test]
        public void OrderedListSimple()
        {
            TestConvert(
                    "# One list entry.",
                    "<ol>\r\n<li>One list entry.</li>\r\n</ol>");
        }

        [Test]
        public void OrderedListSimple2()
        {
            TestConvert(
                    "# Two list entries.\n" +
                    "# Another One.",
                    "<ol>\r\n<li>Two list entries.</li>\r\n<li>Another One.</li>\r\n</ol>");
        }

        [Test]
        public void OrderedListWithSubList()
        {
            TestConvert(
                    "# One list entry.\n" +
                    "## With one sublist.\n" +
                    "## Make that two.",
                    "<ol>\r\n<li>One list entry.\r\n<ol>\r\n<li>With one sublist.</li>\r\n<li>Make that two.</li>\r\n</ol>\r\n</li>\r\n</ol>");
        }

        [Test]
        public void OrderedListsComplex1()
        {
            TestConvert(
                    "# Numbered lists are:\n" +
                    "## Very organized\n" +
                    "## Easy to follow\n" +
                    "#: Previous item continues\n" +
                    "A new line marks the end of the list.\n" +
                    "# New numbering starts with 1.",
                    "<ol>\r\n<li>Numbered lists are:\r\n<ol>\r\n<li>Very organized</li>\r\n<li>Easy to follow</li>\r\n</ol>\r\n<dl>\r\n<dd>Previous item continues</dd>\r\n</dl>\r\n</li>\r\n</ol>\r\n<p>A new line marks the end of the list.</p>\r\n<ol>\r\n<li>New numbering starts with 1.</li>\r\n</ol>");
        }

        [Test]
        public void OrderedListsComplex2()
        {
            TestConvert(
                    "# Numbered lists are:\r\n## Very organized\r\n### Easy to follow\r\n#: Previous item continues\r\n#: A new line marks the end o\r\n## kik\r\n",
                    "<ol>\r\n<li>Numbered lists are:\r\n<ol>\r\n<li>Very organized\r\n<ol>\r\n<li>Easy to follow</li>\r\n</ol>\r\n</li>\r\n</ol>\r\n<dl>\r\n<dd>Previous item continues</dd>\r\n<dd>A new line marks the end o</dd>\r\n</dl>\r\n<ol>\r\n<li>kik</li>\r\n</ol>\r\n</li>\r\n</ol>");
        }

        [Test]
        public void OrderedListsComplex3()
        {
            TestConvert(
                    "# Numbered lists are:\r\n## Very organized\r\n### Easy to follow\r\n##: Continues\r\n### Third\r\n## Second\r\n#: First Cont\r\n## kik\r\n",
                    "<ol>\r\n<li>Numbered lists are:\r\n<ol>\r\n<li>Very organized\r\n<ol>\r\n<li>Easy to follow</li>\r\n</ol>\r\n<dl>\r\n<dd>Continues</dd>\r\n</dl>\r\n<ol>\r\n<li>Third</li>\r\n</ol>\r\n</li>\r\n<li>Second</li>\r\n</ol>\r\n<dl>\r\n<dd>First Cont</dd>\r\n</dl>\r\n<ol>\r\n<li>kik</li>\r\n</ol>\r\n</li>\r\n</ol>");
        }

        [Test]
        public void OrderedListsComplex4()
        {
            TestConvert(
                    "# Numbered lists are:\r\n## Very organized\r\n### Easy to follow\r\n####: Continues\r\n### Third\r\n## Second\r\n#: First Cont\r\n## kik\r\n",
                    "<ol>\r\n<li>Numbered lists are:\r\n<ol>\r\n<li>Very organized\r\n<ol>\r\n<li>Easy to follow\r\n<ol>\r\n<li>\r\n<dl>\r\n<dd>Continues</dd>\r\n</dl>\r\n</li>\r\n</ol>\r\n</li>\r\n<li>Third</li>\r\n</ol>\r\n</li>\r\n<li>Second</li>\r\n</ol>\r\n<dl>\r\n<dd>First Cont</dd>\r\n</dl>\r\n<ol>\r\n<li>kik</li>\r\n</ol>\r\n</li>\r\n</ol>");
        }

        #endregion // Ordered Lists

        #region Pre

        [Test]
        public void PreSingle1()
        {
            TestConvert(
                " h",
                "<pre>h\r\n</pre>");
        }

        [Test]
        public void PreSingle2()
        {
            TestConvert(
                "   an ''this''   is   italic      ",
                "<pre>  an <i>this</i>   is   italic\r\n</pre>");
        }

        [Test]
        public void PreMulti()
        {
            TestConvert(
                " h\r\n y\r\n  z",
                "<pre>h\r\ny\r\n z\r\n</pre>");
        }

        #endregion // Pre

        #region Hr

        [Test]
        public void Hr()
        {
            TestConvert(
                "----",
                "<hr>");
        }

        [Test]
        public void HrLong()
        {
            TestConvert(
                "---------------------",
                "<hr>");
        }

        [Test]
        public void HrText()
        {
            TestConvert(
                "---------------------hi",
                "<hr>\r\n<p>hi</p>");
        }

        #endregion // Hr

        [Test]
        public void ExtLink()
        {
            TestConvert("[http://www.wikipedia.org WikiPipi]",
                "<p><a href=\"http://www.wikipedia.org\" class=\"external text\" rel=\"nofollow\">WikiPipi</a></p>");
        }

        [Test]
        public void Link()
        {
            TestConvert(
                    "[[Brazil|kiko]]",
                    "<p><a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">kiko</a></p>");
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

        [Test]
        public void ParserFunctionSimple()
        {
            TestConvert("{{lc:KIKOS}}",
                "<p>kikos</p>");
        }

        [Test]
        public void DefaultNamespace()
        {
            TestConvert(
                    "{{NAMESPACE}}",
                    string.Empty);
        }

        [Test]
        public void Ns()
        {
            TestConvert(
                    "{{ns:0}}",
                    string.Empty);
            TestConvert(
                    "{{ns:4}}",
                    "<p>Wikipedia</p>");
        }

        [Test]
        public void Magic()
        {
            TestConvert(
                    "{{#ifeq:{{NAMESPACE}}|{{ns:0}}|article|page}}",
                    "<p>article</p>");
        }


        [Test]
        public void FullUrlE()
        {
            TestConvert(
                    "{{fullurl:Special:Allpages|namespace=12&from={{PAGENAMEE}}}}",
                    "<p>http://en.wikipedia.org/w/index.php?title=Special:Allpages&namespace=12&from=TestPage</p>");
        }
        
        [Test]
        public void FullUrl()
        {
            TestConvert(
                    "{{fullurl:Special:Allpages/{{PAGENAME}}|namespace=12}}",
                    "<p>http://en.wikipedia.org/w/index.php?title=Special:Allpages/TestPage&namespace=12</p>");
        }

        #region implementation

        internal static void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html(config_);
            string nameSpace = string.Empty;
            string title = "TestPage";
            string html = converter.Convert(ref nameSpace, ref title, wikicode);
            Assert.AreEqual(expected, html);
        }

        #endregion // implementation

        #region representation

        private static readonly Configuration config_;

        #endregion // representation

    }
}

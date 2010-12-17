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

            //const string EXPECTED = "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\"><img alt=\"Westminstpalace.jpg\" src=\"Westminstpalace.jpg\" width=\"400\" height=\"300\"></a>";
            const string EXPECTED = "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\"></a>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void ImageComplex()
        {
            const string WIKI_CODE = "[[File:Independência ou Morte.jpg|thumb|left|Declaration of the [[Brazilian Declaration of Independence|Brazilian independence]] by Emperor [[Pedro I of Brazil|Pedro I]] on 7 September 1822.]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

//             const string EXPECTED = "<div class="thumb tleft">
// <div class="thumbinner" style="width:222px;"><a href="/wiki/File:Independ%C3%AAncia_ou_Morte.jpg" class="image"><img alt="" src="http://upload.wikimedia.org/wikipedia/commons/thumb/c/ce/Independ%C3%AAncia_ou_Morte.jpg/220px-Independ%C3%AAncia_ou_Morte.jpg" width="220" height="108" class="thumbimage"></a>
// <div class="thumbcaption">
// <div class="magnify"><a href="/wiki/File:Independ%C3%AAncia_ou_Morte.jpg" class="internal" title="Enlarge"><img src="http://bits.wikimedia.org/skins-1.5/common/images/magnify-clip.png" width="15" height="11" alt=""></a></div>
// Declaration of the <a href="/wiki/Brazilian_Declaration_of_Independence" title="Brazilian Declaration of Independence" class="mw-redirect">Brazilian independence</a> by Emperor <a href="/wiki/Pedro_I_of_Brazil" title="Pedro I of Brazil">Pedro I</a> on 7 September 1822.</div>
// </div>
// </div>";
//             Assert.AreEqual(EXPECTED, html);
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

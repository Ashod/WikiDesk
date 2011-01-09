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

        #region Image

        [Test]
        public void ListSimple()
        {
            TestConvert(
                "Blah blah List\n" +
                "* first.\n" +
                "* second.\n" +
                "* last.\n" +
                "Other text.",
                "<p>Blah blah List</p><ul><li>first.</li><li>second.</li><li>last.</li></ul><p>Other text.</p>");
        }

        [Test]
        public void ListMultiple()
        {
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
                "<p>Blah blah List</p><ul><li>first.</li><ul><li>one.</li><li>two.</li></ul><li>second.</li><ul><li>1.</li><ul><li>2.</li></ul></ul><li>last.</li></ul><p>Other text.</p>");
        }

        [Test]
        public void ListSkewed()
        {
            TestConvert(
                "Blah blah List\n" +
                "* first.\n" +
                "** second.\n" +
                "*** third.\n" +
                "**** fourth.\n" +
                "***** fifth.",
                "<p>Blah blah List</p><ul><li>first.</li><ul><li>second.</li><ul><li>third.</li><ul><li>fourth.</li><ul><li>fifth.</li></ul></ul></ul></ul></ul>");
        }

        [Test]
        public void ImageMinimal()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageCaption()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="captione texte" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|captione texte]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"captione texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageAlt()
        {
//         [[Image:Westminstpalace.jpg|alt=alternate texte]]
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="alternate texte" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|alt=alternate texte]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"alternate texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageAltCaption()
        {
//         [[Image:Westminstpalace.jpg|alt=alternate text|captione texte]]
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="alternate text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|alt=alternate text|captione texte]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"alternate text\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageBorder()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|border]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" class=\"thumbborder\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageBorderAlt()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="alternate text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|border|alt=alternate text]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"alternate text\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" class=\"thumbborder\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageBorderCaption()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="captione texte" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|border|captione texte]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"captione texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" class=\"thumbborder\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageFrameless()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|frameless]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageFramelessRight()
        {
//         <div class="floatright">
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </div>

            TestConvert(
                "[[Image:Westminstpalace.jpg|right|frameless]]",
                "<div class=\"floatright\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</div>");
        }

        [Test]
        public void ImageFramelessLeft()
        {
//         <div class="floatleft">
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </div>

            TestConvert(
                "[[Image:Westminstpalace.jpg|left|frameless]]",
                "<div class=\"floatleft\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</div>");
        }

        [Test]
        public void ImageFramelessNone()
        {
//         <div class="floatnone">
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </div>

            TestConvert(
                "[[Image:Westminstpalace.jpg|none|frameless]]",
                "<div class=\"floatnone\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</div>");
        }

        [Test]
        public void ImageFramelessCenter()
        {
//         <div class="center">
//             <div class="floatnone">
//                 <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                     <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//                 </a>
//             </div>
//         </div>

            TestConvert(
                "[[Image:Westminstpalace.jpg|center|frameless]]",
                "<div class=\"center\">" +
                    "<div class=\"floatnone\">" +
                        "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                            "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                        "</a>" +
                    "</div>" +
                "</div>");
        }

        [Test]
        public void ImageFramelessAlt()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="alternate texte" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|frameless|alt=alternate texte]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"alternate texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageFramelessCaption()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|frameless|captione texte]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"captione texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageFramelessAltCaption()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="alternate texte" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </p>

            TestConvert(
                "[[Image:Westminstpalace.jpg|frameless|alt=alternate texte|captione texte]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"alternate texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageNone()
        {
//             <div class="floatnone">
//                 <a href="/wiki/File:Westminstpalace.jpg" class="image" title="caption text">
//                     <img alt="alt text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300">
//                 </a>
//             </div>

            TestConvert(
                "[[Image:Westminstpalace.jpg|none]]",
                "<div class=\"floatnone\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                "</div>");
        }

        [Test]
        public void ImageUpright()
        {
//         <div class="thumb tright">
//             <div class="thumbinner" style="width:172px;">
//                 <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                     <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/170px-Westminstpalace.jpg" width="170" height="128" class="thumbimage">
//                 </a>
//                 <div class="thumbcaption">
//                     <div class="magnify">
//                         <a href="/wiki/File:Westminstpalace.jpg" class="internal" title="Enlarge">
//                             <img src="http://bits.wikimedia.org/skins-1.5/common/images/magnify-clip.png" width="15" height="11" alt="">
//                         </a>
//                     </div>
//                 </div>
//             </div>
//         </div>

            TestConvert(
                "[[Image:Westminstpalace.jpg|upright=|thumb]]",
                "<div class=\"thumb tright\">" +
                "<div class=\"thumbinner\" style=\"width:172px;\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"170\" class=\"thumbimage\">" +
                    "</a>" +
                "</div>" +
                "</div>");
        }

        [Test]
        [Ignore]
        public void ImageSize()
        {
            TestConvert(
                "[[Image:Westminstpalace.jpg|thumb|14x13px]]",
                "<div class=\"thumb tright\">" +
                "<div class=\"thumbinner\" style=\"width:172px;\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"170\" class=\"thumbimage\">" +
                    "</a>" +
                "</div>" +
                "</div>");
        }

        [Test]
        [Ignore]
        public void Image()
        {
            const string WIKI_CODE = "[[Image:Westminstpalace.jpg|frame|none|alt=alt text|caption text]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

//             [[Image:Westminstpalace.jpg|frame|none|alt=alt text|caption text]]
//             <div class="thumb tnone">
//                 <div class="thumbinner" style="width:402px;">
//                     <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                         <img alt="alt text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbimage">
//                     </a>
//                     <div class="thumbcaption">caption text</div>
//                 </div>
//             </div>


//             [[Image:Westminstpalace.jpg|frame|alt=alt text|caption text]]
//             <div class="thumb tright">
//                 <div class="thumbinner" style="width:402px;">
//                     <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                         <img alt="alt text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbimage">
//                     </a>
//                     <div class="thumbcaption">caption text</div>
//                 </div>
//             </div>

//             [[Image:Westminstpalace.jpg|alt=alt text|caption text]]
//             <p>
//                 <a href="/wiki/File:Westminstpalace.jpg" class="image" title="caption text">
//                     <img alt="alt text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300">
//                 </a>
//             </p>

//             [[Image:Westminstpalace.jpg|frame|border|none|alt=alt text|caption text]]
//             <div class="thumb tnone">
//                 <div class="thumbinner" style="width:402px;">
//                     <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                         <img alt="alt text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbimage">
//                     </a>
//                     <div class="thumbcaption">caption text</div>
//                 </div>
//             </div>

            const string EXPECTED = "<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">kiko</a>";
            Assert.AreEqual(EXPECTED, html);
        }

        #endregion // Image

        [Test]
        public void ExtLink()
        {
            TestConvert("[http://www.wikipedia.org WikiPipi]",
                "<p><a href=\"http://www.wikipedia.org\" title=\"http://www.wikipedia.org\">WikiPipi</a></p>");
        }

        #region templates

        [Test]
        public void TemplateLang()
        {
            TestConvert("{{lang-ka|kikos}}",
                "<p><span lang=\"ka\" xml:lang=\"ka\">kikos</span></p>");
        }

        [Test]
        public void TemplateMain()
        {
            TestConvert("{{Main|History of Tbilisi}}",
                "<div class=\"rellink relarticle mainarticle\">Main article: <a href=\"http://en.wikipedia.org/wiki/History_of_Tbilisi\" title=\"History of Tbilisi\">History of Tbilisi</a></div>");
        }

        [Test]
        public void TemplateOCLC()
        {
            TestConvert("{{OCLC|224781861}}",
                "<p><a href=\"http://en.wikipedia.org/wiki/Online_Computer_Library_Center\" title=\"Online Computer Library Center\">OCLC</a>" +
                "&nbsp;<a href=\"http://www.worldcat.org/oclc/224781861\" class=\"external text\" rel=\"nofollow\">224781861</a></p>");
        }

        #endregion // templates

        [Test]
        public void Link()
        {
            TestConvert(
                    "[[Brazil|kiko]]",
                    "<p><a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\" class=\"mw-redirect\">kiko</a></p>");
        }

        [Test]
        [Ignore]
        public void ImageComplex()
        {
            TestConvert(
                    "[[File:Independência ou Morte.jpg|thumb|left|Declaration of the [[Brazilian Declaration of Independence|Brazilian independence]] by Emperor [[Pedro I of Brazil|Pedro I]] on 7 September 1822.]]",
                    "");

//         <div class="thumb tleft">
//             <div class="thumbinner" style="width:222px;">
//                 <a href="/wiki/File:Independ%C3%AAncia_ou_Morte.jpg" class="image">
//                     <img alt="" src="http://upload.wikimedia.org/wikipedia/commons/thumb/c/ce/Independ%C3%AAncia_ou_Morte.jpg/220px-Independ%C3%AAncia_ou_Morte.jpg" width="220" height="108" class="thumbimage">
//                 </a>
//                 <div class="thumbcaption">
//                     <div class="magnify">
//                         <a href="/wiki/File:Independ%C3%AAncia_ou_Morte.jpg" class="internal" title="Enlarge">
//                         <img src="http://bits.wikimedia.org/skins-1.5/common/images/magnify-clip.png" width="15" height="11" alt="">
//                         </a>
//                     </div>
//                     Declaration of the <a href="/wiki/Brazilian_Declaration_of_Independence" title="Brazilian Declaration of Independence" class="mw-redirect">Brazilian independence</a> by Emperor <a href="/wiki/Pedro_I_of_Brazil" title="Pedro I of Brazil">Pedro I</a> on 7 September 1822.
//                 </div>
//             </div>
//         </div>
        }

        [Test]
        public void Redirect()
        {
            TestConvert(
                    "#REDIRECT [[Brazil]]",
                    "<p><a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">Brazil</a></p>");
        }

        private void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(wikicode);
            Assert.AreEqual(expected, html);
        }
    }
}

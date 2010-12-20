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

        #endregion // Header

        #region Bold/Italic

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

        #endregion // Bold/Italic

        #region Image

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
                        "<img alt=\"captione texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" class=\"thumbborder\">" +
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
        public void Link()
        {
            const string WIKI_CODE = "[[Brazil|kiko]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">kiko</a>";
            Assert.AreEqual(EXPECTED, html);
        }

        [Test]
        public void ImageComplex()
        {
            const string WIKI_CODE = "[[File:Independência ou Morte.jpg|thumb|left|Declaration of the [[Brazilian Declaration of Independence|Brazilian independence]] by Emperor [[Pedro I of Brazil|Pedro I]] on 7 September 1822.]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

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
            const string WIKI_CODE = "#REDIRECT [[Brazil]]";
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(WIKI_CODE);

            const string EXPECTED = "<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">Brazil</a>";
            Assert.AreEqual(EXPECTED, html);
        }

        private void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html();
            string html = converter.ConvertX(wikicode);
            Assert.AreEqual(expected, html);
        }
    }
}

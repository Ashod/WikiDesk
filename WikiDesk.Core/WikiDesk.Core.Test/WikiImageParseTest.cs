namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class WikiImageParseTest
    {
        [Test]
        public void ListSimple()
        {
            WikiParseTest.TestConvert(
                "Blah blah List\n" +
                "* first.\n" +
                "* second.\n" +
                "* last.\n" +
                "Other text.",
                "<p>\r\nBlah blah List</p><ul><li>first.</li><li>second.</li><li>last.</li></ul><p>Other text.\r\n</p>\r\n");
        }

        [Test]
        public void ListMultiple()
        {
            WikiParseTest.TestConvert(
                "Blah blah List\n" +
                "* first.\n" +
                "** one.\n" +
                "**two.\n" +
                "* second.\n" +
                "** 1.\n" +
                "***2.\n" +
                "* last.\n" +
                "Other text.",
                "<p>\r\nBlah blah List</p><ul><li>first.</li><ul><li>one.</li><li>two.</li></ul><li>second.</li><ul><li>1.</li><ul><li>2.</li></ul></ul><li>last.</li></ul><p>Other text.\r\n</p>\r\n");
        }

        [Test]
        public void ListSkewed()
        {
            WikiParseTest.TestConvert(
                "Blah blah List\n" +
                "* first.\n" +
                "** second.\n" +
                "*** third.\n" +
                "**** fourth.\n" +
                "***** fifth.",
                "<p>\r\nBlah blah List\r\n</p><ul><li>first.</li><ul><li>second.</li><ul><li>third.</li><ul><li>fourth.</li><ul><li>fifth.</li></ul></ul></ul></ul></ul>");
        }

        [Test]
        public void ImageMinimal()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageAndText()
        {
            WikiParseTest.TestConvert(
                "[[File:Face-smile.svg|18px]] '''Thank you'''",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Face-smile.svg\" class=\"image\">" +
                        "<img alt=\"Face-smile.svg\" src=\"http://upload.wikimedia.org/wikipedia/commons/thumb/7/79/Face-smile.svg/48px-Face-smile.svg.png\" width=\"18\">" +
                    "</a>" +
                    " <b>Thank you</b>" +
                "\r\n</p>\r\n");
        }

        [Test]
        public void ImageCaption()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="captione texte" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|captione texte]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"captione texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void TextImageText()
        {
            WikiParseTest.TestConvert(
                "Hi [[Image:Westminstpalace.jpg|alt=alternate texte]] Bye",
                "<p>\r\nHi " +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"alternate texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                " Bye\r\n</p>\r\n");
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

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|alt=alternate texte]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"alternate texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>\r\n" +
                "</p>\r\n");
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

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|alt=alternate text|captione texte]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"alternate text\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageBorder()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|border]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" class=\"thumbborder\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageBorderAlt()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="alternate text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|border|alt=alternate text]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"alternate text\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" class=\"thumbborder\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageBorderCaption()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="captione texte" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300" class="thumbborder">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|border|captione texte]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"captione texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" class=\"thumbborder\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageFrameless()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|frameless]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageFramelessRight()
        {
//         <div class="floatright">
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </div>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|right|frameless]]",
                "<div class=\"floatright\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</div>\r\n");
        }

        [Test]
        public void ImageFramelessLeft()
        {
//         <div class="floatleft">
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </div>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|left|frameless]]",
                "<div class=\"floatleft\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</div>\r\n");
        }

        [Test]
        public void ImageFramelessNone()
        {
//         <div class="floatnone">
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </div>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|none|frameless]]",
                "<div class=\"floatnone\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>" +
                "</div>\r\n");
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

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|center|frameless]]",
                "<div class=\"center\">" +
                    "<div class=\"floatnone\">" +
                        "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                            "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                        "</a>" +
                    "</div>" +
                "</div>\r\n");
        }

        [Test]
        public void ImageFramelessAlt()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image">
//                 <img alt="alternate texte" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|frameless|alt=alternate texte]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"alternate texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageFramelessCaption()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="Westminstpalace.jpg" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|frameless|captione texte]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"captione texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageFramelessAltCaption()
        {
//         <p>
//             <a href="/wiki/File:Westminstpalace.jpg" class="image" title="captione texte">
//                 <img alt="alternate texte" src="http://upload.wikimedia.org/wikipedia/commons/thumb/3/39/Westminstpalace.jpg/220px-Westminstpalace.jpg" width="220" height="165">
//             </a>
//         </p>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|frameless|alt=alternate texte|captione texte]]",
                "<p>\r\n" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"alternate texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"220\">" +
                    "</a>\r\n" +
                "</p>\r\n");
        }

        [Test]
        public void ImageNone()
        {
//             <div class="floatnone">
//                 <a href="/wiki/File:Westminstpalace.jpg" class="image" title="caption text">
//                     <img alt="alt text" src="http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg" width="400" height="300">
//                 </a>
//             </div>

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|none]]",
                "<div class=\"floatnone\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                "</div>\r\n");
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

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|upright=|thumb]]",
                "<div class=\"thumb tright\">" +
                "<div class=\"thumbinner\" style=\"width:172px;\">" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\" width=\"170\" class=\"thumbimage\">" +
                    "</a>" +
                "</div>" +
                "</div>\r\n");
        }

        [Test]
        [Ignore]
        public void ImageSize()
        {
            WikiParseTest.TestConvert(
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
            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|frame|none|alt=alt text|caption text]]",
                "<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">kiko</a>\r\n");

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

        }

        [Test]
        [Ignore]
        public void ImageComplex()
        {
            WikiParseTest.TestConvert(
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
    }
}

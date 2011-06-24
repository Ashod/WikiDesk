// -----------------------------------------------------------------------------------------
// <copyright file="WikiImageParseTest.cs" company="ashodnakashian.com">
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
//   Defines the WikiImageParseTest type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class WikiImageParseTest
    {
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
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"Westminstpalace.jpg\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void ImageAndText()
        {
            WikiParseTest.TestConvert(
                "[[File:Face-smile.svg|18px]] '''Thank you'''",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Face-smile.svg\" class=\"image\">" +
                        "<img alt=\"Face-smile.svg\" src=\"http://upload.wikimedia.org/wikipedia/commons/thumb/7/79/Face-smile.svg/48px-Face-smile.svg.png\" width=\"18\">" +
                    "</a>" +
                    " <b>Thank you</b>" +
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

            WikiParseTest.TestConvert(
                "[[Image:Westminstpalace.jpg|captione texte]]",
                "<p>" +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\" title=\"captione texte\">" +
                        "<img alt=\"captione texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                "</p>");
        }

        [Test]
        public void TextImageText()
        {
            WikiParseTest.TestConvert(
                "Hi [[Image:Westminstpalace.jpg|alt=alternate texte]] Bye",
                "<p>Hi " +
                    "<a href=\"http://en.wikipedia.org/wiki/File:Westminstpalace.jpg\" class=\"image\">" +
                        "<img alt=\"alternate texte\" src=\"http://upload.wikimedia.org/wikipedia/commons/3/39/Westminstpalace.jpg\">" +
                    "</a>" +
                " Bye</p>");
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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

            WikiParseTest.TestConvert(
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
                "<a href=\"http://en.wikipedia.org/wiki/Brazil\" title=\"Brazil\">kiko</a>");

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

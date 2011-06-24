// -----------------------------------------------------------------------------------------
// <copyright file="WikiTitleTest.cs" company="ashodnakashian.com">
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
//   Defines the WikiTitleTest type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class WikiTitleTest
    {
        [Test]
        public void TitleNormalize()
        {
            Assert.AreEqual("Title", Title.Normalize("title"));
            Assert.AreEqual("Blah_may", Title.Normalize("blah may"));
            Assert.AreEqual("Title", Title.Normalize("Title"));
            Assert.AreEqual("Iron_Curtain", Title.Normalize("iron Curtain"));
        }

        [Test]
        public void TitleDeNormalize()
        {
            Assert.AreEqual("Title", Title.Denormalize("Title"));
            Assert.AreEqual("Blah may", Title.Denormalize("Blah_may"));
            Assert.AreEqual("Title", Title.Denormalize("Title"));
            Assert.AreEqual("Iron Curtain", Title.Denormalize("Iron_Curtain"));
        }

        [Test]
        public void ParseFullPageName()
        {
            string nameSpace;
            Assert.AreEqual("Title", Title.ParseFullPageName("Title", out nameSpace));
            Assert.AreEqual(string.Empty, nameSpace);

            Assert.AreEqual("Title", Title.ParseFullPageName("Wikipedia:Title", out nameSpace));
            Assert.AreEqual("Wikipedia", nameSpace);

            Assert.AreEqual("Main Page", Title.ParseFullPageName("Template:Main Page", out nameSpace));
            Assert.AreEqual("Template", nameSpace);

            Assert.AreEqual("Main Page", Title.ParseFullPageName(" :Main Page", out nameSpace));
            Assert.AreEqual(string.Empty, nameSpace);
        }

        [Test]
        public void FullPageName()
        {
            Assert.AreEqual("Wikipedia:Title", Title.FullPageName("Wikipedia", "Title"));
            Assert.AreEqual("Template:Main_Page", Title.FullPageName("Template", "Main Page"));
            Assert.AreEqual("Template:Main_Page", Title.FullPageName("Template", "main Page"));
            Assert.AreEqual("Template:Main_page", Title.FullPageName("Template", "main page"));
            Assert.AreEqual("Main_Page", Title.FullPageName(null, "Main Page"));
            Assert.AreEqual("Main_Page", Title.FullPageName(string.Empty, "Main Page"));
            Assert.AreEqual("Main_Page", Title.FullPageName("  ", "Main Page"));
        }
    }
}

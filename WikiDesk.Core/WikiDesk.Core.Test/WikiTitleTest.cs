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
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class WikiTitleTest
    {
        [Test]
        public void TitleCanonicalize()
        {
            Assert.AreEqual(string.Empty, Title.Canonicalize(string.Empty));
            Assert.AreEqual("Title", Title.Canonicalize("title"));
            Assert.AreEqual("Blah_may", Title.Canonicalize("blah may"));
            Assert.AreEqual("Title", Title.Canonicalize("Title"));

            Assert.AreEqual(string.Empty, Title.Canonicalize("  _  ___________ "));

            Assert.AreEqual("Iron_Curtain", Title.Canonicalize("iron Curtain"));
            Assert.AreEqual("Iron_Curtain", Title.Canonicalize(" iron   Curtain  "));
            
            Assert.AreEqual("User:Jimbo_Wales", Title.Canonicalize("uSeR:jimbo Wales"));
            Assert.AreEqual("User:Jimbo_Wales", Title.Canonicalize(" _User_: Jimbo_ __ Wales__"));
        }

        [Test]
        public void InvalidTitle()
        {
            Assert.Throws<ArgumentNullException>(() => Title.Canonicalize(null));
            Assert.Throws<ArgumentNullException>(() => Title.Decanonicalize(null));
        }

        [Test]
        public void TitleDecanonicalize()
        {
            Assert.AreEqual(string.Empty, Title.Decanonicalize(string.Empty));
            Assert.AreEqual("Title", Title.Decanonicalize("Title"));
            Assert.AreEqual("Blah may", Title.Decanonicalize("Blah_may"));
            Assert.AreEqual("Title", Title.Decanonicalize("Title"));
            Assert.AreEqual("Iron Curtain", Title.Decanonicalize("Iron_Curtain"));
        }

        [Test]
        public void ParseFullPageName()
        {
            string nameSpace;
            Assert.AreEqual(string.Empty, Title.ParseFullTitle(null, out nameSpace));
            Assert.AreEqual(string.Empty, nameSpace);

            Assert.AreEqual("Title", Title.ParseFullTitle("Title", out nameSpace));
            Assert.AreEqual(string.Empty, nameSpace);

            Assert.AreEqual("Title", Title.ParseFullTitle("Wikipedia:Title", out nameSpace));
            Assert.AreEqual("Wikipedia", nameSpace);

            Assert.AreEqual("Main Page", Title.ParseFullTitle("Template:Main Page", out nameSpace));
            Assert.AreEqual("Template", nameSpace);

            Assert.AreEqual("Main Page", Title.ParseFullTitle(" :Main Page", out nameSpace));
            Assert.AreEqual(string.Empty, nameSpace);
        }

        [Test]
        public void FullPageName()
        {
            Assert.AreEqual("Wikipedia:Title", Title.FullTitleName("Wikipedia", "Title"));
            Assert.AreEqual("Template:Main_Page", Title.FullTitleName("Template", "Main Page"));
            Assert.AreEqual("Template:Main_Page", Title.FullTitleName("Template", "main Page"));
            Assert.AreEqual("Template:Main_page", Title.FullTitleName("Template", "main page"));
            Assert.AreEqual("Main_Page", Title.FullTitleName(null, "Main Page"));
            Assert.AreEqual("Main_Page", Title.FullTitleName(string.Empty, "Main Page"));
            Assert.AreEqual("Main_Page", Title.FullTitleName("  ", "Main Page"));
        }
    }
}

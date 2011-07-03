// -----------------------------------------------------------------------------------------
// <copyright file="Wiki2HtmlTest.cs" company="ashodnakashian.com">
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
//   Defines the Wiki2HtmlTest type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core.Test
{
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    using WikiDesk.Data;

    [TestFixture]
    public class Wiki2HtmlTest
    {
        static Wiki2HtmlTest()
        {
            RootPath = "TestFiles";
            WikiDomain wikiDomain = new WikiDomain("wikipedia");
            WikiLanguage wikiLanguage = new WikiLanguage("English", "en");
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WikiSite wikiSite = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
            Config = new Configuration(wikiSite);
        }

        [Test]
        public void Modules()
        {
            WikiDomain wikiDomain = new WikiDomain("wikipedia");
            WikiLanguage wikiLanguage = new WikiLanguage("Aymara", "ay");
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WikiSite wikiSite = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
        }

        [Test]
        public void HeaderBoldItalic()
        {
            TestConvertFiles("HeaderBoldItalic");
        }

        [Test]
        public void RefCite()
        {
            TestConvertFiles("RefCite");
        }

        [Test]
        public void Cite()
        {
            TestConvertFiles("Cite");
        }

        [Test]
        public void RefCiteReflist()
        {
            TestConvertFiles("RefCiteReflist");
        }

        [Test]
        public void TableWithImages()
        {
            TestConvertFiles("TableWithImages");
        }

        [Test]
        [Ignore]
        public void NavBox()
        {
            TestConvertFiles("NavBox");
        }

        [Test]
        public void InfoBox()
        {
            TestConvertFiles("InfoBox");
        }

        [Test]
        public void TaxoBox()
        {
            TestConvertFiles("TaxoBox");
        }

        [Test]
        public void TaxoBoxCore()
        {
            TestConvertFiles("TaxoBoxCore");
        }

        #region implementation

        private static void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html(Config);
            string nameSpace = string.Empty;
            string title = "TestPage";
            string html = converter.Convert(ref nameSpace, ref title, wikicode);
            Assert.AreEqual(expected, html);
        }

        private static void TestConvertFiles(string baseFilename)
        {
            Wiki2Html converter = new Wiki2Html(Config, null, OnResolveTemplate, null);
            string nameSpace = string.Empty;
            string title = "TestPage";
            string wikicode = File.ReadAllText(Path.Combine(RootPath, baseFilename + ".wiki"));
            string html = converter.Convert(ref nameSpace, ref title, wikicode);
            string expected = File.ReadAllText(Path.Combine(RootPath, baseFilename + ".html"));
            Assert.AreEqual(expected, html);
        }

        private static string OnResolveTemplate(string word, string lanugageCode)
        {
            string title = !word.StartsWith("Template:") ? "Template:" + word : word;

            title = Title.Normalize(title);
            string url = string.Concat("http://", Config.WikiSite.Language.Code, Config.WikiSite.ExportUrl, title);
            string xmlText = Download.DownloadPage(url);
            Page page = DumpParser.PageFromXml(xmlText);
            return page != null ? page.Text : string.Empty;
        }

        #endregion // implementation

        #region representation

        private static readonly string RootPath;
        private static readonly Configuration Config;

        #endregion // representation
    }
}

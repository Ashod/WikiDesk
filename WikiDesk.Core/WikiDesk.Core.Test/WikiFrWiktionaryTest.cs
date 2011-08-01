// -----------------------------------------------------------------------------------------
// <copyright file="WikiFrWiktionaryTest.cs" company="ashodnakashian.com">
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
//   French Wiktionary tests.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    using WikiDesk.Data;

    [TestFixture]
    public class WikiFrWiktionaryTest
    {
        static WikiFrWiktionaryTest()
        {
            WikiDomain wikiDomain = new WikiDomain("wiktionary");
            WikiLanguage wikiLanguage = new WikiLanguage("French", "fr");
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WikiSite wikiSite = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
            config_ = new Configuration(wikiSite);
        }

        [Test]
        public void NomEn()
        {
            const string EXPECTED =
                @"<h3 class=""titredef"" id=""en-nom""><span class=""mw-headline"" id=""Nom_commun_2"">Nom commun</span></h3>";
            
            TestConvert(
                    "{{-nom-|en}}",
                    EXPECTED);

            TestConvert(
                    "{{-déf-|Nom commun|\r\n|type=Noms communs\r\n|code=nom\r\n|num=\r\n|langue=en\r\n}}",
                    EXPECTED);
        }

        #region implementation

        internal static void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html(config_, OnResolveWikiLinks, OnResolveTemplate, null);
            string nameSpace = string.Empty;
            string title = "TestPage";
            string html = converter.Convert(ref nameSpace, ref title, wikicode);
            Assert.AreEqual(expected, html);
        }

        private static string OnResolveWikiLinks(string title, string languageCode)
        {
            return Title.UrlCanonicalize(title);
        }

        private static string OnResolveTemplate(string word, string lanugageCode)
        {
            string templateNamespace = config_.WikiSite.GetNamespaceName(WikiSite.Namespace.Tempalate);

            string title = word;
            if (!word.StartsWith(templateNamespace, StringComparison.InvariantCultureIgnoreCase))
            {
                title = templateNamespace + ':' + word;
            }

            title = Title.Canonicalize(title);
            string url = string.Concat("http://", config_.WikiSite.Language.Code, config_.WikiSite.ExportUrl, title);
            string xmlText = Download.DownloadPage(url);
            Page page = DumpParser.PageFromXml(xmlText);
            return page != null ? page.Text : string.Empty;
        }

        #endregion // implementation

        #region representation

        private static readonly Configuration config_;

        #endregion // representation
    }
}

// -----------------------------------------------------------------------------------------
// <copyright file="MagicWordProcessorTest.cs" company="ashodnakashian.com">
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
//   Defines the WikiMagicWordProcessorTest type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    [TestFixture]
    public class WikiMagicWordProcessorTest
    {
        static WikiMagicWordProcessorTest()
        {
            WikiDomain wikiDomain = new WikiDomain("wikipedia");
            WikiLanguage wikiLanguage = new WikiLanguage("English", "en");
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WikiSite = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
        }

        [SetUp]
        public void Setup()
        {
            proc_ = new MagicWordProcessor();
            proc_.SetContext(WikiSite, string.Empty, "TestPage");
            args_ = new List<KeyValuePair<string, string>>(4);
        }

        #region tests

        #region Lc

        [Test]
        public void Lc()
        {
            TestProcessor("lc:KIKOS", "kikos");
        }

        [Test]
        public void LcA()
        {
            TestProcessor("lc:1=TEST", "test");

            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcB()
        {
            TestProcessor("lc:1=test", "test");

            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcSingleChar()
        {
            TestProcessor("lc:1=A", "a");

            args_.Add(new KeyValuePair<string, string>("1", "A"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual("a", output);
        }

        [Test]
        public void LcEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcNullArgs()
        {
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lc", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // Lc

        #region Uc

        [Test]
        public void UcA()
        {
            TestProcessor("uc:1=test", "TEST");

            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcB()
        {
            TestProcessor("uc:1=TEST", "TEST");

            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcSingleChar()
        {
            TestProcessor("uc:1=a", "A");

            args_.Add(new KeyValuePair<string, string>("1", "a"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual("A", output);
        }

        [Test]
        public void UcEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcNullArgs()
        {
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("uc", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // Uc

        #region LcFirst

        [Test]
        public void LcFirstA()
        {
            TestProcessor("lcfirst:1=TEST", "tEST");

            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("tEST", output);
        }

        [Test]
        public void LcFirstB()
        {
            TestProcessor("lcfirst:1=test", "test");

            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("test", output);
        }

        [Test]
        public void LcFirstSingleChar()
        {
            TestProcessor("lcfirst:1=A", "a");

            args_.Add(new KeyValuePair<string, string>("1", "A"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual("a", output);
        }

        [Test]
        public void LcFirstEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcFirstNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void LcFirstNullArgs()
        {
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("lcfirst", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // LcFirst

        #region UcFirst

        [Test]
        public void UcFirstA()
        {
            TestProcessor("ucfirst:1=test", "Test");

            args_.Add(new KeyValuePair<string, string>("1", "test"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("Test", output);
        }

        [Test]
        public void UcFirstB()
        {
            TestProcessor("ucfirst:1=TEST", "TEST");

            args_.Add(new KeyValuePair<string, string>("1", "TEST"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("TEST", output);
        }

        [Test]
        public void UcFirstSingleChar()
        {
            TestProcessor("ucfirst:1=a", "A");

            args_.Add(new KeyValuePair<string, string>("1", "a"));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual("A", output);
        }

        [Test]
        public void UcFirstEmpty()
        {
            args_.Add(new KeyValuePair<string, string>("1", string.Empty));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcFirstNull()
        {
            args_.Add(new KeyValuePair<string, string>("1", null));

            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", args_, out output));
            Assert.AreEqual(string.Empty, output);
        }

        [Test]
        public void UcFirstNullArgs()
        {
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute("ucfirst", null, out output));
            Assert.AreEqual(string.Empty, output);
        }

        #endregion // UcFirst

        #region FullUrl

        [Test]
        public void FullUrlCategory()
        {
            TestProcessor("fullurl:Category:Top level", "http://en.wikipedia.org/wiki/Category:Top_level");
        }

        [Test]
        public void FullUrlQuery()
        {
            TestProcessor("fullurl:Category:Top level|action=edit", "http://en.wikipedia.org/w/index.php?title=Category:Top_level&action=edit");
        }

        #endregion // UcFirst

        [Test]
        public void Namespaces()
        {
            TestProcessor("TALKSPACE", "Talk");
            TestProcessor("NAMESPACE", string.Empty);

            TestProcessor("ns:-2", "Media");
            TestProcessor("ns:-1", "Special");
            TestProcessor("ns:0", string.Empty);
            TestProcessor("ns:1", "Talk");
            TestProcessor("ns:2", "User");
            TestProcessor("ns:3", "User talk");
            TestProcessor("nse:3", "User_talk");

            // Unsupported, since they aren't included in the published language messages PHPs.
            // TestProcessor("ns:4", "Wikipedia");
            // TestProcessor("ns:5", "Wikipedia_talk");

            TestProcessor("ns:6", "File");
            TestProcessor("ns:7", "File talk");
            TestProcessor("nse:7", "File_talk");
            TestProcessor("ns:8", "MediaWiki");
            TestProcessor("ns:9", "MediaWiki talk");
            TestProcessor("nse:9", "MediaWiki_talk");
            TestProcessor("ns:10", "Template");
            TestProcessor("ns:11", "Template talk");
            TestProcessor("nse:11", "Template_talk");
            TestProcessor("ns:12", "Help");
            TestProcessor("ns:13", "Help talk");
            TestProcessor("nse:13", "Help_talk");
            TestProcessor("ns:14", "Category");
            TestProcessor("ns:15", "Category talk");
            TestProcessor("nse:15", "Category_talk");

            // Unsupported, since they aren't included in the published language messages PHPs.
            // TestProcessor("ns:100", "Portal");
            // TestProcessor("ns:101", "Portal_talk");
            // TestProcessor("ns:108", "Book");
            // TestProcessor("ns:109", "Book_talk");
        }

        [Test]
        [ExpectedException(typeof(System.ApplicationException))]
        public void InvalidNamespaces()
        {
            TestProcessor("ns:55", string.Empty);
        }

        #endregion // tests

        public void TestProcessor(string input, string expected)
        {
            string command = MagicParser.GetMagicWordAndParams(input, out args_);

            string magicId = WikiSite.MagicWords.FindId(command);
            string output;
            Assert.AreEqual(VariableProcessor.Result.Found, proc_.Execute(magicId, args_, out output));
            Assert.AreEqual(expected, output);
        }

        #region representation

        private MagicWordProcessor proc_;

        private List<KeyValuePair<string, string>> args_;

        private static readonly WikiSite WikiSite;

        #endregion // representation
    }
}

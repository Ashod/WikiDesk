// -----------------------------------------------------------------------------------------
// <copyright file="PageTableTests.cs" company="ashodnakashian.com">
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
//   Unit tests for the Page table.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Data.Test
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using SQLite;

    [TestFixture]
    public class PageTableTests : DatabaseTests
    {
        [Test]
        public void Equality()
        {
            Page page1 = new Page { Language = 1, Text = "Some text whatever it is...", Title = "test page" };
            Page page2 = new Page { Language = 1, Text = "Some text whatever it is...", Title = "Test page" };
            Assert.That(page1.CompareTo(page2) < 0);

            page1.Title = page2.Title;
            page1.Language = 2;
            Assert.That(page1.CompareTo(page2) > 0);

            page1.Language = page2.Language;
            Assert.AreEqual(page1, page2);
        }

        [Test]
        public void Insert()
        {
            Page page1 = new Page { Domain = 1, Language = 1, Text = "Some text", Title = "Doo" };
            Assert.AreEqual(1, Database.Insert(page1));

            Page page2 = new Page { Domain = 1, Language = 1, Text = "Some text", Title = "doo" };
            Assert.AreEqual(1, Database.Insert(page2));

            IEnumerator<string> selectPageTitles = Database.SelectPageTitles(1, 1).GetEnumerator();
        }

        [Test]
        public void UpsertAll()
        {
            const int DOMAIN = 1;
            const int LANGUAGE = 1;
            Page page1 = new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "Some text", Title = "Doo" };
            Assert.AreEqual(1, Database.Insert(page1));

            Page page2 = new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "Some text", Title = "doo" };
            Assert.AreEqual(1, Database.Insert(page2));

            List<Page> pages = new List<Page>(2)
                {
                    new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "New Doo text", Title = "Doo" },
                    new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "Default Soo text", Title = "Soo" }
                };

            Assert.AreEqual(2, Database.UpsertAll(pages));

            Assert.AreEqual(3, Database.CountPages(DOMAIN, LANGUAGE));
            Assert.AreEqual("New Doo text", Database.SelectPage(DOMAIN, LANGUAGE, "Doo").Text);
            Assert.AreEqual("Some text", Database.SelectPage(DOMAIN, LANGUAGE, "doo").Text);
            Assert.AreEqual("Default Soo text", Database.SelectPage(DOMAIN, LANGUAGE, "Soo").Text);
        }

        [Test]
        public void InsertWithQuote()
        {
            const int DOMAIN = 1;
            const int LANGUAGE = 1;
            const string TITLE = "Do\"o";
            Page page1 = new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "Some text", Title = TITLE };
            Assert.AreEqual(1, Database.Insert(page1));

            IEnumerator<string> selectPageTitles = Database.SelectPageTitles(DOMAIN, LANGUAGE).GetEnumerator();
            Assert.That(selectPageTitles.MoveNext());
            Assert.AreEqual(TITLE, selectPageTitles.Current);
        }

        [Test]
        public void InsertWithComa()
        {
            const int DOMAIN = 1;
            const int LANGUAGE = 1;
            const string TITLE = "Do,o";
            Page page1 = new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "Some text", Title = TITLE };
            Assert.AreEqual(1, Database.Insert(page1));

            IEnumerator<string> selectPageTitles = Database.SelectPageTitles(DOMAIN, LANGUAGE).GetEnumerator();
            Assert.That(selectPageTitles.MoveNext());
            Assert.AreEqual(TITLE, selectPageTitles.Current);
        }

        [Test]
        public void InsertWithAccent()
        {
            const int DOMAIN = 1;
            const int LANGUAGE = 1;
            const string TITLE = "Néné";
            Page page1 = new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "Some text", Title = TITLE };
            Assert.AreEqual(1, Database.Insert(page1));

            IEnumerator<string> selectPageTitles = Database.SelectPageTitles(DOMAIN, LANGUAGE).GetEnumerator();
            Assert.That(selectPageTitles.MoveNext());
            Assert.AreEqual(TITLE, selectPageTitles.Current);
        }

        [Test]
        public void Delete()
        {
            const int DOMAIN = 1;
            const int LANGUAGE = 1;
            const string TITLE = "Do\"o";
            Page page1 = new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "Some text", Title = TITLE };
            Assert.AreEqual(1, Database.Insert(page1));

            Assert.AreEqual(1, Database.DeletePage(DOMAIN, LANGUAGE, TITLE));
        }

        [Test]
        [ExpectedException(typeof(SQLiteException))]
        public void UniqueTitle()
        {
            const int DOMAIN = 1;
            const int LANGUAGE = 1;
            const string TITLE = "Do\"o";
            Page page = new Page { Domain = DOMAIN, Language = LANGUAGE, Text = "Some text", Title = TITLE };
            Assert.AreEqual(1, Database.Insert(page));
            Assert.AreEqual(1, Database.Insert(page));
        }
    }
}

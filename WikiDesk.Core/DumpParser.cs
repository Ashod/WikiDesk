// -----------------------------------------------------------------------------------------
// <copyright file="DumpParser.cs" company="ashodnakashian.com">
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
//   Defines the DumpParser type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Xml;

    using WikiDesk.Data;

    public class DumpParser
    {
        /// <summary>
        /// Loads articles from an XML dump stream.
        /// </summary>
        /// <param name="stream">The stream which contains the XML dump.</param>
        /// <param name="domainId">The domain ID.</param>
        /// <param name="languageId">The ID of the language of the dump.</param>
        /// <param name="db">The database into which to import the dump.</param>
        /// <param name="dumpDate">The date when the dump was created.</param>
        /// <param name="indexOnly">If True, article text is not added, just the meta data.</param>
        public static string ImportFromXml(
                                Stream stream,
                                Database db,
                                DateTime dumpDate,
                                bool indexOnly,
                                int domainId,
                                int languageId)
        {
            bool cancel = false;
            int pages = 0;
            return ImportFromXml(stream, db, dumpDate, indexOnly, domainId, languageId, ref cancel, ref pages);
        }

        /// <summary>
        /// Loads articles from an XML dump stream.
        /// </summary>
        /// <param name="stream">The stream which contains the XML dump.</param>
        /// <param name="domainId">The domain ID.</param>
        /// <param name="languageId">The ID of the language of the dump.</param>
        /// <param name="db">The database into which to import the dump.</param>
        /// <param name="dumpDate">The date when the dump was created.</param>
        /// <param name="indexOnly">If True, article text is not added, just the meta data.</param>
        /// <param name="cancel">When set, this function terminates.</param>
        /// <param name="pageCount">Cumulates the number of pageCount updated/inserted into the DB.</param>
        public static string ImportFromXml(
                                Stream stream,
                                Database db,
                                DateTime dumpDate,
                                bool indexOnly,
                                int domainId,
                                int languageId,
                                ref bool cancel,
                                ref int pageCount)
        {
            // If we have at least one page in the current domain/language,
            // then we must update existing pages, otherwise we insert.
            bool updating = db.CountPages(domainId, languageId) > 0;

            string title = string.Empty;
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;

                const int BATCH_SIZE = 25000;
                IList<Page> pages = new List<Page>(BATCH_SIZE + 1);
                db.BeginTransaction();
                try
                {
                    while (!cancel)
                    {
                        Page page = ParsePageTag(reader, indexOnly);
                        if (page == null)
                        {
                            break;
                        }

                        title = page.Title;
                        page.Domain = domainId;
                        page.Language = languageId;
                        page.LastUpdateDateUtc = dumpDate;

                        if (updating)
                        {
                            Page selectPage = db.SelectPage(domainId, languageId, page.Title);
                            if (selectPage != null)
                            {
                                db.Delete(selectPage);
                            }
                            else
                            {
                                selectPage = db.SelectPage(domainId, languageId, page.Title);
                            }
                        }

                        pages.Add(page);
                        if (++pageCount % BATCH_SIZE == 0)
                        {
                            db.InsertAll(pages);
                            pages.Clear();
                            db.Commit();
                            db.BeginTransaction();
                        }
                    }

                    if (pages.Count > 0)
                    {
                        db.InsertAll(pages);
                    }

                    db.Commit();
                }
                catch(Exception ex)
                {
                    if (db.IsInTransaction)
                    {
                        db.Rollback();
                    }

                    throw;
                }
            }

            return title;
        }

        /// <summary>
        /// Parses a page from an XML dump stream.
        /// </summary>
        /// <param name="xmlText">The XML dump.</param>
        public static Page PageFromXml(string xmlText)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlText)))
            {
                return PageFromXml(ms);
            }
        }

        /// <summary>
        /// Parses a page from an XML dump stream.
        /// </summary>
        /// <param name="stream">The stream which contains the XML dump.</param>
        public static Page PageFromXml(Stream stream)
        {
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;
                return ParsePageTag(reader, false);
            }
        }

        /// <summary>
        /// Parses a single page from and XML.
        /// </summary>
        /// <param name="reader">The XML reader to read from.</param>
        /// <param name="metaOnly">If true, only meta data is collected and no text.</param>
        /// <returns>A Page instance.</returns>
        private static Page ParsePageTag(XmlReader reader, bool metaOnly)
        {
            // Prime to the next Page tag.
            if (!reader.ReadToFollowing(TAG_PAGE))
            {
                return null;
            }

            Debug.Assert(reader.Name == TAG_PAGE, "Expected Page tag.");
            Debug.Assert(reader.IsStartElement(), "Expected start of Page tag.");

            Page page = new Page();
            while (reader.Read() && reader.Name != TAG_PAGE)
            {
                // Align on a start element.
                if (!reader.IsStartElement())
                {
                    continue;
                }

                switch (reader.Name)
                {
                    case "id":
                        //page.Id = long.Parse(reader.ReadString());
                        continue;

                    case "title":
                        // Don't canonicalize as some projects allow
                        // first lower-case and other case-sensitive entries.
                        page.Title = reader.ReadString();
                        break;

                    case TAG_REVISION:
                        if (!metaOnly)
                        {
                            page.Text = ParseRevisionTag(reader);
                        }
                        else
                        {
                            // Skip the revision tag.
                            reader.Skip();
                        }

                        break;
                }
            }

            return page;
        }

        private static string ParseRevisionTag(XmlReader reader)
        {
            Debug.Assert(reader.Name == TAG_REVISION, "Expected Revision tag.");

            string text = string.Empty;
            while (reader.Read() && reader.Name != TAG_REVISION)
            {
                // Align on a start element.
                if (!reader.IsStartElement())
                {
                    continue;
                }

                switch (reader.Name)
                {
                    case "id":
                        //rev.Id = long.Parse(reader.ReadString());
                        continue;

                    case "timestamp":
                        //rev.Timestamp = DateTime.Parse(reader.ReadString());
                        continue;

                    case TAG_CONTRIBUTOR:
                        // Skip contributor info.
                        reader.Skip();
                        break;

                    case "text":
                        text = reader.ReadString();
                        break;
                }
            }

            return text;
        }

        #region constants

        private const string TAG_CONTRIBUTOR = "contributor";

        private const string TAG_PAGE = "page";

        private const string TAG_REVISION = "revision";

        #endregion
    }
}

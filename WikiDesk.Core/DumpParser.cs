namespace WikiDesk.Core
{
    using System;
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
        /// <param name="pages">Cumulates the number of pages updated/inserted into the DB.</param>
        public static string ImportFromXml(
                                Stream stream,
                                Database db,
                                DateTime dumpDate,
                                bool indexOnly,
                                int domainId,
                                int languageId,
                                ref bool cancel,
                                ref int pages)
        {
            string title = string.Empty;
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while (!cancel)
                {
                    Page page = ParsePageTag(reader);
                    if (page == null)
                    {
                        break;
                    }

                    page.Domain = domainId;
                    page.Language = languageId;
                    page.LastUpdateDateUtc = dumpDate;
                    if (indexOnly)
                    {
                        page.Text = null;
                    }

                    title = page.Title;
                    db.UpdateInsert(page, db.SelectPage(page.Domain, page.Language, page.Title));
                    ++pages;
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
                return ParsePageTag(reader);
            }
        }

        private static Page ParsePageTag(XmlReader reader)
        {
            do
            {
                if (!reader.ReadToFollowing(TAG_PAGE))
                {
                    return null;
                }
            }
            while (!reader.IsStartElement());

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
                        page.Title = Title.Normalize(reader.ReadString());
                        break;

                    case TAG_REVISION:
                        page.Text = ParseRevisionTag(reader);
                        break;
                }
            }

            return !string.IsNullOrEmpty(page.Title) && page.Text != null ? page : null;
        }

        private static string ParseRevisionTag(XmlReader reader)
        {
            Debug.Assert(reader.Name == TAG_REVISION);

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
                        while (reader.Read() && reader.Name != TAG_CONTRIBUTOR)
                        {
                            if (!reader.IsStartElement())
                            {
                                continue;
                            }
                        }
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

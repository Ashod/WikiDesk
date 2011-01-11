namespace WikiDesk.Core
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;

    using WikiDesk.Data;

    public class DumpParser
    {
        /// <summary>
        /// Loads articles from an XML dump stream.
        /// </summary>
        /// <param name="stream">The stream which contains the XML dump.</param>
        /// <param name="domainId">The domain ID.</param>
        /// <param name="languageCode">The language code of the dump.</param>
        /// <param name="db">The database into which to import the dump.</param>
        /// <param name="indexOnly">If True, article text is not added, just the meta data.</param>
        public static void ImportFromXml(Stream stream, Database db, bool indexOnly, long domainId, string languageCode)
        {
            Language language = db.GetLanguageByCode(languageCode);
            if (language == null)
            {
                return; //TODO: Throw?
            }

            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while (true)
                {
                    Page page = ParsePageTag(reader);
                    if (page == null)
                    {
                        break;
                    }

                    page.Domain = domainId;
                    page.Language = language.Id;
                    if (!indexOnly)
                    {
                        Revision oldRev = db.QueryRevision(page.Revision.Id);
                        if (oldRev != null)
                        {
                            if (oldRev.Id != page.LastRevisionId &&
                                page.LastRevisionId != 0)
                            {
                                db.Delete(oldRev);
                                db.Insert(page.Revision);
                            }
                            else
                            {
                                db.Update(page.Revision);
                            }
                        }
                        else
                        {
                            db.Insert(page.Revision);
                        }
                    }
                    else
                    {
                        page.LastRevisionId = 0;
                    }

                    db.UpdateReplacePage(page);
                }
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
                        Revision rev = ParseRevisionTag(reader);
                        if ((rev != null) && rev.Id != 0)
                        {
                            page.LastRevisionId = rev.Id;
                            page.Revision = rev;
                        }
                        break;
                }
            }

            return !string.IsNullOrEmpty(page.Title) && page.Revision != null ? page : null;
        }

        private static Revision ParseRevisionTag(XmlReader reader)
        {
            Debug.Assert(reader.Name == TAG_REVISION);

            Revision rev = new Revision();

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
                        rev.Id = long.Parse(reader.ReadString());
                        continue;

                    case "timestamp":
                        rev.Timestamp = DateTime.Parse(reader.ReadString());
                        continue;

                    case TAG_CONTRIBUTOR:
                        ParseContributorTag(reader, rev);
                        break;

                    case "text":
                        rev.Text = reader.ReadString();
                        break;
                }
            }

            return rev;
        }

        private static void ParseContributorTag(XmlReader reader, Revision rev)
        {
            Debug.Assert(reader.Name == TAG_CONTRIBUTOR);

            while (reader.Read() && reader.Name != TAG_CONTRIBUTOR)
            {
                if (!reader.IsStartElement())
                {
                    continue;
                }

                switch (reader.Name)
                {
                    case "ip":
                        if (string.IsNullOrEmpty(rev.Contributor))
                        {
                            rev.Contributor = reader.ReadString();
                        }
                        break;

                    case "username":
                        rev.Contributor = reader.ReadString();
                        break;

                    case "id":
                        break;
                }
            }
        }

        #region constants

        private const string TAG_CONTRIBUTOR = "contributor";

        private const string TAG_PAGE = "page";

        private const string TAG_REVISION = "revision";

        #endregion
    }
}

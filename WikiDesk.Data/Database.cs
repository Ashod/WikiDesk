namespace WikiDesk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using SQLite;

    public class Database : SQLiteConnection
    {
        public Database(string path)
            : base(path)
        {
            CreateTable<Page>();
            CreateTable<Revision>();
        }

        public IList<Page> GetPages()
        {
            return (from s in Table<Page>() select s).ToList();
        }

        public Page QueryPage(string title)
        {
            return (from s in Table<Page>()
                    where s.Title == title
                    select s).FirstOrDefault();
        }

        public Revision QueryRevision(long id)
        {
            return (from s in Table<Revision>()
                    where s.Id == id
                    select s).FirstOrDefault();
        }

        /// <summary>
        /// Loads articles from an XML dump file.
        /// </summary>
        /// <param name="xmlDumpFilePath">The file path to the XML dump.</param>
        /// <param name="languageCode">The language code of the dump.</param>
        /// <param name="indexOnly">If True, article text is not added, just the meta data.</param>
        public void Load(string xmlDumpFilePath, string languageCode, bool indexOnly)
        {
            using (FileStream stream = new FileStream(xmlDumpFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 64 * 1024))
            {
                ImportFromXml(stream, indexOnly, languageCode);
            }
        }

        public void ImportFromXml(Stream stream, bool indexOnly, string languageCode)
        {
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;

//                 if (From.Length > 0)
//                 {
//                     while (reader.Read())
//                     {
//                         if (reader.NodeType != XmlNodeType.Element) continue;
//
//                         if (reader.Name != "title")
//                         {
//                             reader.ReadToFollowing("page");
//                             continue;
//                         }
//
//                         //reader.ReadToFollowing("title");
//                         articleTitle = reader.ReadString();
//
//                         if (From == articleTitle)
//                         {
//                             break;
//                         }
//                     }
//                 }

                while (true)
                {
                    Page page = ParsePageTag(reader);
                    if (page == null)
                    {
                        break;
                    }

                    if (!indexOnly)
                    {
                        Revision oldRev = QueryRevision(page.Revision.Id);
                        if (oldRev != null)
                        {
                            Update(page.Revision);
                        }
                        else
                        {
                            Insert(page.Revision);
                        }
                    }
                    else
                    {
                        page.LastRevisionId = 0;
                    }

                    page.Language = languageCode;

                    Page oldPage = QueryPage(page.Title);
                    if (oldPage != null)
                    {
                        Update(page);
                    }
                    else
                    {
                        Insert(page);
                    }
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
                        page.Id = long.Parse(reader.ReadString());
                        continue;

                    case "title":
                        page.Title = reader.ReadString();
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

//                     case "restrictions":
//                         //rev.Restrictions = reader.ReadString();
//                     break;

                    case "text":
                        rev.Text = Encoding.UTF8.GetBytes(reader.ReadString());
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

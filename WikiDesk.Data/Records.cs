namespace WikiDesk.Data
{
    using System;

    using SQLite;

    public class Revision
    {
        [PrimaryKey]
        [Indexed]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Contributor { get; set; }

        public byte[] Text { get; set; }
    }

    public class Page
    {
        [PrimaryKey]
        [Indexed]
        public long Id { get; set; }

        [Indexed]
        public string Title { get; set; }

        public string Language { get; set; }

        public long LastRevisionId { get; set; }

        [Ignore]
        public Revision Revision { get; set; }
    }
}

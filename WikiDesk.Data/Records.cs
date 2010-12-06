namespace WikiDesk.Data
{
    using System;

    using SQLite;

    public class Revision
    {
        [PrimaryKey]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Contributor { get; set; }

        public byte[] Text { get; set; }
    }

    public class Page
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string Title { get; set; }

        public long LastRevisionId { get; set; }
    }
}

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

        [MaxLength(32)]
        public string Contributor { get; set; }

        [MaxLength(0)]
        public string Text { get; set; }
    }

    public class Page
    {
        [PrimaryKey]
        [Indexed]
        public long Id { get; set; }

        [Indexed]
        [MaxLength(256)]
        public string Title { get; set; }

        public long Language { get; set; }

        public long LastRevisionId { get; set; }

        [Ignore]
        public Revision Revision { get; set; }
    }
}

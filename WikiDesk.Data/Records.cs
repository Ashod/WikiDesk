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

        /// <summary>
        /// Reference into Domain table.
        /// </summary>
        [Indexed]
        public long Domain { get; set; }

        /// <summary>
        /// Reference into Language table.
        /// </summary>
        [Indexed]
        public long Language { get; set; }

        /// <summary>
        /// Reference into Revision table.
        /// </summary>
        public long LastRevisionId { get; set; }

        [Ignore]
        public Revision Revision { get; set; }
    }
}

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
}

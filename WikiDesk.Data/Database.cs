namespace WikiDesk.Data
{
    using SQLite;

    public partial class Database : SQLiteConnection
    {
        public Database(string path)
            : base(path)
        {
            CreateTable<Page>();
            CreateTable<Language>();
            CreateTable<Domain>();
        }
    }
}

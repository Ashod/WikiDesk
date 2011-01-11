namespace WikiDesk.Data
{
    using System.Linq;

    using SQLite;

    public partial class Database : SQLiteConnection
    {
        public Database(string path)
            : base(path)
        {
            CreateTable<Page>();
            CreateTable<Revision>();
            CreateTable<Language>();
            CreateTable<Domain>();
        }

        public Revision QueryRevision(long id)
        {
            return (from s in Table<Revision>()
                    where s.Id == id
                    select s).FirstOrDefault();
        }
    }
}

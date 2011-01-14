
namespace WikiDesk.Data.Test
{
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    public class DatabaseTests
    {
        [SetUp]
        public void Setup()
        {
            databaseFilename_ = Path.GetTempFileName();
            database_ = new Database(databaseFilename_);
        }

        [TearDown]
        public void TearDown()
        {
            database_.Dispose();

            try
            {
                File.Delete(databaseFilename_);
            }
            catch
            {
            }
        }

        [Test]
        public void SecondaryDatabaseInstance()
        {
            using (Database db = new Database(databaseFilename_))
            {
                Assert.NotNull(db);
            }
        }

        protected Database Database
        {
            get { return database_; }
        }

        #region representation

        private string databaseFilename_;

        private Database database_;

        #endregion // representation
    }
}

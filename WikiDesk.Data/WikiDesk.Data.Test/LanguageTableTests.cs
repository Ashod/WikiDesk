
namespace WikiDesk.Data.Test
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class LanguageTableTests : DatabaseTests
    {
        [Test]
        public void Equality()
        {
            Language lang1 = new Language { Name = "english", Code = "en" };
            Language lang2 = new Language { Name = "English", Code = "en" };
            Assert.That(lang1.CompareTo(lang2) < 0);

            lang2.Name = lang1.Name;
            lang2.Code = "En";
            Assert.That(lang1.CompareTo(lang2) < 0);

            lang2.Name = lang1.Name;
            lang2.Code = lang1.Code;
            Assert.That(lang1.CompareTo(lang2) == 0);
        }

        [Test]
        public void Insert()
        {
            Language langEn = new Language { Name = "english", Code = "en" };
            Assert.AreEqual(1, Database.Insert(langEn));
            Assert.AreEqual(langEn, Database.GetLanguageByName(langEn.Name));
            Assert.AreEqual(langEn, Database.GetLanguageByCode(langEn.Code));

            Language langDe = new Language { Name = "dutch", Code = "de" };
            Assert.AreEqual(1, Database.Insert(langDe));
            Assert.AreEqual(langDe, Database.GetLanguageByName(langDe.Name));
            Assert.AreEqual(langDe, Database.GetLanguageByCode(langDe.Code));

            IList<Language> languages = Database.GetLanguages();
            Assert.NotNull(languages);
            Assert.AreEqual(2, languages.Count);
        }

        [Test]
        public void Update()
        {
            Language langEn = new Language { Name = "english", Code = "en" };
            Assert.AreEqual(1, Database.Insert(langEn));

            langEn.Name = "English";
            langEn.Code = "En";
            Assert.AreEqual(1, Database.Update(langEn));

            Assert.AreEqual(langEn, Database.GetLanguageByName(langEn.Name));
            Assert.AreEqual(langEn, Database.GetLanguageByCode(langEn.Code));

            IList<Language> languages = Database.GetLanguages();
            Assert.NotNull(languages);
            Assert.AreEqual(1, languages.Count);
        }

        [Test]
        public void UpdateInsert()
        {
            Language langEn = new Language { Name = "english", Code = "en" };
            Assert.IsTrue(Database.UpdateInsert(langEn, null));
            Assert.AreEqual(langEn, Database.GetLanguageByName(langEn.Name));
            Assert.AreEqual(langEn, Database.GetLanguageByCode(langEn.Code));

            Language langEng = new Language { Name = "English", Code = "En" };
            Assert.IsFalse(Database.UpdateInsert(langEng, Database.GetLanguageByName(langEn.Name)));

            Assert.AreEqual(langEng, Database.GetLanguageByName(langEng.Name));
            Assert.AreEqual(langEng, Database.GetLanguageByCode(langEng.Code));

            IList<Language> languages = Database.GetLanguages();
            Assert.NotNull(languages);
            Assert.AreEqual(1, languages.Count);
        }

        [Test]
        public void Query()
        {
            Language langEn = new Language { Name = "english", Code = "en" };
            Assert.AreEqual(1, Database.Insert(langEn));

            Language langDe = new Language { Name = "dutch", Code = "de" };
            Assert.AreEqual(1, Database.Insert(langDe));

            Language langFr = new Language { Name = "french", Code = "fr" };
            Assert.AreEqual(1, Database.Insert(langFr));

            IList<Language> languages = Database.GetLanguages();
            Assert.NotNull(languages);
            Assert.AreEqual(3, languages.Count);

            Assert.AreEqual(langEn, languages[0]);
            Assert.AreEqual(langDe, languages[1]);
            Assert.AreEqual(langFr, languages[2]);
        }

        [Test]
        [ExpectedException(typeof(SQLite.SQLiteException))]
        public void LanguageUniqueName()
        {
            Language langEn = new Language { Name = "english", Code = "en" };
            Assert.AreEqual(1, Database.Insert(langEn));

            Language langEng = new Language { Name = "English", Code = "en" };
            Database.Insert(langEng);
        }

        [Test]
        [ExpectedException(typeof(SQLite.SQLiteException))]
        public void LanguageUniqueCode()
        {
            Language langEn = new Language { Name = "english", Code = "en" };
            Assert.AreEqual(1, Database.Insert(langEn));

            Language langEng = new Language { Name = "english", Code = "En" };
            Database.Insert(langEng);
        }
    }
}

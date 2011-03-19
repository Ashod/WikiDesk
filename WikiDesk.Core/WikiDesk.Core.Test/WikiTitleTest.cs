namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class WikiTitleTest
    {
        [Test]
        public void TitleNormalize()
        {
            Assert.AreEqual("Title", Title.Normalize("title"));
            Assert.AreEqual("Blah_may", Title.Normalize("blah may"));
            Assert.AreEqual("Title", Title.Normalize("Title"));
            Assert.AreEqual("Iron_Curtain", Title.Normalize("iron Curtain"));
        }

        [Test]
        public void TitleDeNormalize()
        {
            Assert.AreEqual("Title", Title.Denormalize("Title"));
            Assert.AreEqual("Blah may", Title.Denormalize("Blah_may"));
            Assert.AreEqual("Title", Title.Denormalize("Title"));
            Assert.AreEqual("Iron Curtain", Title.Denormalize("Iron_Curtain"));
        }

        [Test]
        public void ParseFullPageName()
        {
            string nameSpace;
            Assert.AreEqual("Title", Title.ParseFullPageName("Title", out nameSpace));
            Assert.AreEqual(string.Empty, nameSpace);

            Assert.AreEqual("Title", Title.ParseFullPageName("Wikipedia:Title", out nameSpace));
            Assert.AreEqual("Wikipedia", nameSpace);

            Assert.AreEqual("Main Page", Title.ParseFullPageName("Template:Main Page", out nameSpace));
            Assert.AreEqual("Template", nameSpace);

            Assert.AreEqual("Main Page", Title.ParseFullPageName(" :Main Page", out nameSpace));
            Assert.AreEqual(string.Empty, nameSpace);
        }

        [Test]
        public void FullPageName()
        {
            Assert.AreEqual("Wikipedia:Title", Title.FullPageName("Wikipedia", "Title"));
            Assert.AreEqual("Template:Main Page", Title.FullPageName("Template", "Main Page"));
            Assert.AreEqual("Main Page", Title.FullPageName(null, "Main Page"));
            Assert.AreEqual("Main Page", Title.FullPageName(string.Empty, "Main Page"));
            Assert.AreEqual("Main Page", Title.FullPageName("  ", "Main Page"));
        }
    }
}

﻿namespace WikiDesk.Core.Test
{
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    [TestFixture]
    public class Wiki2HtmlTest
    {
        static Wiki2HtmlTest()
        {
            RootPath = "TestFiles";
            WikiDomain wikiDomain = new WikiDomain("wikipedia");
            WikiLanguage wikiLanguage = new WikiLanguage("English", "en");
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WikiSite wikiSite = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
            Config = new Configuration(wikiSite);
        }

        [Test]
        public void RedirectColon()
        {
            TestConvertFiles("HeaderBoldItalic");
        }

        #region implementation

        private static void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html(Config);
            string nameSpace = string.Empty;
            string title = "TestPage";
            string html = converter.Convert(ref nameSpace, ref title, wikicode);
            Assert.AreEqual(expected, html);
        }

        private static void TestConvertFiles(string baseFilename)
        {
            Wiki2Html converter = new Wiki2Html(Config);
            string nameSpace = string.Empty;
            string title = "TestPage";
            string wikicode = File.ReadAllText(Path.Combine(RootPath, baseFilename + ".wiki"));
            string html = converter.Convert(ref nameSpace, ref title, wikicode);
            string expected = File.ReadAllText(Path.Combine(RootPath, baseFilename + ".html"));
            Assert.AreEqual(expected, html);
        }

        #endregion // implementation

        #region representation

        private static readonly string RootPath;
        private static readonly Configuration Config;

        #endregion // representation
    }
}
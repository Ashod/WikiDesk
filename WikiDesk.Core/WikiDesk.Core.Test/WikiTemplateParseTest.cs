namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    using WikiDesk.Data;

    [TestFixture]
    public class WikiTemplateParseTest
    {
        [Test]
        public void FindMagicBlock()
        {
            string magicBlock = MagicParser.FindMagicBlock("{{#if:{{{lang|}}}|{{{{{lang}}}}}&nbsp;}}");
            Assert.AreEqual("#if:{{{lang|}}}|{{{{{lang}}}}}&nbsp;", magicBlock);
        }

        [Test]
        public void FindMagicBlockNested()
        {
            const string RAW =
                "{{#if:{{{cat|{{{category|}}}}}}|a category|{{#if:{{{mul|{{{dab|{{{disambiguation|}}}}}}}}}|articles|{{#if:{{{mulcat|}}}|categories|{{#if:{{{portal|}}}|a portal|an article}}}}}}}}";

            string magicBlock = MagicParser.FindMagicBlock(RAW);
            Assert.AreEqual(
                "#if:{{{cat|{{{category|}}}}}}|a category|{{#if:{{{mul|{{{dab|{{{disambiguation|}}}}}}}}}|articles|{{#if:{{{mulcat|}}}|categories|{{#if:{{{portal|}}}|a portal|an article}}}}}}",
                magicBlock);

            magicBlock = MagicParser.FindMagicBlock(magicBlock);
            Assert.AreEqual(
                "#if:{{{mul|{{{dab|{{{disambiguation|}}}}}}}}}|articles|{{#if:{{{mulcat|}}}|categories|{{#if:{{{portal|}}}|a portal|an article}}}}",
                magicBlock);

            magicBlock = MagicParser.FindMagicBlock(magicBlock);
            Assert.AreEqual(
                "#if:{{{mulcat|}}}|categories|{{#if:{{{portal|}}}|a portal|an article}}",
                magicBlock);

            magicBlock = MagicParser.FindMagicBlock(magicBlock);
            Assert.AreEqual(
                "#if:{{{portal|}}}|a portal|an article",
                magicBlock);
        }

        [Test]
        public void TemplateA()
        {
            TestConvert("{{!}}",
                "<p>|</p>");
        }

        [Test]
        public void TemplateThankYou()
        {
            TestConvert("{{ThankYou}}",
                "<p><a href=\"http://en.wikipedia.org/wiki/File:Face-smile.svg\" class=\"image\"><img alt=\"Face-smile.svg\" src=\"http://upload.wikimedia.org/wikipedia/commons/thumb/7/79/Face-smile.svg/48px-Face-smile.svg.png\" width=\"18\"></a></p> '''Thank you'''<!--Template:Thank you-->");
        }

        [Test]
        public void TemplateLang()
        {
            TestConvert("{{lang-ka|kikos}}",
                "<p><span lang=\"ka\" xml:lang=\"ka\">kikos</span></p>");
        }

        [Test]
        public void TemplateMain()
        {
            TestConvert("{{Main|History of Tbilisi}}",
                "<div class=\"rellink relarticle mainarticle\">Main article: <a href=\"http://en.wikipedia.org/wiki/History_of_Tbilisi\" title=\"History of Tbilisi\">History of Tbilisi</a></div>");
        }

        [Test]
        public void TemplateOCLC()
        {
            TestConvert("{{OCLC|224781861}}",
                "<p><a href=\"http://en.wikipedia.org/wiki/Online_Computer_Library_Center\" title=\"Online Computer Library Center\">OCLC</a>" +
                "&nbsp;<a href=\"http://www.worldcat.org/oclc/224781861\" class=\"external text\" rel=\"nofollow\">224781861</a></p>");
        }

        internal static void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html(config_, null, OnResolveTemplate, null);
            string html = converter.Convert(wikicode);
            Assert.AreEqual(expected, html);
        }

        private static string OnResolveTemplate(string word, string lanugageCode)
        {
            string title = !word.StartsWith("Template:") ? "Template:" + word : word;

            title = Title.Normalize(title);
            string url = string.Concat("http://", config_.CurrentLanguageCode, config_.ExportUrl, title);
            string xmlText = Download.DownloadPage(url);
            Page page = DumpParser.PageFromXml(xmlText);
            if (page == null)
            {
                return string.Empty;
            }

            string text = page.Text;
            while (true)
            {
                int start = text.IndexOf("<noinclude>");
                if (start < 0)
                {
                    break;
                }

                int end = text.IndexOf("</noinclude>", start);
                if (end < 0)
                {
                    break;
                }

                text = text.Remove(start, end - start + "</noinclude>".Length);
            }

            return text;
        }

        #region representation

        private static readonly Configuration config_ = new Configuration(
                            "en",
                            ".wikipedia.org/wiki/",
                            ".wikipedia.org/wiki/Special:Export/");

        #endregion // representation

    }
}

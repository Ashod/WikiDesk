﻿namespace WikiDesk.Core.Test
{
    using System.Collections.Generic;

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
        public void GetMagicWordAndParamsWeather()
        {
            const string RAW = "Weather|Maine|cold";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("Weather", command);
            Assert.AreEqual("1", args[0].Key);
            Assert.AreEqual("Maine", args[0].Value);
            Assert.AreEqual("2", args[1].Key);
            Assert.AreEqual("cold", args[1].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFurther()
        {
            const string RAW = "Further|[[Cats]], [[Dogs]], and [[Fish]]";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("Further", command);
            Assert.AreEqual("1", args[0].Key);
            Assert.AreEqual("[[Cats]], [[Dogs]], and [[Fish]]", args[0].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFunction()
        {
            const string RAW = "#if:{{{lang|}}}|{{{{{lang}}}}}&nbsp;";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("#if", command);
            Assert.AreEqual("1", args[0].Key);
            Assert.AreEqual("{{{lang|}}}", args[0].Value);
            Assert.AreEqual("2", args[1].Key);
            Assert.AreEqual("{{{{{lang}}}}}&nbsp;", args[1].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFunctionWiki()
        {
            const string RAW = "#if:{{{lang|}}}|{{{{{lang}}}}}[[Canton of Zürich|Canton Zürich]]";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("#if", command);
            Assert.AreEqual("1", args[0].Key);
            Assert.AreEqual("{{{lang|}}}", args[0].Value);
            Assert.AreEqual("2", args[1].Key);
            Assert.AreEqual("{{{{{lang}}}}}[[Canton of Zürich|Canton Zürich]]", args[1].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFunctionWiki2()
        {
            const string RAW = "#if:{{{lang|}}}|{{{{{lang}}}}}[[Canton of Zürich|Canton Zürich]]|{{name|}}[[link|link]]{{{name|}}}";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("#if", command);
            Assert.AreEqual("1", args[0].Key);
            Assert.AreEqual("{{{lang|}}}", args[0].Value);
            Assert.AreEqual("2", args[1].Key);
            Assert.AreEqual("{{{{{lang}}}}}[[Canton of Zürich|Canton Zürich]]", args[1].Value);
            Assert.AreEqual("3", args[2].Key);
            Assert.AreEqual("{{name|}}[[link|link]]{{{name|}}}", args[2].Value);
        }

        [Test]
        public void GetMagicWordAndParamsNewLine()
        {
            const string RAW = "navbox\n|name  = Canton Zurich\n|title = Districts of [[Canton of Zürich|Canton Zürich]]\n\n|list1 = [[Affoltern (district)|Affoltern]]{{·}} [[Andelfingen (district)|Andelfingen]]{{·}} [[Bülach (district)|Bülach]]{{·}} [[Dielsdorf (district)|Dielsdorf]]{{·}} [[Dietikon (district)|Dietikon]]{{·}} [[Hinwil (district)|Hinwil]]{{·}} [[Horgen (district)|Horgen]]{{·}} [[Meilen (district)|Meilen]]{{·}} [[Pfäffikon (district)|Pfäffikon]]{{·}} [[Uster (district)|Uster]]{{·}} [[Winterthur (district)|Winterthur]]{{·}} [[Zürich (district)|Zurich]]\n\n|below = [[Districts of Switzerland#Zurich|Districts of Switzerland]]{{·}} [[Municipalities of the canton of Zurich]]\n";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("navbox", command);
            Assert.AreEqual("name", args[0].Key);
            Assert.AreEqual("Canton Zurich", args[0].Value);
            Assert.AreEqual("title", args[1].Key);
            Assert.AreEqual("Districts of [[Canton of Zürich|Canton Zürich]]", args[1].Value);
            Assert.AreEqual("list1", args[2].Key);
            Assert.AreEqual("[[Affoltern (district)|Affoltern]]{{·}} [[Andelfingen (district)|Andelfingen]]{{·}} [[Bülach (district)|Bülach]]{{·}} [[Dielsdorf (district)|Dielsdorf]]{{·}} [[Dietikon (district)|Dietikon]]{{·}} [[Hinwil (district)|Hinwil]]{{·}} [[Horgen (district)|Horgen]]{{·}} [[Meilen (district)|Meilen]]{{·}} [[Pfäffikon (district)|Pfäffikon]]{{·}} [[Uster (district)|Uster]]{{·}} [[Winterthur (district)|Winterthur]]{{·}} [[Zürich (district)|Zurich]]", args[2].Value);
            Assert.AreEqual("below", args[3].Key);
            Assert.AreEqual("[[Districts of Switzerland#Zurich|Districts of Switzerland]]{{·}} [[Municipalities of the canton of Zurich]]", args[3].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFurtherComplex()
        {
            const string RAW = "Further|[[Article 1]], [[Article 2]], and [[Article Something#3|Article 3]]";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("Further", command);
            Assert.AreEqual("1", args[0].Key);
            Assert.AreEqual("[[Article 1]], [[Article 2]], and [[Article Something#3|Article 3]]", args[0].Value);
        }

        [Test]
        public void ProcessTemplateParamsA()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("1", "a")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start-a-end", value);
        }

        [Test]
        public void ProcessTemplateParamsSpace()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("1", " ")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start- -end", value);
        }

        [Test]
        public void ProcessTemplateParamsBlank()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("1", string.Empty)
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start--end", value);
        }

        [Test]
        public void ProcessTemplateParamsNamed()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("1", "no surprise")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start-no surprise-end", value);
        }

        [Test]
        public void ProcessTemplateParamsNone()
        {
            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", null);
            Assert.AreEqual("start-pqr-end", value);
        }

        [Test]
        public void ProcessTemplateParamsWrong()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("2", "a")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start-pqr-end", value);
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
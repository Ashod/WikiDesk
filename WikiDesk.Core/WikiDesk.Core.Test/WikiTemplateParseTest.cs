// -----------------------------------------------------------------------------------------
// <copyright file="WikiTemplateParseTest.cs" company="ashodnakashian.com">
//
// This file is part of WikiDesk.
// Copyright (C) 2010, 2011 Ashod Nakashian
// https://github.com/Ashod/WikiDesk
//
//  WikiDesk is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  WikiDesk is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with WikiDesk. If not, see http://www.gnu.org/licenses/.
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the WikiTemplateParseTest type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    using WikiDesk.Data;

    [TestFixture]
    public class WikiTemplateParseTest
    {
        static WikiTemplateParseTest()
        {
            WikiDomain wikiDomain = new WikiDomain("wikipedia");
            WikiLanguage wikiLanguage = new WikiLanguage("English", "en");
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            WikiSite wikiSite = new WikiSite(wikiDomain, wikiLanguage, folder + "\\..\\");
            config_ = new Configuration(wikiSite);
        }

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
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("Maine", args[0].Value);
            Assert.IsNull(args[1].Key);
            Assert.AreEqual("cold", args[1].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFurther()
        {
            const string RAW = "Further|[[Cats]], [[Dogs]], and [[Fish]]";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("Further", command);
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("[[Cats]], [[Dogs]], and [[Fish]]", args[0].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFunction()
        {
            const string RAW = "#if:{{{lang|}}}|{{{{{lang}}}}}&nbsp;";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("#if:", command);
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("{{{lang|}}}", args[0].Value);
            Assert.IsNull(args[1].Key);
            Assert.AreEqual("{{{{{lang}}}}}&nbsp;", args[1].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFunctionWiki()
        {
            const string RAW = "#if:{{{lang|}}}|{{{{{lang}}}}}[[Canton of Zürich|Canton Zürich]]";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("#if:", command);
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("{{{lang|}}}", args[0].Value);
            Assert.IsNull(args[1].Key);
            Assert.AreEqual("{{{{{lang}}}}}[[Canton of Zürich|Canton Zürich]]", args[1].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFunctionWiki2()
        {
            const string RAW = "#if:{{{lang|}}}|{{{{{lang}}}}}[[Canton of Zürich|Canton Zürich]]|{{name|}}[[link|link]]{{{name|}}}";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("#if:", command);
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("{{{lang|}}}", args[0].Value);
            Assert.IsNull(args[1].Key);
            Assert.AreEqual("{{{{{lang}}}}}[[Canton of Zürich|Canton Zürich]]", args[1].Value);
            Assert.IsNull(args[2].Key);
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
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("[[Article 1]], [[Article 2]], and [[Article Something#3|Article 3]]", args[0].Value);
        }

        [Test]
        public void GetMagicWordAndParamsFullUrl()
        {
            const string RAW = "fullurl:{{FULLPAGENAME}}|action=edit";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("fullurl:", command);
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("{{FULLPAGENAME}}", args[0].Value);
            Assert.AreEqual("action", args[1].Key);
            Assert.AreEqual("edit", args[1].Value);
        }

        [Test]
        public void GetMagicWordAndParamsSwitch1()
        {
            const string RAW = "#switch:{{{2}}}|1=C41E3A|2=FFFFFF|3=000000|4=C41E3A";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("#switch:", command);
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("{{{2}}}", args[0].Value);
            Assert.AreEqual("1", args[1].Key);
            Assert.AreEqual("C41E3A", args[1].Value);
            Assert.AreEqual("2", args[2].Key);
            Assert.AreEqual("FFFFFF", args[2].Value);
            Assert.AreEqual("3", args[3].Key);
            Assert.AreEqual("000000", args[3].Value);
            Assert.AreEqual("4", args[4].Key);
            Assert.AreEqual("C41E3A", args[4].Value);
        }

        [Test]
        public void GetMagicWordAndParamsSwitch2()
        {
            const string RAW = "#switch: baz | foo = Foo | baz = Baz | Bar ";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            Assert.AreEqual("#switch:", command);
            Assert.IsNull(args[0].Key);
            Assert.AreEqual("baz", args[0].Value);
            Assert.AreEqual("foo", args[1].Key);
            Assert.AreEqual("Foo", args[1].Value);
            Assert.AreEqual("baz", args[2].Key);
            Assert.AreEqual("Baz", args[2].Value);
            Assert.IsNull(args[3].Key);
            Assert.AreEqual("Bar", args[3].Value);
        }

        [Test]
        public void GetMagicWordAndParamsInfoboxCountry()
        {
            const string RAW =
@"Infobox Country|
 fullcountryname= Republic of Armenia<br />
Hayastani Hanrapetutyun |
 image_flag= Flag of Armenia.svg |
 image_coa= Coat of arms of Armenia.svg |
 image_location=LocationArmenia.png |
 nationalmotto=One Nation, One Culture |
 nationalsong=Our Fatherland |
 nationalflower=n/a |
 nationalanimal=n/a |
 officiallanguages= [[Armenian language|Armenian]] |
 populationtotal=3,016,000 |
 populationrank=136 |
 populationdensity=73 |
 countrycapital=[[Yerevan]] |
 countrylargestcity=[[Yerevan]] |
 areatotal= 29,800 |
 arearank= 139 |
 areawater= n/a |
 areawaterpercent=n/a |
 establishedin= [[September 21]], [[1991]] |
 leadertitlename=[[President of Armenia|President]]: [[Robert Kocharian]]<br />[[Prime Minister of Armenia|Prime Minister]]: Serzh Sargsyan |
 currency= [[Dram]] (AMD) |
 utcoffset=+05:00 |
 dialingcode=374 |
 internettld=.am
";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);
            Assert.AreEqual("Infobox Country", command);
            Assert.AreEqual("fullcountryname", args[0].Key);
            Assert.AreEqual("Republic of Armenia<br />\r\nHayastani Hanrapetutyun", args[0].Value);
            Assert.AreEqual("image_flag", args[1].Key);
            Assert.AreEqual("Flag of Armenia.svg", args[1].Value);
            Assert.AreEqual("image_coa", args[2].Key);
            Assert.AreEqual("Coat of arms of Armenia.svg", args[2].Value);
            Assert.AreEqual("image_location", args[3].Key);
            Assert.AreEqual("LocationArmenia.png", args[3].Value);
            Assert.AreEqual("nationalmotto", args[4].Key);
            Assert.AreEqual("One Nation, One Culture", args[4].Value);
            Assert.AreEqual("nationalsong", args[5].Key);
            Assert.AreEqual("Our Fatherland", args[5].Value);
            Assert.AreEqual("nationalflower", args[6].Key);
            Assert.AreEqual("n/a", args[6].Value);
            Assert.AreEqual("nationalanimal", args[7].Key);
            Assert.AreEqual("n/a", args[7].Value);
            Assert.AreEqual("officiallanguages", args[8].Key);
            Assert.AreEqual("[[Armenian language|Armenian]]", args[8].Value);
            Assert.AreEqual("populationtotal", args[9].Key);
            Assert.AreEqual("3,016,000", args[9].Value);
            Assert.AreEqual("populationrank", args[10].Key);
            Assert.AreEqual("136", args[10].Value);
            Assert.AreEqual("populationdensity", args[11].Key);
            Assert.AreEqual("73", args[11].Value);
            Assert.AreEqual("countrycapital", args[12].Key);
            Assert.AreEqual("[[Yerevan]]", args[12].Value);
            Assert.AreEqual("countrylargestcity", args[13].Key);
            Assert.AreEqual("[[Yerevan]]", args[13].Value);
            Assert.AreEqual("areatotal", args[14].Key);
            Assert.AreEqual("29,800", args[14].Value);
            Assert.AreEqual("arearank", args[15].Key);
            Assert.AreEqual("139", args[15].Value);
            Assert.AreEqual("areawater", args[16].Key);
            Assert.AreEqual("n/a", args[16].Value);
            Assert.AreEqual("areawaterpercent", args[17].Key);
            Assert.AreEqual("n/a", args[17].Value);
            Assert.AreEqual("establishedin", args[18].Key);
            Assert.AreEqual("[[September 21]], [[1991]]", args[18].Value);
            Assert.AreEqual("leadertitlename", args[19].Key);
            Assert.AreEqual("[[President of Armenia|President]]: [[Robert Kocharian]]<br />[[Prime Minister of Armenia|Prime Minister]]: Serzh Sargsyan", args[19].Value);
            Assert.AreEqual("currency", args[20].Key);
            Assert.AreEqual("[[Dram]] (AMD)", args[20].Value);
            Assert.AreEqual("utcoffset", args[21].Key);
            Assert.AreEqual("+05:00", args[21].Value);
            Assert.AreEqual("dialingcode", args[22].Key);
            Assert.AreEqual("374", args[22].Value);
            Assert.AreEqual("internettld", args[23].Key);
            Assert.AreEqual(".am", args[23].Value);
        }

        [Test]
        public void GetMagicWordAndParamsComplexTemplate()
        {
            const string RAW =
                "Citation/make link" +
"     | 1={{" +
"           #if: {{#if:||http://www.afraidtoask.com/?page_id=119}}" +
"           |{{#if:||http://www.afraidtoask.com/?page_id=119}}" +
"           |{{" +
"              #if: " +
"              |" +
"" +
"              |{{#ifexpr:{{#time: U}} > {{#time: U | 1010-10-10 }}" +
"                |{{" +
"                   #if: " +
"                   |http://www.pubmedcentral.nih.gov/articlerender.fcgi?tool=pmcentrez&artid=" +
"                 }}" +
"               }}" +
"            }}" +
"         }}" +
"     | 2={{" +
"           #if: " +
"           |''<nowiki />{{" +
"    #if:Size and Shape" +
"    |Size and Shape" +
"    |{{" +
"      #if:" +
"      |" +
"      |{{Citation error|no <code>&#124;title&#61;</code> specified|Cite web}}" +
"      }}" +
"    }}<nowiki />''" +
"           |\"{{" +
"    #if:Size and Shape" +
"    |Size and Shape" +
"    |{{" +
"      #if:" +
"      |" +
"      |{{Citation error|no <code>&#124;title&#61;</code> specified|Cite web}}" +
"      }}" +
"    }}{{" +
"             #if: " +
"             |{{" +
"                #if: {{" +
"    #if:Size and Shape" +
"    |Size and Shape" +
"    |{{" +
"      #if:" +
"      |" +
"      |{{Citation error|no <code>&#124;title&#61;</code> specified|Cite web}}" +
"      }}" +
"    }}" +
"                |&#32;" +
"              }}&#91;&#93;" +
"           }}\"" +
"         }}";

            List<KeyValuePair<string, string>> args;
            string command = MagicParser.GetMagicWordAndParams(RAW, out args);

            Assert.AreEqual("Citation/make link", command);
            Assert.AreEqual("1", args[0].Key);
            Assert.AreEqual("2", args[1].Key);
        }

        #region ProcessTemplateParams

        [Test]
        public void ProcessTemplateParamsA()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("1", "a")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start-a-end", value);
        }

        [Test]
        public void ProcessTemplateUnnamedParam()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(string.Empty, "a")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start-a-end", value);
        }

        [Test]
        public void ProcessTemplateParamsSpace()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("1", " ")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start- -end", value);
        }

        [Test]
        public void ProcessTemplateParamsBlank()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("1", string.Empty)
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start--end", value);
        }

        [Test]
        public void ProcessTemplateParamsNamed()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("1", "no surprise")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start-no surprise-end", value);
        }

        [Test]
        public void ProcessTemplateParamsNone()
        {
            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", new List<KeyValuePair<string, string>>());
            Assert.AreEqual("start-pqr-end", value);
        }

        [Test]
        public void ProcessTemplateParamsWrong()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("2", "a")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{1|pqr}}}-end", args);
            Assert.AreEqual("start-pqr-end", value);
        }

        [Test]
        public void ProcessTemplateParamsPartialName()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("title", "Shape and Form")
                };

            string value = MagicParser.ProcessTemplateParams("start-{{{trans_title|}}}-end", args);
            Assert.AreEqual("start--end", value);
        }

        [Test]
        public void ProcessTemplateParamsNestedBraces()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("langue", "en")
                };

            string value = MagicParser.ProcessTemplateParams("Correct title|{{{{{{langue|}}}|reason=oui}}}", args);
            Assert.AreEqual("Correct title|reason=oui", value);
        }

        [Test]
        public void ProcessTemplateParamsNestedCyclic()
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("langue", "langue")
                };

            string value = MagicParser.ProcessTemplateParams("Correct title|{{{{{{langue|langue}}}|langue}}}", args);
            Assert.AreEqual("Correct title|langue", value);
        }

        #endregion // ProcessTemplateParams

        [Test]
        public void TemplateA()
        {
            TestConvert("{{!}}",
                "<p>|</p>");
        }

        [Test]
        public void TemplateExclaimExplicit()
        {
            TestConvert("{{Template:!}}",
                "<p>|</p>");
        }

        [Test]
        public void TemplateThankYou()
        {
            TestConvert("{{ThankYou}}",
                "<p><a href=\"http://en.wikipedia.org/wiki/File:Face-smile.svg\" class=\"image\"><img alt=\"Face-smile.svg\" src=\"http://upload.wikimedia.org/wikipedia/commons/thumb/7/79/Face-smile.svg/512px-Face-smile.svg.png\" width=\"18\"></a> <b>Thank you</b></p>");
        }

        [Test]
        public void TemplateLang()
        {
            TestConvert("{{lang-ka|kikos}}",
                "<p><a href=\"Georgian_language\" title=\"Georgian language\">Georgian</a>: <span lang=\"ka\" xml:lang=\"ka\">kikos</span><a href=\"Category%3aArticles_containing_Georgian_language_text\" title=\"Category:Articles containing Georgian language text\">Category:Articles containing Georgian language text</a></p>");
        }

        [Test]
        public void TemplateCitationError()
        {
            TestConvert("{{Citation error|no <code>&#124;title&#61;</code> specified|Cite web}}",
                "<span class=\"error\">Error: no <code>&#124;title&#61;</code> specified&#32;when using {{<a href=\"Template%3aCite_web\" title=\"Template:Cite web\">Cite web</a>}}</span>");
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
                "<p><a href=\"Online_Computer_Library_Center\" title=\"Online Computer Library Center\">OCLC</a>" +
                "&nbsp;<a href=\"http://www.worldcat.org/oclc/224781861\" class=\"external text\" rel=\"nofollow\">224781861</a></p>");
        }

        [Test]
        public void TwoOtherUses()
        {
            TestConvert(
                    "{{Two other uses|the Male sex|the city|Malé}}",
                    @"<div class=""dablink"">This article is about the Male sex.&#32;&#32;For the city, see <a href=""Mal%c3%a9"" title=""Malé"">Malé</a>.&#32;&#32;For other uses, see <a href=""TestPage_(disambiguation)"" title=""TestPage (disambiguation)"">TestPage (disambiguation)</a>.</div>");
        }

        [Test]
        public void CorrectTitle()
        {
            TestConvert(
                    "{{Correct title|Correct title}}",
                    @"<div class=""dablink""><span class=""plainlinks selfreference"">The correct title of this article is <b>Correct title</b>. It appears incorrectly here because of <a href=""Wikipedia%3aNaming_conventions_(technical_restrictions)"" title=""Wikipedia:Naming conventions (technical restrictions)"">technical restrictions</a>.</span></div>");
        }

        [Test]
        public void CorrectTitleNoArg()
        {
            TestConvert(
                    "{{Correct title}}",
                    @"<div class=""dablink""><span class=""plainlinks selfreference"">The correct title of this article is <b>{{{1}}}</b>. It appears incorrectly here because of <a href=""Wikipedia%3aNaming_conventions_(technical_restrictions)"" title=""Wikipedia:Naming conventions (technical restrictions)"">technical restrictions</a>.</span></div>");
        }

        [Test]
        public void CorrectTitleNested()
        {
            TestConvert(
                    "{{Correct title|{{{{{{langue|}}}|reason=oui}}}}}",
                    @"<div class=""dablink""><span class=""plainlinks selfreference"">The correct title of this article is <b>reason=oui</b>. It appears incorrectly here because of <a href=""Wikipedia%3aNaming_conventions_(technical_restrictions)"" title=""Wikipedia:Naming conventions (technical restrictions)"">technical restrictions</a>.</span></div>");
        }

        [Test]
        public void CorrectTitleSharp()
        {
            TestConvert(
                    "{{Correct title|C# (programming language)|reason=#}}",
                    @"<div class=""dablink""><span class=""plainlinks selfreference"">The correct title of this article is <b>C# (programming language)</b>. The substitution or omission of the <a href=""Number_sign"" title=""Number sign"">#</a> sign is because of <a href=""Wikipedia%3aNaming_conventions_(technical_restrictions)%23Forbidden_characters"" title=""Wikipedia:Naming conventions (technical restrictions)"">technical restrictions</a>.</span></div>");
        }

        #region implementation

        internal static void TestConvert(string wikicode, string expected)
        {
            Wiki2Html converter = new Wiki2Html(config_, OnResolveWikiLinks, OnResolveTemplate, null);
            string nameSpace = string.Empty;
            string title = "TestPage";
            string html = converter.Convert(ref nameSpace, ref title, wikicode);
            Assert.AreEqual(expected, html);
        }

        private static string OnResolveWikiLinks(string title, string languageCode)
        {
            return Title.UrlCanonicalize(title);
        }

        private static string OnResolveTemplate(string word, string lanugageCode)
        {
            string title = !word.StartsWith("Template:") ? "Template:" + word : word;

            title = Title.Canonicalize(title);
            string url = string.Concat("http://", config_.WikiSite.Language.Code, config_.WikiSite.ExportUrl, title);
            string xmlText = Download.DownloadPage(url);
            Page page = DumpParser.PageFromXml(xmlText);
            return page != null ? page.Text : string.Empty;
        }

        #endregion // implementation

        #region representation

        private static readonly Configuration config_;

        #endregion // representation
    }
}

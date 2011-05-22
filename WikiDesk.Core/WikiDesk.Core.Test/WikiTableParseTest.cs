namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class WikiTableParseTest
    {
        [Test]
        public void TableSimple()
        {
            WikiParseTest.TestConvert(
@"{|
| A
| B
|-
| C
| D
|}",
@"<table>
<tbody><tr>
<td>A</td>
<td>B</td>
</tr>
<tr>
<td>C</td>
<td>D</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void TableInlineCells()
        {
            WikiParseTest.TestConvert(
@"{|
|+ The table's caption
|-
|Cell 1 || Cell 2 || Cell 3
|-
|Cell A
|Cell B
|Cell C
|}
",
@"<table>
<caption>The table's caption</caption>
<tbody><tr>
<td>Cell 1</td>
<td>Cell 2</td>
<td>Cell 3</td>
</tr>
<tr>
<td>Cell A</td>
<td>Cell B</td>
<td>Cell C</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void TableBordered()
        {
            WikiParseTest.TestConvert(
@"{| border=""1""
|-
|format modifier (not displayed) |These all  |(including the pipes) |go into  |the first cell
|}
",
@"<table border=""1"">
<tbody><tr>
<td>These all  |(including the pipes) |go into  |the first cell</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void TableModifier()
        {
            WikiParseTest.TestConvert(
@"{| border=""1""
|-
| Cell 1 (no modifier — not aligned)
|-
| align=""right"" | Cell 2 (right aligned)
|}
",
@"<table border=""1"">
<tbody><tr>
<td>Cell 1 (no modifier — not aligned)</td>
</tr>
<tr>
<td align=""right"">Cell 2 (right aligned)</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void TableScopeCol()
        {
            WikiParseTest.TestConvert(
@"{|
|+ The table's caption
! scope=""col"" | Column heading 1
! scope=""col"" | Column heading 2
! scope=""col"" | Column heading 3
|-
| Cell 1 || Cell 2 || Cell 3
|-
| Cell A
| Cell B
| Cell C
|}
",
@"<table>
<caption>The table's caption</caption>
<tbody><tr>
<th scope=""col"">Column heading 1</th>
<th scope=""col"">Column heading 2</th>
<th scope=""col"">Column heading 3</th>
</tr>
<tr>
<td>Cell 1</td>
<td>Cell 2</td>
<td>Cell 3</td>
</tr>
<tr>
<td>Cell A</td>
<td>Cell B</td>
<td>Cell C</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void TableScopeRow()
        {
            WikiParseTest.TestConvert(
@"{|
|+ The table's caption
! scope=""col"" | Column heading 1
! scope=""col"" | Column heading 2
! scope=""col"" | Column heading 3
|-
! scope=""row"" | Row heading 1
| Cell 2 || Cell 3
|-
! scope=""row"" | Row heading A
| Cell B
| Cell C
|}
",
@"<table>
<caption>The table's caption</caption>
<tbody><tr>
<th scope=""col"">Column heading 1</th>
<th scope=""col"">Column heading 2</th>
<th scope=""col"">Column heading 3</th>
</tr>
<tr>
<th scope=""row"">Row heading 1</th>
<td>Cell 2</td>
<td>Cell 3</td>
</tr>
<tr>
<th scope=""row"">Row heading A</th>
<td>Cell B</td>
<td>Cell C</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void TableBorder()
        {
            WikiParseTest.TestConvert(
@"{| border=""1""
|+ The table's caption
! scope=""col"" | Column heading 1
! scope=""col"" | Column heading 2
! scope=""col"" | Column heading 3
|-
! scope=""row"" | Row heading 1
| Cell 2 || Cell 3
|-
! scope=""row"" | Row heading A
| Cell B
| Cell C
|}
",
@"<table border=""1"">
<caption>The table's caption</caption>
<tbody><tr>
<th scope=""col"">Column heading 1</th>
<th scope=""col"">Column heading 2</th>
<th scope=""col"">Column heading 3</th>
</tr>
<tr>
<th scope=""row"">Row heading 1</th>
<td>Cell 2</td>
<td>Cell 3</td>
</tr>
<tr>
<th scope=""row"">Row heading A</th>
<td>Cell B</td>
<td>Cell C</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void MultiplicationTable()
        {
            WikiParseTest.TestConvert(
@"{| class=""wikitable"" style=""text-align: center; width: 200px; height: 200px;""
|+ Multiplication table
|-
! scope=""col"" | ×
! scope=""col"" | 1
! scope=""col"" | 2
! scope=""col"" | 3
|-
! scope=""row"" | 1
| 1 || 2 || 3
|-
! scope=""row"" | 2
| 2 || 4 || 6
|-
! scope=""row"" | 3
| 3 || 6 || 9
|-
! scope=""row"" | 4
| 4 || 8 || 12
|-
! scope=""row"" | 5
| 5 || 10 || 15
|}
",
@"<table class=""wikitable"" style=""text-align: center; width: 200px; height: 200px;"">
<caption>Multiplication table</caption>
<tbody><tr>
<th scope=""col"">×</th>
<th scope=""col"">1</th>
<th scope=""col"">2</th>
<th scope=""col"">3</th>
</tr>
<tr>
<th scope=""row"">1</th>
<td>1</td>
<td>2</td>
<td>3</td>
</tr>
<tr>
<th scope=""row"">2</th>
<td>2</td>
<td>4</td>
<td>6</td>
</tr>
<tr>
<th scope=""row"">3</th>
<td>3</td>
<td>6</td>
<td>9</td>
</tr>
<tr>
<th scope=""row"">4</th>
<td>4</td>
<td>8</td>
<td>12</td>
</tr>
<tr>
<th scope=""row"">5</th>
<td>5</td>
<td>10</td>
<td>15</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void BackgrounColor()
        {
            WikiParseTest.TestConvert(
@"{|
| style=""background: red; color: white"" | abc
| def
| bgcolor=""red"" | <font color=""white""> ghi </font>
| jkl
|}
",
@"<table>
<tbody><tr>
<td style=""background: red; color: white"">abc</td>
<td>def</td>
<td bgcolor=""red""><font color=""white""> ghi </font></td>
<td>jkl</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void BackgroundColorComplex()
        {
            WikiParseTest.TestConvert(
@"{| style=""background: yellow; color: green""
|-
| abc || def || ghi
|- style=""background: red; color: white""
| jkl || mno || pqr
|-
| stu || style=""background: silver"" | vwx || yz
|}
",
@"<table style=""background: yellow; color: green"">
<tbody><tr>
<td>abc</td>
<td>def</td>
<td>ghi</td>
</tr>
<tr style=""background: red; color: white"">
<td>jkl</td>
<td>mno</td>
<td>pqr</td>
</tr>
<tr>
<td>stu</td>
<td style=""background: silver"">vwx</td>
<td>yz</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void HeightWidth()
        {
            WikiParseTest.TestConvert(
@"{| style=""width: 75%; height: 200px"" border=""1""
|-
| abc || def || ghi
|- style=""height: 100px;""
| jkl || style=""width: 200px;"" | mno || pqr
|-
| stu || vwx || yz
|}
",
@"<table style=""width: 75%; height: 200px"" border=""1"">
<tbody><tr>
<td>abc</td>
<td>def</td>
<td>ghi</td>
</tr>
<tr style=""height: 100px;"">
<td>jkl</td>
<td style=""width: 200px;"">mno</td>
<td>pqr</td>
</tr>
<tr>
<td>stu</td>
<td>vwx</td>
<td>yz</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void ColWidth()
        {
            WikiParseTest.TestConvert(
@"{| border=""1"" cellpadding=""2""
! scope=""col"" width=""50"" | Name
! scope=""col"" width=""225"" | Effect
! scope=""col"" width=""225"" | Games Found In
|-
| Poké Ball || Regular Poké Ball || All Versions
|-
| Great Ball || Better than a Poké Ball || All Versions
|}
",
@"<table border=""1"" cellpadding=""2"">
<tbody><tr>
<th scope=""col"" width=""50"">Name</th>
<th scope=""col"" width=""225"">Effect</th>
<th scope=""col"" width=""225"">Games Found In</th>
</tr>
<tr>
<td>Poké Ball</td>
<td>Regular Poké Ball</td>
<td>All Versions</td>
</tr>
<tr>
<td>Great Ball</td>
<td>Better than a Poké Ball</td>
<td>All Versions</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void Width()
        {
            WikiParseTest.TestConvert(
@"{| border=""1"" cellpadding=""2""
|-
| width=""100pt"" | This column is 100 points wide
| width=""200pt"" | This column is 200 points wide
| width=""300pt"" | This column is 300 points wide
|-
| blah || blih || bluh
|}
",
@"<table border=""1"" cellpadding=""2"">
<tbody><tr>
<td width=""100pt"">This column is 100 points wide</td>
<td width=""200pt"">This column is 200 points wide</td>
<td width=""300pt"">This column is 300 points wide</td>
</tr>
<tr>
<td>blah</td>
<td>blih</td>
<td>bluh</td>
</tr>
</tbody></table>
");
        }

        [Test]
        [Ignore]
        public void InfoBox()
        {
            WikiParseTest.TestConvert(
@"{| class=""infobox"" style=\""font-size: 85%; text-align: left; width: 300px;"" summary=""Template:Infobox Website""
|+ style=""padding:7px; font-size: 1.5em; vertical-align: middle;"" | <b>Wiktionary</b>
""2"" style=""text-align: center;""><a href=""http://simple.wikipedia.org/wiki/File:Wiktionary-logo-en.png"" class=""image"" title=""Wiktionary logo in regular English""><img alt=""Wiktionary logo in regular English"" src=""file:///Z:/wikidesk_cache/Wiktionary-logo-en.png"" width=""128""></a> <a href=""http://simple.wikipedia.org/wiki/File:WiktionaryEn.svg"" class=""image"" title=""Wiktionary logo in Simple English""><img alt=""Wiktionary logo in Simple English"" src=""file:///Z:/wikidesk_cache/400px-WiktionaryEn.svg.png"" width=""128""></a></td></tr><tr>
| colspan=""2"" align=""center"" | <a href=""http://simple.wikipedia.org/wiki/File:Www.wiktionary.org_screenshot.png"" class=""image""><img alt=""Www.wiktionary.org screenshot.png"" src=""file:///Z:/wikidesk_cache/Www.wiktionary.org screenshot.png"" width=""256""></a>

|-
! <a href=""wiki://Uniform_Resource_Locator"" title=""Uniform Resource Locator"">URL</a>
| <a href=""http://www.wiktionary.org/"" class=""external text"" rel=""nofollow"">http://www.wiktionary.org/</a>
|-
! Type of site
| Online dictionary<tr><th>Need an account?</th><td>Not needed, though can be used</td></tr>
|- style=""vertical-align: top;""
! Owned by
| <a href=""wiki://Wikimedia_Foundation"" title=""Wikimedia Foundation"">Wikimedia Foundation</a>
|-
! Created by
| <a href=""wiki://Jimmy_Wales"" title=""Jimmy Wales"">Jimmy Wales</a>
|}
",
@"
");
        }
    }
}

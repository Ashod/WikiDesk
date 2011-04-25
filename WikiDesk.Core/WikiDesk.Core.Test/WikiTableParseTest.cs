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
<tbody>
<tr>
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
<tbody>
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
        public void TableBordered()
        {
            WikiParseTest.TestConvert(
"{| border=\"1\"\r\n" +
@"|-
|format modifier (not displayed) |These all  |(including the pipes) |go into  |the first cell
|}
",
"<table border=\"1\">\r\n" +
@"<tbody>
<tr>
<td>These all  |(including the pipes) |go into  |the first cell</td>
</tr>
</tbody></table>
");
        }

        [Test]
        public void TableModifier()
        {
            WikiParseTest.TestConvert(
"{| border=\"1\"\r\n" +
@"|-
| Cell 1 (no modifier — not aligned)
|-
" +
"| align=\"right\" | Cell 2 (right aligned)\r\n" + 
"|}\r\n",
"<table border=\"1\">\r\n" +
@"<tbody>
<tr>
<td>Cell 1 (no modifier — not aligned)</td>
</tr>
<tr>
" +
"<td align=\"right\">Cell 2 (right aligned)</td>\r\n" +
@"</tr>
</tbody></table>
");
        }

    }
}

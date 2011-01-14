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
                "{| \r\n| A \r\n| B\r\n|- \r\n| C\r\n| D\r\n|}",
                "<table>\r\n<tbody><tr>\r\n<td>A</td>\r\n<td>B</td>\r\n</tr>\r\n<tr>\r\n<td>C</td>\r\n<td>D</td>\r\n</tr>\r\n</tbody></table>");
        }
    }
}

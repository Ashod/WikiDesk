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
                "<table><tbody><tr><td>A</td><td>B</td></tr><tr><td>C</td><td>D</td></tr></tbody></table>");
        }
    }
}

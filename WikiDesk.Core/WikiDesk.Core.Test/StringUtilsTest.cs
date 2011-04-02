namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class StringUtilsTest
    {
        [Test]
        public void RemoveComents()
        {
            const string RAW = "blik <!--comment--> more text <!----> bzz";
            const string EXP = "blik  more text  bzz";
            Assert.AreEqual(EXP, StringUtils.RemoveBlocks(RAW, "<!--", "-->"));
        }

        [Test]
        public void RemoveComentsEnd()
        {
            const string RAW = "blik more text bzz <!--comment-->";
            const string EXP = "blik more text bzz ";
            Assert.AreEqual(EXP, StringUtils.RemoveBlocks(RAW, "<!--", "-->"));
        }

        [Test]
        public void RemoveComentsStart()
        {
            const string RAW = "<!--comment-->blik more text bzz";
            const string EXP = "blik more text bzz";
            Assert.AreEqual(EXP, StringUtils.RemoveBlocks(RAW, "<!--", "-->"));
        }
    }
}

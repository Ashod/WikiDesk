namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class StringUtilsTest
    {
        #region RemoveBlocks

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

        [Test]
        public void RemoveComentsNoEnd()
        {
            const string RAW = "blik more text bzz<!--comment";
            const string EXP = "blik more text bzz";
            Assert.AreEqual(EXP, StringUtils.RemoveBlocks(RAW, "<!--", "-->"));
        }

        #endregion // RemoveBlocks

        #region ExtractBlock

        [Test]
        public void ExtractBlockSimple()
        {
            const string RAW = "blik <!--comment--> more text <!----> bzz";
            const string EXP = "comment";
            Assert.AreEqual(EXP, StringUtils.ExtractBlock(RAW, "<!--", "-->"));
        }

        [Test]
        public void ExtractBlockOffset()
        {
            const string RAW = "blik <!--comment--> more text <!----> bzz";

            int offset = 20;
            Assert.AreEqual(string.Empty, StringUtils.ExtractBlock(RAW, "<!--", "-->", ref offset));
            Assert.AreEqual(36, offset);
        }

        [Test]
        public void ExtractBlockStartEnd()
        {
            const string RAW = "blik <!--comment--> more text <!----> bzz";

            int startOffset = 20;
            int endOffset;
            Assert.AreEqual(string.Empty, StringUtils.ExtractBlock(RAW, "<!--", "-->", ref startOffset, out endOffset));
            Assert.AreEqual(30, startOffset);
            Assert.AreEqual(36, endOffset);
        }

        #endregion // ExtractBlock

        #region ExtractTagName

        [Test]
        public void ExtractTagNameB()
        {
            const string RAW = "b";
            const string EXP = "b";
            bool closing;
            string attribs;
            Assert.AreEqual(EXP, StringUtils.ExtractTagName(RAW, out attribs, out closing));
            Assert.False(closing);
            Assert.IsEmpty(attribs);
        }

        [Test]
        public void ExtractTagNameBr()
        {
            const string RAW = "br/";
            const string EXP = "br";
            bool closing;
            string attribs;
            Assert.AreEqual(EXP, StringUtils.ExtractTagName(RAW, out attribs, out closing));
            Assert.True(closing);
            Assert.IsEmpty(attribs);
        }

        [Test]
        public void ExtractTagNameBrSpaced()
        {
            const string RAW = "br /";
            const string EXP = "br";
            bool closing;
            string attribs;
            Assert.AreEqual(EXP, StringUtils.ExtractTagName(RAW, out attribs, out closing));
            Assert.True(closing);
            Assert.IsEmpty(attribs);
        }

        [Test]
        public void ExtractTagNameAttribs()
        {
            const string RAW = "tag name=\"kikos\" ";
            const string EXP = "tag";
            bool closing;
            string attribs;
            Assert.AreEqual(EXP, StringUtils.ExtractTagName(RAW, out attribs, out closing));
            Assert.False(closing);
            Assert.AreEqual("name=\"kikos\"", attribs);
        }

        [Test]
        public void ExtractTagNameEmpty()
        {
            bool closing;
            string attribs;
            Assert.AreEqual(string.Empty, StringUtils.ExtractTagName(string.Empty, out attribs, out closing));
            Assert.False(closing);
            Assert.IsNull(attribs);
        }

        [Test]
        public void ExtractTagNameNull()
        {
            bool closing;
            string attribs;
            Assert.AreEqual(string.Empty, StringUtils.ExtractTagName(null, out attribs, out closing));
            Assert.False(closing);
            Assert.IsNull(attribs);
        }

        [Test]
        public void ExtractTagNameClose()
        {
            const string RAW = "/b";
            const string EXP = "b";
            bool closing;
            string attribs;
            Assert.AreEqual(EXP, StringUtils.ExtractTagName(RAW, out attribs, out closing));
            Assert.True(closing);
            Assert.IsNull(attribs);
        }

        #endregion // ExtractTagName

        #region ExtractTag

        [Test]
        public void ExtractTagSelfClosing()
        {
            const string RAW = "blik more text <br />";
            int offset = 0;
            string tag = null;
            string attribs = null;
            Assert.AreEqual(null, StringUtils.ExtractTag(RAW, ref offset, ref tag, ref attribs));

            Assert.AreEqual("br", tag);
            Assert.IsEmpty(attribs);
            Assert.AreEqual(RAW.Length - 1, offset);
        }

        [Test]
        public void ExtractTagB()
        {
            const string RAW = "blik more text <b>bzz</b>";
            const string EXP = "bzz";
            int offset = 0;
            string tag = null;
            string attribs = null;
            Assert.AreEqual(EXP, StringUtils.ExtractTag(RAW, ref offset, ref tag, ref attribs));

            Assert.AreEqual("b", tag);
            Assert.IsEmpty(attribs);
            Assert.AreEqual(RAW.Length - 1, offset);
        }

        [Test]
        [Ignore]
        public void ExtractTag()
        {
            const string RAW = "blik more text <b>bzz</b>";
            const string EXP = "bzz";
            int offset = 0;
            string tag = null;
            string attribs = null;
            Assert.AreEqual(EXP, StringUtils.ExtractTag(RAW, ref offset, ref tag, ref attribs));
        }

        #endregion // ExtractTag
    }
}

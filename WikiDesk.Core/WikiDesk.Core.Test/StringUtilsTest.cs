namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class StringUtilsTest
    {
        #region BreakAt

        [Test]
        public void BreakAt()
        {
            const string RAW = "something something+else other";
            const string EXP_RET = "something something";
            const string EXP_SEC = "else other";

            string second;
            Assert.AreEqual(EXP_RET, StringUtils.BreakAt(RAW, '+', out second));
            Assert.AreEqual(EXP_SEC, second);
        }

        [Test]
        public void BreakAtMulti()
        {
            const string RAW = "something something+else other+more";
            const string EXP_RET = "something something";
            const string EXP_SEC = "else other+more";

            string second;
            Assert.AreEqual(EXP_RET, StringUtils.BreakAt(RAW, '+', out second));
            Assert.AreEqual(EXP_SEC, second);
        }

        [Test]
        public void BreakAtEmptyFirst()
        {
            const string RAW = "+else other+more";
            const string EXP_SEC = "else other+more";

            string second;
            Assert.IsEmpty(StringUtils.BreakAt(RAW, '+', out second));
            Assert.AreEqual(EXP_SEC, second);
        }

        [Test]
        public void BreakAtEmptySecond()
        {
            const string RAW = "something something+";
            const string EXP_RET = "something something";

            string second;
            Assert.AreEqual(EXP_RET, StringUtils.BreakAt(RAW, '+', out second));
            Assert.IsEmpty(second);
        }

        [Test]
        public void BreakAtNoBreak()
        {
            const string RAW = "something something";
            const string EXP_RET = "something something";

            string second;
            Assert.AreEqual(EXP_RET, StringUtils.BreakAt(RAW, '+', out second));
            Assert.IsNull(second);
        }

        [Test]
        public void BreakAtEmpty()
        {
            string second;
            Assert.IsEmpty(StringUtils.BreakAt(string.Empty, '+', out second));
            Assert.IsNull(second);
        }

        [Test]
        public void BreakAtNull()
        {
            string second;
            Assert.IsNull(StringUtils.BreakAt(null, '+', out second));
            Assert.IsNull(second);
        }

        #endregion // BreakAt

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
            Assert.IsEmpty(StringUtils.ExtractBlock(RAW, "<!--", "-->", ref offset));
            Assert.AreEqual(36, offset);
        }

        [Test]
        public void ExtractBlockStartEnd()
        {
            const string RAW = "blik <!--comment--> more text <!----> bzz";

            int startOffset = 20;
            int endOffset;
            Assert.IsEmpty(StringUtils.ExtractBlock(RAW, "<!--", "-->", ref startOffset, out endOffset));
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
            Assert.IsEmpty(StringUtils.ExtractTagName(string.Empty, out attribs, out closing));
            Assert.False(closing);
            Assert.IsNull(attribs);
        }

        [Test]
        public void ExtractTagNameNull()
        {
            bool closing;
            string attribs;
            Assert.IsEmpty(StringUtils.ExtractTagName(null, out attribs, out closing));
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
            int startOffset = 0;
            int endOffset;
            string tag = null;
            string attribs;
            Assert.AreEqual(null, StringUtils.ExtractTag(RAW, ref startOffset, out endOffset, ref tag, out attribs));

            Assert.AreEqual("br", tag);
            Assert.IsEmpty(attribs);
            Assert.AreEqual(RAW.Length - 6, startOffset);
            Assert.AreEqual(RAW.Length - 1, endOffset);
        }

        [Test]
        public void ExtractTagB()
        {
            const string RAW = "blik more text <b>bzz</b>";
            const string EXP = "bzz";
            int startOffset = 0;
            int endOffset;
            string tag = null;
            string attribs;
            Assert.AreEqual(EXP, StringUtils.ExtractTag(RAW, ref startOffset, out endOffset, ref tag, out attribs));

            Assert.AreEqual("b", tag);
            Assert.IsEmpty(attribs);
            Assert.AreEqual(RAW.Length - 10, startOffset);
            Assert.AreEqual(RAW.Length - 1, endOffset);
        }

        [Test]
        public void ExtractTagSpecific()
        {
            const string RAW = "blik <i>more<i> text <b>bzz</b>";
            const string EXP = "bzz";
            int startOffset = 0;
            int endOffset;
            string tag = "B";
            string attribs;
            Assert.AreEqual(EXP, StringUtils.ExtractTag(RAW, ref startOffset, out endOffset, ref tag, out attribs));

            Assert.That(string.Compare("b", tag, true) == 0);
            Assert.IsEmpty(attribs);
            Assert.AreEqual(RAW.Length - 10, startOffset);
            Assert.AreEqual(RAW.Length - 1, endOffset);
        }

        #endregion // ExtractTag

        #region FindTag

        [Test]
        public void FindTagOpen()
        {
            const string RAW = "blik <i>more<i> text <b>bzz</b>";
            int endOffset;
            bool closing;
            string attribs;
            Assert.AreEqual(RAW.Length - 10, StringUtils.FindTag(RAW, 0, out endOffset, "B", out closing, out attribs));

            Assert.IsEmpty(attribs);
            Assert.False(closing);
            Assert.AreEqual(RAW.Length - 8, endOffset);
        }

        [Test]
        public void FindTagClose()
        {
            const string RAW = "blik <i>more<i> text <b>bzz</b>";
            int endOffset;
            bool closing;
            string attribs;
            Assert.AreEqual(RAW.Length - 4, StringUtils.FindTag(RAW, 22, out endOffset, "B", out closing, out attribs));

            Assert.IsNull(attribs);
            Assert.True(closing);
            Assert.AreEqual(RAW.Length - 1, endOffset);
        }

        #endregion // FindTag
    }
}

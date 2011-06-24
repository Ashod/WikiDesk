// -----------------------------------------------------------------------------------------
// <copyright file="StringUtilsTest.cs" company="ashodnakashian.com">
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
//   Defines the StringUtilsTest type.
// </summary>
// -----------------------------------------------------------------------------------------

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

        [Test]
        public void RemoveTags()
        {
            const string RAW = "txt <b>bold</b> and some <i>italic<b>nested</b></i>";
            const string EXP = "txt bold and some italicnested";
            Assert.AreEqual(EXP, StringUtils.RemoveBlocks(RAW, "<", ">"));
        }


        #endregion // RemoveBlocks

        #region CountRepetition

        [Test]
        public void CountRepetition()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            Assert.AreEqual(2, StringUtils.CountRepetition(RAW, 5));
        }

        [Test]
        public void CountRepetitionNoRepeat()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            Assert.AreEqual(1, StringUtils.CountRepetition(RAW, 3));
        }

        [Test]
        public void CountRepetitionRepeatLast()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            Assert.AreEqual(2, StringUtils.CountRepetition(RAW, RAW.Length - 2));
        }

        [Test]
        public void CountReverseRepetitionRepeatLast()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            Assert.AreEqual(2, StringUtils.CountReverseRepetition(RAW, RAW.Length - 1));
        }

        [Test]
        public void CountReverseRepetitionFirst()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            Assert.AreEqual(1, StringUtils.CountReverseRepetition(RAW, 0));
        }

        [Test]
        public void CountReverseRepetition()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            Assert.AreEqual(2, StringUtils.CountReverseRepetition(RAW, 6));
        }

        #endregion // CountRepetition

        #region CollapseReplace

        [Test]
        public void CollapseReplace()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";
            const string EXPECTED = "blik_''comment''_more_text_<!---->_bzz";

            Assert.AreEqual(EXPECTED, StringUtils.CollapseReplace(RAW, ' ', '_'));
        }

        [Test]
        public void CollapseReplaceRepeats()
        {
            const string RAW = "only in    Print";
            const string EXPECTED = "only_in_Print";

            Assert.AreEqual(EXPECTED, StringUtils.CollapseReplace(RAW, ' ', '_'));
        }

        #endregion // CountRepetition

        #region FindWrappedBlock

        [Test]
        public void FindWrappedBlockSimple()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            int end;
            Assert.AreEqual(5, StringUtils.FindWrappedBlock(RAW, 0, out end, '\'', 2, true));
            Assert.AreEqual(15, end);
        }

        [Test]
        public void FindWrappedBlockNoMatch()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            int end;
            Assert.AreEqual(-1, StringUtils.FindWrappedBlock(RAW, 6, out end, '\'', 2, true));
            Assert.AreEqual(-1, end);
        }

        [Test]
        public void FindWrappedBlockGreedy()
        {
            const string RAW = "blik ''comment'' more text <!----> bzz";

            int end;
            Assert.AreEqual(5, StringUtils.FindWrappedBlock(RAW, 3, out end, '\'', 0, true));
            Assert.AreEqual(15, end);
        }

        [Test]
        public void FindWrappedBlockGreedyNonSymmetric()
        {
            const string RAW = "blik '''comment'' more' text <!----> bzz";

            int end;
            Assert.AreEqual(5, StringUtils.FindWrappedBlock(RAW, 3, out end, '\'', 0, false));
            Assert.AreEqual(16, end);
        }

        #endregion // FindWrappedBlock

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

        [Test]
        [Ignore]
        public void ExtractBlockNested()
        {
            const string RAW = "blik <b>something <b>b</b> else</b>";

            int startOffset = 2;
            int endOffset;
            Assert.AreEqual("something <b>b</b> else", StringUtils.ExtractBlock(RAW, "<b>", "</b>", ref startOffset, out endOffset));
            Assert.AreEqual(5, startOffset);
            Assert.AreEqual(RAW.Length - 1, endOffset);
        }

        [Test]
        public void ExtractBlockNestedChar()
        {
            const string RAW = "[[File:somefile.jpg|caption of [[wikilink]]]]";

            int startOffset = 0;
            int endOffset;
            Assert.AreEqual(
                    "[File:somefile.jpg|caption of [[wikilink]]]",
                    StringUtils.ExtractBlock(RAW, '[', ']', ref startOffset, out endOffset));
            Assert.AreEqual(0, startOffset);
            Assert.AreEqual(RAW.Length - 1, endOffset);
        }

        [Test]
        public void ExtractBlockNestedChar2()
        {
            const string RAW = "something [[File:somefile.jpg|caption of [[wikilink]]]] something [link]";

            int startOffset = 0;
            int endOffset;
            Assert.AreEqual(
                    "[File:somefile.jpg|caption of [[wikilink]]]",
                    StringUtils.ExtractBlock(RAW, '[', ']', ref startOffset, out endOffset));
            Assert.AreEqual(10, startOffset);
            Assert.AreEqual(54, endOffset);
        }


        [Test]
        [Ignore]
        public void ExtractBlockNestedBracket()
        {
            const string RAW = "[[File:somefile.jpg|caption of [[wikilink]]]]";

            int startOffset = 0;
            int endOffset;
            Assert.AreEqual(
                "File:somefile.jpg|caption of [[wikilink]]",
                StringUtils.ExtractBlock(RAW, "[[", "]]", ref startOffset, out endOffset));
            Assert.AreEqual(0, startOffset);
            Assert.AreEqual(RAW.Length - 1, endOffset);
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
        public void ExtractTagBold()
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
        [Ignore]
        public void ExtractTagBoldItalic()
        {
            const string RAW = "blik more text <b><i>bzz</i></b>";
            const string EXP = "<i>bzz</i>";
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

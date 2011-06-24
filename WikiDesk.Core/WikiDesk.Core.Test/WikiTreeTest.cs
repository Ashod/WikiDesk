// -----------------------------------------------------------------------------------------
// <copyright file="WikiTreeTest.cs" company="ashodnakashian.com">
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
//   Defines the WikiTreeTest type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class WikiTreeTest
    {
        [Test]
        public void TitleNormalize()
        {
            const string WIKICODE = "<b>bold text</b><i>italic text</i>";
            WikiTree tree = new WikiTree(WIKICODE);
            Assert.AreEqual(WIKICODE, tree.Text);
            Assert.IsTrue(tree.IsWiki);
            Assert.IsNull(tree.Parent);
            Assert.IsNull(tree.Children);

            int index = tree.Text.IndexOf("<i>");
            tree.Split(index);
            Assert.IsNull(tree.Text);
            Assert.AreEqual(WIKICODE, tree.GetChildrenText());
            Assert.IsNotNull(tree.Children);
            Assert.AreEqual(2, tree.Children.Count);
            Assert.AreEqual("<b>bold text</b>", tree.Children[0].Text);
            Assert.AreEqual("<i>italic text</i>", tree.Children[1].Text);

            Assert.AreEqual(tree, tree.Children[0].Parent);
            Assert.AreEqual(tree, tree.Children[1].Parent);
        }
    }
}

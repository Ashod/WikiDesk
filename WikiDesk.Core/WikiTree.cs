// -----------------------------------------------------------------------------------------
// <copyright file="WikiTree.cs" company="ashodnakashian.com">
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
//   Defines the WikiNode type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System.Collections.Generic;
    using System.Text;

    public class WikiNode
    {
        public WikiNode(WikiNode parent, string text, bool isWiki)
        {
            Parent = parent;
            Text = text;
            IsWiki = isWiki;
        }

        #region properties

        public bool IsWiki { get; set; }

        public string Text { get; set; }

        public WikiNode Parent { get; set; }

        public IList<WikiNode> Children
        {
            get { return children_; }
        }

        #endregion // properties

        public void Split(int index)
        {
            children_ = new List<WikiNode>(2)
                {
                    new WikiNode(this, Text.Substring(0, index), IsWiki),
                    new WikiNode(this, Text.Substring(index), IsWiki)
                };

            Text = null;
        }

        public string GetChildrenText()
        {
            if (children_ == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(children_.Count * 64);
            foreach (WikiNode wikiNode in children_)
            {
                sb.Append(wikiNode.Text);
            }

            return sb.ToString();
        }

        #region representation

        private List<WikiNode> children_;

        #endregion // representation
    }

    public class WikiTree : WikiNode
    {
        public WikiTree(string wikiCode)
            : base(null, wikiCode, true)
        {
        }
    }
}

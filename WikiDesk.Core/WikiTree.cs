
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

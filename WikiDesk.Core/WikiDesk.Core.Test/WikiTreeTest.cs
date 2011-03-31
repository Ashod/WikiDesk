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

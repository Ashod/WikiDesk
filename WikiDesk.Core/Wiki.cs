
namespace WikiDesk.Core
{
    using ScrewTurn.Wiki;
    using ScrewTurn.Wiki.PluginFramework;

    public class Wiki
    {
        public static string Wiki2Html(string wikiText)
        {
            FormattingContext context = FormattingContext.PageContent;
            PageInfo currentPage = null;
            string[] linkedPages = null;
            string output = Formatter.Format(wikiText, false, context, currentPage, out linkedPages, false);
            return output;
        }
    }
}

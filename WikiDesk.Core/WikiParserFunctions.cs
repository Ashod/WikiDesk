
namespace WikiDesk.Core
{
    using System.Collections.Generic;

    public class ParserFunctions
    {
        public ParserFunctions()
        {
            RegisterHandlers();
        }

        public ParserFunctionResult Execute(string functionName, string input, out string output)
        {
            ParserFunction func = FindHandler(functionName);
            if (func != null)
            {
                return func(input, out output);
            }

            output = null;
            return ParserFunctionResult.Unknown;
        }

        public enum ParserFunctionResult
        {
            /// <summary>
            /// Unknown function.
            /// </summary>
            Unknown,

            /// <summary>
            /// The text returned is valid, stop processing the template.
            /// </summary>
            Found,

            /// <summary>
            /// Wiki markup in the return value should be escaped.
            /// </summary>
            NoWiki,

            /// <summary>
            /// The returned text is HTML, armor it against wikitext transformation.
            /// </summary>
            Html
        }

        public delegate ParserFunctionResult ParserFunction(string input, out string output);

        public void RegisterHandler(string name, ParserFunction func)
        {
            functionsMap_[name] = func;
        }

        private ParserFunction FindHandler(string name)
        {
            ParserFunction func;
            if (functionsMap_.TryGetValue(name, out func))
            {
                return func;
            }

            return null;
        }

        #region implementation

        private void RegisterHandlers()
        {
            RegisterHandler("int", DoNothing);
            RegisterHandler("ns",                DoNothing);
            RegisterHandler("nse",               DoNothing);
            RegisterHandler("urlencode",         DoNothing);
            RegisterHandler("lcfirst",           DoNothing);
            RegisterHandler("ucfirst",           DoNothing);
            RegisterHandler("lc",                DoNothing);
            RegisterHandler("uc",                DoNothing);
            RegisterHandler("localurl",          DoNothing);
            RegisterHandler("localurle",         DoNothing);
            RegisterHandler("fullurl",           DoNothing);
            RegisterHandler("fullurle",          DoNothing);
            RegisterHandler("formatnum",         DoNothing);
            RegisterHandler("grammar",           DoNothing);
            RegisterHandler("gender",            DoNothing);
            RegisterHandler("plural",            DoNothing);
            RegisterHandler("numberofpages",     DoNothing);
            RegisterHandler("numberofusers",     DoNothing);
            RegisterHandler("numberofactiveusers", DoNothing);
            RegisterHandler("numberofarticles",  DoNothing);
            RegisterHandler("numberoffiles",     DoNothing);
            RegisterHandler("numberofadmins",    DoNothing);
            RegisterHandler("numberingroup",     DoNothing);
            RegisterHandler("numberofedits",     DoNothing);
            RegisterHandler("numberofviews",     DoNothing);
            RegisterHandler("language",          DoNothing);
            RegisterHandler("padleft",           DoNothing);
            RegisterHandler("padright",          DoNothing);
            RegisterHandler("anchorencode",      DoNothing);
            RegisterHandler("#special",           DoNothing);
            RegisterHandler("defaultsort",       DoNothing);
            RegisterHandler("filepath",          DoNothing);
            RegisterHandler("pagesincategory",   DoNothing);
            RegisterHandler("pagesize",          DoNothing);
            RegisterHandler("protectionlevel",   DoNothing);
            RegisterHandler("namespace",         DoNothing);
            RegisterHandler("namespacee",        DoNothing);
            RegisterHandler("talkspace",         DoNothing);
            RegisterHandler("talkspacee",        DoNothing);
            RegisterHandler("subjectspace",      DoNothing);
            RegisterHandler("subjectspacee",     DoNothing);
            RegisterHandler("pagename",          DoNothing);
            RegisterHandler("pagenamee",         DoNothing);
            RegisterHandler("fullpagename",      DoNothing);
            RegisterHandler("fullpagenamee",     DoNothing);
            RegisterHandler("basepagename",      DoNothing);
            RegisterHandler("basepagenamee",     DoNothing);
            RegisterHandler("subpagename",       DoNothing);
            RegisterHandler("subpagenamee",      DoNothing);
            RegisterHandler("talkpagename",      DoNothing);
            RegisterHandler("talkpagenamee",     DoNothing);
            RegisterHandler("subjectpagename",   DoNothing);
            RegisterHandler("subjectpagenamee",  DoNothing);
            RegisterHandler("tag",               DoNothing);
            RegisterHandler("#formatdate",        DoNothing);
        }

        private ParserFunctionResult DoNothing(string input, out string output)
        {
            output = string.Empty;
            return ParserFunctionResult.Html;
        }

        #endregion // implementation

        #region representation

        private readonly Dictionary<string, ParserFunction> functionsMap_ = new Dictionary<string, ParserFunction>(16);

        #endregion // representation
    }
}


namespace WikiDesk.Core
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class MagicWordProcessor
    {
        public MagicWordProcessor()
        {
            RegisterHandlers();
        }

        public Result Execute(string functionName, List<KeyValuePair<string, string>> args, out string output)
        {
            Handler func = FindHandler(functionName);
            if (func != null)
            {
                string input = string.Empty;
                if (args != null)
                {
                    Debug.Assert(args.Count == 1);
                    input = args[0].Value;
                }

                return func(input, out output);
            }

            output = null;
            return Result.Unknown;
        }

        public enum Result
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

        public delegate Result Handler(string input, out string output);

        public void RegisterHandler(string name, Handler func)
        {
            functionsMap_[name] = func;
        }

        private Handler FindHandler(string name)
        {
            Handler func;
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
            RegisterHandler("lcfirst",           LcFirst);
            RegisterHandler("ucfirst",           UcFirst);
            RegisterHandler("lc",                Lc);
            RegisterHandler("uc",                Uc);
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
            RegisterHandler("#special",          DoNothing);
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
            RegisterHandler("#formatdate",       DoNothing);
        }

        private static Result Lc(string input, out string output)
        {
            output = input.ToLowerInvariant();
            return Result.Found;
        }

        private static Result Uc(string input, out string output)
        {
            output = input.ToUpperInvariant();
            return Result.Found;
        }

        private static Result UcFirst(string input, out string output)
        {
            output = input[0].ToString().ToUpperInvariant() + input.Substring(1);
            return Result.Found;
        }

        private static Result LcFirst(string input, out string output)
        {
            output = input[0].ToString().ToLowerInvariant() + input.Substring(1);
            return Result.Found;
        }

        private Result DoNothing(string input, out string output)
        {
            output = string.Empty;
            return Result.Html;
        }

        #endregion // implementation

        #region representation

        private readonly Dictionary<string, Handler> functionsMap_ = new Dictionary<string, Handler>(16);

        #endregion // representation
    }
}

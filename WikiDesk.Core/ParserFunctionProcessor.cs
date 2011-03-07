
namespace WikiDesk.Core
{
    using System.Collections.Generic;

    public class ParserFunctionProcessor
    {
        public ParserFunctionProcessor()
        {
            RegisterHandlers();
        }

        public Result Execute(string functionName, string[] param, out string output)
        {
            Handler func = FindHandler(functionName);
            if (func != null)
            {
                return func(param, out output);
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

        public delegate Result Handler(string[] param, out string output);

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
            RegisterHandler("#expr", DoNothing);

        }

        private Result DoNothing(string[] param, out string output)
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

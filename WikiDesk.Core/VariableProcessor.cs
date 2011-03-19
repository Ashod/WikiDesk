namespace WikiDesk.Core
{
    using System.Collections.Generic;

    public abstract class VariableProcessor
    {
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

        public delegate Result Handler(List<KeyValuePair<string, string>> args, out string output);

        public Result Execute(string functionName, List<KeyValuePair<string, string>> args, out string output)
        {
            Handler func = FindHandler(functionName);
            if (func != null)
            {
                return func(args, out output);
            }

            output = null;
            return Result.Unknown;
        }

        public void RegisterHandler(string name, Handler func)
        {
            functionsMap_[name] = func;
        }

        #region implementation

        protected Result DoNothing(List<KeyValuePair<string, string>> args, out string output)
        {
            output = string.Empty;
            return Result.Found;
        }

        private Handler FindHandler(string name)
        {
            Handler func;
            if (functionsMap_.TryGetValue(name.ToLowerInvariant(), out func))
            {
                return func;
            }

            return null;
        }

        #endregion // implementation

        #region representation

        private readonly Dictionary<string, Handler> functionsMap_ = new Dictionary<string, Handler>(32);

        #endregion // representation
    }
}
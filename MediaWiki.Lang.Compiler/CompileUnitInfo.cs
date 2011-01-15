
namespace MediaWiki.Lang.Compiler
{
    /// <summary>
    /// Represents a single compilation unit.
    /// </summary>
    public class CompileUnitInfo
    {
        public CompileUnitInfo(
                    string filePath,
                    string namespaceName,
                    string globalsClassName
                    )
        {
            Filename = filePath;
            NamespaceName = namespaceName;
            GlobalsClassName = globalsClassName;
        }

        public string Filename { get; private set; }
        public string NamespaceName { get; private set; }
        public string GlobalsClassName { get; private set; }
    }
}

namespace MediaWiki.Lang
{
    using System.Reflection;

    public class MagicWord
    {
        
    }

    public class Messages
    {
        public Messages(string assemblyFilename)
        {
            assembly_ = Assembly.LoadFrom(assemblyFilename);
        }

//         public Dictionary<string, > MagicWords
//         {
//             
//         }

//         Type globalArrayType = assembly.GetType("Test.Space.GlobalArray");
// 
//         FieldInfo wgLanguageNamesField = globalArrayType.GetField("wgLanguageNames");
// 
//         // Instantiate.
//         object instance = Activator.CreateInstance(globalArrayType, ScriptContext.CurrentContext, true);
// 
//         // Read a predetermined field.
//         object value = wgLanguageNamesField.GetValue(instance);
// 
//         PhpArray array = (PhpArray)((PhpReference)value).value;

        #region representation

        private readonly Assembly assembly_;

        #endregion // representation
    }
}

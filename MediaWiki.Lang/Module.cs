
namespace MediaWiki.Lang
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using PHP.Core;

    #endregion // using

    public class Module
    {
        public Module(string moduleFilePath)
        {
            // Introspect.
            Assembly assembly = Assembly.LoadFrom(moduleFilePath);

            // The class name is the filename in all lower-case.
            string className = Path.GetFileNameWithoutExtension(moduleFilePath).ToLowerInvariant();

            type_ = assembly.GetType("MediaWiki.Lang." + className);

            // Instantiate.
            instance_ = Activator.CreateInstance(type_, ScriptContext.CurrentContext, true);
        }

        #region operations

        public Dictionary<string, string> GetString2StringMapField(string name)
        {
            FieldInfo fieldInfo = type_.GetField(name);
            object value = fieldInfo.GetValue(instance_);

            PhpArray array = (PhpArray)((PhpReference)value).value;

            Dictionary<string, string> map = new Dictionary<string, string>(array.Count);

            ICollection<IntStringKey> intStringKeys = array.Keys;
            ICollection<object> collection = array.Values;
            using (IEnumerator<object> enumerator = collection.GetEnumerator())
            {
                foreach (IntStringKey key in intStringKeys)
                {
                    if (enumerator.MoveNext() && enumerator.Current is string)
                    {
                        map[key.String] = enumerator.Current as string;
                    }
                }
            }

            return map;
        }

        public Dictionary<string, string[]> GetString2StringsMapField(string name)
        {
            FieldInfo fieldInfo = type_.GetField(name);
            if (fieldInfo == null)
            {
                return new Dictionary<string, string[]>(0);
            }

            object value = fieldInfo.GetValue(instance_);
            PhpArray array = (PhpArray)((PhpReference)value).value;

            Dictionary<string, string[]> map = new Dictionary<string, string[]>(array.Count);

            ICollection<IntStringKey> intStringKeys = array.Keys;
            ICollection<object> collection = array.Values;
            using (IEnumerator<object> enumerator = collection.GetEnumerator())
            {
                foreach (IntStringKey key in intStringKeys)
                {
                    if (enumerator.MoveNext() && enumerator.Current != null)
                    {
                        PhpArray stringsArray = (PhpArray)enumerator.Current;
                        string[] strings = new string[stringsArray.Count];
                        int index = 0;
                        foreach (object o in stringsArray.Values)
                        {
                            if (o != null)
                            {
                                strings[index++] = o.ToString();
                            }
                        }

                        map[key.String] = strings;
                    }
                }
            }

            return map;
        }

        #endregion // operations

        #region representation

        private readonly Type type_;
        private readonly object instance_;

        #endregion // representation
    }
}

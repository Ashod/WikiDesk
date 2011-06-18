
namespace MediaWiki.Lang
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using PHP.Core;

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
            PhpArray array = GetPhpArrayField(name);
            if (array == null)
            {
                return new Dictionary<string, string>(0);
            }

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

        public object GetField(string name)
        {
            FieldInfo fieldInfo = type_.GetField(name);
            if (fieldInfo == null)
            {
                return null;
            }

            PhpReference value = fieldInfo.GetValue(instance_) as PhpReference;
            return value == null ? null : value.value;
        }

        public string GetStringField(string name)
        {
            string value = GetField(name) as string;
            return value;
        }

        public Dictionary<string, string[]> GetString2StringsMapField(string name)
        {
            PhpArray array = GetPhpArrayField(name);
            if (array == null)
            {
                return new Dictionary<string, string[]>(0);
            }

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

        #region implementation

        private PhpArray GetPhpArrayField(string name)
        {
            PhpArray array = GetField(name) as PhpArray;
            return array;
        }

        #endregion // implementation

        #region representation

        private readonly Type type_;
        private readonly object instance_;

        #endregion // representation
    }
}

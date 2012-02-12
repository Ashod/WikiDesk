// -----------------------------------------------------------------------------------------
// <copyright file="Module.cs" company="ashodnakashian.com">
//
// This file is part of WikiDesk.
// Copyright (C) 2010, 2011 Ashod Nakashian
// https://github.com/Ashod/WikiDesk
//
//  WikiDesk is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  WikiDesk is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with WikiDesk. If not, see http://www.gnu.org/licenses/.
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Wiki module manager.
// </summary>
// -----------------------------------------------------------------------------------------

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

            string typeName = "MediaWiki.Lang." + className;
            type_ = assembly.GetType(typeName);
            if (type_ == null)
            {
                string msg = string.Format("Failed to load type [{0}] from class [{1}] in assembly [{2}].", typeName, className, moduleFilePath);
                throw new TypeLoadException(msg);
            }

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

// -----------------------------------------------------------------------------------------
// <copyright file="VariableProcessor.cs" company="ashodnakashian.com">
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
//   Defines the VariableProcessor type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System.Collections.Generic;

    public abstract class VariableProcessor
    {
        #region Nested Types

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

        /// <summary>
        /// Delegate to process nested magic words.
        /// </summary>
        /// <param name="wikicode">The code to process.</param>
        /// <returns>Processed result or the original if no magic is found.</returns>
        public delegate string ProcessMagicWords(string wikicode);

        #endregion // Nested Types

        #region construction

        protected VariableProcessor(ProcessMagicWords processMagicWordsDel)
        {
            processMagicWordsDel_ = processMagicWordsDel;
        }

        #endregion // construction

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

        protected string ProcessMagicWord(string wikiCode)
        {
            return processMagicWordsDel_ != null ? processMagicWordsDel_(wikiCode) : wikiCode;
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
        private readonly ProcessMagicWords processMagicWordsDel_;

        #endregion // representation
    }
}

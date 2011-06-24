// -----------------------------------------------------------------------------------------
// <copyright file="WikiMagicWords.cs" company="ashodnakashian.com">
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
//   Maps magic words and their aliases, possibly in different languages.
//   Each magic word has a canonical, English and typically lower-case versions.
//   These canonical versions are the IDs and the aliases are the words.
//   This class helps us lookup a magic word alias into its canonical version (ID).
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Maps magic words and their aliases, possibly in different languages.
    /// Each magic word has a canonical, English and typically lower-case versions.
    /// These canonical versions are the IDs and the aliases are the words.
    /// This class helps us lookup a magic word alias into its canonical version (ID).
    /// </summary>
    public class WikiMagicWords
    {
        /// <summary>
        /// Finds a magic word ID, if registered.
        /// </summary>
        /// <param name="word">A magic word alias in any language.</param>
        /// <returns>The magic word ID, if registered, otherwise null.</returns>
        public string FindId(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return null;
            }

            string id;

            //TODO: Should we really trim the colon at the end?

            // Assume case-sensitive.
            if (caseSensitiveWordsMap_.TryGetValue(word, out id) ||
                caseSensitiveWordsMap_.TryGetValue(word.TrimEnd(':'), out id))
            {
                return id;
            }

            // Try case-insensitive.
            word = word.ToUpperInvariant();
            if (caseInsensitiveWordsMap_.TryGetValue(word, out id) ||
                caseInsensitiveWordsMap_.TryGetValue(word.TrimEnd(':'), out id))
            {
                return id;
            }

            // No luck.
            return null;
        }

        /// <summary>
        /// Registers a magic word for a given ID.
        /// </summary>
        /// <param name="id">The ID of the magic word.</param>
        /// <param name="word">The magic word to register.</param>
        /// <param name="caseSensitive">Whether the word is case-sensitive or not.</param>
        public void RegisterWord(string id, string word, bool caseSensitive)
        {
            if (caseSensitive)
            {
                caseSensitiveWordsMap_[word] = id;
            }
            else
            {
                //TODO: Should this be converted by a language-specific converter?
                caseInsensitiveWordsMap_[word.ToUpperInvariant()] = id;
            }
        }

        #region representation

        private readonly Dictionary<string, string> caseSensitiveWordsMap_ = new Dictionary<string, string>(16);
        private readonly Dictionary<string, string> caseInsensitiveWordsMap_ = new Dictionary<string, string>(16);

        #endregion // representation
    }
}

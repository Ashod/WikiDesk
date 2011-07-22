// -----------------------------------------------------------------------------------------
// <copyright file="Placeholder.cs" company="ashodnakashian.com">
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
//   Defines the Placeholder type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Given some text, returns a unique identifier that can be used to
    /// retrieve the original text again.
    /// Used to replace nowiki and similar blocks with the identifier
    /// until a later time when the original is replaced back.
    /// </summary>
    internal class Placeholder
    {
        #region operations

        /// <summary>
        /// Adds a text and gets a unique placeholder ID in return.
        /// </summary>
        /// <param name="text">The text to hold.</param>
        /// <returns>The unique ID of the text.</returns>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        public string Add(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            string id = GenerateNewId();
            repo_.Add(text, id);
            return id;
        }

        /// <summary>
        /// Retrieve the original text associated with the given ID.
        /// </summary>
        /// <param name="id">The ID of the text.</param>
        /// <returns>The text, if found, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        public string Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            string text;
            if (repo_.TryGetValue(id, out text))
            {
                return text;
            }

            return null;
        }

        #endregion // operations

        #region implementation

        /// <summary>
        /// Generates a unique ID.
        /// </summary>
        /// <returns>A unique ID.</returns>
        private string GenerateNewId()
        {
            return string.Format("$${0}$$", Interlocked.Increment(ref uniqueValue_));
        }

        #endregion // implementation

        #region representation

        private readonly IDictionary<string, string> repo_ = new Dictionary<string, string>(256);

        private int uniqueValue_ = 100;

        #endregion // representation
    }
}

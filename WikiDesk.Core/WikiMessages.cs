// -----------------------------------------------------------------------------------------
// <copyright file="WikiMessages.cs" company="ashodnakashian.com">
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
//   Maps a message name to a specific message.
//   The IDs are short names for the messages.
//   Typically a default message is overridden by a language-specific version.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Maps a message name to a specific message.
    /// The IDs are short names for the messages.
    /// Typically a default message is overridden by a language-specific version.
    /// </summary>
    public class WikiMessages
    {
        /// <summary>
        /// Finds a message given its name, if registered.
        /// </summary>
        /// <param name="name">A message name to find.</param>
        /// <returns>The message, if found, otherwise null.</returns>
        public string FindMessage(string name)
        {
            string message;

            if (messagesMap_.TryGetValue(name, out message))
            {
                return message;
            }

            // No luck.
            return null;
        }

        /// <summary>
        /// Registers a message with a unique name.
        /// Newer names override older versions.
        /// </summary>
        /// <param name="name">The message name.</param>
        /// <param name="message">The message text.</param>
        public void Register(string name, string message)
        {
            messagesMap_[name] = message;
        }

        #region representation

        private readonly Dictionary<string, string> messagesMap_ = new Dictionary<string, string>(64);

        #endregion // representation
    }
}

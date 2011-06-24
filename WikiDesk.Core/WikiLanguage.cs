// -----------------------------------------------------------------------------------------
// <copyright file="WikiLanguage.cs" company="ashodnakashian.com">
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
//   Defines the WikiLanguage type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WikiLanguage : IComparer<WikiLanguage>, IComparable<WikiLanguage>
    {
        #region construction

        public WikiLanguage(string name, string code)
        {
            Name = name;
            Code = code;
            LocalName = name;
            RightToLeft = false;
            Disabled = false;
        }

        private WikiLanguage()
        {
        }

        #endregion // construction

        public string Code { get; set; }

        /// <summary>
        /// The language family ISO 639-1 code and sub-tag.
        /// Used when Code is not a language code (such as 'Simple).
        /// </summary>
        public string MimeCode { get; set; }

        public string Name { get; set; }

        public string LocalName { get; set; }

        public string Notes { get; set; }

        public bool RightToLeft { get; set; }

        public bool Disabled { get; set; }

        #region Implementation of IComparer<WikiLanguage>

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Less than zero if <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero if <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public int Compare(WikiLanguage x, WikiLanguage y)
        {
            return x.CompareTo(y);
        }

        #endregion

        #region Implementation of IComparable<WikiLanguage>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Less than zero if this object is less than the <paramref name="other"/> parameter.
        /// Zero if this object is equal to <paramref name="other"/>.
        /// Greater than zero if this object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(WikiLanguage other)
        {
            int val = string.Compare(LocalName, other.LocalName);
            if (val != 0)
            {
                return val;
            }

            val = string.Compare(Name, other.Name);
            if (val != 0)
            {
                return val;
            }

            val = string.Compare(Code, other.Code);
            if (val != 0)
            {
                return val;
            }

            val = string.Compare(Notes, other.Notes);
            return val;
        }

        #endregion // Implementation of IComparable<WikiLanguage>
    }
}

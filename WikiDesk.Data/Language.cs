// -----------------------------------------------------------------------------------------
// <copyright file="Language.cs" company="ashodnakashian.com">
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
//   Defines the Language type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SQLite;

    public class Language : IRecord, IComparer<Language>, IComparable<Language>, IEquatable<Language>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Unique]
        [MaxLength(16)]
        public string Code { get; set; }

        [Unique]
        [MaxLength(64)]
        public string Name { get; set; }

        #region Implementation of IComparer<Language>

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
        public int Compare(Language x, Language y)
        {
            return x.CompareTo(y);
        }

        #endregion // Implementation of IComparer<Language>

        #region Implementation of IComparable<Language>

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
        public int CompareTo(Language other)
        {
            int val = Code.CompareTo(other.Code);
            if (val != 0)
            {
                return val;
            }

            val = Name.CompareTo(other.Name);
            return val;
        }

        #endregion // Implementation of IComparable<Language>

        #region Equality

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(Language))
            {
                return false;
            }

            return Equals((Language)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(Language other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.Code, Code) &&
                   Equals(other.Name, Name);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = Id;
                result = (result * 397) ^ (Code != null ? Code.GetHashCode() : 0);
                result = (result * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(Language left, Language right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Language left, Language right)
        {
            return !Equals(left, right);
        }

        #endregion // Equality
    }

    public partial class Database
    {
        /// <summary>
        /// Selects all available languages from the DB.
        /// </summary>
        /// <returns>A list of all languages.</returns>
        public IList<Language> GetLanguages()
        {
            return (from l in Table<Language>() select l).ToList();
        }

        /// <summary>
        /// Given a language code, selects the relevant record from the DB.
        /// </summary>
        /// <param name="languageCode">The language code to select.</param>
        /// <returns>A language record if one is found, otherwise null.</returns>
        public Language GetLanguageByCode(string languageCode)
        {
            return (from l in Table<Language>()
                    where l.Code == languageCode
                    select l).FirstOrDefault();
        }

        public Language GetLanguageByName(string languageName)
        {
            return (from l in Table<Language>()
                    where l.Name == languageName
                    select l).FirstOrDefault();
        }
    }
}

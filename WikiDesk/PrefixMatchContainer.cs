// -----------------------------------------------------------------------------------------
// <copyright file="PrefixMatchContainer.cs" company="ashodnakashian.com">
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
//   Matches entries with partial, prefix matching.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// Matches entries with partial, prefix matching.
    /// </summary>
    public class PrefixMatchContainer<TValue> where TValue : class
    {
        public event CollectionChangeEventHandler OnCollectionChanged;

        public PrefixMatchContainer()
        {
            strings_ = new AutoCompleteStringCollection();
            strings_.CollectionChanged += strings__CollectionChanged;
        }

        public int Count
        {
            get { return strings_.Count; }
        }

        public AutoCompleteStringCollection AutoCompleteStringCollection
        {
            get { return strings_; }
        }

        public string this[int index]
        {
            get { return strings_[index]; }
        }

        public void Add(string key, TValue value)
        {
            // Find where this key should go.
            int index = FindIndex(key);

            if (index < Count && string.Compare(strings_[index], key, false) == 0)
            {
                // Already exists.
                return;
            }

            // Insert at the designated location.
            strings_.Insert(index, key);
        }

        public int Find(string key, bool ignoreCase, bool exact)
        {
            if (!ignoreCase && exact)
            {
                return strings_.IndexOf(key);
            }

            if (exact)
            {
                for (int i = 0; i < strings_.Count; ++i)
                {
                    string s = strings_[i];
                    if (String.Compare(key, s, true, CultureInfo.CurrentCulture) == 0)
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < strings_.Count; ++i)
                {
                    string s = strings_[i];
                    if (String.Compare(key, 0, s, 0, key.Length, ignoreCase, CultureInfo.CurrentCulture) == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        #region implementation

        private int FindIndex(string key)
        {
            if (strings_.Count == 0)
            {
                return 0;
            }

            int low = 0;
            int high = strings_.Count - 1;
            while (high - low > 1)
            {
                int index = low + ((high - low + 1) / 2);
                int dir = String.Compare(strings_[index], key, true, CultureInfo.InvariantCulture);
                if (dir > 0)
                {
                    high = index;
                }
                else
                if (dir < 0)
                {
                    low = index;
                }
                else
                {
                    return index;
                }
            }

            if (String.Compare(strings_[low], key, true, CultureInfo.InvariantCulture) < 0)
            {
                if (String.Compare(strings_[high], key, true, CultureInfo.InvariantCulture) < 0)
                {
                    return high + 1;
                }

                return low + 1;
            }

            return low;
        }

        private void InvokeOnCollectionChanged(CollectionChangeEventArgs e)
        {
            CollectionChangeEventHandler handler = OnCollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void strings__CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            InvokeOnCollectionChanged(e);
        }

        #endregion // implementation

        #region representation

        private readonly AutoCompleteStringCollection strings_;

        #endregion // representation
    }
}

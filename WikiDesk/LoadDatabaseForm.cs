// -----------------------------------------------------------------------------------------
// <copyright file="LoadDatabaseForm.cs" company="ashodnakashian.com">
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
//   Defines the LoadDatabaseForm type.
// </summary>
// -----------------------------------------------------------------------------------------

using System.Windows.Forms;

namespace WikiDesk
{
    using System;

    public partial class LoadDatabaseForm : Form
    {
        public LoadDatabaseForm(
                string databasePath,
                SharedReference<long> entries,
                long totalEntires)
        {
            entries_ = entries;
            totalEntires_ = totalEntires;

            InitializeComponent();

            prgProgress_.Maximum = (int)(totalEntires / 1024);
            lblDatabasePathValue_.Text = databasePath;

            timer_.Interval = 60;
            timer_.Tick += OnTimer;
            timer_.Start();
        }

        public bool Cancel
        {
            get { return cancel_; }
        }

        #region implementation

        private void OnTimer(object sender, EventArgs e)
        {
            lblEntriesLoadedValue_.Text = string.Format("{0} / {1}", (long)entries_, totalEntires_);
            prgProgress_.Value = (int)(entries_ / 1024);
        }

        private void btnCancel__Click(object sender, EventArgs e)
        {
            cancel_ = true;
        }

        #endregion // implementation

        #region representation

        private readonly SharedReference<long> entries_;
        private readonly long totalEntires_;
        private readonly Timer timer_ = new Timer();

        private bool cancel_;

        #endregion // representation
    }

    public sealed class SharedReference<T>
    {
        public T Reference { get; set; }

        public static implicit operator T(SharedReference<T> rhs)
        {
            return rhs.Reference;
        }
    }
}

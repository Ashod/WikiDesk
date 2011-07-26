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

namespace WikiDesk
{
    using System;
    using System.Windows.Forms;

    internal partial class LoadDatabaseForm
        : Form,
          IProgress
    {
        public LoadDatabaseForm()
        {
            InitializeComponent();

            timer_.Interval = 60;
            timer_.Tick += OnTimer;
            timer_.Start();
        }

        #region IProgress

        /// <summary>
        /// An event that may be called from the progress reporter to passively
        /// update the report.
        /// </summary>
        public event Action<IProgress, EventArgs> OnUpdate;

        /// <summary>
        /// Gets or sets the main operation being performed.
        /// </summary>
        public string Operation
        {
            get { return lblDatabasePathValue_.Text; }
            set { lblDatabasePathValue_.Text = value; }
        }

        /// <summary>
        /// Gets or sets a message describing a current activity or note.
        /// </summary>
        public string Message
        {
            get { return lblEntriesLoadedValue_.Text; }
            set { lblEntriesLoadedValue_.Text = value; }
        }

        /// <summary>
        /// Gets or sets the total 100% progress points.
        /// </summary>
        public int Total
        {
            get { return prgProgress_.Maximum; }
            set { prgProgress_.Maximum = value; }
        }

        /// <summary>
        /// Gets or sets the current progress points.
        /// </summary>
        public int Current
        {
            get { return prgProgress_.Value; }
            set { prgProgress_.Value = Math.Min(value, Total); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation is cancelled.
        /// </summary>
        public bool Cancel { get; set; }

        #endregion // IProgress

        #region implementation

        private void OnTimer(object sender, EventArgs e)
        {
            InvokeOnUpdate(e);
        }

        private void btnCancel__Click(object sender, EventArgs e)
        {
            Cancel = true;
        }

        private void InvokeOnUpdate(EventArgs e)
        {
            Action<IProgress, EventArgs> handler = OnUpdate;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion // implementation

        #region representation

        /// <summary>
        /// Update timer.
        /// </summary>
        private readonly Timer timer_ = new Timer();

        #endregion // representation
    }
}

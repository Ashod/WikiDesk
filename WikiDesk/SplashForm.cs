// -----------------------------------------------------------------------------------------
// <copyright file="SplashForm.cs" company="ashodnakashian.com">
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
//   Defines the SplashForm type.
// </summary>

namespace WikiDesk
{
    using System;
    using System.Windows.Forms;

    internal partial class SplashForm
        : Form,
          IProgress
    {
        public SplashForm()
        {
            InitializeComponent();

            timer_.Interval = 80;
            timer_.Tick += OnTimer;
            timer_.Start();
        }

        /// <summary>
        /// Called in a timer to give allow subscribers to
        /// update our properties before we update the controls.
        /// </summary>
        public event Action<IProgress, EventArgs> OnUpdate;

        /// <summary>
        /// Gets or sets the main operation being performed.
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// Gets or sets a message describing a current activity or note.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the total 100% progress points.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the current progress points.
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation is cancelled.
        /// </summary>
        public bool Cancel { get; set; }

        #region implementation

        private void UpdateControls()
        {
            pbProgress_.Maximum = Total;
            pbProgress_.Value = Current;
            txtMessage_.Text = Operation + Environment.NewLine + Message;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            InvokeOnUpdate(e);
            UpdateControls();
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

        /// <summary>The update timer.</summary>
        private readonly Timer timer_ = new Timer();

        #endregion // representation
    }
}

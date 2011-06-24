// -----------------------------------------------------------------------------------------
// <copyright file="OptionsForm.cs" company="ashodnakashian.com">
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
//   Defines the OptionsForm type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    public partial class OptionsForm : Form
    {
        public OptionsForm(Settings settings)
        {
            InitializeComponent();

            settings_ = settings;

            #region General

            numAutoUpdateOld_.Value = settings_.AutoUpdateDays;
            chkAutoRetrieveMissing_.Checked = settings_.AutoRetrieveMissing;
            chkTrackBrowseHistory_.Checked = settings_.TrackBrowseHistory;
            txtDefaultDatabase_.Text = settings_.DefaultDatabaseFilename;

            #endregion // General

            #region Cache

            chkEnableCaching_.Checked = settings_.EnableCaching;
            txtCacheFolder_.Text = settings_.FileCacheFolder;
            barCacheSize_.Value = GetFileCacheLogSize(settings_.FileCacheSizeMB);
            chkClearCacheOnExit_.Checked = settings_.ClearFileCacheOnExit;

            #endregion // Cache

            #region Wiki

            numThumbWidth_.Value = settings_.ThumbnailWidthPixels;
            LoadSkins();
            cbSkinName_.SelectedIndex = cbSkinName_.FindStringExact(settings_.SkinName);
            txtCustomCss_.Text = settings_.CustomCss;

            #endregion // Wiki
        }

        private void LoadSkins()
        {
            string skinsPath = Path.Combine(settings_.InstallationFolder, "Skins");
            foreach (string directory in Directory.GetDirectories(skinsPath))
            {
                string dirName = Path.GetFileName(directory);

                // Skip the common folder.
                if (!string.IsNullOrEmpty(dirName) &&
                    string.Compare(dirName, "common", true) != 0)
                {
                    cbSkinName_.Items.Add(dirName);
                }
            }
        }

        #region implementation

        private static int GetFileCacheLogSize(long sizeMB)
        {
            long size = sizeMB / BASE_SIZE_FACTOR;
            return (int)Math.Log(size, 2) + 1;
        }

        #endregion // implementation

        private Settings settings_;

        private const int BASE_SIZE_FACTOR = 128;
    }
}

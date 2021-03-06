﻿// -----------------------------------------------------------------------------------------
// <copyright file="SearchControl.cs" company="ashodnakashian.com">
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
//   Defines the SearchControl type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    using WeifenLuo.WinFormsUI.Docking;

    using WikiDesk.Core;
    using WikiDesk.Data;

    public partial class SearchControl : DockContent
    {
        public delegate void OnTitleNavigate(string domainName, string languageName, string title);

        public SearchControl(
                    Database db,
                    Dictionary<string,
                                Dictionary<string, PrefixMatchContainer<string>>> entriesMap,
                    OnTitleNavigate onTitleNavigate)
        {
            InitializeComponent();

            db_ = db;
            entriesMap_ = entriesMap;
            onTitleNavigate_ = onTitleNavigate;

            UpdateListItems();
        }

        public void UpdateListItems()
        {
            cboDomains_.Items.Clear();
            foreach (KeyValuePair<string, Dictionary<string, PrefixMatchContainer<string>>> domainLangPair in entriesMap_)
            {
                if (domainLangPair.Value.Count > 0)
                {
                    cboDomains_.Items.Add(domainLangPair.Key);
                }
            }

            if (cboDomains_.SelectedIndex >= 0)
            {
                cboDomains_.SelectedIndex = cboDomains_.SelectedIndex;
            }
            else
            {
                cboDomains_.SelectedIndex = (cboDomains_.Items.Count > 0) ? 0 : -1;
            }
        }

        private void cboDomains__SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int langsIndex = -1;
            if (cboDomains_.SelectedIndex >= 0)
            {
                Dictionary<string, PrefixMatchContainer<string>> langEntries;
                if (entriesMap_.TryGetValue(cboDomains_.Text, out langEntries))
                {
                    cboLanguages_.Items.Clear();

                    foreach (KeyValuePair<string, PrefixMatchContainer<string>> langTitlesPair in langEntries)
                    {
                        cboLanguages_.Items.Add(langTitlesPair.Key);
                    }

                    //TODO: Automatically select the default or current language.
                    langsIndex = (cboLanguages_.Items.Count > 0) ? 0 : -1;
                }
            }

            cboLanguages_.SelectedIndex = langsIndex;
            if (langsIndex >= 0)
            {
                cboLanguages_.Enabled = true;
                txtTitle_.Enabled = true;
                lstTitles_.Enabled = true;
                btnSearch_.Enabled = !string.IsNullOrEmpty(txtTitle_.Text);
            }
            else
            {
                cboLanguages_.Enabled = false;
                txtTitle_.Enabled = false;
                lstTitles_.Enabled = false;
                btnSearch_.Enabled = false;
            }
        }

        private void txtTitle__KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch__Click(sender, e);
            }
        }

        private void txtTitle__KeyUp(object sender, KeyEventArgs e)
        {
            btnSearch_.Enabled = !string.IsNullOrEmpty(txtTitle_.Text);
        }

        private void btnSearch__Click(object sender, System.EventArgs e)
        {
            lstTitles_.Items.Clear();

            if (!string.IsNullOrEmpty(txtTitle_.Text))
            {
                string query = txtTitle_.Text;
                query = '%' + query.Trim('%') + '%';

                int domainId = db_.GetDomain(cboDomains_.Text).Id;
                int languageId = db_.GetLanguageByName(cboLanguages_.Text).Id;
                foreach (string title in db_.SearchPages(domainId, languageId, query))
                {
                    string titleDenorm = Title.Decanonicalize(title);
                    lstTitles_.Items.Add(titleDenorm);
                }
            }
        }

        private void lstTitles__DoubleClick(object sender, System.EventArgs e)
        {
            string title = lstTitles_.FocusedItem != null ? lstTitles_.FocusedItem.Text : null;
            NavigateTo(title);
        }

        private void NavigateTo(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                txtTitle_.Text = title;
                if (onTitleNavigate_ != null)
                {
                    onTitleNavigate_(cboDomains_.Text, cboLanguages_.Text, title);
                }
            }
        }

        private void lstTitles__Resize(object sender, System.EventArgs e)
        {
            lstTitles_.Columns[0].Width = lstTitles_.ClientSize.Width - 5;
        }

        #region representation

        private readonly Database db_;

        private readonly OnTitleNavigate onTitleNavigate_;

        private readonly Dictionary<string, Dictionary<string, PrefixMatchContainer<string>>> entriesMap_;

        #endregion // representation
    }
}

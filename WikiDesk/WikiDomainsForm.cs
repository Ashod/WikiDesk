// -----------------------------------------------------------------------------------------
// <copyright file="WikiDomainsForm.cs" company="ashodnakashian.com">
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
//   Wiki Domains Editor.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System;
    using System.Windows.Forms;

    using WikiDesk.Core;

    public partial class WikiDomainsForm : Form
    {
        public WikiDomainsForm()
        {
            InitializeComponent();

            Domains = new WikiDomains();
        }

        public WikiDomains Domains { get; set; }

        private void WikiDomainsForm_Load(object sender, EventArgs e)
        {
            ToList();
        }

        private void lvDomains__SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDomains_.SelectedItems == null ||
                lvDomains_.SelectedItems.Count == 0)
            {
                btnAddUpdate_.Text = "Add";
                btnRemove_.Enabled = false;
                return;
            }

            btnAddUpdate_.Text = "Update";
            btnRemove_.Enabled = true;
            WikiDomain wikiDomain = lvDomains_.SelectedItems[0].Tag as WikiDomain;
            if (wikiDomain != null)
            {
                txtName_.Text = wikiDomain.Name;
                txtDomain_.Text = wikiDomain.Domain;
                txtFullPath_.Text = wikiDomain.FullPath;
                txtFriendlyPath_.Text = wikiDomain.FriendlyPath;
            }
        }

        private void btnAddUpdate__Click(object sender, EventArgs e)
        {
            if (lvDomains_.SelectedItems != null &&
                lvDomains_.SelectedItems.Count == 1)
            {
                // Update.
                ListViewItem lvi = lvDomains_.SelectedItems[0];
                WikiDomain wikiDomain = lvi.Tag as WikiDomain;
                if (wikiDomain != null)
                {
                    wikiDomain.Name = txtName_.Text;
                    wikiDomain.Domain = txtDomain_.Text;
                    wikiDomain.FullPath = txtFullPath_.Text;
                    wikiDomain.FriendlyPath = txtFriendlyPath_.Text;

                    lvi.Text = wikiDomain.Name;
                    lvi.SubItems.Clear();
                    lvi.SubItems.Add(wikiDomain.Domain);
                    lvi.SubItems.Add(wikiDomain.FullPath);
                    lvi.SubItems.Add(wikiDomain.FriendlyPath);
                }
            }
            else
            {
                // Add.
                WikiDomain wikiDomain = new WikiDomain(txtName_.Text, txtDomain_.Text);
                wikiDomain.FullPath = txtFullPath_.Text;
                wikiDomain.FriendlyPath = txtFriendlyPath_.Text;
                int index = Domains.Domains.FindIndex(x => x.Name == wikiDomain.Name && x.Domain == wikiDomain.Domain);
                if (index >= 0)
                {
                    MessageBox.Show("Domain already exists.", "Add Domain", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AddDomainToList(wikiDomain);
            }

            FromList();
        }

        private void btnRemove__Click(object sender, EventArgs e)
        {
            if (lvDomains_.SelectedItems != null &&
                lvDomains_.SelectedItems.Count == 1)
            {
                lvDomains_.Items.Remove(lvDomains_.SelectedItems[0]);
                FromList();
            }
        }

        private void btnSave__Click(object sender, EventArgs e)
        {
            FromList();
        }

        private void FromList()
        {
            Domains.Domains.Clear();
            foreach (ListViewItem lvi in lvDomains_.Items)
            {
                Domains.Domains.Add((WikiDomain)lvi.Tag);
            }
        }

        private void ToList()
        {
            foreach (WikiDomain wikiDomain in Domains.Domains)
            {
                AddDomainToList(wikiDomain);
            }
        }

        private void AddDomainToList(WikiDomain wikiDomain)
        {
            ListViewItem lvi = new ListViewItem(wikiDomain.Name);
            lvi.SubItems.Add(wikiDomain.Domain);
            lvi.SubItems.Add(wikiDomain.FullPath);
            lvi.SubItems.Add(wikiDomain.FriendlyPath);
            lvi.Tag = wikiDomain;
            lvDomains_.Items.Add(lvi);
        }
    }
}

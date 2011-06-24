// -----------------------------------------------------------------------------------------
// <copyright file="ImportForm.cs" company="ashodnakashian.com">
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
//   Defines the ImportForm type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using WikiDesk.Core;

    public partial class ImportForm : Form
    {
        public ImportForm(WikiDomains domains, LanguageCodes languages)
        {
            InitializeComponent();

            domains_ = domains;
            languages_ = languages;

            foreach (WikiDomain wikiDomain in domains.Domains)
            {
                cboDomains_.Items.Add(wikiDomain.Name);
            }

            cboDomains_.SelectedIndex = -1;

            foreach (WikiLanguage wikiLanguage in languages_.Languages)
            {
                cboLanguages_.Items.Add(wikiLanguage.LocalName);
            }

            //TODO: Automatically select the default or current language.
            cboLanguages_.SelectedIndex = (cboLanguages_.Items.Count > 0) ? 0 : -1;

            UpdateDumpInfo();
        }

        #region properties

        public bool LocalDump
        {
            get { return rdFileDump_.Checked; }
        }

        public string DumpFileSource
        {
            get { return rdFileDump_.Checked ? txtFileDumpPath_.Text : txtWebDumpUrl_.Text; }
        }

        public string DomainName
        {
            get { return cboDomains_.Text; }
        }

        public string LanguageName
        {
            get { return cboLanguages_.Text; }
        }

        public DateTime Date
        {
            get { return dateTimePicker_.Value; }
        }

        public bool IndexOnly
        {
            get { return chkIndexOnly_.Checked; }
        }

        public long ResumePosition
        {
            get { return (long)(numResumePosKbytes_.Value * 1024); }
        }

        #endregion // properties

        private void rdFileDump_CheckedChanged(object sender, EventArgs e)
        {
            txtFileDumpPath_.Enabled = rdFileDump_.Checked;
            btnBrowse_.Enabled = rdFileDump_.Checked;
            UpdateDumpInfo();
        }

        private void rdWebDump_CheckedChanged(object sender, EventArgs e)
        {
            txtWebDumpUrl_.Enabled = rdWebDump_.Checked;
            UpdateDumpInfo();
        }

        private void txtWebDumpUrl__TextChanged(object sender, EventArgs e)
        {
            UpdateDumpInfo();
        }

        private void btnBrowse__Click(object sender, EventArgs e)
        {
            openFileDialog_.CheckFileExists = true;
            openFileDialog_.DefaultExt = "bz2";
            openFileDialog_.Filter = "Compressed XML files (*.xml.bz2)|*.xml.bz2|XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog_.ShowDialog(this) == DialogResult.OK)
            {
                txtFileDumpPath_.Text = openFileDialog_.FileName;
            }

            UpdateDumpInfo();
        }

        private void UpdateDumpInfo()
        {
            string domainName;
            string languageCode;
            DateTime date;
            bool validDumpFileName = ParseDumpFileName(DumpFileSource, out domainName, out languageCode, out date);

            // In dumps, Wikipedia is shortened to Wiki.
            if (string.Compare(domainName, "wiki", true) == 0)
            {
                domainName = "Wikipedia";
            }

            cboDomains_.SelectedIndex = domains_.Domains.FindIndex(domain => string.Compare(domain.Name, domainName, true) == 0);
            cboLanguages_.SelectedIndex = languages_.Languages.FindIndex(lang => lang.Code == languageCode);
            dateTimePicker_.Value = date;

            UpdateImportButton(validDumpFileName);
        }

        private void cboDomains__SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateImportButton();
        }

        private void cboLanguages__SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateImportButton();
        }

        private void dateTimePicker__ValueChanged(object sender, EventArgs e)
        {
            UpdateImportButton();
        }

        private void UpdateImportButton()
        {
            UpdateImportButton(true);
        }

        private void UpdateImportButton(bool validDumpFileName)
        {
            gbDumpInfo_.Enabled = validDumpFileName;
            gbImportOptions_.Enabled = validDumpFileName;

            btnImport_.Enabled = validDumpFileName &&
                                 cboDomains_.SelectedIndex >= 0 &&
                                 cboLanguages_.SelectedIndex >= 0 &&
                                 dateTimePicker_.Value.Year > 1990;
        }

        private bool ParseDumpFileName(string source, out string domain, out string lang, out DateTime date)
        {
            string fileName = Path.GetFileName(source);
            Match match = rexWikiDumpFilename_.Match(fileName);
            if (match.Success)
            {
                lang = match.Groups[1].Value;
                domain = match.Groups[2].Value;
                date = DateTime.ParseExact(match.Groups[3].Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                return true;
            }

            domain = string.Empty;
            lang = string.Empty;
            date = DateTimePicker.MinDateTime;
            return false;
        }

        private void btnImport__Click(object sender, EventArgs e)
        {
            if (LocalDump && !File.Exists(DumpFileSource))
            {
                MessageBox.Show(
                        "Please select a valid dump file to import from.",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(cboDomains_.Text))
            {
                MessageBox.Show(
                        "Please select a valid domain.",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(cboLanguages_.Text))
            {
                MessageBox.Show(
                        "Please select a valid language.",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel__Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region representation

        private readonly WikiDomains domains_;

        private readonly LanguageCodes languages_;

        private readonly Regex rexWikiDumpFilename_ = new Regex(@"^(.+?)(WIK.+?)\-(\d{8})\-(.+?)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion // representation
    }
}

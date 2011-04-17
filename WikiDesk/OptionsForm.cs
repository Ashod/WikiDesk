
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

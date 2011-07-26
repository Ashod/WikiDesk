// -----------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="ashodnakashian.com">
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
//   Defines the Settings type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    [Serializable]
    public class Settings
    {
        public static Settings Deserialize(string filename)
        {
            Settings settings;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    settings = (Settings)serializer.Deserialize(fs);
                }

                if (settings != null)
                {
                    return settings;
                }
            }
            catch (Exception)
            {
            }

            settings = new Settings();
            return settings;
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        #region General

        public int AutoUpdateDays = 7;
        public bool AutoRetrieveMissing = true;
        public bool TrackBrowseHistory;

        /// <summary>
        /// WikiDesk layout settings.
        /// </summary>
        public string Layout;

        public string FontName;
        public float FontSize;

        #endregion // General

        #region Cache

        public bool EnableCaching;
        public string FileCacheFolder = "Z:\\wikidesk_cache\\";
        public long FileCacheSizeMB = 128;
        public bool ClearFileCacheOnExit;

        #endregion // Cache

        #region Wiki

        public int ThumbnailWidthPixels = 220;
        public string SkinName = "simple";

        /// <summary>
        /// Valid CSS inserted right after all css inclusions. Optional.
        /// Typically contains customization and overrides to the skin CSS.
        /// </summary>
        public string CustomCss;

        #endregion // Wiki

        /// <summary>
        /// The default database filename. Loaded at startup.
        /// </summary>
        public string DefaultDatabaseFilename = "Z:\\wikidesk.db";

        public string InstallationFolder;

        #region domain

        /// <summary>
        /// The wiki domains configuration filename.
        /// </summary>
        public string DomainsFilename = "WikiDomains.xml";

        /// <summary>
        /// The current domain name.
        /// </summary>
        public string CurrentDomainName = "wikipedia";

        /// <summary>
        /// The default domain name. When no domain is specified.
        /// </summary>
        public string DefaultDomainName = "wikipedia";

        #endregion // domain

        #region language

        /// <summary>
        /// The wiki languages configuration filename.
        /// </summary>
        public string LanguagesFilename = "WikiLanguages.xml";

        /// <summary>
        /// The current language code.
        /// </summary>
        public string CurrentLanguageCode = "en";

        /// <summary>
        /// The default language code. When no language is specified.
        /// </summary>
        public string DefaultLanguageCode = "en";

        #endregion // language
    }
}

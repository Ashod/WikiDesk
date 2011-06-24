// -----------------------------------------------------------------------------------------
// <copyright file="FileCache.cs" company="ashodnakashian.com">
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
//   Defines the FileCache type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using WikiDesk.Core;

    public class FileCache : IFileCache
    {
        private class State
        {
            public long Size;
            public bool Cached;
        }

        #region construction

        public FileCache(string cacheFolder)
        {
            if (string.IsNullOrEmpty(cacheFolder))
            {
                throw new ArgumentNullException("cacheFolder");
            }

            if (!Directory.Exists(cacheFolder))
            {
                Directory.CreateDirectory(cacheFolder);
            }

            cacheFolder_ = cacheFolder;
            cacheFolderUrl_ = "file:///" + cacheFolder;
            cache_ = new Dictionary<string, State>(1024);

            Refresh();
        }

        #endregion // construction

        #region Implementation of IFileCache

        public long Size
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Shrink(long limit)
        {
            long totalSize = 0;
            string[] filenames = Directory.GetFiles(cacheFolder_);
            SortedList<DateTime, FileInfo> files = new SortedList<DateTime, FileInfo>(filenames.Length);
            foreach (string filename in filenames)
            {
                FileInfo fi = new FileInfo(filename);
                files.Add(fi.LastAccessTimeUtc, fi);
                totalSize += fi.Length;
            }

            if (totalSize > limit)
            {
                foreach (KeyValuePair<DateTime, FileInfo> keyValuePair in files)
                {
                    try
                    {
                        File.Delete(keyValuePair.Value.FullName);
                        totalSize -= keyValuePair.Value.Length;
                        if (totalSize <= limit)
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                Refresh();
            }
        }

        /// <summary>
        /// Refreshes the cached files from disk.
        /// </summary>
        public void Refresh()
        {
            lock (guard_)
            {
                cache_.Clear();
            }

            string[] fullnames = Directory.GetFiles(cacheFolder_);
            foreach (string fullname in fullnames)
            {
                string filename = Path.GetFileName(fullname);
                if (!string.IsNullOrEmpty(filename))
                {
                    FileInfo fi = new FileInfo(fullname);
                    if (fi.Exists)
                    {
                        if (fi.Length > 0)
                        {
                            filename = filename.ToUpperInvariant();
                            State state = new State();
                            state.Cached = true;
                            state.Size = fi.Length;

                            lock (guard_)
                            {
                                cache_.Add(filename, state);
                            }
                        }
                        else
                        {
                            File.Delete(fullname);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether a file is cached or not.
        /// </summary>
        /// <param name="mediaName">The media file name.</param>
        /// <param name="languageCode">The wiki language code.</param>
        /// <returns>True if the file is cached, False otherwise.</returns>
        public bool IsSourceCached(string mediaName, string languageCode)
        {
            mediaName = FixupMediaName(mediaName).ToUpperInvariant();

            bool hasFile;
            lock (guard_)
            {
                hasFile = cache_.ContainsKey(mediaName);
            }

            if (hasFile)
            {
                if (File.Exists(Path.Combine(cacheFolder_, mediaName)))
                {
                    return true;
                }

                lock (guard_)
                {
                    cache_.Remove(mediaName);
                }
            }

            return false;
        }

        /// <summary>
        /// Resolves the URL of a media file.
        /// For cached items, this is a file:/// url.
        /// </summary>
        /// <param name="mediaName">The media file name.</param>
        /// <param name="languageCode">The wiki language code.</param>
        /// <returns></returns>
        public string ResolveSourceUrl(string mediaName, string languageCode)
        {
            string url = cacheFolderUrl_ + FixupMediaName(mediaName);

            return url.Replace('\\', '/');
        }

        /// <summary>
        /// Requests the cache to download and insure the media
        /// file is cached.
        /// </summary>
        /// <param name="mediaName">The media file name.</param>
        /// <param name="languageCode">The wiki language code.</param>
        /// <param name="url">The url of the media file to cache.</param>
        public void CacheMedia(string mediaName, string languageCode, string url)
        {
            mediaName = FixupMediaName(mediaName);

            string filename = Path.Combine(cacheFolder_, mediaName);
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                WebStream.WriteToStream(url, "WikiDesk", string.Empty, fs);
            }

            FileInfo fi = new FileInfo(filename);
            if (fi.Exists && fi.Length > 0)
            {
                mediaName = mediaName.ToUpperInvariant();
                State state = new State { Cached = true, Size = fi.Length };
                cache_[mediaName] = state;
                totalSize_ += state.Size;
            }
        }

        #endregion

        #region implementation

        private static string FixupMediaName(string mediaName)
        {
            if (mediaName.ToUpperInvariant().EndsWith(".SVG"))
            {
                mediaName = "400px-" + mediaName + ".png";
            }

            return mediaName;
        }

        #endregion // implementation

        #region representation

        private long totalSize_;
        private readonly string cacheFolder_;
        private readonly string cacheFolderUrl_;

        private readonly Dictionary<string, State> cache_;

        /// <summary>
        /// Thread-safety guard.
        /// </summary>
        private readonly object guard_ = new object();

        #endregion // representation
    }
}

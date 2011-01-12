
namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    using WikiDesk.Core;

    public class FileCache : IFileCache
    {
        private class State
        {
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
            cacheFolderUrl_ = "file:///" + cacheFolder; //.Replace('\\', '/');
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
            string[] fullnames = Directory.GetFiles(cacheFolder_);
            foreach (string fullname in fullnames)
            {
                string filename = Path.GetFileName(fullname);
                if (!string.IsNullOrEmpty(filename))
                {
                    filename = filename.ToUpperInvariant();
                    State state = new State();
                    state.Cached = true;

                    lock (guard_)
                    {
                        cache_.Add(filename, state);
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether or not a file is cached or not.
        /// </summary>
        /// <param name="mediaName">The media file name.</param>
        /// <param name="languageCode">The wiki language code.</param>
        /// <returns>True if the file is cached, False otherwise.</returns>
        public bool IsSourceCached(string mediaName, string languageCode)
        {
            mediaName = mediaName.ToUpperInvariant();
            bool hasFile;
            lock (guard_)
            {
                hasFile = cache_.ContainsKey(mediaName);
            }

            return hasFile && File.Exists(Path.Combine(cacheFolder_, mediaName));
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
            return cacheFolderUrl_ + mediaName;
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
            // Open a connection
            HttpWebRequest webRequestObject = (HttpWebRequest)WebRequest.Create(url);

//             // You can also specify additional header values like
//             // the user agent or the referrer:
             webRequestObject.UserAgent = ".NET Framework/2.0";
//             webRequestObject.Referer = "http://www.example.com/";

            // Request response:
            using (WebResponse response = webRequestObject.GetResponse())
            {
                using (Stream webStream = response.GetResponseStream())
                {
                    if (webStream == null)
                    {
                        return;
                    }

                    string filename = Path.Combine(cacheFolder_, mediaName);
                    using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                    {
                        byte[] buffer = new byte[64 * 1024];
                        int read = webStream.Read(buffer, 0, buffer.Length);
                        while (read > 0)
                        {
                            fs.Write(buffer, 0, read);
                            read = webStream.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
            }

            mediaName = mediaName.ToUpperInvariant();
            State state = new State();
            state.Cached = true;
            cache_[mediaName] = state;
        }

        #endregion

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

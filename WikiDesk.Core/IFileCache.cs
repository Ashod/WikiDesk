namespace WikiDesk.Core
{
    public interface IFileCache
    {
        long Size { get; set; }

        /// <summary>
        /// Shrinks the disk space used by the cache to the provided limit.
        /// </summary>
        /// <param name="limit">The limit to reduce to, in bytes.</param>
        void Shrink(long limit);

        /// <summary>
        /// Refreshes the cached files from disk.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Checks whether or not a file is cached or not.
        /// </summary>
        /// <param name="mediaName">The media file name.</param>
        /// <param name="languageCode">The wiki language code.</param>
        /// <returns>True if the file is cached, False otherwise.</returns>
        bool IsSourceCached(string mediaName, string languageCode);

        /// <summary>
        /// Resolves the URL of a media file.
        /// For cached items, this is a file:/// url.
        /// </summary>
        /// <param name="mediaName">The media file name.</param>
        /// <param name="languageCode">The wiki language code.</param>
        /// <returns></returns>
        string ResolveSourceUrl(string mediaName, string languageCode);

        /// <summary>
        /// Requests the cache to download and insure the media
        /// file is cached.
        /// </summary>
        /// <param name="mediaName">The media file name.</param>
        /// <param name="languageCode">The wiki language code.</param>
        /// <param name="url">The url of the media file to cache.</param>
        void CacheMedia(string mediaName, string languageCode, string url);
    }
}

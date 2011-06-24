// -----------------------------------------------------------------------------------------
// <copyright file="IFileCache.cs" company="ashodnakashian.com">
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
//   Defines the IFileCache type.
// </summary>
// -----------------------------------------------------------------------------------------

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

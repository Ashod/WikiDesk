// -----------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="ashodnakashian.com">
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
//   Defines the Configuration type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    public class Configuration
    {
        public Configuration(WikiSite wikiSite)
        {
            wikiSite_ = wikiSite;
        }

        public WikiSite WikiSite
        {
            get { return wikiSite_; }
        }

        public bool AutoRedirect = false;

        /// <summary>
        /// The number of H1 headers to automatically add TOC.
        /// Default is 4.
        /// -ve number to never automatically add TOC.
        /// 0 number to always add TOC.
        /// +ve number to add when at least that many headers exist.
        /// </summary>
        public int AutoTocHeaderCount = 4;

        /// <summary>
        /// The maximum heading number to include in the TOC.
        /// Default is 2.
        /// </summary>
        public int MaxHeadingToIncludeInToc = 2;

        public string InternalLinkPrefix = "wiki://";

        public string SkinsPath = string.Empty;

        /// <summary>
        /// The common-images path within the skins folder.
        /// </summary>
        public string CommonImagesPath
        {
            get { return "common\\images"; }
        }

        public int ThumbnailWidthPixels = 220;

        public string FileCacheFolder = "Z:\\wikidesk_cache\\";

        private readonly WikiSite wikiSite_;
    }
}

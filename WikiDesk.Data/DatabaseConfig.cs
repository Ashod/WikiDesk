// -----------------------------------------------------------------------------------------
// <copyright file="DatabaseConfig.cs" company="ashodnakashian.com">
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
//   The database configuration structure.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Data
{
    using System;

    /// <summary>
    /// The database configuration structure.
    /// </summary>
    [Serializable]
    public class DatabaseConfig
    {
        public DatabaseConfig()
        {
            CacheSizePages = 2000;
            CaseSensitiveLike = false;
            LockMode = LockingMode.Normal;
            SyncMode = SynchronousMode.Full;
        }

        /// <summary>
        /// Possible database locking modes.
        /// </summary>
        public enum LockingMode : byte
        {
            /// <summary>
            /// In NORMAL locking-mode (the default unless overridden at compile-time),
            /// a database connection unlocks the database file at the conclusion of
            /// each read or write transaction.
            /// </summary>
            Normal = 0,

            /// <summary>
            /// When the locking-mode is set to EXCLUSIVE, the database connection never
            /// releases file-locks. The first time the database is read in EXCLUSIVE mode,
            /// a shared lock is obtained and held. The first time the database is written,
            /// an exclusive lock is obtained and held.
            /// </summary>
            Exclusive = 1
        }

        /// <summary>
        /// Possible database synchronization modes.
        /// </summary>
        public enum SynchronousMode : byte
        {
            /// <summary>
            /// SQLite continues without syncing as soon as it has handed data off to the
            /// operating system. If the application running SQLite crashes, the data will
            /// be safe, but the database might become corrupted if the operating system
            /// crashes or the computer loses power before that data has been written to 
            /// the disk surface. On the other hand, some operations are as much as 50 or
            /// more times faster with synchronous OFF.
            /// </summary>
            Off = 0,

            /// <summary>
            /// The SQLite database engine will still sync at the most critical moments,
            /// but less often than in FULL mode. There is a very small (though non-zero)
            /// chance that a power failure at just the wrong time could corrupt the database
            /// in NORMAL mode. But in practice, you are more likely to suffer a catastrophic
            /// disk failure or some other unrecoverable hardware fault.
            /// </summary>
            Normal = 1,

            /// <summary>
            /// The SQLite database engine will use the xSync method of the VFS to ensure
            /// that all content is safely written to the disk surface prior to continuing.
            /// This ensures that an operating system crash or power failure will not corrupt
            /// the database. FULL synchronous is very safe, but it is also slower.
            /// Default.
            /// </summary>
            Full = 2
        }

        /// <summary>
        /// Gets or sets the cache size in number of database pages.
        /// </summary>
        public int CacheSizePages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Like and Glob are case-sensitive.
        /// Default is False.
        /// </summary>
        public bool CaseSensitiveLike { get; set; }

        /// <summary>
        /// Gets or sets the database locking mode. <see cref="LockingMode"/>.
        /// </summary>
        public LockingMode LockMode { get; set; }

        /// <summary>
        /// Gets or sets the database sync mode. <see cref="SynchronousMode"/>.
        /// </summary>
        public SynchronousMode SyncMode { get; set; }
    }
}

﻿// -----------------------------------------------------------------------------------------
// <copyright file="IProgress.cs" company="ashodnakashian.com">
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
//   Progress reporting and cancellation interface.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System;

    /// <summary>
    /// Progress reporting and cancellation interface.
    /// </summary>
    internal interface IProgress
    {
        /// <summary>
        /// An event that may be called from the progress reporter to passively
        /// update the report.
        /// </summary>
        event Action<IProgress, EventArgs> OnUpdate;

        /// <summary>
        /// Gets or sets the main operation being performed.
        /// </summary>
        string Operation { get; set; }

        /// <summary>
        /// Gets or sets a message describing a current activity or note.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the total 100% progress points.
        /// </summary>
        int Total { get; set; }

        /// <summary>
        /// Gets or sets the current progress points.
        /// </summary>
        int Current { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation is cancelled.
        /// </summary>
        bool Cancel { get; set; }
    }
}
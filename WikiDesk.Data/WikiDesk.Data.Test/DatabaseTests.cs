// -----------------------------------------------------------------------------------------
// <copyright file="DatabaseTests.cs" company="ashodnakashian.com">
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
//   Defines the DatabaseTests type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Data.Test
{
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    public class DatabaseTests
    {
        [SetUp]
        public void Setup()
        {
            databaseFilename_ = Path.GetTempFileName();
            database_ = new Database(databaseFilename_);
        }

        [TearDown]
        public void TearDown()
        {
            database_.Dispose();

            try
            {
                File.Delete(databaseFilename_);
            }
            catch
            {
            }
        }

        [Test]
        public void SecondaryDatabaseInstance()
        {
            using (Database db = new Database(databaseFilename_))
            {
                Assert.NotNull(db);
            }
        }

        protected Database Database
        {
            get { return database_; }
        }

        #region representation

        private string databaseFilename_;

        private Database database_;

        #endregion // representation
    }
}

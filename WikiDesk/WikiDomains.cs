// -----------------------------------------------------------------------------------------
// <copyright file="WikiDomains.cs" company="ashodnakashian.com">
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
//   Defines the WikiDomains type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using WikiDesk.Core;

    [Serializable]
    public class WikiDomains
    {
        public static WikiDomains DefaultDomains()
        {
            WikiDomains domains = new WikiDomains();
            domains.Domains.Add(new WikiDomain("wikipedia"));
            domains.Domains.Add(new WikiDomain("wiktionary"));

            return domains;
        }

        public static WikiDomains Deserialize(string filename)
        {
            try
            {
                WikiDomains domains;
                XmlSerializer serializer = new XmlSerializer(typeof(WikiDomains));
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    domains = (WikiDomains)serializer.Deserialize(fs);
                }

                if (domains.Domains != null && domains.Domains.Count > 0)
                {
                    return domains;
                }
            }
            catch (Exception)
            {
            }

            return DefaultDomains();
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WikiDomains));
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        public WikiDomain FindByName(string currentDomainName)
        {
            currentDomainName = currentDomainName.ToUpperInvariant();
            foreach (WikiDomain wikiDomain in Domains)
            {
                if (wikiDomain.Name.ToUpperInvariant() == currentDomainName)
                {
                    return wikiDomain;
                }
            }

            return null;
        }

        public readonly List<WikiDomain> Domains = new List<WikiDomain>(16);
    }
}

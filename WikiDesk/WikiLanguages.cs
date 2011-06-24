// -----------------------------------------------------------------------------------------
// <copyright file="WikiLanguages.cs" company="ashodnakashian.com">
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
//   Defines the LanguageCodes type.
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
    public class LanguageCodes
    {
        public static LanguageCodes DefaultLanguageCodes()
        {
            // Add default languages.
            LanguageCodes codes = new LanguageCodes();
            codes.Languages.Add(new WikiLanguage("English", "en") { LocalName = "English" });
            codes.Languages.Add(new WikiLanguage("German", "de") { LocalName = "Deutsch" });
            codes.Languages.Add(new WikiLanguage("French", "fr") { LocalName = "Français" });
            codes.Languages.Add(new WikiLanguage("Polish",  "pl") { LocalName = "Polski" });
            codes.Languages.Add(new WikiLanguage("Italian", "it") { LocalName = "Italiano" });
            codes.Languages.Add(new WikiLanguage("Japanese", "ja") { LocalName = "日本語" });
            codes.Languages.Add(new WikiLanguage("Spanish", "es") { LocalName = "Español" });
            codes.Languages.Add(new WikiLanguage("Dutch", "nl") { LocalName = "Nederlands" });
            codes.Languages.Add(new WikiLanguage("Portugues", "pt") { LocalName = "Português" });
            codes.Languages.Add(new WikiLanguage("Russian", "ru") { LocalName = "Русский" });
            codes.Languages.Add(new WikiLanguage("Swedish", "sv") { LocalName = "Svenska" });
            codes.Languages.Add(new WikiLanguage("Chinese", "zh") { LocalName = "中文" });
            codes.Languages.Add(new WikiLanguage("Catalan", "ca") { LocalName = "Català" });
            codes.Languages.Add(new WikiLanguage("Norwegian", "no") { LocalName = "Norsk (Bokmål)" });
            codes.Languages.Add(new WikiLanguage("Finnish", "fi") { LocalName = "Suomi" });
            codes.Languages.Add(new WikiLanguage("Ukrainian", "uk") { LocalName = "Українська" });
            codes.Languages.Add(new WikiLanguage("Hungarian", "hu") { LocalName = "Magyar" });
            codes.Languages.Add(new WikiLanguage("Czech", "cs") { LocalName = "Čeština" });
            codes.Languages.Add(new WikiLanguage("Romanian", "ro") { LocalName = "Română" });
            codes.Languages.Add(new WikiLanguage("Turkish", "tr") { LocalName = "Türkçe" });
            return codes;
        }

        public static LanguageCodes Deserialize(string filename)
        {
            try
            {
                LanguageCodes codes;
                XmlSerializer serializer = new XmlSerializer(typeof(LanguageCodes));
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    codes = (LanguageCodes)serializer.Deserialize(fs);
                }

                if (codes.Languages != null && codes.Languages.Count > 0)
                {
                    codes.Languages.Sort();
                    return codes;
                }
            }
            catch (Exception)
            {
            }

            return DefaultLanguageCodes();
        }

        public void Serialize(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LanguageCodes));
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(fs, this);
            }
        }

        public readonly List<WikiLanguage> Languages = new List<WikiLanguage>(32);
    }

    public class WikiArticleName
    {
        public WikiArticleName(string title, WikiLanguage lang)
        {
            Name = title;
            if (lang != null)
            {
                LanguageCode = lang.Code;
                if (!string.IsNullOrEmpty(lang.Name))
                {
                    LanguageName = lang.Name;
                    return;
                }

                if (!string.IsNullOrEmpty(lang.LocalName))
                {
                    LanguageName = lang.LocalName;
                    return;
                }
            }

            LanguageCode = "??";
            LanguageName = "Unknown";
        }

        public string Name { get; set; }

        public string LanguageCode { get; set; }

        public string LanguageName { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                return string.Format("{0} - {1}", LanguageName, Name);
            }

            return string.Format("{0} [{1}]", LanguageName, LanguageCode);
        }
    }
}

// -----------------------------------------------------------------------------------------
// <copyright file="WikiImage2Html.cs" company="ashodnakashian.com">
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
//   Defines the WikiImage2Html type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace WikiDesk.Core
{
    using System.Text;

    public static class WikiImage2Html
    {
        public enum Type
        {
            Thumbnail,
            Framed,
            Frameless
        }

        public enum Location
        {
            Right,
            Left,
            Center,
            None
        }

        public static string Convert(
                                string imagePageUrl,
                                string imageSrcUrl,
                                Type? type,
                                bool border,
                                Location? location,
                                int width,
                                int height,
                                string altText,
                                string caption,
                                string commonImagesUri)
        {
            StringBuilder sb = new StringBuilder(256);

            if ((location != null) || (type != null && type != Type.Frameless))
            {
                if (location == Location.Center)
                {
                    sb.Append("<div class=\"center\">");
                }

                if (type == null || type == Type.Frameless)
                {
                    if (location == null || location == Location.Right)
                    {
                        sb.Append("<div class=\"floatright\">");
                    }
                    else
                    {
                        if (location == Location.Left)
                        {
                            sb.Append("<div class=\"floatleft\">");
                        }
                        else
                        {
                            sb.Append("<div class=\"floatnone\">");
                        }
                    }
                }
                else
                {
                    sb.Append("<div class=\"thumb");
                    if (location == null || location == Location.Right)
                    {
                        sb.Append(" tright\">");
                    }
                    else
                    {
                        if (location == Location.Left)
                        {
                            sb.Append(" tleft\">");
                        }
                        else
                        {
                            sb.Append(" tnone\">");
                        }
                    }

                    sb.Append("<div class=\"thumbinner\" style=\"width:");
                    sb.Append(width + 2);
                    sb.Append("px;\">");
                }
            }

            bool visibleCaption = (caption != null && type != null && type != Type.Frameless);

            sb.Append("<a href=\"").Append(imagePageUrl).Append("\" class=\"image");
            if (caption != null)
            {
                if ((type == null) || (type == Type.Frameless))
                {
                    sb.Append("\" title=\"").Append(caption);
                }
            }

            sb.Append("\">");

            sb.Append("<img alt=\"");

            // Alt. text.
            if (!visibleCaption)
            {
                sb.Append(altText);
            }

            sb.Append("\" src=\"").Append(imageSrcUrl);
            if (width >= 0)
            {
                sb.Append("\" width=\"").Append(width);
            }

            if (height >= 0)
            {
                sb.Append("\" height=\"").Append(height);
            }

            if (type == Type.Thumbnail)
            {
                sb.Append("\" class=\"thumbimage");
            }
            else
            if (border)
            {
                sb.Append("\" class=\"thumbborder");
            }

            sb.Append("\">");
            sb.Append("</a>");

            if (visibleCaption)
            {
                sb.Append("<div class=\"thumbcaption\">");
                sb.Append("<div class=\"magnify\">");
                sb.Append("<a href=\"").Append(imagePageUrl).Append("\" class=\"internal\" title=\"Enlarge\">");
                sb.Append("<img src=\"");
                sb.Append(commonImagesUri);
                sb.Append("magnify-clip.png\" width=\"15\" height=\"11\" alt=\"\">");
                sb.Append("</a>");
                sb.Append("</div>");
                sb.Append(caption);
                sb.Append("</div>");
            }

            if ((location != null) || (type != null && type != Type.Frameless))
            {
                if (location == Location.Center)
                {
                    sb.Append("</div>");
                }

                sb.Append("</div>");

                if (type != null && type != Type.Frameless)
                {
                    sb.Append("</div>");
                }
            }

            return sb.ToString();
        }
    }
}

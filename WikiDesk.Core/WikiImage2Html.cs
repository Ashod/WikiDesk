using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiDesk.Core
{
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
                                string caption)
        {
            StringBuilder sb = new StringBuilder(256);

            if ((location == null) &&
                (type == null || type == Type.Frameless))
            {
                sb.Append("<p>");
            }
            else
            {
                if (location == Location.Center)
                {
                    sb.Append("<div class=\"center\">");
                }

                if ((type == null) || (type == Type.Frameless))
                {
                    if (location == Location.Right)
                    {
                        sb.Append("<div class=\"floatright\">");
                    }
                    else
                    if (location == Location.Left)
                    {
                        sb.Append("<div class=\"floatleft\">");
                    }
                    else
                    {
                        sb.Append("<div class=\"floatnone\">");
                    }
                }
                else
                {
                    sb.Append("<div class=\"thumb");
                    if (location == Location.Right)
                    {
                        sb.Append(" tright\">");
                    }
                    else
                    if (location == Location.Left)
                    {
                        sb.Append(" tleft\">");
                    }
                    else
                    {
                        sb.Append(" tnone\">");
                    }
                }
            }

            sb.Append("<a href=\"").Append(imagePageUrl).Append("\" class=\"image");
            if (caption != null)
            {
                sb.Append("\" title=\"").Append(caption);
            }

            sb.Append("\">");

            sb.Append("<img alt=\"").Append(altText);
            sb.Append("\" src=\"").Append(imageSrcUrl);
            if (width >= 0)
            {
                sb.Append("\" width=\"").Append(width);
            }

            if (height >= 0)
            {
                sb.Append("\" height=\"").Append(height);
            }

            if (border)
            {
                sb.Append("\" class=\"thumbborder");
            }

            sb.Append("\">");
            sb.Append("</a>");

            if ((location == null) &&
                (type == null || type == Type.Frameless))
            {
                sb.Append("</p>");
            }
            else
            {
                if (location == Location.Center)
                {
                    sb.Append("</div>");
                }

                sb.Append("</div>");
            }

            return sb.ToString();
        }
    }
}

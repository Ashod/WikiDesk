
namespace WikiDesk.Core
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    internal class WikiList2Html
    {
        public WikiList2Html(char marker, string listTag, string nodeTag)
        {
            this.marker = marker;
            listTagOpen = string.Format("<{0}>", listTag);
            listTagClose = string.Format("</{0}>", listTag);
            nodeTagOpen = string.Format("<{0}>", nodeTag);
            nodeTagClose = string.Format("</{0}>", nodeTag);
        }

        public string ConvertListCode(string line, StringReader sr)
        {
            Debug.Assert(line.StartsWith(marker.ToString()), "List must start with " + marker + '.');

            StringBuilder sb = new StringBuilder(1024);
            int curDepth = ConvertListCode(line, sb, sr, 0);
            if (insideContinuation)
            {
                // End a previous Continuation.
                insideContinuation = false;
                sb.AppendLine().Append("</dl>");
                sb.AppendLine();
            }
            
            sb.Append(nodeTagClose);
            while (--curDepth > 0)
            {
                sb.AppendLine().Append(listTagClose);
                sb.AppendLine().Append(nodeTagClose);
            }

            sb.AppendLine().Append(listTagClose);
            return sb.ToString();
        }

        private int ConvertListCode(string line, StringBuilder sb, StringReader sr, int prevDepth)
        {
            int curDepth = prevDepth;
            int depth = StringUtils.CountRepetition(line, 0);
            
            bool continuation = false;
            if (line[depth] == ':')
            {
                continuation = true;
                depth++;
                if (!insideContinuation)
                {
                    while (curDepth < depth - 1)
                    {
                        sb.AppendLine().Append(listTagOpen);
                        sb.AppendLine().Append(nodeTagOpen);
                        ++curDepth;
                    }
                }
            }
            else
            if (insideContinuation)
            {
                // End a previous Continuation.
                insideContinuation = false;
                sb.AppendLine().Append("</dl>");
                if (depth <= curDepth)
                {
                    sb.AppendLine();
                }
            }

            if (!insideContinuation)
            {
                if (depth <= curDepth)
                {
                    sb.Append(nodeTagClose);
                    while (curDepth > depth)
                    {
                        sb.AppendLine().Append(listTagClose);
                        sb.AppendLine().Append(nodeTagClose);
                        --curDepth;
                    }
                }
                else
                if (!continuation)
                {
                    sb.AppendLine().Append(listTagOpen);
                    while (++curDepth < depth)
                    {
                        sb.AppendLine().Append(nodeTagOpen);
                        sb.AppendLine().Append(listTagOpen);
                    }
                }
            }

            // Current Node.
            if (continuation)
            {
                if (!insideContinuation)
                {
                    insideContinuation = true;
                    if (depth == curDepth)
                    {
                        --curDepth;
                        sb.AppendLine().Append(listTagClose);
                    }

                    sb.AppendLine().Append("<dl>");
                }

                sb.AppendLine().Append("<dd>");
                sb.Append(line.Substring(depth).Trim());
                sb.Append("</dd>");
            }
            else
            {
                sb.AppendLine().Append(nodeTagOpen);
                sb.Append(line.Substring(depth).Trim());
            }

            // Get next line.
            if (sr.Peek() != marker || (line = sr.ReadLine()) == null)
            {
                return curDepth;
            }

            return ConvertListCode(line, sb, sr, curDepth);
        }

        #region representation

        private readonly char marker;
        private readonly string listTagOpen;
        private readonly string listTagClose;
        private readonly string nodeTagOpen;
        private readonly string nodeTagClose;

        private bool insideContinuation;

        #endregion // representation
    }
}

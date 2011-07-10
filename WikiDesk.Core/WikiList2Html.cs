
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

            StringBuilder sb = new StringBuilder(512);
            int curDepth = ConvertListCode(line, sb, sr, 0);
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
            {
                sb.AppendLine().Append(listTagOpen);
                while (++curDepth < depth)
                {
                    sb.AppendLine().Append(nodeTagOpen);
                    sb.AppendLine().Append(listTagOpen);
                }
            }

            // Current Node.
            sb.AppendLine().Append(nodeTagOpen);
            sb.Append(line.TrimStart(marker).Trim());

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

        #endregion // representation
    }
}

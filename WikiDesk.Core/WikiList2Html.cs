
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
            sb.AppendLine().Append(listTagOpen);

            int curDepth = ConvertListCode(line, sb, sr, 1);
            for (int i = 0; i < curDepth - 1; ++i)
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

            for (int i = 0; i < prevDepth - depth; ++i)
            {
                sb.AppendLine().Append(listTagClose);
                sb.AppendLine().Append(nodeTagClose);
                --curDepth;
            }

            for (int i = 0; i < depth - prevDepth; ++i)
            {
                sb.AppendLine().Append(nodeTagOpen);
                sb.AppendLine().Append(listTagOpen);
                ++curDepth;
            }

            // Current Node.
            sb.AppendLine().Append(nodeTagOpen);
            sb.Append(line.TrimStart(marker).Trim());

            // Get next line.
            if (sr.Peek() != marker || (line = sr.ReadLine()) == null)
            {
                sb.Append(nodeTagClose);
                return curDepth;
            }

            int newDepth = StringUtils.CountRepetition(line, 0);
            if (newDepth == depth)
            {
                sb.Append(nodeTagClose);
                return ConvertListCode(line, sb, sr, depth);
            }

            if (newDepth > depth)
            {
                // Sublist.
                sb.AppendLine().Append(listTagOpen);
                curDepth = ConvertListCode(line, sb, sr, depth + 1) - 1;
                if (curDepth <= 0)
                {
                    return 0;
                }

                sb.AppendLine().Append(listTagClose);
                sb.AppendLine();
            }

            sb.Append(nodeTagClose);

            if (newDepth > 0 && newDepth <= depth)
            {
                return ConvertListCode(line, sb, sr, curDepth);
            }

            return curDepth;
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

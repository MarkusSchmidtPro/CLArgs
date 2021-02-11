using System;
using System.Collections;
using System.Text;



namespace MSPro.CLArgs
{
    class Helper
    {
        /// <summary>
        /// Wrap a given text (insert meaningful line breaks) as a specified column.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="atColumn"></param>
        /// <returns></returns>
        public static WrappedText Wrap(string text, int atColumn = 60)
        {
            if (string.IsNullOrWhiteSpace(text)) return new WrappedText();
            StringBuilder sb = new(text.Replace(Environment.NewLine, "|"));
            sb.Replace("\r\n", "|");
            sb.Replace("\r", "|");
            sb.Replace("\n", "|");
            string[] lines = sb.ToString().Split('|');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > atColumn) lines[i] = insertLineBreaks(lines[i], atColumn);
            }

            return new WrappedText(string.Join(Environment.NewLine, lines));
        }


        /// <summary>
        ///     Wraps long line by using sb.AppendLine for each new line.
        /// </summary>
        private static string insertLineBreaks(string line, int lineBreakColumn)
        {
            char[] lineBreaker = {' ', '-'};
            StringBuilder sb = new();
            int pStartSegment = 0;
            do
            {
                int pStartFind = pStartSegment + lineBreakColumn;
                int pEnd = line.LastIndexOfAny(lineBreaker, pStartFind);

                // cannot break line BEFORE lineBreakColumn --> insert hard-break
                if (pEnd < pStartSegment) pEnd = pStartFind;

                sb.AppendLine(line.Substring(pStartSegment, pEnd - pStartSegment + 1).TrimEnd());
                pStartSegment = pEnd + 1;
            } while (pStartSegment < line.Length - lineBreakColumn);

            // add last Segment w/o NewLine
            sb.Append(line.Substring(pStartSegment, line.Length - pStartSegment));
            return sb.ToString();
        }



        public class WrappedText
        {
            public WrappedText(string allText=null, string defaultText="---")
            {
                AllText = allText ?? defaultText;

                int firstNewLine = AllText.IndexOf(Environment.NewLine, StringComparison.Ordinal);
                this.AllLines = firstNewLine < 0
                    ? new[] {AllText}
                    : AllText.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }

            public string[] AllLines { get; }
            public string AllText { get; }
        }
    }
}
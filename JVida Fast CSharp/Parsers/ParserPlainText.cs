using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JVida_Fast_CSharp.Parsers
{
    [ForExtensions(".cells")]
    public class ParserPlainText : ParserBase
    {
        private static readonly string TokenCommentStart;
        private static readonly string TokenName;
        private static readonly char AliveCell;
        private static readonly char DeadCell;

        static ParserPlainText()
        {
            TokenCommentStart = "!";
            TokenName = "!Name: ";
            AliveCell = 'O';
            DeadCell = '.';
        }

        public override Pattern Parse(StreamReader sr)
        {
            var pattern = new Pattern();
            var list = new List<char[]>();
            string line;
            int lineNumber = 0;
            while ((line = sr.ReadLine()) != null)
            {
                lineNumber++;
                line = line.Trim();
                if (line.StartsWith(TokenName, StringComparison.InvariantCultureIgnoreCase))
                {
                    pattern.Name = line.Substring(TokenName.Length);
                    continue;
                }
                if (line.StartsWith(TokenCommentStart, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                if (line.Any(c => c != AliveCell && c != DeadCell))
                {
                    throw new ParserErrorException(lineNumber, $"Line {lineNumber} contains incorrect characters");
                }
                list.Add(line.ToCharArray());
            }
            if (list.Count == 0)
            {
                throw new ParserErrorException(lineNumber, $"No cells info");
            }
            var maxX = list.Max(c => c.Length);
            var maxY = list.Count;
            pattern.Bitmap = new byte[maxX, maxY];
            for (int y = 0; y < list.Count; y++)
            {
                for (int x = 0; x < list[y].Length; x++)
                {
                    if (list[y][x] == AliveCell)
                    {
                        pattern.Bitmap[x, y] = 1;
                    }
                }
            }
            // Assume all .cells files are 23/3
            pattern.Algorithm = new Algorithm("23/3");
            return pattern;
        }
    }
}
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JVida_Fast_CSharp.Parsers
{
    internal class ParserLif106
    {
        private static readonly Regex CellRegex = new Regex(@"^(-?\d*)\s(-?\d*)$", RegexOptions.IgnoreCase);

        public Pattern Parse(StreamReader sr)
        {
            var pattern = new Pattern();
            var alive = new List<Point>();
            string line;
            int lineNumber = 0;
            while ((line = sr.ReadLine()) != null)
            {
                lineNumber++;
                line = line.Trim();
                if (line.Length == 0)
                {
                    continue;
                }
                var match = CellRegex.Match(line);
                if (!match.Success)
                {
                    throw new ParserErrorException(lineNumber, $"Line {lineNumber} contains invalid characters");
                }
                alive.Add(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            }
            var upperLeftMost = new Point(alive.Min(a => a.X), alive.Min(a => a.Y));
            var lowerRightMost = new Point(alive.Max(a => a.X), alive.Max(a => a.Y));
            var width = lowerRightMost.X - upperLeftMost.X + 1;
            var height = lowerRightMost.Y - upperLeftMost.Y + 1;
            pattern.Bitmap = new byte[width, height];
            foreach (var point in alive)
            {
                var x = point.X - upperLeftMost.X;
                var y = point.Y - upperLeftMost.Y;
                pattern.Bitmap[x, y] = 1;
            }
            pattern.Algorithm = new Algorithm("23/3");
            return pattern;
        }
    }
}
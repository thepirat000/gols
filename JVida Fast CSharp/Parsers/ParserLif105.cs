using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JVida_Fast_CSharp.Parsers
{
    internal class ParserLif105
    {
        private class Block
        {
            public Point StartPoint { get; set; }
            public int Width { get; set; }
            public int Height
            {
                get { return Lines == null ? 0 : Lines.Count; }
            }
            public List<string> Lines { get; set; }
        }

        private const char AliveCell = '*';
        private const char DeadCell = '.';
        private const string BlockStartToken = "#P";
        private const string AlgorithmToken = "#R";
        private const string AlgorithmNormalToken = "#N";
        private const string CommentToken = "#D";
        private const string NameToken = "#D Name: ";
        private static Regex AlgorithmRegex = new Regex(@"^#R\s*(\d*)/(\d*)\s*$", RegexOptions.IgnoreCase);
        private static Regex BlockPositionRegex = new Regex(@"^#P\s*(-?\d*)\s*(-?\d*)\s*$", RegexOptions.IgnoreCase);

        public Pattern Parse(StreamReader sr)
        {
            var pattern = new Pattern();
            string line;
            int lineNumber = 0;
            var blocks = new List<Block>();
            Block runningBlock = null;
            while ((line = sr.ReadLine()) != null)
            {
                lineNumber++;
                line = line.Trim();
                if (line.Length == 0 || line.StartsWith(CommentToken, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                if (line.StartsWith(NameToken, StringComparison.InvariantCultureIgnoreCase))
                {
                    pattern.Name = line.Substring(NameToken.Length);
                    continue;
                }
                if (line.StartsWith(AlgorithmNormalToken, StringComparison.InvariantCultureIgnoreCase))
                {
                    pattern.Algorithm = new Algorithm("23/3");
                    continue;
                }
                if (line.StartsWith(AlgorithmToken, StringComparison.InvariantCultureIgnoreCase))
                {
                    var match = AlgorithmRegex.Match(line);
                    if (match.Success)
                    {
                        pattern.Algorithm = new Algorithm($"{match.Groups[1].Value}/{match.Groups[2].Value}");
                    }
                    continue;
                }
                if (line.StartsWith(BlockStartToken, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (runningBlock != null)
                    {
                        if (runningBlock.Lines.Count > 0)
                        {
                            blocks.Add(runningBlock);
                        }
                        runningBlock = null;
                    }
                    var match = BlockPositionRegex.Match(line);
                    if (match.Success)
                    {
                        runningBlock = new Block()
                        {
                            StartPoint = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                            Lines = new List<string>()
                        };
                    }
                    continue;
                }
                if (line.Any(l => l != AliveCell && l != DeadCell))
                {
                    throw new ParserErrorException(lineNumber, $"Line {lineNumber} contains invalid characters");
                }
                // It's a block line
                if (runningBlock != null)
                {
                    runningBlock.Lines.Add(line);
                    runningBlock.Width = Math.Max(runningBlock.Width, line.Length);
                }
            }
            if (runningBlock != null)
            {
                if (runningBlock.Lines.Count > 0)
                {
                    blocks.Add(runningBlock);
                }
            }
            var upperLeftMost = new Point(blocks.Min(b => b.StartPoint.X), blocks.Max(b => b.StartPoint.Y));
            var lowerRightMost = new Point(blocks.Max(b => b.StartPoint.X + b.Width), blocks.Max(b => b.StartPoint.Y + b.Height));
            var width = lowerRightMost.X - upperLeftMost.X;
            var height = lowerRightMost.Y - upperLeftMost.Y;
            pattern.Bitmap = new byte[width, height];
            int x = 0, y = 0;
            foreach (var block in blocks)
            {
                x = block.StartPoint.X - upperLeftMost.X;
                y = block.StartPoint.Y - upperLeftMost.Y;
                foreach (var row in block.Lines)
                {
                    foreach (var cell in row)
                    {
                        if (cell == AliveCell)
                        {
                            pattern.Bitmap[x, y] = 1;
                        }
                        x++;
                    }
                    x = block.StartPoint.X - upperLeftMost.X;
                    y++;
                }
                
            }
            return pattern;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JVida_Fast_CSharp.Parsers
{
    [ForExtensions(".rle")]
    public class ParserRle : ParserBase
    {
        private static readonly string _tokenCommentStart;
        private static readonly string _tokenName;
        private static readonly char _aliveCell;
        private static readonly char _deadCell;
        private static readonly char _eol;
        private static readonly char _eof;
        private static readonly string _infoToken;
        private static readonly Regex _wrongDataRegex;
        private static readonly Regex _infoRegex;
        private static readonly Regex _ruleRegex;

        static ParserRle()
        {
            _tokenCommentStart = "#";
            _tokenName = "#N ";
            _aliveCell = 'o';
            _deadCell = 'b';
            _eol = '$';
            _eof = '!';
            _infoToken = "x";
            _wrongDataRegex = new Regex(@"[^0-9bo$!]", RegexOptions.IgnoreCase);
            _infoRegex = new Regex(@"x\s*=\s*(\d+)\s*,\s*y\s*=\s*(\d+)\s*,\s*rule\s*=\s*(.*)", RegexOptions.IgnoreCase);
            _ruleRegex = new Regex(@"B(\d*)/S(\d*)", RegexOptions.IgnoreCase);
        }

        private List<string> GetTokensForString(string line)
        {
            var list = new List<string>();
            string temp = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == _eol)
                {
                    if (temp.Length > 0)
                    {
                        list.Add(temp);
                    }
                    list.Add(_eol.ToString());
                    temp = "";
                    continue;
                }
                if (line[i] == _eof)
                {
                    if (temp.Length > 0)
                    {
                        list.Add(temp);
                    }
                    list.Add(_eof.ToString());
                    temp = "";
                    break;
                }
                temp += line[i];
            }
            if (temp.Length > 0)
            {
                list.Add(temp);
            }
            return list;
        }

        public override Pattern Parse(StreamReader sr)
        {
            var pattern = new Pattern();
            var tokens = new List<string>();
            string line;
            int lineNumber = 0;
            int x = 0, y = 0;
            string rule = null;
            Match match;
            while ((line = sr.ReadLine()) != null)
            {
                lineNumber++;
                line = line.Trim();
                if (line.StartsWith(_tokenName, StringComparison.InvariantCultureIgnoreCase))
                {
                    pattern.Name = line.Substring(_tokenName.Length);
                    continue;
                }
                if (line.StartsWith(_tokenCommentStart, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                if (line.StartsWith(_infoToken, StringComparison.InvariantCultureIgnoreCase))
                {
                    match = _infoRegex.Match(line);
                    x = int.Parse(match.Groups[1].Value);
                    y = int.Parse(match.Groups[2].Value);
                    rule = match.Groups[3].Value;
                    continue;
                }
                if (_wrongDataRegex.IsMatch(line))
                {
                    throw new ParserErrorException(lineNumber, $"Line {lineNumber} contains incorrect characters");
                }
                tokens.AddRange(GetTokensForString(line));
            }
            if (tokens.Count == 0)
            {
                throw new ParserErrorException(lineNumber, $"No cells info");
            }
            if (tokens.Last() != _eof.ToString())
            {
                throw new ParserErrorException(lineNumber, $"Not EOF found");
            }
            pattern.Bitmap = new byte[x, y];
            int px = 0, py = 0;
            foreach (var lineToken in GetLineTokens(tokens))
            {
                var infoList = GetInfoForToken(lineToken);
                foreach (var info in infoList)
                {
                    if (info.Alive)
                    {
                        for (int i = 0; i < info.Count; i++)
                        {
                            pattern.Bitmap[px + i, py] = 1;
                        }
                        px += info.Count;
                    }
                    else
                    {
                        if (info.Count == 0)
                        {
                            py++;
                            px = 0;
                        }
                        else
                        {
                            px += info.Count;
                        }
                    }
                }
                px = 0;
                py++;
            }
            if (string.IsNullOrWhiteSpace(rule))
            {
                rule = "B3/S23";
            }
            match = _ruleRegex.Match(rule);
            if (match.Success)
            {
                pattern.Algorithm = new Algorithm($"{match.Groups[2].Value}/{match.Groups[1].Value}");
            }
            return pattern;
        }

        private IEnumerable<string> GetLineTokens(List<string> tokens)
        {
            string temp = "";
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == _eol.ToString() || tokens[i] == _eof.ToString())
                {
                    yield return temp;
                    temp = "";
                }
                else
                {
                    temp += tokens[i];
                }
            }
        }

        private List<TokenInfo> GetInfoForToken(string token)
        {
            // i.e. token: 33bob2o10bo10b
            var list = new List<TokenInfo>();
            string counter = "";
            for (int i = 0; i < token.Length; i++)
            {
                if (char.IsNumber(token[i]))
                {
                    counter += token[i];
                }
                else
                {
                    if ((token[i] != _aliveCell) && (token[i] != _deadCell))
                    {
                        throw new ParserErrorException(0, $"Invalid token: {token[i]}");
                    }
                    int count = counter.Length == 0 ? 1 : int.Parse(counter);
                    counter = "";
                    list.Add(new TokenInfo() { Count = count, Alive = (token[i] == _aliveCell)});
                }
            }
            if (counter.Length > 0)
            {
                // there is a number at the end of the token, meaning multiple empty lines
                int count = int.Parse(counter);
                for (int i = 0; i < count-1; i++)
                {
                    list.Add(new TokenInfo() {Count = 0, Alive = false});
                }

            }
            return list;
        }

        private class TokenInfo
        {
            public int Count { get; set; }
            public bool Alive { get; set; }
        }
    }
}

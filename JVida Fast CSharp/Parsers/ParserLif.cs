using System;
using System.IO;

namespace JVida_Fast_CSharp.Parsers
{
    [ForExtensions(".lif", ".life")]
    public class ParserLif : ParserBase
    {
        private const string Identifier105 = "#Life 1.05";
        private const string Identifier106 = "#Life 1.06";

        public override Pattern Parse(StreamReader sr)
        {
            var identifier = sr.ReadLine();
            if (identifier.Trim().Equals(Identifier105, StringComparison.InvariantCultureIgnoreCase))
            {
                return new ParserLif105().Parse(sr);
            }
            if (identifier.Trim().Equals(Identifier106, StringComparison.InvariantCultureIgnoreCase))
            {
                return new ParserLif106().Parse(sr);
            }
            throw new ParserErrorException(1, $"First line should be the version identifier, but was {identifier}");
        }
    }
}
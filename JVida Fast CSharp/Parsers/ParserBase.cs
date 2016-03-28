using System.IO;

namespace JVida_Fast_CSharp.Parsers
{
    public abstract class ParserBase
    {
        public abstract Pattern Parse(StreamReader sr);
    }
}
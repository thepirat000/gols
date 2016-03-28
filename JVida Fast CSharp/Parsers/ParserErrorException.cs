using System;

namespace JVida_Fast_CSharp.Parsers
{
    public class ParserErrorException : Exception
    {
        public ParserErrorException(int line, string error)
        {
            Line = line;
            Error = error;
        }
        public int Line { get; set; }
        public string Error { get; set; }
    }
}
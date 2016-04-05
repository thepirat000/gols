using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JVida_Fast_CSharp.Parsers
{
    public static class ParserFactory
    {
        public static ParserBase GetParser(string filename)
        {
            var ext = Path.GetExtension(filename);
            var type = typeof (ParserBase).Assembly.GetTypes()
                .FirstOrDefault(t => typeof (ParserBase).IsAssignableFrom(t)
                                     && t.GetCustomAttributes(typeof(ForExtensionsAttribute), false).Any()
                                     && ((ForExtensionsAttribute)t.GetCustomAttributes(typeof(ForExtensionsAttribute), false).First()).Extensions.Contains(ext));
            if (type == null)
            {
                type = typeof (ParserPlainText);
            }
            return (ParserBase) Activator.CreateInstance(type);
        }

        public static IEnumerable<string> GetAvailableExtensions()
        {
            return typeof(ParserBase).Assembly.GetTypes()
                .Where(t => typeof(ParserBase).IsAssignableFrom(t)
                       && t.GetCustomAttributes(typeof(ForExtensionsAttribute), false).Any())
                .SelectMany(t => ((ForExtensionsAttribute)t.GetCustomAttributes(typeof(ForExtensionsAttribute), false).First()).Extensions);
        }
    }
    
}

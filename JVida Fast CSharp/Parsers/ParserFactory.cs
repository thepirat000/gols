using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JVida_Fast_CSharp.Parsers
{
    public static class ParserFactory
    {
        public static ParserBase GetParser(string filename)
        {
            var ext = Path.GetExtension(filename);
            var type = typeof (ParserBase).Assembly.GetTypes()
                .FirstOrDefault(t => typeof (ParserBase).IsAssignableFrom(t)
                                     && t.GetCustomAttribute<ForExtensionsAttribute>() != null
                                     && t.GetCustomAttribute<ForExtensionsAttribute>().Extensions.Contains(ext));
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
                       && t.GetCustomAttribute<ForExtensionsAttribute>() != null)
                .SelectMany(t => t.GetCustomAttribute<ForExtensionsAttribute>().Extensions);
        }
    }
    
}

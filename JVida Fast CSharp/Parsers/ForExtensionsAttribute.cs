using System;

namespace JVida_Fast_CSharp.Parsers
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ForExtensionsAttribute : Attribute
    {
        public ForExtensionsAttribute(params string[] extensions)
        {
            Extensions = extensions;
        }
        public string[] Extensions { get; set; }
    }
}
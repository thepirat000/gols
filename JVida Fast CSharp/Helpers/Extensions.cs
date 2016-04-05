// Thepirat 2011
// thepirat000@hotmail.com

using System.Collections.Generic;
using System.Drawing;

namespace JVida_Fast_CSharp
{
    public static class Extensions
    {
        public static void Add(this IList<Point> a, int x, int y)
        {
            a.Add(new Point(x, y));
        }
    }
}
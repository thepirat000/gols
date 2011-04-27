// Thepirat 2011
// thepirat000@hotmail.com
namespace JVida_Fast_CSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public static class Extensions
    {
        public static void Add(this IList<Point> a, int x, int y)
        {
            a.Add(new Point(x, y));
        }
    }
}
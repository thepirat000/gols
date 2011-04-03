//Thepirat 2011
//thepirat000@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace JVida_Fast_CSharp
{
    public static class Extensiones
    {
        public static void Add(this IList<Point> a, int X, int Y)
        {
            a.Add(new Point(X, Y));
        }
    }

}


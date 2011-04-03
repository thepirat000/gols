using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace JVida_Fast_CSharp
{
    public struct MiPoint
    {
        private int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public MiPoint(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    public static class Extensiones
    {
        public static void Add(this IList<MiPoint> a, int X, int Y)
        {
            a.Add(new MiPoint(X, Y));
        }
    }

}


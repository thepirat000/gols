//Thepirat 2011
//thepirat000@hotmail.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JVida_Fast_CSharp
{
    public struct Cell
    {
        #region Properties
        public Point Location { get; set; }
        public List<Point> Neighbors { get; set; }
        public byte Quantity { get; set; }
        public bool IsAlive { get; set; }
        public int Age { get; set; } 
        #endregion

        #region Constructor
        public Cell(int X, int Y)
            : this()
        {
            this.Location = new Point(X, Y);
            this.Quantity = 0;
            this.Neighbors = new List<Point>(8);
            this.Age = 0;
            this.IsAlive = false;
        } 
        #endregion
    }
}

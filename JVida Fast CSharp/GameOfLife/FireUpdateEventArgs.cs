using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JVida_Fast_CSharp
{
    public class PointSelectedEventArgs : EventArgs
    {
        public PointSelectedEventArgs(int x, int y)
        {
            Point = new Point(x, y);
        }

        public PointSelectedEventArgs(int x, int y, MouseButtons button)
        {
            Point = new Point(x, y);
            Button = button;
        }

        public Point Point { get; set; }
        public MouseButtons Button { get; set; }
    }

    public class FireUpdateEventArgs : EventArgs
    {
        public ICollection<Point> Born { get; set; }

        public ICollection<Point> Dead { get; set; }

        public FireUpdateEventArgs(ICollection<Point> born, ICollection<Point> dead)
        {
            Born = born;
            Dead = dead;
        }
    }
}

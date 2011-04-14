using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JVida_Fast_CSharp
{
    public class FireUpdateEventArgs : EventArgs
    {
        public List<Point> Born { get; set; }
        public List<Point> Dead { get; set; }

        public FireUpdateEventArgs(List<Point> Born, List<Point> Dead)
        {
            this.Born = Born;
            this.Dead = Dead;
        }
    }
}

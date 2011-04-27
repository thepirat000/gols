namespace JVida_Fast_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    public class FireUpdateEventArgs : EventArgs
    {
        public ICollection<Point> Born { get; set; }

        public ICollection<Point> Dead { get; set; }

        public FireUpdateEventArgs(ICollection<Point> Born, ICollection<Point> Dead)
        {
            this.Born = Born;
            this.Dead = Dead;
        }
    }
}

//Thepirat 2011
//thepirat000@hotmail.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JVida_Fast_CSharp
{
    /// <summary>
    /// Represents a cell (an individual) inside the universe
    /// </summary>
    public struct Cell
    {
        #region Properties
        /// <summary>
        /// Cell current location
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// Cell neighbors
        /// </summary>
        public List<Point> Neighbors { get; set; }
        /// <summary>
        /// number of living neighbors
        /// </summary>
        public byte AliveNeighbors { get; set; }
        /// <summary>
        /// Indicates if the cell is alive
        /// </summary>
        public bool IsAlive { get; set; }
        /// <summary>
        /// Indicates the current cell age
        /// </summary>
        public int Age { get; set; } 
        #endregion

        #region Constructor
        public Cell(int X, int Y)
            : this()
        {
            this.Location = new Point(X, Y);
            this.AliveNeighbors = 0;
            this.Neighbors = new List<Point>(8);
            this.Age = 0;
            this.IsAlive = false;
        } 
        #endregion
    }
}

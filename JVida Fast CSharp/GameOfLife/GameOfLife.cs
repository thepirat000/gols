// Thepirat 2011
// thepirat000@hotmail.com
// http://es.wikipedia.org/wiki/Juego_de_la_vida

// Definitions:
//  Each cell has two possible states: Alive (dot) or Dead (not drawn)
//  Each cell has 8 neighbors

// Algorithm example: Conway = 23/3:
//  A living cell still alive if and only if it has exactly 2 or 3 neighbors alive (23/)
//  A dead cell borns if and only if it has exactly 3 neighbors alive              (/3)

namespace JVida_Fast_CSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;

    public class GameOfLife
    {
        #region Fields
        private Cell[,] Matrix;

        private List<Point> Alive;

        private Algorithm Algorithm;

        private int MaximumAge;

        private double InitialOccupation;

        private bool @Stop = false;

        private delegate void MyEventHandler(List<Point> Born, List<Point> Dead);

        private MyEventHandler MyFireUpdater; 
        #endregion
        
        #region Events
        public event FireUpdateEventHandler FireUpdate;

        public delegate void FireUpdateEventHandler(object sender, FireUpdateEventArgs e);

        private void OnFireUpdate(ICollection<Point> Born, ICollection<Point> Dead)
        {
            if (this.FireUpdate != null)
            {
                this.FireUpdate(this, new FireUpdateEventArgs(Born, Dead));
            }
        } 
        #endregion

        #region Properties
        public string AlgorithmSymbol
        {
            get { return this.Algorithm.Symbol; }
        } 
        #endregion

        #region Constructor
        public GameOfLife(int maxX, int maxY, string AlgorithmSymbol, int MaximumAge, double InitialOccupation)
        {
            this.MyFireUpdater = new MyEventHandler(this.OnFireUpdate);
            this.MaximumAge = MaximumAge;
            this.Algorithm = new Algorithm(AlgorithmSymbol);
            this.InitialOccupation = InitialOccupation;
            this.Init(maxX, maxY);
        } 
        #endregion

        #region Private Methods
        private void Init(int maxX, int maxY)
        {
            this.Matrix = new Cell[maxX, maxY];
            for (int x = 0; x <= maxX - 1; x++)
            {
                for (int y = 0; y <= maxY - 1; y++)
                {
                    Cell cell = new Cell(x, y);
                    this.Matrix[x, y] = cell;
                }
            }
            for (int x = 0; x <= maxX - 1; x++)
            {
                for (int y = 0; y <= maxY - 1; y++)
                {
                    Cell cell = this.Matrix[x, y];
                    cell.Neighbors.Add(prev(maxX, x), prev(maxY, y));  // Upper Left
                    cell.Neighbors.Add(x, prev(maxY, y));               // Upper Center
                    cell.Neighbors.Add(next(maxX, x), prev(maxY, y));  // Upper Right
                    cell.Neighbors.Add(prev(maxX, x), y);               // Middle Left
                    cell.Neighbors.Add(next(maxX, x), y);               // Middle Right
                    cell.Neighbors.Add(prev(maxX, x), next(maxY, y));  // Lower Left
                    cell.Neighbors.Add(x, next(maxY, y));               // Lower Center
                    cell.Neighbors.Add(next(maxX, x), next(maxY, y));  // Lower Right
                }
            }
            this.Alive = new List<Point>();
        }

        private static int next(int max, int i)
        {
            return i == max - 1 ? 0 : i + 1;
        }
        private static int prev(int max, int i)
        {
            return i == 0 ? max - 1 : i - 1;
        }

        private void SetAlive(List<Point> alive)
        {
            foreach (Point item in alive)
            {
                this.Matrix[item.X, item.Y].IsAlive = true;
                this.Alive.Add(item);
            }
        }

        /// <summary>
        /// Make a step in the game
        /// </summary>
        private void GameStep(ref List<Point> Born, ref List<Point> Dead)
        {
            // Set the neighbor alive count to 0, and increment age
            foreach (Point item in this.Alive.AsParallel())
            {
                this.Matrix[item.X, item.Y].AliveNeighbors = 0;
                this.Matrix[item.X, item.Y].Age++;
                foreach (Point neighbor in this.Matrix[item.X, item.Y].Neighbors)
                {
                    this.Matrix[neighbor.X, neighbor.Y].AliveNeighbors = 0;
                }
            }

            // Compute neighbors alive in each alive cell
            // Add 1 to each neighbor of the living cells
            foreach (Point item in this.Alive.AsParallel())
            {
                foreach (Point neighbor in this.Matrix[item.X, item.Y].Neighbors)
                {
                    this.Matrix[neighbor.X, neighbor.Y].AliveNeighbors++;
                }
            }

            Born = this.GetBorn();
            List<Point> survivors = this.GetSurvivors();
            Dead = this.GetDeads();
            foreach (Point m in Dead)
            {
                this.Matrix[m.X, m.Y].IsAlive = false;
            }
            this.Alive = Born.Concat(survivors).ToList();
        }

        private List<Point> GetBorn()
        {
            List<Point> ret = new List<Point>();

            // Born
            foreach (Point item in this.Alive.AsParallel())
            {
                foreach (Point neighbor in this.Matrix[item.X, item.Y].Neighbors)
                {
                    Cell neighborCell = this.Matrix[neighbor.X, neighbor.Y];
                    if (!neighborCell.IsAlive)
                    {
                        bool willBorn = this.Algorithm.NextState(0, neighborCell.AliveNeighbors);
                        if (willBorn)
                        {
                            this.Matrix[neighbor.X, neighbor.Y].IsAlive = true;
                            this.Matrix[neighbor.X, neighbor.Y].Age = 0;
                            ret.Add(neighborCell.Location);
                        }
                    }
                }
            }
            return ret;
        }

        private List<Point> GetSurvivors()
        {
            List<Point> ret = new List<Point>();

            // Survives
            foreach (Point item in this.Alive.AsParallel())
            {
                Cell c1 = this.Matrix[item.X, item.Y];
                bool survives = this.Algorithm.NextState(1, c1.AliveNeighbors);
                if (survives)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        private List<Point> GetDeads()
        {
            List<Point> ret = new List<Point>();
            foreach (Point item in this.Alive.AsParallel())
            {
                Cell c1 = this.Matrix[item.X, item.Y];
                if (c1.Age >= this.MaximumAge)
                {
                    ret.Add(item);
                    continue;
                }
                bool alive = this.Algorithm.NextState(1, c1.AliveNeighbors);
                if (!alive)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        private void Randomize()
        {
            Random rnd = new Random();
            List<Point> alive = new List<Point>();
            for (int i = 0; i <= this.Matrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= this.Matrix.GetLength(1) - 1; j++)
                {
                    if (rnd.NextDouble() > (1 - this.InitialOccupation) &&
                        i > this.Matrix.GetLength(0) * 2 / 5 &&
                        i < this.Matrix.GetLength(0) * 3 / 5 &&
                        j > this.Matrix.GetLength(1) * 2 / 5 &&
                        j < this.Matrix.GetLength(1) * 3 / 5)
                    {
                        alive.Add(i, j);
                    }
                }
            }
            this.SetAlive(alive);
            List<Point> deads = new List<Point>();
            this.OnFireUpdate(alive, deads);
        } 
        #endregion

        #region Public Methods
        public void Play()
        {
            this.Randomize();
            List<Point> born = null;
            List<Point> dead = null;
            while (!@Stop)
            {
                this.GameStep(ref born, ref dead);
                this.MyFireUpdater.Invoke(born, dead);
                System.Threading.Thread.Sleep(1);
            }
        } 
        #endregion
    }
}

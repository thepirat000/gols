// Thepirat 2011 (revised on 2016)
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
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class GameOfLife
    {
        #region Fields
        public Cell[,] Matrix { get; private set; }

        private List<Point> Alive;

        private int MaximumAge;

        private double InitialOccupation;
        private double InitialMargin;

        private bool @Stop = false;

        public bool Paused { get; set; }
        // To indicate how many algorithm steps to do when paused (0 means, pause forever)
        public int DoSteps { get; set; }

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
        public Algorithm Algorithm { get; set; }

        public string AlgorithmSymbol
        {
            get { return this.Algorithm.Symbol; }
        } 
        #endregion

        #region Constructor
        public GameOfLife(int maxX, int maxY, string AlgorithmSymbol, int MaximumAge, double InitialOccupation, double initialMargin, bool wrapAround)
        {
            this.MyFireUpdater = new MyEventHandler(this.OnFireUpdate);
            this.MaximumAge = MaximumAge;
            this.Algorithm = new Algorithm(AlgorithmSymbol);
            this.InitialOccupation = InitialOccupation;
            this.Init(maxX, maxY, wrapAround);
            this.InitialMargin = initialMargin;
        }
        #endregion

        #region Private Methods
        private void Init(int maxX, int maxY, bool wrapAround)
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
                    bool isFirstColumn = x == 0;
                    bool isLastColumn = x == maxX;
                    bool isFirstRow = y == 0;
                    bool isLastRow = y == maxY;
                    if (wrapAround || (!isFirstColumn && !isFirstRow))
                    {
                        cell.Neighbors.Add(prev(maxX, x), prev(maxY, y));  // Upper Left
                    }
                    if (wrapAround || (!isFirstRow))
                    {
                        cell.Neighbors.Add(x, prev(maxY, y));               // Upper Center
                    }
                    if (wrapAround || (!isFirstRow && !isLastColumn))
                    {
                        cell.Neighbors.Add(next(maxX, x), prev(maxY, y));  // Upper Right
                    }
                    if (wrapAround || (!isFirstColumn && !isLastRow))
                    {
                        cell.Neighbors.Add(prev(maxX, x), y);               // Middle Left
                    }
                    if (wrapAround || (!isLastColumn))
                    {
                        cell.Neighbors.Add(next(maxX, x), y);               // Middle Right
                    }
                    if (wrapAround || (!isFirstRow && !isLastRow))
                    {
                        cell.Neighbors.Add(prev(maxX, x), next(maxY, y));  // Lower Left
                    }
                    if (wrapAround || (!isFirstColumn && !isLastRow))
                    {
                        cell.Neighbors.Add(x, next(maxY, y)); // Lower Center
                    }
                    if (wrapAround || (!isLastRow && !isLastColumn))
                    {
                        cell.Neighbors.Add(next(maxX, x), next(maxY, y)); // Lower Right
                    }
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
            foreach (Point item in this.Alive)
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
            foreach (Point item in this.Alive)
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
            foreach (Point item in this.Alive)
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
            foreach (Point item in this.Alive)
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
            foreach (Point item in this.Alive)
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

        public void Randomize()
        {
            double margin = this.InitialMargin;
            Random rnd = new Random();
            List<Point> alive = new List<Point>();
            for (int i = 0; i <= this.Matrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= this.Matrix.GetLength(1) - 1; j++)
                {
                    if (rnd.NextDouble() > (1 - this.InitialOccupation) &&
                        i >= this.Matrix.GetLength(0) * margin &&
                        i <= this.Matrix.GetLength(0) * (1 - margin) &&
                        j >= this.Matrix.GetLength(1) * margin &&
                        j <= this.Matrix.GetLength(1) * (1 - margin))
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
        public void TogglePause()
        {
            Paused = !Paused;
        }

        public void Pause()
        {
            Paused = true;
        }
        public void Resume()
        {
            Paused = false;
        }

        public void Clear()
        {
            var wasPaused = Paused;
            Paused = true;
            System.Threading.Thread.Sleep(1);
            for (int x = 0; x < this.Matrix.GetLength(0); x++)
            {
                for (int y = 0; y < this.Matrix.GetLength(1); y++)
                {
                    this.Matrix[x, y].Age = 0;
                    this.Matrix[x, y].AliveNeighbors = 0;
                    this.Matrix[x, y].IsAlive = false;
                }
            }
            MyFireUpdater.Invoke(new List<Point>(), this.Alive);
            this.Alive = new List<Point>();
            Paused = wasPaused;
        }

        public void Plot(Point offset, Cell[,] matrix)
        {
            byte[,] byteMatrix = new byte[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    byteMatrix[i, j] = (byte)(matrix[i, j].IsAlive ? 1 : 0);
            Plot(offset, byteMatrix);
        }
        /// <summary>
        /// Plots the specified glyph on the given offset.
        /// </summary>
        public void Plot(Point offset, byte[,] matrix)
        {
            var wasPaused = Paused;
            Paused = true;
            System.Threading.Thread.Sleep(5);
            var dead = new List<Point>();
            var born = new List<Point>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    var x = offset.X + i;
                    var y = offset.Y + j;
                    if (x >= Matrix.GetLength(0) || y >= Matrix.GetLength(1))
                    {
                        continue;
                    }
                    this.Matrix[x, y].Age = 0;
                    var wasAlive = this.Matrix[x, y].IsAlive;
                    this.Matrix[x, y].IsAlive = matrix[i,j] > 0;
                    if (wasAlive != this.Matrix[x, y].IsAlive)
                    {
                        if (wasAlive)
                        {
                            // was alive and is dead
                            foreach (Point neighbor in this.Matrix[x, y].Neighbors)
                            {
                                Cell neighborCell = this.Matrix[neighbor.X, neighbor.Y];
                                neighborCell.AliveNeighbors--;
                            }
                            this.Alive.Remove(new Point(x, y));
                            dead.Add(new Point(x, y));
                        }
                        else
                        {
                            // was dead and is alive
                            foreach (Point neighbor in this.Matrix[x, y].Neighbors)
                            {
                                Cell neighborCell = this.Matrix[neighbor.X, neighbor.Y];
                                neighborCell.AliveNeighbors++;
                            }
                            this.Alive.Add(new Point(x, y));
                            born.Add(new Point(x, y));
                        }
                    }
                }
            }
            MyFireUpdater.Invoke(born, dead);
            Paused = wasPaused;
        }

        public void Play()
        {
            List<Point> born = null;
            List<Point> dead = null;
            while (!@Stop)
            {
                this.GameStep(ref born, ref dead);
                this.MyFireUpdater.Invoke(born, dead);
                if (DoSteps > 0)
                {
                    DoSteps--;
                }
                System.Threading.Thread.Sleep(1);
                while (Paused && DoSteps <= 0)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
        } 
        #endregion
    }
}

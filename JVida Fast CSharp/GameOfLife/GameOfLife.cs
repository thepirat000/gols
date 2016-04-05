// Thepirat 2011 (revised on 2016)
// thepirat000@hotmail.com
// http://es.wikipedia.org/wiki/Juego_de_la_vida

// Definitions:
//  Each cell has two possible states: Alive (dot) or Dead (not drawn)
//  Each cell has 8 neighbors

// Algorithm example: Conway = 23/3:
//  A living cell still alive if and only if it has exactly 2 or 3 neighbors alive (23/)
//  A dead cell borns if and only if it has exactly 3 neighbors alive              (/3)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace JVida_Fast_CSharp
{
    public class GameOfLife
    {
        #region Fields
        public Cell[,] Matrix { get; private set; }
        private List<Point> _alive;
        private readonly int _maximumAge;
        private readonly double _initialOccupation;
        private readonly double _initialMargin;
        private readonly bool _stop = false;
        private bool _userPaused;
        private bool _reallyPaused;
        public bool Paused { get { return _reallyPaused; } set { _userPaused = value; } }
        // To indicate how many algorithm steps to do when paused (0 means, pause forever)
        public int DoSteps { get; set; }

        private delegate void MyEventHandler(List<Point> born, List<Point> dead);

        private readonly MyEventHandler _myFireUpdater; 
        #endregion
        
        #region Events
        public event FireUpdateEventHandler FireUpdate;

        public delegate void FireUpdateEventHandler(object sender, FireUpdateEventArgs e);

        private void OnFireUpdate(ICollection<Point> born, ICollection<Point> dead)
        {
            if (FireUpdate != null)
            {
                FireUpdate(this, new FireUpdateEventArgs(born, dead));
            }
        }
        #endregion

        #region Properties
        public Algorithm Algorithm { get; set; }

        public string AlgorithmSymbol
        {
            get { return Algorithm.Symbol; }
        } 
        #endregion

        #region Constructor
        public GameOfLife(int maxX, int maxY, string algorithmSymbol, int maximumAge, double initialOccupation, double initialMargin, bool wrapAround)
        {
            _myFireUpdater = OnFireUpdate;
            _maximumAge = maximumAge;
            Algorithm = new Algorithm(algorithmSymbol);
            _initialOccupation = initialOccupation;
            Init(maxX, maxY, wrapAround);
            _initialMargin = initialMargin;
        }
        #endregion

        #region Private Methods
        private void Init(int maxX, int maxY, bool wrapAround)
        {
            Matrix = new Cell[maxX, maxY];
            for (int x = 0; x <= maxX - 1; x++)
            {
                for (int y = 0; y <= maxY - 1; y++)
                {
                    Cell cell = new Cell(x, y);
                    Matrix[x, y] = cell;
                }
            }
            for (int x = 0; x <= maxX - 1; x++)
            {
                for (int y = 0; y <= maxY - 1; y++)
                {
                    Cell cell = Matrix[x, y];
                    bool isFirstColumn = x == 0;
                    bool isLastColumn = x == maxX;
                    bool isFirstRow = y == 0;
                    bool isLastRow = y == maxY;
                    if (wrapAround || (!isFirstColumn && !isFirstRow))
                    {
                        cell.Neighbors.Add(Prev(maxX, x), Prev(maxY, y));  // Upper Left
                    }
                    if (wrapAround || (!isFirstRow))
                    {
                        cell.Neighbors.Add(x, Prev(maxY, y));               // Upper Center
                    }
                    if (wrapAround || (!isFirstRow && !isLastColumn))
                    {
                        cell.Neighbors.Add(Next(maxX, x), Prev(maxY, y));  // Upper Right
                    }
                    if (wrapAround || (!isFirstColumn && !isLastRow))
                    {
                        cell.Neighbors.Add(Prev(maxX, x), y);               // Middle Left
                    }
                    if (wrapAround || (!isLastColumn))
                    {
                        cell.Neighbors.Add(Next(maxX, x), y);               // Middle Right
                    }
                    if (wrapAround || (!isFirstRow && !isLastRow))
                    {
                        cell.Neighbors.Add(Prev(maxX, x), Next(maxY, y));  // Lower Left
                    }
                    if (wrapAround || (!isFirstColumn && !isLastRow))
                    {
                        cell.Neighbors.Add(x, Next(maxY, y)); // Lower Center
                    }
                    if (wrapAround || (!isLastRow && !isLastColumn))
                    {
                        cell.Neighbors.Add(Next(maxX, x), Next(maxY, y)); // Lower Right
                    }
                }
            }
            _alive = new List<Point>();
        }

        private static int Next(int max, int i)
        {
            return i == max - 1 ? 0 : i + 1;
        }
        private static int Prev(int max, int i)
        {
            return i == 0 ? max - 1 : i - 1;
        }

        private void SetAlive(List<Point> alive)
        {
            foreach (Point item in alive)
            {
                Matrix[item.X, item.Y].IsAlive = true;
                _alive.Add(item);
            }
        }

        /// <summary>
        /// Make a step in the game
        /// </summary>
        private void GameStep(ref List<Point> born, ref List<Point> dead)
        {
            // Set the neighbor alive count to 0, and increment age
            foreach (Point item in _alive)
            {
                Matrix[item.X, item.Y].AliveNeighbors = 0;
                Matrix[item.X, item.Y].Age++;
                foreach (Point neighbor in Matrix[item.X, item.Y].Neighbors)
                {
                    Matrix[neighbor.X, neighbor.Y].AliveNeighbors = 0;
                }
            }

            // Compute neighbors alive in each alive cell
            // Add 1 to each neighbor of the living cells
            foreach (Point item in _alive)
            {
                foreach (Point neighbor in Matrix[item.X, item.Y].Neighbors)
                {
                    Matrix[neighbor.X, neighbor.Y].AliveNeighbors++;
                }
            }

            born = GetBorn();
            List<Point> survivors = GetSurvivors();
            dead = GetDeads();
            foreach (Point m in dead)
            {
                Matrix[m.X, m.Y].IsAlive = false;
            }
            _alive = born.Concat(survivors).ToList();
        }

        private List<Point> GetBorn()
        {
            List<Point> ret = new List<Point>();

            // Born
            foreach (Point item in _alive)
            {
                foreach (Point neighbor in Matrix[item.X, item.Y].Neighbors)
                {
                    Cell neighborCell = Matrix[neighbor.X, neighbor.Y];
                    if (!neighborCell.IsAlive)
                    {
                        bool willBorn = Algorithm.NextState(0, neighborCell.AliveNeighbors);
                        if (willBorn)
                        {
                            Matrix[neighbor.X, neighbor.Y].IsAlive = true;
                            Matrix[neighbor.X, neighbor.Y].Age = 0;
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
            foreach (Point item in _alive)
            {
                Cell c1 = Matrix[item.X, item.Y];
                bool survives = Algorithm.NextState(1, c1.AliveNeighbors);
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
            foreach (Point item in _alive)
            {
                Cell c1 = Matrix[item.X, item.Y];
                if (c1.Age >= _maximumAge)
                {
                    ret.Add(item);
                    continue;
                }
                bool alive = Algorithm.NextState(1, c1.AliveNeighbors);
                if (!alive)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        public void Randomize()
        {
            double margin = _initialMargin;
            Random rnd = new Random();
            List<Point> alive = new List<Point>();
            for (int i = 0; i <= Matrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= Matrix.GetLength(1) - 1; j++)
                {
                    if (rnd.NextDouble() > (1 - _initialOccupation) &&
                        i >= Matrix.GetLength(0) * margin &&
                        i <= Matrix.GetLength(0) * (1 - margin) &&
                        j >= Matrix.GetLength(1) * margin &&
                        j <= Matrix.GetLength(1) * (1 - margin))
                    {
                        alive.Add(i, j);
                    }
                }
            }
            SetAlive(alive);
            List<Point> deads = new List<Point>();
            OnFireUpdate(alive, deads);
        }
        #endregion

        #region Public Methods
        public void TogglePause()
        {
            _userPaused = !Paused;
        }

        public void Pause()
        {
            _userPaused = true;
        }
        public void Resume()
        {
            _userPaused = false;
        }

        public void Clear()
        {
            var wasPaused = _reallyPaused;
            _userPaused = true;
            Thread.Sleep(1);
            for (int x = 0; x < Matrix.GetLength(0); x++)
            {
                for (int y = 0; y < Matrix.GetLength(1); y++)
                {
                    Matrix[x, y].Age = 0;
                    Matrix[x, y].AliveNeighbors = 0;
                    Matrix[x, y].IsAlive = false;
                }
            }
            _myFireUpdater.Invoke(new List<Point>(), _alive);
            _alive = new List<Point>();
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
                    Matrix[x, y].Age = 0;
                    var wasAlive = Matrix[x, y].IsAlive;
                    Matrix[x, y].IsAlive = matrix[i,j] > 0;
                    if (wasAlive != Matrix[x, y].IsAlive)
                    {
                        if (wasAlive)
                        {
                            // was alive and is dead
                            foreach (Point neighbor in Matrix[x, y].Neighbors)
                            {
                                Cell neighborCell = Matrix[neighbor.X, neighbor.Y];
                                neighborCell.AliveNeighbors--;
                            }
                            _alive.Remove(new Point(x, y));
                            dead.Add(new Point(x, y));
                        }
                        else
                        {
                            // was dead and is alive
                            foreach (Point neighbor in Matrix[x, y].Neighbors)
                            {
                                Cell neighborCell = Matrix[neighbor.X, neighbor.Y];
                                neighborCell.AliveNeighbors++;
                            }
                            _alive.Add(new Point(x, y));
                            born.Add(new Point(x, y));
                        }
                    }
                }
            }
            _myFireUpdater.Invoke(born, dead);
        }

        public void Play()
        {
            List<Point> born = null;
            List<Point> dead = null;
            while (!_stop)
            {
                if (!_userPaused || DoSteps > 0)
                {
                    _reallyPaused = false;
                    GameStep(ref born, ref dead);
                    _myFireUpdater.Invoke(born, dead);
                    if (DoSteps > 0)
                    {
                        DoSteps--;
                    }
                }
                Thread.Sleep(1);
                while (_userPaused && DoSteps <= 0)
                {
                    _reallyPaused = true;
                    Thread.Sleep(1);
                }
            }
        } 
        #endregion
    }
}

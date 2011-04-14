//Thepirat 2011
//thepirat000@hotmail.com
//http://es.wikipedia.org/wiki/Juego_de_la_vida
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Drawing;

//Definitions:
// Each cell has two possible states: Alive (dot) or Dead (not drawn)
// Each cell has 8 neighbors

//Algorithm example: Conway = 23/3:
// A living cell still alive if and only if it has exactly 2 or 3 neighbors alive (23/)
// A dead cell borns if and only if it has exactly 3 neighbors alive              (/3)

namespace JVida_Fast_CSharp
{
    public class GameOfLife
    {
        #region Events
        public event FireUpdateEventHandler FireUpdate;
        public delegate void FireUpdateEventHandler(object sender, FireUpdateEventArgs e);
        private void OnFireUpdate(List<Point> Born, List<Point> Dead)
        {
            if (FireUpdate != null)
            {
                FireUpdate(this, new FireUpdateEventArgs(Born, Dead));
            }
        } 
        #endregion

        #region Fields
        private Cell[,] Matrix;
        private List<Point> Alive;
        private Algorithm algorithm;
        private int MaximumAge;
        private double InitialOccupation;
        private bool @stop = false;
        private delegate void MyEventHandler(List<Point> Born, List<Point> Dead);
        private MyEventHandler MyFireUpdater; 
        #endregion

        #region Properties
        public string Algorithm
        {
            get { return algorithm.Symbol; }
        } 
        #endregion

        #region Constructor
        public GameOfLife(int maxX, int maxY, string AlgorithmSymbol, int MaximumAge, double InitialOccupation)
        {
            MyFireUpdater = new MyEventHandler(OnFireUpdate);
            this.MaximumAge = MaximumAge;
            this.algorithm = new Algorithm(AlgorithmSymbol);
            this.InitialOccupation = InitialOccupation;
            Init(maxX, maxY);
        } 
        #endregion

        #region Private Methods
        private void Init(int maxX, int maxY)
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
                    //vecinos
                    Cell cell = Matrix[x, y];
                    cell.Neighbors.Add(_prev(maxX, x), _prev(maxY, y));  //Upper Left
                    cell.Neighbors.Add(x, _prev(maxY, y));               //Upper Center
                    cell.Neighbors.Add(_next(maxX, x), _prev(maxY, y));  //Upper Right
                    cell.Neighbors.Add(_prev(maxX, x), y);               //Middle Left
                    cell.Neighbors.Add(_next(maxX, x), y);               //Middle Right
                    cell.Neighbors.Add(_prev(maxX, x), _next(maxY, y));  //Lower Left
                    cell.Neighbors.Add(x, _next(maxY, y));               //Lower Center
                    cell.Neighbors.Add(_next(maxX, x), _next(maxY, y));  //Lower Right
                }
            }
            Alive = new List<Point>();
        }

        private int _next(int max, int i)
        {
            return i == max - 1 ? 0 : i + 1;
        }
        private int _prev(int max, int i)
        {
            return i == 0 ? max - 1 : i - 1;
        }

        private void SetAlive(List<Point> alive)
        {
            foreach (Point item in alive)
            {
                Matrix[item.X, item.Y].IsAlive = true;
                Alive.Add(item);
            }
        }

        /// <summary>
        /// Make a step in the game
        /// </summary>
        private void GameStep(ref List<Point> Born, ref List<Point> Dead)
        {
            //Set the neighbor alive count to 0, and increment age
            foreach (Point item in Alive.AsParallel())
            {
                Matrix[item.X, item.Y].AliveNeighbors = 0;
                Matrix[item.X, item.Y].Age++;
                foreach (Point neighbor in Matrix[item.X, item.Y].Neighbors)
                {
                    Matrix[neighbor.X, neighbor.Y].AliveNeighbors = 0;
                }
            }
            //Compute neighbors alive in each alive cell
            //Add 1 to each neighbor of the living cells
            foreach (Point item in Alive.AsParallel())
            {
                foreach (Point neighbor in Matrix[item.X, item.Y].Neighbors)
                {
                    Matrix[neighbor.X, neighbor.Y].AliveNeighbors++;
                }
            }

            Born = GetBorn();
            List<Point> survivors = GetSurvivors();
            Dead = GetDeads();
            foreach (Point m in Dead)
            {
                Matrix[m.X, m.Y].IsAlive = false;
            }
            Alive = Born.Concat(survivors).ToList();
        }

        private List<Point> GetBorn()
        {
            List<Point> ret = new List<Point>();
            //Born
            foreach (Point item in Alive.AsParallel())
            {
                foreach (Point neighbor in Matrix[item.X, item.Y].Neighbors)
                {
                    Cell neighborCell = Matrix[neighbor.X, neighbor.Y];
                    if (!neighborCell.IsAlive)
                    {
                        bool willBorn = algorithm.NextState(0, neighborCell.AliveNeighbors);
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
            //Survives
            foreach (Point item in Alive.AsParallel())
            {
                Cell c1 = Matrix[item.X, item.Y];
                bool survives = algorithm.NextState(1, c1.AliveNeighbors);
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
            foreach (Point item in Alive.AsParallel())
            {
                Cell c1 = Matrix[item.X, item.Y];
                if (c1.Age >= MaximumAge)
                {
                    ret.Add(item);
                    continue;
                }
                bool alive = algorithm.NextState(1, c1.AliveNeighbors);
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
            for (int i = 0; i <= Matrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= Matrix.GetLength(1) - 1; j++)
                {
                    if (rnd.NextDouble() > (1 - this.InitialOccupation) &&
                        i > Matrix.GetLength(0) * 2 / 5 &&
                        i < Matrix.GetLength(0) * 3 / 5 &&
                        j > Matrix.GetLength(1) * 2 / 5 &&
                        j < Matrix.GetLength(1) * 3 / 5)
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
        public void Play()
        {
            Randomize();
            List<Point> born = null;
            List<Point> dead = null;
            while (!@stop)
            {
                GameStep(ref born, ref dead);
                MyFireUpdater.Invoke(born, dead);
                System.Threading.Thread.Sleep(1);
            }
        } 
        #endregion
    }
}

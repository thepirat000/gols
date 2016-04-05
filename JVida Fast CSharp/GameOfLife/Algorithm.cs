// Thepirat 2011
// thepirat000@hotmail.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace JVida_Fast_CSharp
{
    /// <summary>
    /// Represents an algorithm of game of life
    /// </summary>
    public struct Algorithm
    {
        #region Fields
        private static readonly Random Rnd = new Random();

        // Values ​​that indicate when a cell keeps alive (number of alive neighbors)
        private readonly ICollection<byte> _stillAliveIf;

        // Values ​​that indicate when a cell born (number of alive neighbors)
        private readonly ICollection<byte> _bornIf;
        #endregion

        #region Constructor
        // Symbol: "nnn/nnn"
        public Algorithm(string symbol)
        {
            string[] ns = symbol.Split('/');
            if (ns.Length != 2)
            {
                throw new ArgumentException("Incorrect Format");
            }
            _stillAliveIf = new List<byte>();
            foreach (char c in ns[0])
            {
                _stillAliveIf.Add(Convert.ToByte(char.GetNumericValue(c)));
            }
            _bornIf = new List<byte>();
            foreach (char c in ns[1])
            {
                _bornIf.Add(Convert.ToByte(char.GetNumericValue(c)));
            }
        }

        public Algorithm(ICollection<byte> stillAliveIf, ICollection<byte> bornIf)
        {
            _stillAliveIf = stillAliveIf;
            _bornIf = bornIf;
        }
        #endregion

        #region Properties
        public string Symbol
        {
            get
            {
                string s = string.Empty;
                foreach (byte i in _stillAliveIf)
                {
                    s += i.ToString();
                }
                s += "/";
                foreach (byte i in _bornIf)
                {
                    s += i.ToString();
                }
                return s;
            }
        }
        #endregion

        #region Public methods
        public bool NextState(byte currentState, byte aliveNeighbors)
        {
            // Is dead, check if it born
            if (currentState == 0 && _bornIf.Contains(aliveNeighbors))
            {
                return true;
            }

            // Is alive, check if it keeps living or dies
            if (currentState > 0 && _stillAliveIf.Contains(aliveNeighbors))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the symbol of a random algorithm
        /// </summary>
        public static string GetRandomAlgorithm()
        {
            int qtyD = Rnd.Next(0, 9);
            int qty_D = Rnd.Next(1, 9);
            var qryD = (from i in Enumerable.Range(1, 8)
                         select new {i, order = Rnd.NextDouble() })
                    .OrderBy(n => n.order)
                    .Take(qtyD)
                    .OrderBy(n => n.i)
                    .Select(n => n.i);
            var qry_D = (from i in Enumerable.Range(1, 8)
                         select new {i, order = Rnd.NextDouble() })
                    .OrderBy(n => n.order)
                    .Take(qty_D)
                    .OrderBy(n => n.i)
                    .Select(n => n.i);
            string res = string.Empty;
            foreach (int i in qryD)
            {
                res += i.ToString();
            }
            res += "/";
            foreach (int i in qry_D)
            {
                res += i.ToString();
            }
            return res;
        }
        #endregion

    }
}
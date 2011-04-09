//Thepirat 2011
//thepirat000@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace JVida_Fast_CSharp
{
    /// <summary>
    /// Represents an algorithm of game of life
    /// </summary>
    public struct Algorithm
    {
        #region Fields
        //Values ​​that indicate when a cell keeps alive (number of alive neighbors)
        private List<byte> StillAliveIf;
        //Values ​​that indicate when a cell born (number of alive neighbors)
        private List<byte> BornIf;
        #endregion

        #region Constructor
        //Symbol: "nnn/nnn"
        public Algorithm(string Symbol)
        {
            string[] ns = Symbol.Split('/');
            if (ns.Length != 2)
            {
                throw new ArgumentException("Incorrect Format");
            }
            StillAliveIf = new List<byte>();
            foreach (char c in ns[0])
            {
                StillAliveIf.Add(Convert.ToByte(Char.GetNumericValue(c)));
            }
            BornIf = new List<byte>();
            foreach (char c in ns[1])
            {
                BornIf.Add(Convert.ToByte(Char.GetNumericValue(c)));
            }
        }

        public Algorithm(List<byte> StillAliveIf, List<byte> BornIf)
        {
            this.StillAliveIf = StillAliveIf;
            this.BornIf = BornIf;
        }
        #endregion

        #region Properties
        public string Symbol
        {
            get
            {
                string s = "";
                foreach (byte i in StillAliveIf)
                {
                    s += i.ToString();
                }
                s += "/";
                foreach (byte i in BornIf)
                {
                    s += i.ToString();
                }
                return s;
            }
        }
        #endregion

        #region Public methods
        public bool NextState(byte CurrentState, byte AliveNeighbors)
        {
            //Está muerta, ver si puede nacer
            if (CurrentState == 0 && BornIf.Contains(AliveNeighbors))
            {
                return true;
            }
            //Está viva, ver si puede seguir viva
            if (CurrentState > 0 && StillAliveIf.Contains(AliveNeighbors))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}


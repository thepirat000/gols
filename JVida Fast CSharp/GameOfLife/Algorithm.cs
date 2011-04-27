﻿// Thepirat 2011
// thepirat000@hotmail.com
namespace JVida_Fast_CSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;

    /// <summary>
    /// Represents an algorithm of game of life
    /// </summary>
    public struct Algorithm
    {
        #region Fields
        // Values ​​that indicate when a cell keeps alive (number of alive neighbors)
        private ICollection<byte> StillAliveIf;

        // Values ​​that indicate when a cell born (number of alive neighbors)
        private ICollection<byte> BornIf;
        #endregion

        #region Constructor
        // Symbol: "nnn/nnn"
        public Algorithm(string Symbol)
        {
            string[] ns = Symbol.Split('/');
            if (ns.Length != 2)
            {
                throw new ArgumentException("Incorrect Format");
            }
            this.StillAliveIf = new List<byte>();
            foreach (char c in ns[0])
            {
                this.StillAliveIf.Add(Convert.ToByte(char.GetNumericValue(c)));
            }
            this.BornIf = new List<byte>();
            foreach (char c in ns[1])
            {
                this.BornIf.Add(Convert.ToByte(char.GetNumericValue(c)));
            }
        }

        public Algorithm(ICollection<byte> StillAliveIf, ICollection<byte> BornIf)
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
                string s = string.Empty;
                foreach (byte i in this.StillAliveIf)
                {
                    s += i.ToString();
                }
                s += "/";
                foreach (byte i in this.BornIf)
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
            // Is dead, check if it born
            if (CurrentState == 0 && this.BornIf.Contains(AliveNeighbors))
            {
                return true;
            }

            // Is alive, check if it keeps living or dies
            if (CurrentState > 0 && this.StillAliveIf.Contains(AliveNeighbors))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
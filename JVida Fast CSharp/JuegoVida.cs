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

//Definición:
//Cada célula tiene dos estados: viva (puntito) o muerta (no se dibuja)
//Cada célula tiene 8 vecinas 

//Reglas ejemplo: algoritmo de Conway, 23/3:
//Una célula viva sigue viva sii tiene exactamente 2 ó 3 células vecinas vivas         (23/)
//Una célula vacía (o muerta) nace sii tiene exactamente 3 células vecinas vivas       (/3)

namespace JVida_Fast_CSharp
{
    public class FireUpdateEventArgs : EventArgs
    {
        public List<Point> Nacidos { get; set; }
        public List<Point> Muertos { get; set; }

        public FireUpdateEventArgs(List<Point> Nacidos, List<Point> Muertos)
        {
            this.Nacidos = Nacidos;
            this.Muertos = Muertos;
        }
    }

    public class JuegoVida
    {
        #region Events
        public event FireUpdateEventHandler FireUpdate;
        public delegate void FireUpdateEventHandler(object sender, FireUpdateEventArgs e);
        private void OnFireUpdate(List<Point> Nacidos, List<Point> Muertos)
        {
            if (FireUpdate != null)
            {
                FireUpdate(this, new FireUpdateEventArgs(Nacidos, Muertos));
            }
        } 
        #endregion

        #region Fields
        private Celda[,] Matriz;
        private List<Point> Vivientes;
        private Algoritmo algoritmo;
        private int EdadMaxima;
        private double OcupacionInicial;
        private bool @stop = false;
        private delegate void MyEventHandler(List<Point> Nacidos, List<Point> Muertos);
        private MyEventHandler MyFireUpdater; 
        #endregion

        #region Properties
        public string Algoritmo
        {
            get { return algoritmo.Simbolo; }
        } 
        #endregion

        #region Constructor
        public JuegoVida(int maxX, int maxY, string SimboloAlgoritmo, int EdadMaxima, double OcupacionInicial)
        {
            MyFireUpdater = new MyEventHandler(OnFireUpdate);
            this.EdadMaxima = EdadMaxima;
            this.algoritmo = new Algoritmo(SimboloAlgoritmo);
            this.OcupacionInicial = OcupacionInicial;
            Init(maxX, maxY);
        } 
        #endregion

        #region Private Methods
        private void Init(int maxX, int maxY)
        {
            Matriz = new Celda[maxX, maxY];
            for (int x = 0; x <= maxX - 1; x++)
            {
                for (int y = 0; y <= maxY - 1; y++)
                {
                    dynamic celda = new Celda(x, y);
                    Matriz[x, y] = celda;
                }
            }
            for (int x = 0; x <= maxX - 1; x++)
            {
                for (int y = 0; y <= maxY - 1; y++)
                {
                    //vecinos
                    Celda celda = Matriz[x, y];
                    celda.Vecinos.Add(_prev(maxX, x), _prev(maxY, y));  //Arriba/Izq
                    celda.Vecinos.Add(x, _prev(maxY, y));               //Arriba/Centro
                    celda.Vecinos.Add(_next(maxX, x), _prev(maxY, y));  //Arriba/Der
                    celda.Vecinos.Add(_prev(maxX, x), y);               //Centro/Izquierda
                    celda.Vecinos.Add(_next(maxX, x), y);               //Centro/Derecha
                    celda.Vecinos.Add(_prev(maxX, x), _next(maxY, y));  //Abajo/Izq
                    celda.Vecinos.Add(x, _next(maxY, y));               //Abajo/Centro
                    celda.Vecinos.Add(_next(maxX, x), _next(maxY, y));  //Abajo/Der
                }
            }
            Vivientes = new List<Point>();
        }

        private int _next(int max, int i)
        {
            return i == max - 1 ? 0 : i + 1;
        }
        private int _prev(int max, int i)
        {
            return i == 0 ? max - 1 : i - 1;
        }

        private void SetVivos(List<Point> vivos)
        {
            foreach (Point item in vivos)
            {
                Matriz[item.X, item.Y].Viva = true;
                Vivientes.Add(item);
            }
        }

        // Efectuar un "turno" en el juego
        private void Turno(ref List<Point> nacen, ref List<Point> mueren)
        {
            //Seteo la cuenta de vecinos vivos en 0 a todos además incremento la edad de los vivos
            foreach (Point item in Vivientes.AsParallel())
            {
                Matriz[item.X, item.Y].Cantidad = 0;
                Matriz[item.X, item.Y].Edad++;
                foreach (Point vecino in Matriz[item.X, item.Y].Vecinos)
                {
                    Matriz[vecino.X, vecino.Y].Cantidad = 0;
                }
            }
            //Computar el numero de vecinos (vivos) en cada celda (viva)
            //A cada vecino del actual viviente le sumo 1 en la cantidad
            foreach (Point item in Vivientes.AsParallel())
            {
                foreach (Point vecino in Matriz[item.X, item.Y].Vecinos)
                {
                    Matriz[vecino.X, vecino.Y].Cantidad++;
                }
            }

            nacen = GetNacen();
            List<Point> sobreviven = GetSobreviven();
            mueren = GetMueren();

            foreach (Point m in mueren)
            {
                Matriz[m.X, m.Y].Viva = false;
            }

            Vivientes = nacen.Concat(sobreviven).ToList();
        }

        private List<Point> GetNacen()
        {
            List<Point> ret = new List<Point>();
            //Nacen
            foreach (Point item in Vivientes.AsParallel())
            {
                foreach (Point vecino in Matriz[item.X, item.Y].Vecinos)
                {
                    Celda celdaVecina = Matriz[vecino.X, vecino.Y];
                    if (!celdaVecina.Viva)
                    {
                        bool nace = algoritmo.EvualuarProximoEstado(0, celdaVecina.Cantidad, celdaVecina.Cantidad);
                        if (nace)
                        {
                            Matriz[vecino.X, vecino.Y].Viva = true;
                            Matriz[vecino.X, vecino.Y].Edad = 0;
                            ret.Add(celdaVecina.Ubicacion);
                        }
                    }
                }
            }
            return ret;
        }

        private List<Point> GetSobreviven()
        {
            List<Point> ret = new List<Point>();
            //Sobreviven
            foreach (Point item in Vivientes.AsParallel())
            {
                Celda c1 = Matriz[item.X, item.Y];
                bool sobrevive = algoritmo.EvualuarProximoEstado(1, c1.Cantidad, c1.Cantidad);
                if (sobrevive)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        private List<Point> GetMueren()
        {
            List<Point> ret = new List<Point>();
            foreach (Point item in Vivientes.AsParallel())
            {
                Celda c1 = Matriz[item.X, item.Y];
                if (c1.Edad >= EdadMaxima)
                {
                    ret.Add(item);
                    continue;
                }
                bool vive = algoritmo.EvualuarProximoEstado(1, c1.Cantidad, c1.Cantidad);
                if (!vive)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        private void Randomizar()
        {
            Random rnd = new Random();
            List<Point> vivos = new List<Point>();
            for (int i = 0; i <= Matriz.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= Matriz.GetLength(1) - 1; j++)
                {
                    if (rnd.NextDouble() > (1 - this.OcupacionInicial) &&
                        i > Matriz.GetLength(0) * 2 / 5 &&
                        i < Matriz.GetLength(0) * 3 / 5 &&
                        j > Matriz.GetLength(1) * 2 / 5 &&
                        j < Matriz.GetLength(1) * 3 / 5)
                    {
                        vivos.Add(i, j);
                    }
                }
            }
            SetVivos(vivos);
            List<Point> muertos = new List<Point>();
            OnFireUpdate(vivos, muertos);
        } 
        #endregion

        #region Public Methods
        public void Jugar()
        {
            Randomizar();
            List<Point> nacen = null;
            List<Point> mueren = null;
            while (!@stop)
            {
                Turno(ref nacen, ref mueren);
                MyFireUpdater.Invoke(nacen, mueren);
                System.Threading.Thread.Sleep(1);
            }
        } 
        #endregion
    }
}

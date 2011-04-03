//Thepirat 2011
//thepirat000@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Text;

namespace JVida_Fast_CSharp
{
    public partial class MainForm : Form
    {
        #region Fields
        private JuegoVida jv;
        private Grafiquito graf;
        private int GridSize = 400;
        private double OcupacionInicial = .5;
        private int EdadMaxima = 200;
        private string Algoritmo = "23/3";
        private Thread workerThread; 
        #endregion

        #region Constructor
        public MainForm()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            this.Shown += Form1_Shown;
            this.KeyDown += Form1_KeyDown;
            this.FormClosing += Form1_FormClosing;
            Inicializar();
        } 
        #endregion

        #region Private Methods
        private void Inicializar()
        {
            this.Controls.Remove(graf);
            jv = new JuegoVida(GridSize, GridSize, Algoritmo, EdadMaxima, this.OcupacionInicial);
            jv.FireUpdate += jv_FireUpdate;
            graf = new Grafiquito(GridSize, GridSize);
            graf.Dock = DockStyle.Fill;
            this.Controls.Add(graf);
        }

        private void Comenzar()
        {
            AbortWorker();
            workerThread = new Thread(jv.Jugar);
            workerThread.Priority = ThreadPriority.AboveNormal;
            workerThread.Start();
        }

        private void AbortWorker()
        {
            if (workerThread != null && workerThread.IsAlive)
            {
                workerThread.Abort();
                workerThread.Join();
            }
        }

        private string GetKeysHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("F1: Mostrar/Ocultar ayuda");
            sb.AppendLine("A: Cambiar algoritmo");
            sb.AppendLine("E: Cambiar la edad máxima");
            sb.AppendLine("F: Mostrar/Ocultar fps");
            sb.AppendLine("G: Cambiar tamaño de grid");
            sb.AppendLine("O: Cambiar la densidad inicial");
            sb.AppendLine("R: Reiniciar");
            sb.AppendLine("Esc: Salir de la aplicación");
            sb.AppendLine("Enter: Cambiar color de celdas vivas");
            sb.AppendLine();
            sb.AppendFormat("Algoritmo: {0}. Edad máx: {1}. Tamaño: {2}.", this.Algoritmo, this.EdadMaxima, this.GridSize);
            return sb.ToString();
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            bool redraw = false;
            switch (e.KeyCode)
            {
                case Keys.R:
                    // Comenzar de nuevo
                    redraw = true;
                    break;
                case Keys.Return:
                    // Cambiar color de celdas vivas
                    Random rnd = new Random();
                    graf.ForeColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                    break;
                case Keys.A:
                    // Cambiar algoritmo
                    string newAlgoritmo;
                    string help = "Ingrese el nuevo algoritmo en formato ddd/DDD\nUna celda viva sigue viva sii tiene exactamente d celdas vecinas vivas\nUna celda vacía nace sii tiene exactamente D células vecinas vivas\nEjemplo: 23/3 = Algoritmo de conway. 34/34 = Vida 34";

                    if (InputBox.Show("Algoritmo", help, this.Algoritmo, out newAlgoritmo) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.Algoritmo = newAlgoritmo;
                        redraw = true;
                    }
                    break;
                case Keys.G:
                    // Cambiar gridsize
                    string newgridSzie;
                    if (InputBox.Show("Grid", "Ingrese nuevo tamaño de grid", this.GridSize.ToString(), out newgridSzie) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.GridSize = int.Parse(newgridSzie);
                        redraw = true;
                    }
                    break;
                case Keys.E:
                    // Cambiar edad máxima
                    string newEdadMax;
                    if (InputBox.Show("Edad", "Ingrese nueva edad máxima.\nLa edad máxima es la cantidad máxima de ciclos luego de la cual cualquier celda viva muere.", this.EdadMaxima.ToString(), out newEdadMax) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.EdadMaxima = int.Parse(newEdadMax);
                        redraw = true;
                    }
                    break;
                case Keys.F:
                    graf.ShowFps = !graf.ShowFps;
                    break;
                case Keys.O:
                    // Cambiar ocupación inicial
                    string newOcup;
                    if (InputBox.Show("Ocupación inicial", "Ingrese el porcentaje de ocupación inicial (densidad).\nValor entre 1 y 100.", (this.OcupacionInicial * 100).ToString(), out newOcup) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.OcupacionInicial = double.Parse(newOcup) / 100;
                        redraw = true;
                    }
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
                case Keys.F1:
                    graf.OverlayInfo = string.IsNullOrEmpty(graf.OverlayInfo) ? GetKeysHelp() : null;
                    break;
            }
            if (redraw)
            {
                AbortWorker();
                Inicializar();
                Comenzar();
            }
        }

        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            AbortWorker();
        }

        private void Form1_Shown(object sender, System.EventArgs e)
        {
            Comenzar();
        }

        private void jv_FireUpdate(object sender, FireUpdateEventArgs e)
        {
            foreach (Point nace in e.Nacidos)
            {
                graf.PlotBmp(nace.X, nace.Y, 1);
            }
            foreach (Point muere in e.Muertos)
            {
                graf.PlotBmp(muere.X, muere.Y, 0);
            }
            graf.Invalidate();
        } 
        #endregion
    }
}
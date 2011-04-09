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
        private GameOfLife Gol;
        private UniverseGraph Graph;
        private int GridSize = 400;
        private double InitialOccupation = .5;
        private int MaximumAge = 200;
        private string Algorithm = "23/3";
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
            Initialize();
        } 
        #endregion

        #region Private Methods
        private void Initialize()
        {
            this.Controls.Remove(Graph);
            Gol = new GameOfLife(GridSize, GridSize, Algorithm, MaximumAge, this.InitialOccupation);
            Gol.FireUpdate += jv_FireUpdate;
            Graph = new UniverseGraph(GridSize, GridSize);
            Graph.Dock = DockStyle.Fill;
            this.Controls.Add(Graph);
        }

        private void Start()
        {
            AbortWorker();
            workerThread = new Thread(Gol.Play);
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
            sb.AppendLine("F1: Show/Hide help");
            sb.AppendLine("A: Change Algorithm");
            sb.AppendLine("E: Change Maximum Age");
            sb.AppendLine("F: Show/Hide fps");
            sb.AppendLine("G: Change Grid Size");
            sb.AppendLine("O: Change initial density (occupation)");
            sb.AppendLine("R: Restart");
            sb.AppendLine("Esc: Exit Application");
            sb.AppendLine("Enter: Change alive cell color");
            sb.AppendLine();
            sb.AppendFormat("Algorithm: {0}. Max Age: {1}. Size: {2}.", this.Algorithm, this.MaximumAge, this.GridSize);
            return sb.ToString();
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            bool redraw = false;
            switch (e.KeyCode)
            {
                case Keys.R:
                    // Start again
                    redraw = true;
                    break;
                case Keys.Return:
                    // Change alive cell color
                    Random rnd = new Random();
                    Graph.ForeColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                    break;
                case Keys.A:
                    // change algorithm
                    string newAlgorithm;
                    string help = "Enter the new algorithm in ddd/DDD format.\nA living cell still alive iif it has exactly d neighbors.\nAn empty cell is born iif it has exactly D alive neighbors.\nExample: 23/3 = Conway algorithm. 34/34 = Life 34.";
                    if (InputBox.Show("Algorithm", help, this.Algorithm, out newAlgorithm) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.Algorithm = newAlgorithm;
                        redraw = true;
                    }
                    break;
                case Keys.G:
                    // Change gridsize
                    string newgridSzie;
                    if (InputBox.Show("Grid size", "Enter the new Grid Size", this.GridSize.ToString(), out newgridSzie) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.GridSize = int.Parse(newgridSzie);
                        redraw = true;
                    }
                    break;
                case Keys.E:
                    // Change maximum age
                    string newMaxAge;
                    if (InputBox.Show("Age", "Enter the new Maximum Age.\nThe maximum age is the maximum number of cycles after which any living cell dies.", this.MaximumAge.ToString(), out newMaxAge) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.MaximumAge = int.Parse(newMaxAge);
                        redraw = true;
                    }
                    break;
                case Keys.F:
                    Graph.ShowFps = !Graph.ShowFps;
                    break;
                case Keys.O:
                    // Chage initial occupation
                    string newOccup;
                    if (InputBox.Show("Initial occupation", "Enter the initial occupation percentage (density).\nValue between 1 and 100.", (this.InitialOccupation * 100).ToString(), out newOccup) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.InitialOccupation = double.Parse(newOccup) / 100;
                        redraw = true;
                    }
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
                case Keys.F1:
                    Graph.OverlayInfo = string.IsNullOrEmpty(Graph.OverlayInfo) ? GetKeysHelp() : null;
                    break;
            }
            if (redraw)
            {
                AbortWorker();
                Initialize();
                Start();
            }
        }

        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            AbortWorker();
        }

        private void Form1_Shown(object sender, System.EventArgs e)
        {
            Start();
        }

        private void jv_FireUpdate(object sender, FireUpdateEventArgs e)
        {
            foreach (Point born in e.Born)
            {
                Graph.PlotBmp(born.X, born.Y, 1);
            }
            foreach (Point dead in e.Dead)
            {
                Graph.PlotBmp(dead.X, dead.Y, 0);
            }
            Graph.Invalidate();
        } 
        #endregion
    }
}
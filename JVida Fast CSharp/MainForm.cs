// Thepirat 2011
// thepirat000@hotmail.com
namespace JVida_Fast_CSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        #region Fields
        private GameOfLife Gol;
        private UniverseGraph Graph;
        private int GridSize = 400;
        private double InitialOccupation = .5;
        private int MaximumAge = 200;
        private string AlgorithmSymbol = "23/3";
        private Thread workerThread;

        private bool IsRecording = false;
        private AviWriter Avi;
        private Bitmap BmpAvi;
        #endregion

        #region Constructor
        public MainForm()
        {
            // This call is required by the Windows Form Designer.
            this.InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            this.Shown += this.Form1_Shown;
            this.KeyDown += this.Form1_KeyDown;
            this.FormClosing += this.Form1_FormClosing;
            this.Initialize();
        } 
        #endregion

        #region Private Methods
        private void Restart()
        {
            this.AbortWorker();
            this.Initialize();
            this.Start();
        }
        
        private void Initialize()
        {
            Color prevColor = this.Graph == null ? Color.Red : this.Graph.ForeColor;
            string prevUpperRightInfo = this.Graph == null ? string.Empty : this.AlgorithmSymbol;
            bool prevShowFps = this.Graph == null ? true : this.Graph.ShowFps;
            this.Controls.Remove(this.Graph);
            this.Gol = new GameOfLife(this.GridSize, this.GridSize, this.AlgorithmSymbol, this.MaximumAge, this.InitialOccupation);
            this.Gol.FireUpdate += this.jv_FireUpdate;
            this.Graph = new UniverseGraph(this.GridSize, this.GridSize)
            {
                Dock = DockStyle.Fill,
                ForeColor = prevColor,
                UpperRightInfo = prevUpperRightInfo,
                ShowFps = prevShowFps
            };
            this.Controls.Add(this.Graph);
        }
        
        private void Start()
        {
            this.AbortWorker();
            this.workerThread = new Thread(this.Gol.Play);
            this.workerThread.Priority = ThreadPriority.AboveNormal;
            this.workerThread.Start();
        }

        private void AbortWorker()
        {
            if (this.IsRecording && !this.Avi.IsClosed)
            {
                this.Avi.Close();
            }
            if (this.workerThread != null && this.workerThread.IsAlive)
            {
                this.workerThread.Abort();
                this.workerThread.Join();
            }
        }

        private string GetKeysHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("F1: Show/Hide help");
            sb.AppendLine("A: Change Algorithm");
            sb.AppendLine("Q: Random Algorithm");
            sb.AppendLine("E: Change Maximum Age");
            sb.AppendLine("F: Show/Hide fps");
            sb.AppendLine("L: Show/Hide Algorithm");
            sb.AppendLine("G: Change Grid Size");
            sb.AppendLine("O: Change initial density (occupation)");
            sb.AppendLine("R: Restart");
            sb.AppendLine("S: Start/Stop recording AVI video");
            sb.AppendLine("Esc: Exit Application");
            sb.AppendLine("Enter: Change alive cell color");
            sb.AppendLine();
            sb.AppendFormat("Algorithm: {0}. Max Age: {1}. Size: {2}.", this.AlgorithmSymbol, this.MaximumAge, this.GridSize);
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
                    this.Graph.ForeColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                    break;
                case Keys.A:
                    // change algorithm
                    string newAlgorithm;
                    string help = "Enter the new algorithm in ddd/DDD format.\nA living cell still alive iif it has exactly d neighbors.\nAn empty cell is born iif it has exactly D alive neighbors.\nExample: 23/3 = Conway algorithm. 34/34 = Life 34.";
                    if (InputBox.Show("Algorithm", help, this.AlgorithmSymbol, out newAlgorithm) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.AlgorithmSymbol = newAlgorithm;
                        redraw = true;
                    }
                    break;
                case Keys.Q:
                // change to random algorithm
                    newAlgorithm = Algorithm.GetRandomAlgorithm();
                    this.AlgorithmSymbol = newAlgorithm;
                    redraw = true;
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
                    this.Graph.ShowFps = !this.Graph.ShowFps;
                    break;
                case Keys.L:
                    if (string.IsNullOrEmpty(this.Graph.UpperRightInfo))
                    {
                        this.Graph.UpperRightInfo = this.AlgorithmSymbol;
                    }
                    else
                    {
                        this.Graph.UpperRightInfo = string.Empty;
                    }
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
                    this.Graph.OverlayInfo = string.IsNullOrEmpty(this.Graph.OverlayInfo) ? this.GetKeysHelp() : null;
                    break;
                case Keys.S:
                    if (!this.IsRecording)
                    {
                        this.AbortWorker();
                        this.Avi = new AviWriter();
                        this.saveFileDialog1.FileName = "Algorithm " + this.AlgorithmSymbol.Replace('/', '^') + " - MaxAge " + this.MaximumAge + " - Density " + (int)(this.InitialOccupation * 100);
                        if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            this.BmpAvi = this.Avi.Open(this.saveFileDialog1.FileName, 30, this.GridSize, this.GridSize);
                            this.Restart();
                            this.IsRecording = true;
                            this.Graph.FootInfo = "Recording... Press 's' to stop.";
                            this.Graph.ShowFps = false;
                        }
                    }
                    else
                    {
                        // Stop recording
                        this.IsRecording = false;
                        this.Graph.FootInfo = string.Empty;
                        this.Graph.ShowFps = true;
                        this.Avi.Close();
                        this.BmpAvi.Dispose();
                        Process.Start(@"explorer", @"/select,""" + this.saveFileDialog1.FileName + @"""");
                    }
                    break;
            }
            if (redraw)
            {
                this.Restart();
            }
        }

        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            this.AbortWorker();
        }

        private void Form1_Shown(object sender, System.EventArgs e)
        {
            this.Start();
        }

        private void jv_FireUpdate(object sender, FireUpdateEventArgs e)
        {
            foreach (Point born in e.Born)
            {
                this.Graph.PlotBmp(born.X, born.Y, 1);
            }
            foreach (Point dead in e.Dead)
            {
                this.Graph.PlotBmp(dead.X, dead.Y, 0);
            }
            this.Graph.Invalidate();
            if (this.IsRecording)
            {
                this.RecordFrame();
            }
        }

        private void RecordFrame()
        {
            lock (this.Graph.UniverseBitmap)
            {
                if (!this.Avi.IsClosed)
                {
                    using (Graphics g = Graphics.FromImage(this.BmpAvi))
                    {
                        g.DrawImage(this.Graph.UniverseBitmap, 0, 0, this.BmpAvi.Width, this.BmpAvi.Height);
                    }
                    this.Avi.AddFrame();
                }
            }
        }
        #endregion
    }
}
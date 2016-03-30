﻿// Thepirat 2011
// thepirat000@hotmail.com

using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using JVida_Fast_CSharp.Helpers;

namespace JVida_Fast_CSharp
{
    using Parsers;
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
        private Size GridSize = new Size(400, 300);

        private double InitialOccupation = .5;
        private int MaximumAge = int.MaxValue;
        private string AlgorithmSymbol = "23/3";
        private Thread workerThread;
        private double InitialMargin = .3;
        private bool WrapAround = true;

        private bool IsRecording = false;
        private AviWriter Avi;
        private Bitmap BmpAvi;
        private string InitialFilePath;
        public const string LoadPatternPipeName = "GOLS_LoadPattern";
        private NamedPipeServerStream loadPipe;
        private HashSet<string> availableExtensions;
        #endregion

        #region Constructor
        public MainForm(string initialFilePath)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            InitialFilePath = initialFilePath;
            // Add any initialization after the InitializeComponent() call.
            Shown += Form1_Shown;
            KeyDown += Form1_KeyDown;
            FormClosing += Form1_FormClosing;
            txtGridSize.GotFocus += txtGridSize_GotFocus;
            txtGridSize.LostFocus += txtGridSize_LostFocus;
            FillAlgorithmCombo();
            SetBtnFileAssocText();
            SetFilePatternListener();
            availableExtensions = new HashSet<string>(ParserFactory.GetAvailableExtensions());
            Initialize(false);
        }
        #endregion

        #region Private Methods

        private void SetFilePatternListener()
        {
            loadPipe = new NamedPipeServerStream(LoadPatternPipeName, PipeDirection.In);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    loadPipe.WaitForConnection();
                    StreamReader reader = new StreamReader(loadPipe);
                    var filePath = reader.ReadLine();
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        this.Invoke(new Action(() => LoadPatternFileCentered(filePath)));
                    }
                    loadPipe.Disconnect();
                }
            });
        }

        private void Restart(bool restoreState)
        {
            AbortWorker();
            Initialize(restoreState);
            Start(!restoreState);
        }

        private void Initialize(bool restoreState)
        {
            Color prevColor = Graph == null ? Color.Red : Graph.ForeColor;
            string prevUpperRightInfo = Graph == null ? string.Empty : AlgorithmSymbol;
            bool prevShowFps = Graph == null ? true : Graph.ShowFps;
            splitContainer.Panel2.Controls.Remove(Graph);
            Cell[,] ex_matrix = null;
            bool exPaused = false;
            if (restoreState)
            {
                exPaused = Gol.Paused;
                ex_matrix = Gol.Matrix;
            }
            Gol = new GameOfLife(GridSize.Width, GridSize.Height, AlgorithmSymbol, MaximumAge,
                                      InitialOccupation, InitialMargin, WrapAround);
            if (restoreState)
            {
                Gol.Plot(new Point(0, 0), ex_matrix);
            }
            if (restoreState && exPaused)
            {
                Pause();
            }
            Gol.FireUpdate += jv_FireUpdate;
            Graph = new UniverseGraph(GridSize.Width, GridSize.Height)
            {
                Dock = DockStyle.Fill,
                ForeColor = prevColor,
                UpperRightInfo = prevUpperRightInfo,
                ShowFps = prevShowFps,
                AllowDrop = true
            };
            Graph.PointSelected += Graph_PointSelected;
            Graph.DragEnter += Graph_DragEnter;
            Graph.DragDropCell += Graph_DragDropCell;
            splitContainer.Panel2.Controls.Add(Graph);
            chkFps.Checked = Graph.ShowFps;
            chkAlgo.Checked = !string.IsNullOrEmpty(Graph.UpperRightInfo);
            chkWrapAround.Checked = WrapAround;
            txtGridSize.Text = $"{GridSize.Width}x{GridSize.Height}";
            picColor.BackColor =  Graph.ForeColor;
            colorDialog1.Color = picColor.BackColor;
        }

        private void FillAlgorithmCombo()
        {
            var algos = new [] {
                    new { Value = "23/3", Text = "Conway's Life (23/3)", Description = "23/3 - Conway's Life\nA chaotic rule that is by far the most well-known and well-studied. It exhibits highly complex behavior."},
                    new { Value = "23/36", Text = "HighLife (23/36)", Description = "23/36 - HighLife\nA chaotic rule very similar to Conway's Life that is of interest because it has a simple replicator."},
                    new { Value = "1/1", Text = "Gnarl (1/1)", Description = "1/1 - Gnarl\nA simple exploding rule that forms complex patterns from even a single live cell."},
                    new { Value = "1357/1357", Text = "Replicator (1357/1357)", Description = "1357/1357 - Replicator\nA rule in which every pattern is a replicator."},
                    new { Value = "02468/1357", Text = "Fredkin (02468/1357)", Description = "02468/1357 - Fredkin\nA rule in which, like Replicator, every pattern is a replicator."},
                    new { Value = "/2", Text = "Seeds (/2)", Description = "/2 - Seeds\nAn exploding rule in which every cell dies in every generation. It has many simple orthogonal spaceships, though it is in general difficult to create patterns that don't explode."},
                    new { Value = "0/2", Text = "Live Free or Die (0/2)", Description = "0/2 - Live Free or Die\nAn exploding rule in which only cells with no neighbors survive. It has many spaceships, puffers, and oscillators, some of infinitely extensible size and period."},
                    new { Value = "/234", Text = "Serviettes (/234)", Description = "/234 - Serviettes\nAn exploding rule in which every cell dies every generation (like seeds). This rule is of interest because of the fabric-like beauty of the patterns that it produces."},
                    new { Value = "023/3", Text = "DotLife (023/3)", Description = "023/3 - DotLife\nAn exploding rule closely related to Conway's Life. The B-heptomino is a common infinite growth pattern in this rule, though it can be stabilized into a spaceship."},
                    new { Value = "012345678/3", Text = "Life without death (012345678/3)", Description = "012345678/3 - Life without death\nAn expanding rule that produces complex flakes. It also has important ladder patterns."},
                    new { Value = "1234/3", Text = "Mazectric (1234/3)", Description = "1234/3 - Mazectric\nAn expanding rule that crystalizes to form maze-like designs that tend to be straighter (ie. have longer \"halls\") than the standard maze rule."},
                    new { Value = "12345/3", Text = "Maze (12345/3)", Description = "12345/3 - Maze\nAn expanding rule that crystalizes to form maze-like designs."},
                    new { Value = "45678/3", Text = "Coral (45678/3)", Description = "45678/3 - Coral\nAn exploding rule in which patterns grow slowly and form coral-like textures."},
                    new { Value = "34/34", Text = "34 Life (34/34)", Description = "34/34 - 34 Life\nAn exploding rule that was initially thought to be a stable alternative to Conway's Life, until computer simulation found that most patterns tend to explode. It has many small oscillators and simple period 3 orthogonal and diagonal spaceships."},
                    new { Value = "4567/345", Text = "Assimilation (4567/345)", Description = "4567/345 - Assimilation\nA very stable rule that forms permanent diamond-shaped patterns with partially filled interiors."},
                    new { Value = "5/345", Text = "Long Life (5/345)", Description = "5/345 - Long Life\nA stable rule that gets its name from the fact that it has many simple extremely high period oscillators."},
                    new { Value = "5678/35678", Text = "Diamoeba (5678/35678)", Description = "5678/35678 - Diamoeba\nA chaotic pattern that forms large diamonds with chaotically oscillating boundaries. Known to have quadratically-growing patterns."},
                    new { Value = "1358/357", Text = "Amoeba (1358/357)", Description = "1358/357 - Amoeba\nA chaotic rule that is well balanced between life and death; it forms patterns with chaotic interiors and wildly moving boundaries."},
                    new { Value = "238/357", Text = "Pseudo Life (238/357)", Description = "238/357 - Pseudo Life\nA chaotic rule with evolution that resembles Conway's Life, but few patterns from Life work in this rule because the glider is unstable."},
                    new { Value = "125/36", Text = "2x2 (125/36)", Description = "125/36 - 2x2\nA chaotic rule with many simple still lifes, oscillators and spaceships. Its name comes from the fact that it sends patterns made up of 2x2 blocks to patterns made up of 2x2 blocks."},
                    new { Value = "245/368", Text = "Move (245/368)", Description = "245/368 - Move\nA rule in which random patterns tend to stabilize extremely quickly. Has a very common slow-moving spaceship and slow-moving puffer."},
                    new { Value = "235678/3678", Text = "Stains (235678/3678)", Description = "235678/3678 - Stains\nA stable rule in which most patterns tend to \"fill in\" bounded regions. Most nearby rules (such as coagulations) tend to explode."},
                    new { Value = "34678/3678", Text = "Day & Night (34678/3678)", Description = "34678/3678 - Day & Night\nA stable rule that is symmetric under on-off reversal. Many patterns exhibiting highly complex behavior have been found for it."},
                    new { Value = "23/37", Text = "DryLife (23/37)", Description = "23/37 - DryLife\nAn exploding rule closely related to Conway's Life, named after the fact that the standard spaceships bigger than the glider do not function in the rule. Has a small puffer based on the R-pentomino, which resembles the switch engine in the possibility of combining several to form a spaceship."},
                    new { Value = "235678/378", Text = "Coagulations (235678/378)", Description = "235678/378 - Coagulations\nAn exploding rule in which patterns tend to expand forever, producing a thick \"goo\" as it does so. Suprisingly, Coagulations actually has one less birth condition than Stains."},
                    new { Value = "2345/45678", Text = "Walled cities (2345/45678)", Description = "2345/45678 - Walled cities\nA stable rule that forms centers of pseudo-random activity separated by walls."},
                    new { Value = "35678/4678", Text = "Vote 4/5 (35678/4678)", Description = "35678/4678 - Vote 4/5\nA modification of the standard Gérard Vichniac voting rule, also known as \"Anneal\", used as a model for majority voting."},
                    new { Value = "45678/5678", Text = "Vote (45678/5678)", Description = "45678/5678 - Vote\nStandard Gérard Vichniac voting rule, also known as \"Majority\", used as a model for majority voting."},
                    new { Value = "Custom", Text = "Custom...", Description = ""}
                };
            cmbAlgorithm.Items.Clear();
            for (int i = 0; i < algos.Length; i++)
            {
                cmbAlgorithm.Items.Add(algos[i]);
            }
            cmbAlgorithm.DisplayMember = "Text";
            cmbAlgorithm.ValueMember = "Value";
            cmbAlgorithm.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the PointSelected event of the Graph control.
        /// This is fired when the user selects a point in the graph to insert an imported glyph
        /// </summary>
        private void Graph_PointSelected(object sender, PointSelectedEventArgs e)
        {
            if (!e.IsImporting)
            {
                return;
            }
            Graph.ExitSelectionMode();
            Pattern pattern = TryParsePattern(openFileDialog1.FileName);
            if (pattern == null)
            { 
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                LoadPatternFileCentered(openFileDialog1.FileName);
                return;
            }
            PlotPattern(e.Point, pattern);
        }

        private Pattern TryParsePattern(string filePath)
        {
            Pattern pattern = null;
            var parser = ParserFactory.GetParser(filePath);
            try
            {
                pattern = parser.Parse(new StreamReader(filePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing file {filePath}. {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return pattern;
        }

        private void FindSelectAlgorithm(string symbol)
        {
            foreach (dynamic item in cmbAlgorithm.Items)
            {
                if (item.Value == symbol || item.Value == "Custom")
                {
                    cmbAlgorithm.SelectedIndexChanged -= cmbAlgorithm_SelectedIndexChanged;
                    cmbAlgorithm.SelectedItem = item;
                    cmbAlgorithm.SelectedIndexChanged += cmbAlgorithm_SelectedIndexChanged;
                    return;
                }
            }
        }

        private void Start(bool randomize)
        {
            AbortWorker();
            if (randomize)
            {
                Gol.Randomize();
            }
            workerThread = new Thread(Gol.Play);
            workerThread.Priority = ThreadPriority.AboveNormal;
            workerThread.Start();
        }

        private void AbortWorker()
        {
            if (IsRecording && !Avi.IsClosed)
            {
                Avi.Close();
            }
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
            sb.AppendLine("Q: Random Algorithm");
            sb.AppendLine("E: Change Maximum Age");
            sb.AppendLine("F: Show/Hide fps");
            sb.AppendLine("L: Show/Hide Algorithm");
            sb.AppendLine("G: Change Grid Size");
            sb.AppendLine("O: Change initial density (occupation)");
            sb.AppendLine("R: Restart");
            sb.AppendLine("S: Start/Stop recording AVI video");
            sb.AppendLine("C: Clear");
            sb.AppendLine("P: Pause/Resume");
            sb.AppendLine("I: Import cells file");
            sb.AppendLine("Esc: Exit Application");
            sb.AppendLine("Enter: Change alive cell color");
            sb.AppendLine();
            sb.AppendFormat("Algorithm: {0}. Max Age: {1}. Size: {2}.", AlgorithmSymbol, MaximumAge, GridSize);
            return sb.ToString();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtGridSize.Focused || cmbAlgorithm.Focused)
            {
                return;
            }
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
                    picColor.BackColor = Graph.ForeColor;
                    break;
                case Keys.A:
                    // change algorithm
                    string newAlgorithm;
                    string help = "Enter the new algorithm in ddd/DDD format.\nA living cell still alive iif it has exactly d neighbors.\nAn empty cell is born iif it has exactly D alive neighbors.\nExample: 23/3 = Conway algorithm. 34/34 = Life 34.";
                    if (InputBox.Show("Algorithm", help, AlgorithmSymbol, out newAlgorithm) == DialogResult.OK)
                    {
                        AlgorithmSymbol = newAlgorithm;
                        Restart(true);
                    }
                    break;
                case Keys.Q:
                    // change to random algorithm
                    newAlgorithm = Algorithm.GetRandomAlgorithm();
                    AlgorithmSymbol = newAlgorithm;
                    redraw = true;
                    break;
                case Keys.G:
                    // Change gridsize
                    string newgridSize;
                    if (InputBox.Show("Grid size", "Enter the new Grid Size (format WIDTHxHEIGHT)", $"{GridSize.Width}x{GridSize.Height}", out newgridSize) == DialogResult.OK)
                    {
                        var newSizeValues = newgridSize.Split('x');
                        if (newSizeValues.Length == 2)
                        {
                            GridSize = new Size(int.Parse(newSizeValues[0]), int.Parse(newSizeValues[1]));
                            Restart(true);
                        }
                    }
                    break;
                case Keys.E:
                    // Change maximum age
                    string newMaxAge;
                    if (InputBox.Show("Age", "Enter the new Maximum Age.\nThe maximum age is the maximum number of cycles after which any living cell dies.", MaximumAge.ToString(), out newMaxAge) == DialogResult.OK)
                    {
                        MaximumAge = int.Parse(newMaxAge);
                        Restart(true);
                    }
                    break;
                case Keys.F:
                    Graph.ShowFps = !Graph.ShowFps;
                    break;
                case Keys.L:
                    Graph.UpperRightInfo = string.IsNullOrEmpty(Graph.UpperRightInfo) ? AlgorithmSymbol : string.Empty;
                    break;
                case Keys.O:
                    // Chage initial occupation
                    redraw = ChangeOccupation();
                    break;
                case Keys.C:
                    Gol.Clear();
                    break;
                case Keys.P:
                    Gol.TogglePause();
                    Graph.LowerLeftInfo = Gol.Paused ? "< PAUSED >" : "";
                    break;
                case Keys.I:
                    var res = openFileDialog1.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        Graph.EnterSelectionMode(true);
                    }
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
                case Keys.F1:
                    Graph.OverlayInfo = string.IsNullOrEmpty(Graph.OverlayInfo) ? GetKeysHelp() : null;
                    break;
                case Keys.S:
                    if (!IsRecording)
                    {
                        StartRecording();
                    }
                    else
                    {
                        StopRecording();
                    }
                    break;
            }
            if (redraw)
            {
                Restart(false);
            }
        }

        private bool ChangeOccupation()
        {
            string newOccup;
            var def = string.Format("{0},{1}", InitialOccupation * 100, InitialMargin * 100);
            if (InputBox.Show("Initial occupation", "Enter the initial occupation percentage (density) optionally followed by the initial margin percentage to not cover: PPP,AAA.\nValues of PPP between 1 and 100\nValues of AAA between 0 and 49.", def, out newOccup) == DialogResult.OK)
            {
                var values = newOccup.Split(',');
                InitialOccupation = double.Parse(values[0]) / 100;
                if (values.Length > 1)
                {
                    InitialMargin = double.Parse(values[1]) / 100;
                }
                return true;
            }
            return false;
        }

        private void StartRecording()
        {
            bool restart = !Gol.Paused;
            Gol.Pause();
            Avi = new AviWriter();
            saveFileDialog1.FileName = "Algorithm " + AlgorithmSymbol.Replace('/', '^') + " - MaxAge " + MaximumAge + " - Density " + (int)(InitialOccupation * 100);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (restart)
                {
                    AbortWorker();
                }
                BmpAvi = Avi.Open(saveFileDialog1.FileName, 30, GridSize.Width, GridSize.Height);
                if (restart)
                {
                    Restart(false);
                }
                IsRecording = true;
                Graph.FootInfo = "Recording... Press 'S' or click the Stop button to finish.";
                Graph.ShowFps = false;
                if (!restart)
                {
                    Resume();
                }
            }
        }

        private void Resume()
        {
            Gol.Resume();
            Graph.LowerLeftInfo = "";
        }

        private void StopRecording()
        {
            IsRecording = false;
            Thread.Sleep(5);
            Graph.FootInfo = string.Empty;
            Graph.ShowFps = true;
            Avi.Close();
            BmpAvi.Dispose();
            Process.Start(@"explorer", @"/select,""" + saveFileDialog1.FileName + @"""");
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AbortWorker();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Graph.Focus();
            if (InitialFilePath == null)
            {
                Start(true);
            }
            else
            {
                if (File.Exists(InitialFilePath))
                {
                    LoadPatternFileCentered(InitialFilePath);
                }
            }
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
            if (IsRecording)
            {
                RecordFrame();
            }
        }

        private void RecordFrame()
        {
            lock (Graph.UniverseBitmap)
            {
                if (!Avi.IsClosed)
                {
                    using (Graphics g = Graphics.FromImage(BmpAvi))
                    {
                        g.ScaleTransform(1.0F, -1.0F);
                        g.TranslateTransform(0.0F, -(float)BmpAvi.Height);
                        g.DrawImage(Graph.UniverseBitmap, 0, 0, BmpAvi.Width, BmpAvi.Height);
                    }
                    Avi.AddFrame();
                }
            }
        }
        #endregion

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (IsRecording)
            {
                StopRecording();
                return;
            }
            Gol.Pause();
            Graph.LowerLeftInfo = "< STOPPED >";
            Gol.Clear();
        }

        private void btnStop_MouseEnter(object sender, EventArgs e)
        {
            btnStop.Image = imageList1.Images["button_black_stop.ico"];
        }

        private void btnStop_MouseLeave(object sender, EventArgs e)
        {
            btnStop.Image = imageList1.Images["button_grey_stop.ico"];
        }

        private void btnPlay_MouseEnter(object sender, EventArgs e)
        {
            btnPlay.Image = imageList1.Images["button_black_play.ico"];
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            btnPlay.Image = imageList1.Images["button_grey_play.ico"];
        }

        private void btnPause_MouseEnter(object sender, EventArgs e)
        {
            btnPause.Image = imageList1.Images["button_black_pause.ico"];
        }

        private void btnPause_MouseLeave(object sender, EventArgs e)
        {
            btnPause.Image = imageList1.Images["button_grey_pause.ico"];
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void Pause()
        {
            Gol.Pause();
            Graph.LowerLeftInfo = "< PAUSED >";
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Resume();
        }

        private void btnRandom_MouseEnter(object sender, EventArgs e)
        {
            btnRandom.Image = imageList1.Images["button_black_random.ico"];
        }

        private void btnRandom_MouseLeave(object sender, EventArgs e)
        {
            btnRandom.Image = imageList1.Images["button_grey_random.ico"];
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
        }

        private void btnRandom_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ChangeOccupation();
            }
            Restart(false);
        }

        private void btnForward_MouseEnter(object sender, EventArgs e)
        {
            btnForward.Image = imageList1.Images["button_black_ffw.ico"];
        }

        private void btnForward_MouseLeave(object sender, EventArgs e)
        {
            btnForward.Image = imageList1.Images["button_grey_ffw.ico"];
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            Pause();
            Gol.DoSteps = (int) txtStepSize.Value;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Graph.OverlayInfo = string.IsNullOrEmpty(Graph.OverlayInfo) ? GetKeysHelp() : null;
            if (Gol.Paused)
            {
                Gol.DoSteps = 1;
            }
        }

        private void btnHelp_MouseEnter(object sender, EventArgs e)
        {
        }

        private void btnHelp_MouseLeave(object sender, EventArgs e)
        {
        }

        private bool GridSizeFocused;

        private void txtGridSize_MouseUp(object sender, MouseEventArgs e)
        {
            if (!GridSizeFocused && txtGridSize.SelectionLength == 0)
            {
                GridSizeFocused = true;
                txtGridSize.SelectAll();
            }
        }

        private void txtGridSize_Leave(object sender, EventArgs e)
        {
            GridSizeFocused = false;
        }

        private void txtGridSize_GotFocus(object sender, EventArgs eventArgs)
        {
            if (MouseButtons == MouseButtons.None)
            {
                txtGridSize.SelectAll();
                GridSizeFocused = true;
            }
        }

        private void picColor_Click(object sender, EventArgs e)
        {
            Gol.Paused = true;
            var dlg = colorDialog1.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                picColor.BackColor = colorDialog1.Color;
                Graph.ForeColor = colorDialog1.Color;
            }
            Resume();
        }

        private void chkFps_CheckedChanged(object sender, EventArgs e)
        {
            Graph.ShowFps = chkFps.Checked;
        }

        private void chkAlgo_CheckedChanged(object sender, EventArgs e)
        {
            Graph.UpperRightInfo = chkAlgo.Checked ? AlgorithmSymbol : string.Empty;
        }

        private void chkWrapAround_CheckedChanged(object sender, EventArgs e)
        {
            WrapAround = chkWrapAround.Checked;
            Restart(true);
        }

        private void txtGridSize_LostFocus(object sender, EventArgs e)
        {
            ChangeGridSize();
        }

        private void txtGridSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ChangeGridSize();
            }
        }

        private void ChangeGridSize()
        {
            var newSizeValues = txtGridSize.Text.Split('x');
            if (newSizeValues.Length == 2)
            {
                var newSize = new Size(int.Parse(newSizeValues[0]), int.Parse(newSizeValues[1]));
                if (GridSize != newSize)
                {
                    GridSize = newSize;
                    Restart(true);
                    Graph?.Focus();
                }
            }
        }

        private void btnRandomColor_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Graph.ForeColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            picColor.BackColor = Graph.ForeColor;
        }

        private void btnRecord_MouseEnter(object sender, EventArgs e)
        {
            btnRecord.Image = imageList1.Images["button_black_rec.ico"];
        }

        private void btnRecord_MouseLeave(object sender, EventArgs e)
        {
            btnRecord.Image = imageList1.Images["button_grey_rec.ico"];
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (!IsRecording)
            {
                StartRecording();
            }
        }

        private void chkPosition_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPosition.Checked)
            {
                Graph.EnterSelectionMode(false);
            }
            else
            {
                Graph.ExitSelectionMode();
            }
        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valueSelected = ((dynamic) cmbAlgorithm.SelectedItem).Value;
            if (valueSelected == "Custom")
            {
                string newAlgorithm;
                string help = "Enter the new algorithm in ddd/DDD format.\nA living cell still alive iif it has exactly d neighbors.\nAn empty cell is born iif it has exactly D alive neighbors.\nExample: 23/3 = Conway algorithm. 34/34 = Life 34.";
                if (InputBox.Show("Algorithm", help, AlgorithmSymbol, out newAlgorithm) == DialogResult.OK)
                {
                    AlgorithmSymbol = newAlgorithm;
                    Restart(true);
                }
            }
            else if (valueSelected != AlgorithmSymbol)
            {
                AlgorithmSymbol = valueSelected;
                Restart(true);
            }
            Graph?.Focus();
        }

        private void btnOneToOne_Click(object sender, EventArgs e)
        {
            txtGridSize.Text = $"{Graph.Width}x{Graph.Height}";
            ChangeGridSize();
        }

        private void btnRandomAlgorithm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmbAlgorithm.SelectedIndexChanged -= cmbAlgorithm_SelectedIndexChanged;
                cmbAlgorithm.SelectedIndex = cmbAlgorithm.Items.Count - 1;
                cmbAlgorithm.SelectedIndexChanged += cmbAlgorithm_SelectedIndexChanged;
                AlgorithmSymbol = Algorithm.GetRandomAlgorithm();
                Restart(true);
            }
            else if (e.Button == MouseButtons.Left)
            {
                var rnd = new Random();
                cmbAlgorithm.SelectedIndex = rnd.Next(0, cmbAlgorithm.Items.Count - 2);
            }
        }

        private void btnImport_MouseEnter(object sender, EventArgs e)
        {
            btnImport.Image = imageList1.Images["button_black_eject.ico"];

        }

        private void btnImport_MouseLeave(object sender, EventArgs e)
        {
            btnImport.Image = imageList1.Images["button_blue_eject.ico"];
        }

        private void btnImport_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var res = openFileDialog1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    Graph.EnterSelectionMode(true);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Gol.Clear();
        }

        private void txtStepSize_ValueChanged(object sender, EventArgs e)
        {

        }

        // Plots a pattern starting at a given point
        private void PlotPattern(Point startPoint, Pattern pattern)
        {
            if (startPoint.X + pattern.Bitmap.GetLength(0) > GridSize.Width || startPoint.Y + pattern.Bitmap.GetLength(1) > GridSize.Height)
            {
                Pause();
                var dlg =
                    MessageBox.Show(
                        $"Pattern will overflow {pattern.Bitmap.GetLength(0)}x{pattern.Bitmap.GetLength(1)}. Automatically resize/reposition the pattern?",
                        "Reposition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg == DialogResult.Yes)
                {
                    startPoint = new Point(0, 0);
                    if (pattern.Bitmap.GetLength(0) > GridSize.Width || pattern.Bitmap.GetLength(1) > GridSize.Height)
                    {
                        //resize grid to fit the pattern
                        GridSize = new Size(pattern.Bitmap.GetLength(0), pattern.Bitmap.GetLength(1));
                        Restart(true);
                    }
                }
                else
                {
                    Resume();
                    return;
                }
            }
            if (pattern.Algorithm.HasValue && pattern.Algorithm.Value.Symbol != Gol.AlgorithmSymbol)
            {
                // change algorythm
                Gol.Algorithm = pattern.Algorithm.Value;
                AlgorithmSymbol = pattern.Algorithm.Value.Symbol;
                FindSelectAlgorithm(AlgorithmSymbol);
                Restart(true);
            }
            Gol.Plot(startPoint, pattern.Bitmap);
        }

        // Stops and Clears the current simulation (if any), and loads a centered pattern into the simulation
        private void LoadPatternFileCentered(string filePath)
        {
            Pause();
            Gol.Clear();
            Pattern pattern = TryParsePattern(filePath);
            if (pattern == null)
            { 
                return;
            }
            var startPoint = new Point(0, 0);
            if (pattern.Bitmap.GetLength(0) > GridSize.Width || pattern.Bitmap.GetLength(1) > GridSize.Height)
            {
                //overflow, resize grid to fit the pattern
                GridSize = new Size(pattern.Bitmap.GetLength(0), pattern.Bitmap.GetLength(1));
                Restart(true);
            }
            else
            {
                // calculate upper-left point to show the pattern centered
                startPoint.X = GridSize.Width/2 - pattern.Bitmap.GetLength(0)/2;
                startPoint.Y = GridSize.Height/2 - pattern.Bitmap.GetLength(1)/2;
            }
            if (pattern.Algorithm.HasValue && pattern.Algorithm.Value.Symbol != Gol.AlgorithmSymbol)
            {
                // change algorythm
                Gol.Algorithm = pattern.Algorithm.Value;
                AlgorithmSymbol = pattern.Algorithm.Value.Symbol;
                FindSelectAlgorithm(AlgorithmSymbol);
                Restart(true);
            }
            Gol.Plot(startPoint, pattern.Bitmap);


        }

        private void btnSetFileAssoc_Click(object sender, EventArgs e)
        {
            if (btnFileAssoc.Text.StartsWith("Set"))
            {
                FileAssociation.AssociateFileTypes(Application.ExecutablePath);
            }
            else
            {
                FileAssociation.RemoveFileTypeAssociations();
            }
            SetBtnFileAssocText();
        }

        private void SetBtnFileAssocText()
        {
            if (!HasAdminPrivileges())
            {
                btnFileAssoc.Visible = false;
            }
            else
            {
                bool associated = FileAssociation.AlreadyAssociated();
                btnFileAssoc.Text = associated ? "Remove file assoc." : "Set file assoc.";
                toolTip.SetToolTip(btnFileAssoc, $"{(associated ? "Remove" : "Set")} the file associations to open .cells, .rle and .lif files");
            }
        }

        public static bool HasAdminPrivileges()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void Graph_DragDropCell(object sender, DragEventArgs e)
        {
            Graph.ExitSelectionMode();
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                var file = files[0];
                var pattern = TryParsePattern(file);
                if (pattern != null)
                {
                    PlotPattern(new Point(e.X, e.Y), pattern);
                }
            }
        }

        private void Graph_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileName = ((string[])e.Data.GetData("FileName", true))[0];
                if (availableExtensions.Any(ext => fileName.EndsWith(ext)))
                {
                    e.Effect = DragDropEffects.Copy;
                    Graph.EnterSelectionMode(false);
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }


    }
}
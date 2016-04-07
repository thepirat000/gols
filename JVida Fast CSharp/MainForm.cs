// Thepirat 2011
// thepirat000@hotmail.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JVida_Fast_CSharp.Helpers;
using JVida_Fast_CSharp.Parsers;

namespace JVida_Fast_CSharp
{
    public partial class MainForm : Form
    {
        #region Fields
        private GameOfLife _gol;
        private UniverseGraph _graph;
        private Size _gridSize;

        private double _initialOccupation = .5;
        private int _maximumAge = int.MaxValue;
        private string _algorithmSymbol = "23/3";
        private Thread _workerThread;
        private double _initialMargin = .3;
        private bool _wrapAround = true;

        private bool _isRecording;
        private AviWriter _avi;
        private Bitmap _bmpAvi;
        private readonly string _initialFilePath;
        public const string LoadPatternPipeName = "GOLS_LoadPattern";
        private NamedPipeServerStream _loadPipe;
        private readonly HashSet<string> _availableExtensions;
        private bool _gridSizeFocused;
        private readonly string _patternsDir;
        private readonly string _patternZipFile;
        #endregion

        #region Constructor
        public MainForm(string initialFilePath)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            _patternsDir = Path.Combine(Directory.GetCurrentDirectory(), "Patterns");
            _patternZipFile = Path.Combine(Directory.GetCurrentDirectory(), "Patterns.zip");
            _initialFilePath = initialFilePath;
            // Add any initialization after the InitializeComponent() call.
            Shown += Form1_Shown;
            KeyDown += Form1_KeyDown;
            FormClosing += Form1_FormClosing;
            txtGridSize.GotFocus += txtGridSize_GotFocus;
            txtGridSize.LostFocus += txtGridSize_LostFocus;
            FillAlgorithmCombo();
            FillInterpolationCombo();
            SetBtnFileAssocText();
            SetFilePatternListener();
            _availableExtensions = new HashSet<string>(ParserFactory.GetAvailableExtensions());
            splitContainer.SplitterDistance = pnlDisplay.Height + 3;
            _gridSize = new Size(splitContainer.Panel2.Width / 2, splitContainer.Panel2.Height / 2);
            openFileDialog1.InitialDirectory = _patternsDir;
            Initialize(false);
        }
        #endregion

        #region Private Methods
        private void SetFilePatternListener()
        {
            _loadPipe = new NamedPipeServerStream(LoadPatternPipeName, PipeDirection.In);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _loadPipe.WaitForConnection();
                    StreamReader reader = new StreamReader(_loadPipe);
                    var filePath = reader.ReadLine();
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        Invoke(new Action(() => LoadPatternFileCentered(filePath)));
                    }
                    _loadPipe.Disconnect();
                }
            });
        }

        private void Restart(bool restoreState, bool forcePause = false)
        {
            AbortWorker();
            chkPaintMode.Checked = false;
            _graph.ExitPaintMode();
            Initialize(restoreState);
            Start(!restoreState, forcePause);
        }

        private void Initialize(bool restoreState)
        {
            Color prevColor = _graph == null ? Color.FromArgb(255, 12, 160, 180) : _graph.ForeColor;
            string prevUpperRightInfo = _graph == null ? string.Empty : _algorithmSymbol;
            bool prevShowFps = _graph == null ? true : _graph.ShowFps;
            splitContainer.Panel2.Controls.Remove(_graph);
            Cell[,] exMatrix = null;
            if (restoreState)
            {
                exMatrix = _gol.Matrix;
            }
            _gol = new GameOfLife(_gridSize.Width, _gridSize.Height, _algorithmSymbol, _maximumAge,
                                      _initialOccupation, _initialMargin, _wrapAround);
            if (restoreState)
            {
                _gol.Plot(new Point(0, 0), exMatrix);
            }
            _gol.FireUpdate += jv_FireUpdate;
            _graph = new UniverseGraph(_gridSize.Width, _gridSize.Height)
            {
                Dock = DockStyle.Fill,
                ForeColor = prevColor,
                UpperRightInfo = prevUpperRightInfo,
                ShowFps = prevShowFps,
                AllowDrop = true
            };
            _graph.InterpolationMode = cmbInterpolationMode.SelectedItem != null ? (InterpolationMode) cmbInterpolationMode.SelectedItem : InterpolationMode.Default;
            _graph.PointSelected += Graph_PointSelected;
            _graph.CellPaint += Graph_CellPaint;
            _graph.DragEnter += Graph_DragEnter;
            _graph.DragDropCell += Graph_DragDropCell;
            splitContainer.Panel2.Controls.Add(_graph);
            chkFps.Checked = _graph.ShowFps;
            chkAlgo.Checked = !string.IsNullOrEmpty(_graph.UpperRightInfo);
            chkWrapAround.Checked = _wrapAround;
            txtGridSize.Text = $"{_gridSize.Width}x{_gridSize.Height}";
            picColor.BackColor =  _graph.ForeColor;
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

        private void FillInterpolationCombo()
        {
            cmbInterpolationMode.Items.Clear();
            cmbInterpolationMode.Items.Add(InterpolationMode.Bilinear);
            cmbInterpolationMode.Items.Add(InterpolationMode.NearestNeighbor);
            cmbInterpolationMode.Items.Add(InterpolationMode.HighQualityBilinear);
            cmbInterpolationMode.Items.Add(InterpolationMode.Bicubic);
            cmbInterpolationMode.SelectedIndexChanged -= cmbInterpolationMode_SelectedIndexChanged;
            cmbInterpolationMode.SelectedIndex = 0;
            cmbInterpolationMode.SelectedIndexChanged += cmbInterpolationMode_SelectedIndexChanged;
        }

        /// <summary>
        /// Handles the PointSelected event of the Graph control.
        /// This is fired when the user selects a point in the graph to insert an imported glyph
        /// </summary>
        private void Graph_PointSelected(object sender, PointSelectedEventArgs e)
        {
            _graph.ExitSelectionMode();
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

        private void Start(bool randomize, bool forcePause = false)
        {
            AbortWorker();
            if (randomize)
            {
                _gol.Randomize();
            }
            if (forcePause)
            {
                _gol.Paused = true;
            }
            _workerThread = new Thread(_gol.Play);
            _workerThread.Priority = ThreadPriority.AboveNormal;
            _workerThread.Start();
        }

        private void AbortWorker()
        {
            if (_isRecording && !_avi.IsClosed)
            {
                _avi.Close();
            }
            if (_workerThread != null && _workerThread.IsAlive)
            {
                _workerThread.Abort();
                _workerThread.Join();
            }
        }

        private string GetKeysHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Keyboard Shortcuts");
            sb.AppendLine();
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
            sb.AppendLine("You can drag & drop pattern files here (.cells, .rle or .lif).");
            sb.AppendLine("Click on 'Download Patterns' to download the LifeWiki's pattern collection.");
            sb.AppendLine("Click on 'paint' to set alive/dead cells with the mouse buttons.");
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
                    _graph.ForeColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                    picColor.BackColor = _graph.ForeColor;
                    break;
                case Keys.A:
                    // change algorithm
                    string newAlgorithm;
                    string help = "Enter the new algorithm in ddd/DDD format.\nA living cell still alive iif it has exactly d neighbors.\nAn empty cell is born iif it has exactly D alive neighbors.\nExample: 23/3 = Conway algorithm. 34/34 = Life 34.";
                    if (InputBox.Show("Algorithm", help, _algorithmSymbol, out newAlgorithm) == DialogResult.OK)
                    {
                        _algorithmSymbol = newAlgorithm;
                        Restart(true);
                    }
                    break;
                case Keys.Q:
                    // change to random algorithm
                    newAlgorithm = Algorithm.GetRandomAlgorithm();
                    _algorithmSymbol = newAlgorithm;
                    redraw = true;
                    break;
                case Keys.G:
                    // Change gridsize
                    string newgridSize;
                    if (InputBox.Show("Grid size", "Enter the new Grid Size (format WIDTHxHEIGHT)", $"{_gridSize.Width}x{_gridSize.Height}", out newgridSize) == DialogResult.OK)
                    {
                        var newSizeValues = newgridSize.Split('x');
                        if (newSizeValues.Length == 2)
                        {
                            _gridSize = new Size(int.Parse(newSizeValues[0]), int.Parse(newSizeValues[1]));
                            Restart(true);
                        }
                    }
                    break;
                case Keys.E:
                    // Change maximum age
                    string newMaxAge;
                    if (InputBox.Show("Age", "Enter the new Maximum Age.\nThe maximum age is the maximum number of cycles after which any living cell dies.", _maximumAge.ToString(), out newMaxAge) == DialogResult.OK)
                    {
                        _maximumAge = int.Parse(newMaxAge);
                        Restart(true);
                    }
                    break;
                case Keys.F:
                    _graph.ShowFps = !_graph.ShowFps;
                    break;
                case Keys.L:
                    _graph.UpperRightInfo = string.IsNullOrEmpty(_graph.UpperRightInfo) ? _algorithmSymbol : string.Empty;
                    break;
                case Keys.O:
                    // Chage initial occupation
                    redraw = ChangeOccupation();
                    break;
                case Keys.C:
                    _gol.Clear();
                    break;
                case Keys.P:
                    if (_gol.Paused)
                    {
                        Resume();
                    }
                    else
                    {
                        Pause();
                    }
                    break;
                case Keys.I:
                    var res = openFileDialog1.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        _graph.EnterSelectionMode();
                    }
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
                case Keys.F1:
                    _graph.OverlayInfo = string.IsNullOrEmpty(_graph.OverlayInfo) ? GetKeysHelp() : null;
                    break;
                case Keys.S:
                    if (!_isRecording)
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
            var def = string.Format("{0},{1}", _initialOccupation * 100, _initialMargin * 100);
            if (InputBox.Show("Initial occupation", "Enter the initial occupation percentage (density) optionally followed by the initial margin percentage to not cover: PPP,AAA.\nValues of PPP between 1 and 100\nValues of AAA between 0 and 49.", def, out newOccup) == DialogResult.OK)
            {
                var values = newOccup.Split(',');
                _initialOccupation = double.Parse(values[0]) / 100;
                if (values.Length > 1)
                {
                    _initialMargin = double.Parse(values[1]) / 100;
                }
                return true;
            }
            return false;
        }

        private void StartRecording()
        {
            _gol.Pause();
            _avi = new AviWriter();
            saveFileDialog1.FileName = $"GOLS_Simulation_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _bmpAvi = _avi.Open(saveFileDialog1.FileName, 30, _gridSize.Width, _gridSize.Height);
                _isRecording = true;
                _graph.FootInfo = "Recording... Press 'S' or click the Stop button to finish.";
                _graph.ShowFps = false;
                Resume();
            }
        }

        private void StopRecording()
        {
            _isRecording = false;
            Thread.Sleep(5);
            _graph.FootInfo = string.Empty;
            _graph.ShowFps = true;
            _avi.Close();
            _bmpAvi.Dispose();
            Process.Start(@"explorer", @"/select,""" + saveFileDialog1.FileName + @"""");
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AbortWorker();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            _graph.Focus();
            if (_initialFilePath == null)
            {
                Start(true);
            }
            else
            {
                if (File.Exists(_initialFilePath))
                {
                    LoadPatternFileCentered(_initialFilePath);
                }
            }
        }

        private void jv_FireUpdate(object sender, FireUpdateEventArgs e)
        {
            foreach (Point born in e.Born)
            {
                _graph.PlotBmp(born.X, born.Y, 1);
            }
            foreach (Point dead in e.Dead)
            {
                _graph.PlotBmp(dead.X, dead.Y, 0);
            }
            _graph.Invalidate();
            if (_isRecording)
            {
                RecordFrame();
            }
        }

        private void RecordFrame()
        {
            lock (_graph.UniverseBitmap)
            {
                if (!_avi.IsClosed)
                {
                    using (Graphics g = Graphics.FromImage(_bmpAvi))
                    {
                        g.InterpolationMode = _graph.InterpolationMode;
                        g.ScaleTransform(1.0F, -1.0F);
                        g.TranslateTransform(0.0F, -(float)_bmpAvi.Height);
                        g.DrawImage(_graph.UniverseBitmap, 0, 0, _bmpAvi.Width, _bmpAvi.Height);
                    }
                    _avi.AddFrame();
                }
            }
        }

        private void Pause()
        {
            _gol.Pause();
            _graph.LowerLeftInfo = "< PAUSED >";
            while (!_gol.Paused)
            {
                Thread.Sleep(1);
            }
            SetPlayerButtonState();
        }

        private void Resume()
        {
            _gol.Resume();
            chkPaintMode.Checked = false;
            _graph.ExitPaintMode();
            _graph.LowerLeftInfo = "";
            while (_gol.Paused)
            {
                Thread.Sleep(1);
            }
            SetPlayerButtonState();
        }

        private void ChangeGridSize()
        {
            var newSizeValues = txtGridSize.Text.Split('x');
            if (newSizeValues.Length == 2)
            {
                var newSize = new Size(int.Parse(newSizeValues[0]), int.Parse(newSizeValues[1]));
                if (_gridSize != newSize)
                {
                    _gridSize = newSize;
                    Restart(true);
                    _graph?.Focus();
                }
            }
        }

        // Plots a pattern starting at a given point
        private void PlotPattern(Point startPoint, Pattern pattern)
        {
            var wasPaused = _gol.Paused;
            if (startPoint.X + pattern.Bitmap.GetLength(0) > _gridSize.Width || startPoint.Y + pattern.Bitmap.GetLength(1) > _gridSize.Height)
            {
                startPoint = new Point(0, 0);
                if (pattern.Bitmap.GetLength(0) > _gridSize.Width || pattern.Bitmap.GetLength(1) > _gridSize.Height)
                {
                    //resize grid to fit the pattern
                    _gridSize = new Size(pattern.Bitmap.GetLength(0), pattern.Bitmap.GetLength(1));
                    Restart(true);
                    if (wasPaused)
                    {
                        Pause();
                    }
                }
            }
            if (pattern.Algorithm.HasValue && pattern.Algorithm.Value.Symbol != _gol.AlgorithmSymbol)
            {
                // change algorythm
                _gol.Algorithm = pattern.Algorithm.Value;
                _algorithmSymbol = pattern.Algorithm.Value.Symbol;
                FindSelectAlgorithm(_algorithmSymbol);
                Restart(true);
            }
            Pause();
            _gol.Plot(startPoint, pattern.Bitmap);
            if (!wasPaused)
            {
                Resume();
            }
        }

        // Stops and Clears the current simulation (if any), and loads a centered pattern into the simulation
        private void LoadPatternFileCentered(string filePath)
        {
            var shiftPressed = ModifierKeys.HasFlag(Keys.Shift);
            Pause();
            _gol.Clear();
            Pattern pattern = TryParsePattern(filePath);
            if (pattern == null)
            {
                return;
            }
            openFileDialog1.FileName = filePath;
            var startPoint = new Point(0, 0);
            if (pattern.Bitmap.GetLength(0) > _gridSize.Width || pattern.Bitmap.GetLength(1) > _gridSize.Height)
            {
                //overflow, resize grid to fit the pattern
                _gridSize = new Size(pattern.Bitmap.GetLength(0), pattern.Bitmap.GetLength(1));
                Restart(true);
            }
            else
            {
                // calculate upper-left point to show the pattern centered
                startPoint.X = _gridSize.Width / 2 - pattern.Bitmap.GetLength(0) / 2;
                startPoint.Y = _gridSize.Height / 2 - pattern.Bitmap.GetLength(1) / 2;
            }
            if (pattern.Algorithm.HasValue && pattern.Algorithm.Value.Symbol != _gol.AlgorithmSymbol)
            {
                // change algorythm
                _gol.Algorithm = pattern.Algorithm.Value;
                _algorithmSymbol = pattern.Algorithm.Value.Symbol;
                FindSelectAlgorithm(_algorithmSymbol);
                Restart(true);
            }
            Pause();
            _gol.Plot(startPoint, pattern.Bitmap);
            if (shiftPressed)
            {
                Resume();
            }
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

        private void UnzipPatterns()
        {
            var zip = ZipStorer.Open(_patternZipFile, FileAccess.Read);
            var dir = zip.ReadCentralDir();
            foreach (var entry in dir)
            {
                if (ParserFactory.GetAvailableExtensions().Contains(Path.GetExtension(entry.FilenameInZip)))
                {
                    zip.ExtractFile(entry, Path.Combine(_patternsDir, entry.FilenameInZip));
                }
            }
            zip.Close();
            btnDownloadPatterns.Enabled = true;
            btnDownloadPatterns.Text = "Download Patterns";
            Process.Start("explorer.exe", _patternsDir);
        }

        private void SetPlayerButtonState()
        {
            btnPlay.Image = !_gol.Paused ? imageList1.Images["play_red.ico"] : imageList1.Images["play_blue.ico"];
            btnRecord.Image = _isRecording ? imageList1.Images["rec_red.ico"] : imageList1.Images["rec_blue.ico"];
            btnPause.Image = _gol.Paused ? imageList1.Images["pause_red.ico"] : imageList1.Images["pause_blue.ico"];
        }
        #endregion

        #region Control Events
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_isRecording)
            {
                StopRecording();
                return;
            }
            Pause();
            _gol.Clear();
        }

        private void btnStop_MouseEnter(object sender, EventArgs e)
        {
            btnStop.Image = imageList1.Images["stop_yellow.ico"];
        }

        private void btnStop_MouseLeave(object sender, EventArgs e)
        {
            btnStop.Image = imageList1.Images["stop_blue.ico"];
        }

        private void btnPlay_MouseEnter(object sender, EventArgs e)
        {
            btnPlay.Image = imageList1.Images["play_yellow.ico"];
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            btnPlay.Image = !_gol.Paused ? imageList1.Images["play_red.ico"] : imageList1.Images["play_blue.ico"];
        }

        private void btnPause_MouseEnter(object sender, EventArgs e)
        {
            btnPause.Image = imageList1.Images["pause_yellow.ico"];
        }

        private void btnPause_MouseLeave(object sender, EventArgs e)
        {
            btnPause.Image = _gol.Paused ? imageList1.Images["pause_red.ico"] : imageList1.Images["pause_blue.ico"];
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Resume();
        }

        private void btnRandom_MouseEnter(object sender, EventArgs e)
        {
            btnRandom.Image = imageList1.Images["shuffle_yellow.ico"];
        }

        private void btnRandom_MouseLeave(object sender, EventArgs e)
        {
            btnRandom.Image = imageList1.Images["shuffle_blue.ico"];
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
            Restart(false, _gol.Paused);
        }

        private void btnForward_MouseEnter(object sender, EventArgs e)
        {
            btnForward.Image = imageList1.Images["ff_yellow.ico"];
        }

        private void btnForward_MouseLeave(object sender, EventArgs e)
        {
            btnForward.Image = imageList1.Images["ff_blue.ico"];
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            Pause();
            _gol.DoSteps = (int) txtStepSize.Value;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            _graph.OverlayInfo = string.IsNullOrEmpty(_graph.OverlayInfo) ? GetKeysHelp() : null;
            if (_gol.Paused)
            {
                _gol.DoSteps = 1;
            }
        }

        private void btnHelp_MouseEnter(object sender, EventArgs e)
        {
            btnHelp.Image = imageList1.Images["question_yellow.ico"];
        }

        private void btnHelp_MouseLeave(object sender, EventArgs e)
        {
            btnHelp.Image = imageList1.Images["question_blue.ico"];
        }

        private void txtGridSize_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_gridSizeFocused && txtGridSize.SelectionLength == 0)
            {
                _gridSizeFocused = true;
                txtGridSize.SelectAll();
            }
        }

        private void txtGridSize_Leave(object sender, EventArgs e)
        {
            _gridSizeFocused = false;
        }

        private void txtGridSize_GotFocus(object sender, EventArgs eventArgs)
        {
            if (MouseButtons == MouseButtons.None)
            {
                txtGridSize.SelectAll();
                _gridSizeFocused = true;
            }
        }

        private void picColor_Click(object sender, EventArgs e)
        {
            var dlg = colorDialog1.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                picColor.BackColor = colorDialog1.Color;
                _graph.ForeColor = colorDialog1.Color;
            }
        }

        private void chkFps_CheckedChanged(object sender, EventArgs e)
        {
            _graph.ShowFps = chkFps.Checked;
        }

        private void chkAlgo_CheckedChanged(object sender, EventArgs e)
        {
            _graph.UpperRightInfo = chkAlgo.Checked ? _algorithmSymbol : string.Empty;
        }

        private void chkWrapAround_CheckedChanged(object sender, EventArgs e)
        {
            _wrapAround = chkWrapAround.Checked;
            Restart(true);
        }

        private void chkPaintMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPaintMode.Checked)
            {
                if (!_gol.Paused)
                {
                    Pause();
                }
                _graph.EnterPaintMode();
            }
            else
            {
                _graph.ExitPaintMode();
            }
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

        private void btnRandomColor_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            _graph.ForeColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            picColor.BackColor = _graph.ForeColor;
        }

        private void btnRecord_MouseEnter(object sender, EventArgs e)
        {
            btnRecord.Image = imageList1.Images["rec_yellow.ico"];
        }

        private void btnRecord_MouseLeave(object sender, EventArgs e)
        {
            btnRecord.Image = imageList1.Images["rec_blue.ico"];
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (!_isRecording)
            {
                StartRecording();
            }
        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valueSelected = ((dynamic)cmbAlgorithm.SelectedItem).Value;
            if (valueSelected == "Custom")
            {
                string newAlgorithm;
                string help = "Enter the new algorithm in ddd/DDD format.\nA living cell still alive iif it has exactly d neighbors.\nAn empty cell is born iif it has exactly D alive neighbors.\nExample: 23/3 = Conway algorithm. 34/34 = Life 34.";
                if (InputBox.Show("Algorithm", help, _algorithmSymbol, out newAlgorithm) == DialogResult.OK)
                {
                    _algorithmSymbol = newAlgorithm;
                    Restart(true);
                }
            }
            else if (valueSelected != _algorithmSymbol)
            {
                _algorithmSymbol = valueSelected;
                Restart(true);
            }
            _graph?.Focus();
        }

        private void cmbInterpolationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_graph != null)
            {
                var mode = (InterpolationMode) cmbInterpolationMode.SelectedItem;
                _graph.InterpolationMode = mode;
                _graph.Invalidate();
            }
        }

        private void btnOneToOne_Click(object sender, EventArgs e)
        {
            txtGridSize.Text = $"{_graph.Width}x{_graph.Height}";
            ChangeGridSize();
        }

        private void btnRandomAlgorithm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmbAlgorithm.SelectedIndexChanged -= cmbAlgorithm_SelectedIndexChanged;
                cmbAlgorithm.SelectedIndex = cmbAlgorithm.Items.Count - 1;
                cmbAlgorithm.SelectedIndexChanged += cmbAlgorithm_SelectedIndexChanged;
                _algorithmSymbol = Algorithm.GetRandomAlgorithm();
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
            btnImport.Image = imageList1.Images["eject_yellow.ico"];

        }

        private void btnImport_MouseLeave(object sender, EventArgs e)
        {
            btnImport.Image = imageList1.Images["eject_blue.ico"];
        }

        private void btnImport_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var res = openFileDialog1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    _graph.EnterSelectionMode();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                Process.Start("explorer.exe", _patternsDir);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _gol.Clear();
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

        private void Graph_DragDropCell(object sender, DragEventArgs e)
        {
            _graph.ExitSelectionMode();
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                var file = files[0];
                var pattern = TryParsePattern(file);
                if (pattern != null)
                {
                    openFileDialog1.FileName = file;
                    PlotPattern(new Point(e.X, e.Y), pattern);
                }
            }
        }

        private void Graph_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileName = ((string[])e.Data.GetData("FileName", true))[0];
                if (_availableExtensions.Any(ext => fileName.EndsWith(ext)))
                {
                    e.Effect = DragDropEffects.Copy;
                    _graph.EnterSelectionMode();
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void btnDownloadPatterns_Click(object sender, EventArgs e)
        {
            btnDownloadPatterns.Enabled = false;
            var webClient = new WebClient();
            Directory.CreateDirectory(_patternsDir);
            webClient.DownloadFileAsync(new Uri("http://www.conwaylife.com/patterns/all.zip"), _patternZipFile);
            webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClientOnDownloadProgressChanged;
        }

        private void WebClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var p = Math.Round((decimal)e.BytesReceived / e.TotalBytesToReceive * 100, 0);
            btnDownloadPatterns.Text = e.BytesReceived < e.TotalBytesToReceive ? $"Downloading {p}%..." : "Unzipping...";
        }

        private void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                btnDownloadPatterns.Text = "Unzipping...";
                UnzipPatterns();
            }
            else
            {
                btnDownloadPatterns.Enabled = true;
                btnDownloadPatterns.Text = "Download Patterns";
            }
        }

        private void Graph_CellPaint(object sender, PointSelectedEventArgs e)
        {
            var m = new byte[1, 1];
            m[0, 0] = e.Button != MouseButtons.Left ? (byte)0 : (byte)1;
            _gol.Plot(e.Point, m);
        }


        #endregion


    }
}
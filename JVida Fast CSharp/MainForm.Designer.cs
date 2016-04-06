using System.ComponentModel;
using System.Windows.Forms;

namespace JVida_Fast_CSharp
{
    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlPlayer = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.PictureBox();
            this.btnImport = new System.Windows.Forms.PictureBox();
            this.txtStepSize = new System.Windows.Forms.NumericUpDown();
            this.btnHelp = new System.Windows.Forms.PictureBox();
            this.btnRecord = new System.Windows.Forms.PictureBox();
            this.btnStop = new System.Windows.Forms.PictureBox();
            this.btnRandom = new System.Windows.Forms.PictureBox();
            this.btnForward = new System.Windows.Forms.PictureBox();
            this.btnPause = new System.Windows.Forms.PictureBox();
            this.btnPlay = new System.Windows.Forms.PictureBox();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.cmbInterpolationMode = new System.Windows.Forms.ComboBox();
            this.btnOneToOne = new System.Windows.Forms.PictureBox();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.picColor = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRandomAlgorithm = new System.Windows.Forms.PictureBox();
            this.btnRandomColor = new System.Windows.Forms.PictureBox();
            this.txtGridSize = new System.Windows.Forms.TextBox();
            this.chkWrapAround = new System.Windows.Forms.CheckBox();
            this.pnlDisplay = new System.Windows.Forms.Panel();
            this.btnDownloadPatterns = new System.Windows.Forms.Button();
            this.btnFileAssoc = new System.Windows.Forms.Button();
            this.chkPosition = new System.Windows.Forms.CheckBox();
            this.chkAlgo = new System.Windows.Forms.CheckBox();
            this.chkFps = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.chkPaintMode = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.pnlPlayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStepSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRandom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnForward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlay)).BeginInit();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnOneToOne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRandomAlgorithm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRandomColor)).BeginInit();
            this.pnlDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "avi";
            this.saveFileDialog1.Filter = "Video files (*.avi)|*.avi";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "Enter the new video filename to create";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Cells files|*.cells;*.rle;*.lif";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.flowLayoutPanel);
            this.splitContainer.Size = new System.Drawing.Size(889, 609);
            this.splitContainer.SplitterDistance = 111;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 0;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.pnlPlayer);
            this.flowLayoutPanel.Controls.Add(this.pnlGrid);
            this.flowLayoutPanel.Controls.Add(this.pnlDisplay);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(889, 111);
            this.flowLayoutPanel.TabIndex = 1;
            // 
            // pnlPlayer
            // 
            this.pnlPlayer.BackColor = System.Drawing.Color.White;
            this.pnlPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPlayer.Controls.Add(this.btnClear);
            this.pnlPlayer.Controls.Add(this.btnImport);
            this.pnlPlayer.Controls.Add(this.txtStepSize);
            this.pnlPlayer.Controls.Add(this.btnHelp);
            this.pnlPlayer.Controls.Add(this.btnRecord);
            this.pnlPlayer.Controls.Add(this.btnStop);
            this.pnlPlayer.Controls.Add(this.btnRandom);
            this.pnlPlayer.Controls.Add(this.btnForward);
            this.pnlPlayer.Controls.Add(this.btnPause);
            this.pnlPlayer.Controls.Add(this.btnPlay);
            this.pnlPlayer.Location = new System.Drawing.Point(4, 4);
            this.pnlPlayer.Margin = new System.Windows.Forms.Padding(4);
            this.pnlPlayer.Name = "pnlPlayer";
            this.pnlPlayer.Size = new System.Drawing.Size(251, 100);
            this.pnlPlayer.TabIndex = 2;
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(202, 55);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(39, 39);
            this.btnClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClear.TabIndex = 9;
            this.btnClear.TabStop = false;
            this.toolTip.SetToolTip(this.btnClear, "Clear all");
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.Location = new System.Drawing.Point(202, 7);
            this.btnImport.Margin = new System.Windows.Forms.Padding(0);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(39, 39);
            this.btnImport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnImport.TabIndex = 8;
            this.btnImport.TabStop = false;
            this.toolTip.SetToolTip(this.btnImport, "Load a pattern from a file (.cells, .lif or .rle). Right click to open the patter" +
        "ns folder.");
            this.btnImport.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnImport_MouseClick);
            this.btnImport.MouseEnter += new System.EventHandler(this.btnImport_MouseEnter);
            this.btnImport.MouseLeave += new System.EventHandler(this.btnImport_MouseLeave);
            // 
            // txtStepSize
            // 
            this.txtStepSize.Location = new System.Drawing.Point(90, 62);
            this.txtStepSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtStepSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtStepSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtStepSize.Name = "txtStepSize";
            this.txtStepSize.Size = new System.Drawing.Size(64, 22);
            this.txtStepSize.TabIndex = 7;
            this.toolTip.SetToolTip(this.txtStepSize, "Step count (N steps)");
            this.txtStepSize.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.txtStepSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnHelp
            // 
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.Location = new System.Drawing.Point(7, 52);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(39, 39);
            this.btnHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnHelp.TabIndex = 2;
            this.btnHelp.TabStop = false;
            this.toolTip.SetToolTip(this.btnHelp, "Show/Hide help screen");
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            this.btnHelp.MouseEnter += new System.EventHandler(this.btnHelp_MouseEnter);
            this.btnHelp.MouseLeave += new System.EventHandler(this.btnHelp_MouseLeave);
            // 
            // btnRecord
            // 
            this.btnRecord.Image = ((System.Drawing.Image)(resources.GetObject("btnRecord.Image")));
            this.btnRecord.Location = new System.Drawing.Point(85, 7);
            this.btnRecord.Margin = new System.Windows.Forms.Padding(0);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(39, 39);
            this.btnRecord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnRecord.TabIndex = 2;
            this.btnRecord.TabStop = false;
            this.toolTip.SetToolTip(this.btnRecord, "Start recording an AVI file");
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            this.btnRecord.MouseEnter += new System.EventHandler(this.btnRecord_MouseEnter);
            this.btnRecord.MouseLeave += new System.EventHandler(this.btnRecord_MouseLeave);
            // 
            // btnStop
            // 
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.Location = new System.Drawing.Point(46, 7);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(39, 39);
            this.btnStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnStop.TabIndex = 2;
            this.btnStop.TabStop = false;
            this.toolTip.SetToolTip(this.btnStop, "Clear & Stop");
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            this.btnStop.MouseEnter += new System.EventHandler(this.btnStop_MouseEnter);
            this.btnStop.MouseLeave += new System.EventHandler(this.btnStop_MouseLeave);
            // 
            // btnRandom
            // 
            this.btnRandom.Image = ((System.Drawing.Image)(resources.GetObject("btnRandom.Image")));
            this.btnRandom.Location = new System.Drawing.Point(7, 7);
            this.btnRandom.Margin = new System.Windows.Forms.Padding(0);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(39, 39);
            this.btnRandom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnRandom.TabIndex = 3;
            this.btnRandom.TabStop = false;
            this.toolTip.SetToolTip(this.btnRandom, "Randomize & Restart (right-click to config)");
            this.btnRandom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnRandom_MouseClick);
            this.btnRandom.MouseEnter += new System.EventHandler(this.btnRandom_MouseEnter);
            this.btnRandom.MouseLeave += new System.EventHandler(this.btnRandom_MouseLeave);
            // 
            // btnForward
            // 
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.Location = new System.Drawing.Point(46, 52);
            this.btnForward.Margin = new System.Windows.Forms.Padding(0);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(39, 39);
            this.btnForward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnForward.TabIndex = 4;
            this.btnForward.TabStop = false;
            this.toolTip.SetToolTip(this.btnForward, "Fast forward N steps");
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            this.btnForward.MouseEnter += new System.EventHandler(this.btnForward_MouseEnter);
            this.btnForward.MouseLeave += new System.EventHandler(this.btnForward_MouseLeave);
            // 
            // btnPause
            // 
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.Location = new System.Drawing.Point(163, 7);
            this.btnPause.Margin = new System.Windows.Forms.Padding(0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(39, 39);
            this.btnPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnPause.TabIndex = 5;
            this.btnPause.TabStop = false;
            this.toolTip.SetToolTip(this.btnPause, "Pause");
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            this.btnPause.MouseEnter += new System.EventHandler(this.btnPause_MouseEnter);
            this.btnPause.MouseLeave += new System.EventHandler(this.btnPause_MouseLeave);
            // 
            // btnPlay
            // 
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.Location = new System.Drawing.Point(124, 7);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(39, 39);
            this.btnPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnPlay.TabIndex = 6;
            this.btnPlay.TabStop = false;
            this.toolTip.SetToolTip(this.btnPlay, "Start / Resume");
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            this.btnPlay.MouseEnter += new System.EventHandler(this.btnPlay_MouseEnter);
            this.btnPlay.MouseLeave += new System.EventHandler(this.btnPlay_MouseLeave);
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.cmbInterpolationMode);
            this.pnlGrid.Controls.Add(this.btnOneToOne);
            this.pnlGrid.Controls.Add(this.chkPaintMode);
            this.pnlGrid.Controls.Add(this.cmbAlgorithm);
            this.pnlGrid.Controls.Add(this.picColor);
            this.pnlGrid.Controls.Add(this.label2);
            this.pnlGrid.Controls.Add(this.label3);
            this.pnlGrid.Controls.Add(this.label4);
            this.pnlGrid.Controls.Add(this.label1);
            this.pnlGrid.Controls.Add(this.btnRandomAlgorithm);
            this.pnlGrid.Controls.Add(this.btnRandomColor);
            this.pnlGrid.Controls.Add(this.txtGridSize);
            this.pnlGrid.Controls.Add(this.chkWrapAround);
            this.pnlGrid.Location = new System.Drawing.Point(263, 4);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(4);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(317, 102);
            this.pnlGrid.TabIndex = 3;
            // 
            // cmbInterpolationMode
            // 
            this.cmbInterpolationMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInterpolationMode.DropDownWidth = 150;
            this.cmbInterpolationMode.FormattingEnabled = true;
            this.cmbInterpolationMode.Location = new System.Drawing.Point(90, 22);
            this.cmbInterpolationMode.Name = "cmbInterpolationMode";
            this.cmbInterpolationMode.Size = new System.Drawing.Size(142, 24);
            this.cmbInterpolationMode.TabIndex = 11;
            this.toolTip.SetToolTip(this.cmbInterpolationMode, "Specifies the interpolation mode for the graphics");
            this.cmbInterpolationMode.SelectedIndexChanged += new System.EventHandler(this.cmbInterpolationMode_SelectedIndexChanged);
            // 
            // btnOneToOne
            // 
            this.btnOneToOne.Image = ((System.Drawing.Image)(resources.GetObject("btnOneToOne.Image")));
            this.btnOneToOne.Location = new System.Drawing.Point(62, 2);
            this.btnOneToOne.Margin = new System.Windows.Forms.Padding(0);
            this.btnOneToOne.Name = "btnOneToOne";
            this.btnOneToOne.Size = new System.Drawing.Size(21, 20);
            this.btnOneToOne.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnOneToOne.TabIndex = 9;
            this.btnOneToOne.TabStop = false;
            this.toolTip.SetToolTip(this.btnOneToOne, "Set the grid size as 1 cell per pixel");
            this.btnOneToOne.Click += new System.EventHandler(this.btnOneToOne_Click);
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlgorithm.DropDownWidth = 200;
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Location = new System.Drawing.Point(7, 72);
            this.cmbAlgorithm.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAlgorithm.MaxDropDownItems = 10;
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(190, 24);
            this.cmbAlgorithm.TabIndex = 8;
            this.cmbAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cmbAlgorithm_SelectedIndexChanged);
            // 
            // picColor
            // 
            this.picColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picColor.Location = new System.Drawing.Point(239, 24);
            this.picColor.Margin = new System.Windows.Forms.Padding(4);
            this.picColor.Name = "picColor";
            this.picColor.Size = new System.Drawing.Size(33, 20);
            this.picColor.TabIndex = 7;
            this.picColor.TabStop = false;
            this.picColor.Click += new System.EventHandler(this.picColor_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(237, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Color";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 52);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Algorithm";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(90, 4);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Interpolation mode";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Size";
            // 
            // btnRandomAlgorithm
            // 
            this.btnRandomAlgorithm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnRandomAlgorithm.Image = ((System.Drawing.Image)(resources.GetObject("btnRandomAlgorithm.Image")));
            this.btnRandomAlgorithm.Location = new System.Drawing.Point(201, 74);
            this.btnRandomAlgorithm.Margin = new System.Windows.Forms.Padding(0);
            this.btnRandomAlgorithm.Name = "btnRandomAlgorithm";
            this.btnRandomAlgorithm.Size = new System.Drawing.Size(31, 20);
            this.btnRandomAlgorithm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnRandomAlgorithm.TabIndex = 3;
            this.btnRandomAlgorithm.TabStop = false;
            this.toolTip.SetToolTip(this.btnRandomAlgorithm, "Select a known random algorithm (right-click for any random algorithm)");
            this.btnRandomAlgorithm.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnRandomAlgorithm_MouseClick);
            // 
            // btnRandomColor
            // 
            this.btnRandomColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnRandomColor.Image = ((System.Drawing.Image)(resources.GetObject("btnRandomColor.Image")));
            this.btnRandomColor.Location = new System.Drawing.Point(277, 24);
            this.btnRandomColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnRandomColor.Name = "btnRandomColor";
            this.btnRandomColor.Size = new System.Drawing.Size(31, 20);
            this.btnRandomColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnRandomColor.TabIndex = 3;
            this.btnRandomColor.TabStop = false;
            this.toolTip.SetToolTip(this.btnRandomColor, "Pick a random color");
            this.btnRandomColor.Click += new System.EventHandler(this.btnRandomColor_Click);
            // 
            // txtGridSize
            // 
            this.txtGridSize.AcceptsReturn = true;
            this.txtGridSize.Location = new System.Drawing.Point(7, 23);
            this.txtGridSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtGridSize.Name = "txtGridSize";
            this.txtGridSize.Size = new System.Drawing.Size(76, 22);
            this.txtGridSize.TabIndex = 5;
            this.txtGridSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGridSize_KeyPress);
            this.txtGridSize.Leave += new System.EventHandler(this.txtGridSize_Leave);
            this.txtGridSize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtGridSize_MouseUp);
            // 
            // chkWrapAround
            // 
            this.chkWrapAround.AutoSize = true;
            this.chkWrapAround.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWrapAround.Location = new System.Drawing.Point(254, 72);
            this.chkWrapAround.Margin = new System.Windows.Forms.Padding(4);
            this.chkWrapAround.Name = "chkWrapAround";
            this.chkWrapAround.Size = new System.Drawing.Size(56, 21);
            this.chkWrapAround.TabIndex = 4;
            this.chkWrapAround.Text = "wrap";
            this.toolTip.SetToolTip(this.chkWrapAround, "Wrap around the limits");
            this.chkWrapAround.UseVisualStyleBackColor = true;
            this.chkWrapAround.CheckedChanged += new System.EventHandler(this.chkWrapAround_CheckedChanged);
            // 
            // pnlDisplay
            // 
            this.pnlDisplay.BackColor = System.Drawing.Color.White;
            this.pnlDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDisplay.Controls.Add(this.btnDownloadPatterns);
            this.pnlDisplay.Controls.Add(this.btnFileAssoc);
            this.pnlDisplay.Controls.Add(this.chkPosition);
            this.pnlDisplay.Controls.Add(this.chkAlgo);
            this.pnlDisplay.Controls.Add(this.chkFps);
            this.pnlDisplay.Location = new System.Drawing.Point(588, 4);
            this.pnlDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDisplay.Name = "pnlDisplay";
            this.pnlDisplay.Size = new System.Drawing.Size(232, 102);
            this.pnlDisplay.TabIndex = 4;
            // 
            // btnDownloadPatterns
            // 
            this.btnDownloadPatterns.Location = new System.Drawing.Point(12, 68);
            this.btnDownloadPatterns.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownloadPatterns.Name = "btnDownloadPatterns";
            this.btnDownloadPatterns.Size = new System.Drawing.Size(149, 28);
            this.btnDownloadPatterns.TabIndex = 10;
            this.btnDownloadPatterns.Text = "Download Patterns";
            this.toolTip.SetToolTip(this.btnDownloadPatterns, "Download the pattern collection from LifeWiki");
            this.btnDownloadPatterns.UseVisualStyleBackColor = true;
            this.btnDownloadPatterns.Click += new System.EventHandler(this.btnDownloadPatterns_Click);
            // 
            // btnFileAssoc
            // 
            this.btnFileAssoc.Location = new System.Drawing.Point(12, 40);
            this.btnFileAssoc.Margin = new System.Windows.Forms.Padding(4);
            this.btnFileAssoc.Name = "btnFileAssoc";
            this.btnFileAssoc.Size = new System.Drawing.Size(149, 28);
            this.btnFileAssoc.TabIndex = 8;
            this.btnFileAssoc.Text = "Set file assoc.";
            this.toolTip.SetToolTip(this.btnFileAssoc, "Set the file associations to open .cells and .rle files");
            this.btnFileAssoc.UseVisualStyleBackColor = true;
            this.btnFileAssoc.Click += new System.EventHandler(this.btnSetFileAssoc_Click);
            // 
            // chkPosition
            // 
            this.chkPosition.AutoSize = true;
            this.chkPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPosition.Location = new System.Drawing.Point(147, 11);
            this.chkPosition.Margin = new System.Windows.Forms.Padding(4);
            this.chkPosition.Name = "chkPosition";
            this.chkPosition.Size = new System.Drawing.Size(75, 21);
            this.chkPosition.TabIndex = 5;
            this.chkPosition.Text = "position";
            this.chkPosition.UseVisualStyleBackColor = true;
            this.chkPosition.CheckedChanged += new System.EventHandler(this.chkPosition_CheckedChanged);
            // 
            // chkAlgo
            // 
            this.chkAlgo.AutoSize = true;
            this.chkAlgo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAlgo.Location = new System.Drawing.Point(61, 11);
            this.chkAlgo.Margin = new System.Windows.Forms.Padding(4);
            this.chkAlgo.Name = "chkAlgo";
            this.chkAlgo.Size = new System.Drawing.Size(84, 21);
            this.chkAlgo.TabIndex = 6;
            this.chkAlgo.Text = "algorithm";
            this.toolTip.SetToolTip(this.chkAlgo, "Show current algorithm");
            this.chkAlgo.UseVisualStyleBackColor = true;
            this.chkAlgo.CheckedChanged += new System.EventHandler(this.chkAlgo_CheckedChanged);
            // 
            // chkFps
            // 
            this.chkFps.AutoSize = true;
            this.chkFps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkFps.Location = new System.Drawing.Point(12, 11);
            this.chkFps.Margin = new System.Windows.Forms.Padding(4);
            this.chkFps.Name = "chkFps";
            this.chkFps.Size = new System.Drawing.Size(45, 21);
            this.chkFps.TabIndex = 7;
            this.chkFps.Text = "fps";
            this.toolTip.SetToolTip(this.chkFps, "Show frames per second");
            this.chkFps.UseVisualStyleBackColor = true;
            this.chkFps.CheckedChanged += new System.EventHandler(this.chkFps_CheckedChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "alien_black_help.ico");
            this.imageList1.Images.SetKeyName(1, "alien_gray_help.ico");
            this.imageList1.Images.SetKeyName(2, "eject_blue.ico");
            this.imageList1.Images.SetKeyName(3, "eject_green.ico");
            this.imageList1.Images.SetKeyName(4, "eject_yellow.ico");
            this.imageList1.Images.SetKeyName(5, "ff_blue.ico");
            this.imageList1.Images.SetKeyName(6, "ff_greeen.ico");
            this.imageList1.Images.SetKeyName(7, "ff_yellow.ico");
            this.imageList1.Images.SetKeyName(8, "pause_blue.ico");
            this.imageList1.Images.SetKeyName(9, "pause_green.ico");
            this.imageList1.Images.SetKeyName(10, "pause_yellow.ico");
            this.imageList1.Images.SetKeyName(11, "play_blue.ico");
            this.imageList1.Images.SetKeyName(12, "play_green.ico");
            this.imageList1.Images.SetKeyName(13, "play_yellow.ico");
            this.imageList1.Images.SetKeyName(14, "shuffle_blue.ico");
            this.imageList1.Images.SetKeyName(15, "shuffle_yellow.ico");
            this.imageList1.Images.SetKeyName(16, "stop_blue.ico");
            this.imageList1.Images.SetKeyName(17, "stop_green.ico");
            this.imageList1.Images.SetKeyName(18, "stop_yellow.ico");
            this.imageList1.Images.SetKeyName(19, "question_blue.ico");
            this.imageList1.Images.SetKeyName(20, "question_green.ico");
            this.imageList1.Images.SetKeyName(21, "question_yellow.ico");
            this.imageList1.Images.SetKeyName(22, "rec_red.ico");
            this.imageList1.Images.SetKeyName(23, "rec_yellow.ico");
            this.imageList1.Images.SetKeyName(24, "rec_blue.ico");
            this.imageList1.Images.SetKeyName(25, "pause_red.ico");
            this.imageList1.Images.SetKeyName(26, "play_red.ico");
            // 
            // colorDialog1
            // 
            this.colorDialog1.FullOpen = true;
            // 
            // chkPaintMode
            // 
            this.chkPaintMode.AutoSize = true;
            this.chkPaintMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPaintMode.Location = new System.Drawing.Point(254, 50);
            this.chkPaintMode.Margin = new System.Windows.Forms.Padding(4);
            this.chkPaintMode.Name = "chkPaintMode";
            this.chkPaintMode.Size = new System.Drawing.Size(57, 21);
            this.chkPaintMode.TabIndex = 5;
            this.chkPaintMode.Text = "paint";
            this.toolTip.SetToolTip(this.chkPaintMode, "Enable drawing cells into the graph (left click alive cell, right click dead cell" +
        ")");
            this.chkPaintMode.UseVisualStyleBackColor = true;
            this.chkPaintMode.CheckedChanged += new System.EventHandler(this.chkPaintMode_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 609);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Game Of Life Simulator";
            this.splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.flowLayoutPanel.ResumeLayout(false);
            this.pnlPlayer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStepSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRandom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnForward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlay)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.pnlGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnOneToOne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRandomAlgorithm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRandomColor)).EndInit();
            this.pnlDisplay.ResumeLayout(false);
            this.pnlDisplay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        private SplitContainer splitContainer;
        private ImageList imageList1;
        private ToolTip toolTip;
        private FlowLayoutPanel flowLayoutPanel;
        private Panel pnlPlayer;
        private NumericUpDown txtStepSize;
        private PictureBox btnStop;
        private PictureBox btnRandom;
        private PictureBox btnForward;
        private PictureBox btnPause;
        private PictureBox btnPlay;
        private PictureBox btnHelp;
        private Panel pnlGrid;
        private Label label1;
        private TextBox txtGridSize;
        private CheckBox chkWrapAround;
        private PictureBox picColor;
        private Label label2;
        private ColorDialog colorDialog1;
        private PictureBox btnRandomColor;
        private PictureBox btnRecord;
        private Panel pnlDisplay;
        private CheckBox chkPosition;
        private CheckBox chkAlgo;
        private CheckBox chkFps;
        private ComboBox cmbAlgorithm;
        private Label label3;
        private PictureBox btnRandomAlgorithm;
        private PictureBox btnOneToOne;
        private PictureBox btnImport;
        private PictureBox btnClear;
        private Button btnFileAssoc;
        private Button btnDownloadPatterns;
        private ComboBox cmbInterpolationMode;
        private Label label4;
        private CheckBox chkPaintMode;
    }
}

namespace JVida_Fast_CSharp
{
    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.btnOneToOne = new System.Windows.Forms.PictureBox();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.picColor = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRandomAlgorithm = new System.Windows.Forms.PictureBox();
            this.btnRandomColor = new System.Windows.Forms.PictureBox();
            this.txtGridSize = new System.Windows.Forms.TextBox();
            this.chkWrapAround = new System.Windows.Forms.CheckBox();
            this.pnlDisplay = new System.Windows.Forms.Panel();
            this.chkPosition = new System.Windows.Forms.CheckBox();
            this.chkAlgo = new System.Windows.Forms.CheckBox();
            this.chkFps = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
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
            this.openFileDialog1.Filter = "Cells files|*.cells;*.rle";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.flowLayoutPanel);
            this.splitContainer.Size = new System.Drawing.Size(868, 601);
            this.splitContainer.SplitterDistance = 86;
            this.splitContainer.TabIndex = 0;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Controls.Add(this.pnlPlayer);
            this.flowLayoutPanel.Controls.Add(this.pnlGrid);
            this.flowLayoutPanel.Controls.Add(this.pnlDisplay);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(868, 86);
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
            this.pnlPlayer.Location = new System.Drawing.Point(3, 3);
            this.pnlPlayer.Name = "pnlPlayer";
            this.pnlPlayer.Size = new System.Drawing.Size(207, 82);
            this.pnlPlayer.TabIndex = 2;
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(174, 49);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(24, 24);
            this.btnClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClear.TabIndex = 9;
            this.btnClear.TabStop = false;
            this.toolTip.SetToolTip(this.btnClear, "Clear all");
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.Location = new System.Drawing.Point(170, 10);
            this.btnImport.Margin = new System.Windows.Forms.Padding(0);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(32, 32);
            this.btnImport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnImport.TabIndex = 8;
            this.btnImport.TabStop = false;
            this.toolTip.SetToolTip(this.btnImport, "Import a pattern file (.cells, etc)");
            this.btnImport.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnImport_MouseClick);
            this.btnImport.MouseEnter += new System.EventHandler(this.btnImport_MouseEnter);
            this.btnImport.MouseLeave += new System.EventHandler(this.btnImport_MouseLeave);
            // 
            // txtStepSize
            // 
            this.txtStepSize.Location = new System.Drawing.Point(74, 50);
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
            this.txtStepSize.Size = new System.Drawing.Size(65, 20);
            this.txtStepSize.TabIndex = 7;
            this.toolTip.SetToolTip(this.txtStepSize, "Step count (N steps)");
            this.txtStepSize.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.txtStepSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtStepSize.ValueChanged += new System.EventHandler(this.txtStepSize_ValueChanged);
            // 
            // btnHelp
            // 
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.Location = new System.Drawing.Point(5, 42);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(32, 32);
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
            this.btnRecord.Location = new System.Drawing.Point(71, 10);
            this.btnRecord.Margin = new System.Windows.Forms.Padding(0);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(32, 32);
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
            this.btnStop.Location = new System.Drawing.Point(38, 10);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(32, 32);
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
            this.btnRandom.Location = new System.Drawing.Point(5, 10);
            this.btnRandom.Margin = new System.Windows.Forms.Padding(0);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(32, 32);
            this.btnRandom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnRandom.TabIndex = 3;
            this.btnRandom.TabStop = false;
            this.toolTip.SetToolTip(this.btnRandom, "Randomize & Restart (right-click to config)");
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            this.btnRandom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnRandom_MouseClick);
            this.btnRandom.MouseEnter += new System.EventHandler(this.btnRandom_MouseEnter);
            this.btnRandom.MouseLeave += new System.EventHandler(this.btnRandom_MouseLeave);
            // 
            // btnForward
            // 
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.Location = new System.Drawing.Point(39, 42);
            this.btnForward.Margin = new System.Windows.Forms.Padding(0);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(32, 32);
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
            this.btnPause.Location = new System.Drawing.Point(137, 10);
            this.btnPause.Margin = new System.Windows.Forms.Padding(0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(32, 32);
            this.btnPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnPause.TabIndex = 5;
            this.btnPause.TabStop = false;
            this.toolTip.SetToolTip(this.btnPause, "Pause / Step");
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            this.btnPause.MouseEnter += new System.EventHandler(this.btnPause_MouseEnter);
            this.btnPause.MouseLeave += new System.EventHandler(this.btnPause_MouseLeave);
            // 
            // btnPlay
            // 
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.Location = new System.Drawing.Point(104, 10);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(32, 32);
            this.btnPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnPlay.TabIndex = 6;
            this.btnPlay.TabStop = false;
            this.toolTip.SetToolTip(this.btnPlay, "Resume");
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            this.btnPlay.MouseEnter += new System.EventHandler(this.btnPlay_MouseEnter);
            this.btnPlay.MouseLeave += new System.EventHandler(this.btnPlay_MouseLeave);
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.btnOneToOne);
            this.pnlGrid.Controls.Add(this.cmbAlgorithm);
            this.pnlGrid.Controls.Add(this.picColor);
            this.pnlGrid.Controls.Add(this.label2);
            this.pnlGrid.Controls.Add(this.label3);
            this.pnlGrid.Controls.Add(this.label1);
            this.pnlGrid.Controls.Add(this.btnRandomAlgorithm);
            this.pnlGrid.Controls.Add(this.btnRandomColor);
            this.pnlGrid.Controls.Add(this.txtGridSize);
            this.pnlGrid.Controls.Add(this.chkWrapAround);
            this.pnlGrid.Location = new System.Drawing.Point(216, 3);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(208, 83);
            this.pnlGrid.TabIndex = 3;
            // 
            // btnOneToOne
            // 
            this.btnOneToOne.Image = ((System.Drawing.Image)(resources.GetObject("btnOneToOne.Image")));
            this.btnOneToOne.Location = new System.Drawing.Point(74, 20);
            this.btnOneToOne.Margin = new System.Windows.Forms.Padding(0);
            this.btnOneToOne.Name = "btnOneToOne";
            this.btnOneToOne.Size = new System.Drawing.Size(16, 16);
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
            this.cmbAlgorithm.Location = new System.Drawing.Point(5, 57);
            this.cmbAlgorithm.MaxDropDownItems = 10;
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(168, 21);
            this.cmbAlgorithm.TabIndex = 8;
            this.cmbAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cmbAlgorithm_SelectedIndexChanged);
            // 
            // picColor
            // 
            this.picColor.BackColor = System.Drawing.Color.Red;
            this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picColor.Location = new System.Drawing.Point(148, 19);
            this.picColor.Name = "picColor";
            this.picColor.Size = new System.Drawing.Size(25, 17);
            this.picColor.TabIndex = 7;
            this.picColor.TabStop = false;
            this.picColor.Click += new System.EventHandler(this.picColor_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(146, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Color";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Algorithm";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Size";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // btnRandomAlgorithm
            // 
            this.btnRandomAlgorithm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.btnRandomAlgorithm.Image = ((System.Drawing.Image)(resources.GetObject("btnRandomAlgorithm.Image")));
            this.btnRandomAlgorithm.Location = new System.Drawing.Point(176, 58);
            this.btnRandomAlgorithm.Margin = new System.Windows.Forms.Padding(0);
            this.btnRandomAlgorithm.Name = "btnRandomAlgorithm";
            this.btnRandomAlgorithm.Size = new System.Drawing.Size(24, 17);
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
            this.btnRandomColor.Location = new System.Drawing.Point(176, 19);
            this.btnRandomColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnRandomColor.Name = "btnRandomColor";
            this.btnRandomColor.Size = new System.Drawing.Size(24, 17);
            this.btnRandomColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnRandomColor.TabIndex = 3;
            this.btnRandomColor.TabStop = false;
            this.toolTip.SetToolTip(this.btnRandomColor, "Pick a random color");
            this.btnRandomColor.Click += new System.EventHandler(this.btnRandomColor_Click);
            // 
            // txtGridSize
            // 
            this.txtGridSize.AcceptsReturn = true;
            this.txtGridSize.Location = new System.Drawing.Point(5, 18);
            this.txtGridSize.Name = "txtGridSize";
            this.txtGridSize.Size = new System.Drawing.Size(65, 20);
            this.txtGridSize.TabIndex = 5;
            this.txtGridSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGridSize_KeyPress);
            this.txtGridSize.Leave += new System.EventHandler(this.txtGridSize_Leave);
            this.txtGridSize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtGridSize_MouseUp);
            // 
            // chkWrapAround
            // 
            this.chkWrapAround.AutoSize = true;
            this.chkWrapAround.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWrapAround.Location = new System.Drawing.Point(97, 19);
            this.chkWrapAround.Name = "chkWrapAround";
            this.chkWrapAround.Size = new System.Drawing.Size(47, 18);
            this.chkWrapAround.TabIndex = 4;
            this.chkWrapAround.Text = "wrap";
            this.toolTip.SetToolTip(this.chkWrapAround, "Wrap around the limits");
            this.chkWrapAround.UseCompatibleTextRendering = true;
            this.chkWrapAround.UseVisualStyleBackColor = true;
            this.chkWrapAround.CheckedChanged += new System.EventHandler(this.chkWrapAround_CheckedChanged);
            // 
            // pnlDisplay
            // 
            this.pnlDisplay.BackColor = System.Drawing.Color.White;
            this.pnlDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDisplay.Controls.Add(this.chkPosition);
            this.pnlDisplay.Controls.Add(this.chkAlgo);
            this.pnlDisplay.Controls.Add(this.chkFps);
            this.pnlDisplay.Location = new System.Drawing.Point(430, 3);
            this.pnlDisplay.Name = "pnlDisplay";
            this.pnlDisplay.Size = new System.Drawing.Size(114, 83);
            this.pnlDisplay.TabIndex = 4;
            // 
            // chkPosition
            // 
            this.chkPosition.AutoSize = true;
            this.chkPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPosition.Location = new System.Drawing.Point(9, 9);
            this.chkPosition.Name = "chkPosition";
            this.chkPosition.Size = new System.Drawing.Size(93, 18);
            this.chkPosition.TabIndex = 5;
            this.chkPosition.Text = "Show position";
            this.chkPosition.UseCompatibleTextRendering = true;
            this.chkPosition.UseVisualStyleBackColor = true;
            this.chkPosition.CheckedChanged += new System.EventHandler(this.chkPosition_CheckedChanged);
            // 
            // chkAlgo
            // 
            this.chkAlgo.AutoSize = true;
            this.chkAlgo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAlgo.Location = new System.Drawing.Point(9, 57);
            this.chkAlgo.Name = "chkAlgo";
            this.chkAlgo.Size = new System.Drawing.Size(100, 18);
            this.chkAlgo.TabIndex = 6;
            this.chkAlgo.Text = "Show algorithm";
            this.toolTip.SetToolTip(this.chkAlgo, "Show current algorithm");
            this.chkAlgo.UseCompatibleTextRendering = true;
            this.chkAlgo.UseVisualStyleBackColor = true;
            this.chkAlgo.CheckedChanged += new System.EventHandler(this.chkAlgo_CheckedChanged);
            // 
            // chkFps
            // 
            this.chkFps.AutoSize = true;
            this.chkFps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkFps.Location = new System.Drawing.Point(9, 33);
            this.chkFps.Name = "chkFps";
            this.chkFps.Size = new System.Drawing.Size(69, 18);
            this.chkFps.TabIndex = 7;
            this.chkFps.Text = "Show fps";
            this.toolTip.SetToolTip(this.chkFps, "Show frames per second");
            this.chkFps.UseCompatibleTextRendering = true;
            this.chkFps.UseVisualStyleBackColor = true;
            this.chkFps.CheckedChanged += new System.EventHandler(this.chkFps_CheckedChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "button_black_pause.ico");
            this.imageList1.Images.SetKeyName(1, "button_black_play.ico");
            this.imageList1.Images.SetKeyName(2, "button_black_rec.ico");
            this.imageList1.Images.SetKeyName(3, "button_black_stop.ico");
            this.imageList1.Images.SetKeyName(4, "button_grey_pause.ico");
            this.imageList1.Images.SetKeyName(5, "button_grey_play.ico");
            this.imageList1.Images.SetKeyName(6, "button_grey_rec.ico");
            this.imageList1.Images.SetKeyName(7, "button_grey_stop.ico");
            this.imageList1.Images.SetKeyName(8, "button_black_random.ico");
            this.imageList1.Images.SetKeyName(9, "button_grey_random.ico");
            this.imageList1.Images.SetKeyName(10, "button_black_last.ico");
            this.imageList1.Images.SetKeyName(11, "button_grey_last.ico");
            this.imageList1.Images.SetKeyName(12, "button_black_ffw.ico");
            this.imageList1.Images.SetKeyName(13, "button_grey_ffw.ico");
            this.imageList1.Images.SetKeyName(14, "alien_black_help.ico");
            this.imageList1.Images.SetKeyName(15, "alien_gray_help.ico");
            this.imageList1.Images.SetKeyName(16, "button_black_eject.ico");
            this.imageList1.Images.SetKeyName(17, "button_grey_eject.ico");
            this.imageList1.Images.SetKeyName(18, "button_blue_eject.ico");
            // 
            // colorDialog1
            // 
            this.colorDialog1.FullOpen = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 601);
            this.Controls.Add(this.splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
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

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Panel pnlPlayer;
        private System.Windows.Forms.NumericUpDown txtStepSize;
        private System.Windows.Forms.PictureBox btnStop;
        private System.Windows.Forms.PictureBox btnRandom;
        private System.Windows.Forms.PictureBox btnForward;
        private System.Windows.Forms.PictureBox btnPause;
        private System.Windows.Forms.PictureBox btnPlay;
        private System.Windows.Forms.PictureBox btnHelp;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGridSize;
        private System.Windows.Forms.CheckBox chkWrapAround;
        private System.Windows.Forms.PictureBox picColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.PictureBox btnRandomColor;
        private System.Windows.Forms.PictureBox btnRecord;
        private System.Windows.Forms.Panel pnlDisplay;
        private System.Windows.Forms.CheckBox chkPosition;
        private System.Windows.Forms.CheckBox chkAlgo;
        private System.Windows.Forms.CheckBox chkFps;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox btnRandomAlgorithm;
        private System.Windows.Forms.PictureBox btnOneToOne;
        private System.Windows.Forms.PictureBox btnImport;
        private System.Windows.Forms.PictureBox btnClear;
    }
}

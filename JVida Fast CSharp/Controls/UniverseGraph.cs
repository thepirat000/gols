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
    using System.Windows.Forms;

    public partial class UniverseGraph : UserControl
    {
        #region Fields
        private Color backColor = Color.Black;
        private Color foreColor = Color.Red;
        private Color fontColor = Color.LightBlue;
        private int maxX;
        private int maxY;
        private BitmapLocker imgBit = new BitmapLocker();
        private Bitmap bmp;
        private Rectangle region;
        private long qty = 0;
        private Stopwatch stp = new Stopwatch();
        private Font font = new Font("Times New Roman", 9);
        #endregion

        #region Properties
        public override Color BackColor
        {
            get { return this.backColor; }
            set { this.backColor = value; }
        }

        public override Color ForeColor
        {
            get { return this.foreColor; }
            set { this.foreColor = value; }
        }

        public Color FontColor
        {
            get { return this.fontColor; }
            set { this.fontColor = value; }
        }

        public Bitmap UniverseBitmap
        {
            get { return this.bmp; }
        }

        public string OverlayInfo { get; set; }

        public string FootInfo { get; set; }

        public bool ShowFps { get; set; }

        public string UpperRightInfo { get; set; }
        #endregion

        #region Constructor
        public UniverseGraph(int maxX, int maxY)
        {
            Resize += this.Graph_Resize;
            Paint += this.Graph_Paint;

            // This call is required by the Windows Form Designer.
            this.InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            this.maxX = maxX;
            this.maxY = maxY;
            this.Initialize();
        } 
        #endregion

        #region Public Methods
        /// <summary>
        /// Draw the bitmap at the given position
        /// </summary>
        public void PlotBmp(int x, int y, byte state)
        {
            lock (this.bmp)
            {
                this.bmp.SetPixel(x, y, state > 0 ? this.ForeColor : this.BackColor);
            }
        }

        public void Restart()
        {
            if (null != this.bmp)
            {
                this.bmp.Dispose();
            }
            this.Initialize();
        }
        #endregion

        #region Private Methods
        public void Initialize()
        {
            this.ShowFps = true;
            this.OverlayInfo = null;
            this.bmp = new Bitmap(this.maxX, this.maxY, PixelFormat.Format24bppRgb);
            this.ClearBmp();
        }

        private void ClearBmp()
        {
            this.imgBit.LockBitmap(this.bmp);
            for (int i = 0; i <= this.bmp.Width - 1; i++)
            {
                for (int j = 0; j <= this.bmp.Height - 1; j++)
                {
                    this.imgBit.SetPixel(i, j, this.BackColor);
                }
            }
            this.imgBit.UnlockBitmap(this.bmp);
        }

        private void Graph_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Brush br = new SolidBrush(this.FontColor);
            Graphics g = e.Graphics;
            if (!this.stp.IsRunning || this.qty > 10000)
            {
                this.stp.Start();
                this.qty = 0;
            }
            this.qty += 1;
            lock (this.bmp)
            {
                this.region = new Rectangle(0, 0, this.maxX - 1, this.maxY - 1);
                Rectangle rect = this.ClientRectangle;
                g.DrawImage(this.bmp, rect, this.region, GraphicsUnit.Pixel);
                if (this.ShowFps)
                {
                    string info = string.Format("{0} fps", (1000 * this.qty / this.stp.ElapsedMilliseconds));
                    SizeF size = g.MeasureString(info, this.font);
                    g.DrawString(info, this.font, br, this.Width - size.Width, this.Height - size.Height);
                }
                if (!string.IsNullOrEmpty(this.OverlayInfo))
                {
                    g.DrawString(this.OverlayInfo, this.font, br, 0, 0);
                }
                if (!string.IsNullOrEmpty(this.FootInfo))
                {
                    SizeF size = g.MeasureString(this.FootInfo, this.font);
                    g.DrawString(this.FootInfo, this.font, br, 0, this.Height - size.Height);
                }
                if (!string.IsNullOrEmpty(this.UpperRightInfo))
                {
                    SizeF size = g.MeasureString(this.UpperRightInfo, this.font);
                    g.DrawString(this.UpperRightInfo, this.font, br, this.Width - size.Width, 0);
                }
            }
        }

        private void Graph_Resize(object sender, System.EventArgs e)
        {
            this.Invalidate();
        } 

        
        #endregion
    }
}

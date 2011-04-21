//Thepirat 2011
//thepirat000@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace JVida_Fast_CSharp
{
    public partial class UniverseGraph : UserControl
    {
        #region Fields
        private Color backColor = Color.Black;
        private Color foreColor = Color.Red;
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
            get { return backColor; }
            set { backColor = value; }
        }
        public override Color ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        public Bitmap UniverseBitmap
        {
            get { return bmp; }
        }


        public string OverlayInfo { get; set; }
        public string FootInfo { get; set; }
        public bool ShowFps { get; set; }
        #endregion

        #region Constructor
        public UniverseGraph(int maxX, int maxY)
        {
            Resize += Graph_Resize;
            Paint += Graph_Paint;
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            // Add any initialization after the InitializeComponent() call.
            this.maxX = maxX;
            this.maxY = maxY;
            this.ShowFps = true;
            this.OverlayInfo = null;
            bmp = new Bitmap(maxX, maxY, PixelFormat.Format24bppRgb);
            ClearBmp();
        } 
        #endregion

        #region Public Methods
        /// <summary>
        /// Draw the bitmap at the given position
        /// </summary>
        public void PlotBmp(int x, int y, byte state)
        {
            lock (bmp)
            {
                bmp.SetPixel(x, y, state > 0 ? ForeColor : BackColor);
            }
        } 
        #endregion

        #region Private Methods
        private void ClearBmp()
        {
            imgBit.LockBitmap(bmp);
            for (int i = 0; i <= bmp.Width - 1; i++)
            {
                for (int j = 0; j <= bmp.Height - 1; j++)
                {
                    imgBit.SetPixel(i, j, Color.Black);
                }
            }
            imgBit.UnlockBitmap(bmp);
        }

        private void Graph_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (!stp.IsRunning || qty > 10000)
            {
                stp.Start();
                qty = 0;
            }
            qty += 1;
            lock (bmp)
            {
                region = new Rectangle(0, 0, maxX - 1, maxY - 1);
                Rectangle rect = this.ClientRectangle;
                g.DrawImage(bmp, rect, region, GraphicsUnit.Pixel);
                if (this.ShowFps)
                {
                    string info = string.Format("{0} fps", (1000 * qty / stp.ElapsedMilliseconds));
                    SizeF size = g.MeasureString(info, font);
                    g.DrawString(info, font, Brushes.LightBlue, this.Width - size.Width, this.Height - size.Height);
                }
                if (!string.IsNullOrEmpty(this.OverlayInfo))
                {
                    g.DrawString(this.OverlayInfo, font, Brushes.LightBlue, 0, 0);
                }
                if (!string.IsNullOrEmpty(this.FootInfo))
                {
                    SizeF size = g.MeasureString(this.FootInfo, font);
                    g.DrawString(this.FootInfo, font, Brushes.LightBlue, 0, this.Height - size.Height);
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

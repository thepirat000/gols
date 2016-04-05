// Thepirat 2011
// thepirat000@hotmail.com

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace JVida_Fast_CSharp
{
    public partial class UniverseGraph : UserControl
    {
        #region Fields
        private bool _selectionModeEnabled;
        private Color _backColor = Color.Black;
        private Color _foreColor = Color.Red;
        private Color _fontColor = Color.LightBlue;
        private readonly int _maxX;
        private readonly int _maxY;
        private readonly BitmapLocker _imgBit = new BitmapLocker();
        private Bitmap _bmp;
        private Rectangle _region;
        private long _qty;
        private readonly Stopwatch _stp = new Stopwatch();
        private readonly Font _font = new Font("Times New Roman", 9);
        private bool _importing;
        private InterpolationMode _interpolationMode = InterpolationMode.Default;

        #endregion

        #region Properties
        public event EventHandler<PointSelectedEventArgs> PointSelected;
        protected virtual void OnPointSelected(PointSelectedEventArgs e)
        {
            var eh = PointSelected;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        public event EventHandler<DragEventArgs> DragDropCell;
        protected virtual void OnDragDropCell(DragEventArgs e)
        {
            var eh = DragDropCell;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        public override Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        public override Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        public Bitmap UniverseBitmap
        {
            get { return _bmp; }
        }

        public string OverlayInfo { get; set; }

        public string FootInfo { get; set; }

        public bool ShowFps { get; set; }

        public string UpperRightInfo { get; set; }

        public string SelectionInfo { get; set; }

        public string LowerLeftInfo { get; set; }

        public InterpolationMode InterpolationMode
        {
            get { return _interpolationMode; }
            set { _interpolationMode = value; }
        }

        #endregion

        #region Constructor
        public UniverseGraph(int maxX, int maxY)
        {
            Resize += Graph_Resize;
            Paint += Graph_Paint;

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            _maxX = maxX;
            _maxY = maxY;
            Initialize();
        }
        #endregion

        #region Public Methods
        public void EnterSelectionMode(bool isImporting)
        {
            _selectionModeEnabled = true;
            _importing = isImporting;
            Invalidate();
        }
        public void ExitSelectionMode()
        {
            _selectionModeEnabled = false;
            SelectionInfo = string.Empty;
            Invalidate();
        }
        /// <summary>
        /// Draw the bitmap at the given position
        /// </summary>
        public void PlotBmp(int x, int y, byte state)
        {
            lock (_bmp)
            {
                _bmp.SetPixel(x, y, state > 0 ? ForeColor : BackColor);
            }
        }

        public void Restart()
        {
            if (null != _bmp)
            {
                _bmp.Dispose();
            }
            Initialize();
        }
        #endregion

        #region Private Methods
        public void Initialize()
        {
            ShowFps = true;
            OverlayInfo = null;
            _bmp = new Bitmap(_maxX, _maxY, PixelFormat.Format24bppRgb);
            ClearBmp();
        }

        private void ClearBmp()
        {
            _imgBit.LockBitmap(_bmp);
            for (int i = 0; i <= _bmp.Width - 1; i++)
            {
                for (int j = 0; j <= _bmp.Height - 1; j++)
                {
                    _imgBit.SetPixel(i, j, BackColor);
                }
            }
            _imgBit.UnlockBitmap(_bmp);
        }

        private void Graph_Paint(object sender, PaintEventArgs e)
        {
            Brush br = new SolidBrush(FontColor);
            Graphics g = e.Graphics;
            if (!_stp.IsRunning || _qty > 10000)
            {
                _stp.Start();
                _qty = 0;
            }
            _qty++;
            lock (_bmp)
            {
                _region = new Rectangle(0, 0, _maxX - 1, _maxY - 1);
                Rectangle rect = ClientRectangle;
                g.InterpolationMode = _interpolationMode;
                g.DrawImage(_bmp, rect, _region, GraphicsUnit.Pixel);
                if (ShowFps)
                {
                    string info = string.Format("{0} fps", (int)(1000 * _qty / _stp.Elapsed.TotalMilliseconds));
                    SizeF size = g.MeasureString(info, _font);
                    g.DrawString(info, _font, br, Width - size.Width, Height - size.Height);
                }
                if (!string.IsNullOrEmpty(OverlayInfo))
                {
                    g.DrawString(OverlayInfo, _font, br, 0, 0);
                }
                if (!string.IsNullOrEmpty(FootInfo))
                {
                    SizeF size = g.MeasureString(FootInfo, _font);
                    g.DrawString(FootInfo, _font, br, 0, Height - size.Height);
                }
                if (!string.IsNullOrEmpty(UpperRightInfo))
                {
                    SizeF size = g.MeasureString(UpperRightInfo, _font);
                    g.DrawString(UpperRightInfo, _font, br, Width - size.Width, 0);
                }
                if (!string.IsNullOrEmpty(SelectionInfo))
                {
                    SizeF size = g.MeasureString(SelectionInfo, _font);
                    g.DrawString(SelectionInfo, _font, br, Width /2 - size.Width/2, Height/2 - size.Height/2);
                }
                if (!string.IsNullOrEmpty(LowerLeftInfo))
                {
                    SizeF size = g.MeasureString(LowerLeftInfo, _font);
                    g.DrawString(LowerLeftInfo, _font, br, 0, Height - size.Height);
                }
            }
        }

        private void Graph_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void UniverseGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectionModeEnabled)
            {
                var x = e.Location.X * _maxX / Width;
                var y = e.Location.Y * _maxY / Height;
                SelectionInfo = (_importing ? "Select location\n" : "") + new Point(x, y);
                Invalidate();
            }
        }

        private void UniverseGraph_MouseClick(object sender, MouseEventArgs e)
        {
            if (_selectionModeEnabled)
            {
                var x = e.Location.X * _maxX / Width;
                var y = e.Location.Y * _maxY / Height;
                OnPointSelected(new PointSelectedEventArgs(x, y, _importing, e.Button));
                _selectionModeEnabled = false;
            }
        }
        #endregion

        private void UniverseGraph_DragOver(object sender, DragEventArgs e)
        {
            if (_selectionModeEnabled)
            {
                var point = PointToClient(new Point(e.X, e.Y));
                var x = point.X * _maxX / Width;
                var y = point.Y * _maxY / Height;
                SelectionInfo = (_importing ? "Select location\n" : "") + new Point(x, y);
                Invalidate();
            }
        }

        private void UniverseGraph_DragLeave(object sender, EventArgs e)
        {
            SelectionInfo = "";
            Invalidate();
        }

        private void UniverseGraph_DragDrop(object sender, DragEventArgs e)
        {
            var point = PointToClient(new Point(e.X, e.Y));
            var x = point.X * _maxX / Width;
            var y = point.Y * _maxY / Height;
            OnDragDropCell(new DragEventArgs(e.Data, e.KeyState, x, y, e.AllowedEffect, e.Effect));
        }
    }
}

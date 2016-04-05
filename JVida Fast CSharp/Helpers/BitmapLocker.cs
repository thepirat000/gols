// Thepirat 2011
// thepirat000@hotmail.com

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

/// <summary>
/// Speed up the handling of bitmaps
/// </summary>
/// <remarks></remarks>
public class BitmapLocker
{
    public enum ColorChannel
    { Red = 2, Green = 1, Blue = 0, Alpha = 3 }

    #region Fields
    private int _rowSizeBytes;
    private byte[] _pixBytes;
    private BitmapData _bitmapData; 
    #endregion

    #region Public Methods
    /// <summary>
    /// Lock the bitmap's data.
    /// </summary>
    public void LockBitmap(Bitmap bm, bool readOnly = false)
    {
        // Lock the bitmap data.
        Rectangle bounds = new Rectangle(0, 0, bm.Width, bm.Height);
        _bitmapData = bm.LockBits(bounds, readOnly ? ImageLockMode.ReadOnly : ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        _rowSizeBytes = _bitmapData.Stride;

        // Allocate room for the data.
        int totalSize = _bitmapData.Stride * _bitmapData.Height;
        _pixBytes = new byte[totalSize + 1];

        // Copy the data into the g_PixBytes array.
        Marshal.Copy(_bitmapData.Scan0, _pixBytes, 0, totalSize);
    }

    /// <summary>
    /// Unlock the bitmap's data.
    /// </summary>
    public void UnlockBitmap(Bitmap bm)
    {
        // Copy the data back into the bitmap.
        int totalSize = _bitmapData.Stride * _bitmapData.Height;
        Marshal.Copy(_pixBytes, 0, _bitmapData.Scan0, totalSize);

        // Unlock the bitmap.
        bm.UnlockBits(_bitmapData);

        // Release resources.
        _pixBytes = null;
        _bitmapData = null;
    }

    public Color GetPixel(int x, int y)
    {
        return Color.FromArgb(_pixBytes[GetArrayPos(x, y, ColorChannel.Alpha)],
                              _pixBytes[GetArrayPos(x, y, ColorChannel.Red)],
                              _pixBytes[GetArrayPos(x, y, ColorChannel.Green)],
                              _pixBytes[GetArrayPos(x, y, ColorChannel.Blue)]);
    }

    public void SetPixel(int x, int y, Color c)
    {
        _pixBytes[GetArrayPos(x, y, ColorChannel.Red)] = c.R;
        _pixBytes[GetArrayPos(x, y, ColorChannel.Green)] = c.G;
        _pixBytes[GetArrayPos(x, y, ColorChannel.Blue)] = c.B;
        _pixBytes[GetArrayPos(x, y, ColorChannel.Alpha)] = c.A;
    }

    public int GetArrayPos(int x, int y, ColorChannel c)
    {
        return Convert.ToInt32(((_rowSizeBytes * y) + (x * 4)) + c);
    } 
    #endregion
}

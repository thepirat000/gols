// Thepirat 2011
// thepirat000@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

/// <summary>
/// Speed up the handling of bitmaps
/// </summary>
/// <remarks></remarks>
public class BitmapLocker
{
    public enum ColorChannel : int { RED = 2, GREEN = 1, BLUE = 0, ALPHA = 3 }

    #region Fields
    private int RowSizeBytes;
    private byte[] PixBytes;
    private BitmapData BitmapData; 
    #endregion

    #region Public Methods
    /// <summary>
    /// Lock the bitmap's data.
    /// </summary>
    public void LockBitmap(Bitmap bm, bool ReadOnly = false)
    {
        // Lock the bitmap data.
        Rectangle bounds = new Rectangle(0, 0, bm.Width, bm.Height);
        this.BitmapData = bm.LockBits(bounds, ReadOnly ? ImageLockMode.ReadOnly : ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        this.RowSizeBytes = this.BitmapData.Stride;

        // Allocate room for the data.
        int total_size = this.BitmapData.Stride * this.BitmapData.Height;
        this.PixBytes = new byte[total_size + 1];

        // Copy the data into the g_PixBytes array.
        Marshal.Copy(this.BitmapData.Scan0, this.PixBytes, 0, total_size);
    }

    /// <summary>
    /// Unlock the bitmap's data.
    /// </summary>
    public void UnlockBitmap(Bitmap bm)
    {
        // Copy the data back into the bitmap.
        int total_size = this.BitmapData.Stride * this.BitmapData.Height;
        Marshal.Copy(this.PixBytes, 0, this.BitmapData.Scan0, total_size);

        // Unlock the bitmap.
        bm.UnlockBits(this.BitmapData);

        // Release resources.
        this.PixBytes = null;
        this.BitmapData = null;
    }

    public Color GetPixel(int x, int y)
    {
        return Color.FromArgb(this.PixBytes[this.GetArrayPos(x, y, ColorChannel.ALPHA)],
                              this.PixBytes[this.GetArrayPos(x, y, ColorChannel.RED)],
                              this.PixBytes[this.GetArrayPos(x, y, ColorChannel.GREEN)],
                              this.PixBytes[this.GetArrayPos(x, y, ColorChannel.BLUE)]);
    }

    public void SetPixel(int x, int y, Color c)
    {
        this.PixBytes[this.GetArrayPos(x, y, ColorChannel.RED)] = c.R;
        this.PixBytes[this.GetArrayPos(x, y, ColorChannel.GREEN)] = c.G;
        this.PixBytes[this.GetArrayPos(x, y, ColorChannel.BLUE)] = c.B;
        this.PixBytes[this.GetArrayPos(x, y, ColorChannel.ALPHA)] = c.A;
    }

    public int GetArrayPos(int x, int y, ColorChannel c)
    {
        return Convert.ToInt32(((this.RowSizeBytes * y) + (x * 4)) + c);
    } 
    #endregion
}

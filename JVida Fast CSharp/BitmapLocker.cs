//Thepirat 2011
//thepirat000@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

//Ejemplo de uso:
//
//Bitmap bmp = new Bitmap("d:\abc.BMP");
//clsImg img = New clsImg();

//img.LockBitmap(bmp);
//for (int Y = 0; Y < bmp.Height; Y++)
//    for (int X = 0; X < bmp.Widthl X++)
//        if (img.GetPixel(X, Y).GetSaturation > 0.5) 
//            img.SetPixel(X, Y, Color.White);
//img.UnlockBitmap(bmp);

/// <summary>
/// Clase para agilizar la manipulación de bitmaps
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
    // Lock the bitmap's data.
    public void LockBitmap(Bitmap bm, bool ReadOnly = false)
    {
        // Lock the bitmap data.
        Rectangle bounds = new Rectangle(0, 0, bm.Width, bm.Height);
        BitmapData = bm.LockBits(bounds, ReadOnly ? ImageLockMode.ReadOnly : ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        RowSizeBytes = BitmapData.Stride;

        // Allocate room for the data.
        int total_size = BitmapData.Stride * BitmapData.Height;
        PixBytes = new byte[total_size + 1];

        // Copy the data into the g_PixBytes array.
        Marshal.Copy(BitmapData.Scan0, PixBytes, 0, total_size);
    }

    public void UnlockBitmap(Bitmap bm)
    {
        // Copy the data back into the bitmap.
        int total_size = BitmapData.Stride * BitmapData.Height;
        Marshal.Copy(PixBytes, 0, BitmapData.Scan0, total_size);

        // Unlock the bitmap.
        bm.UnlockBits(BitmapData);

        // Release resources.
        PixBytes = null;
        BitmapData = null;
    }

    public Color GetPixel(int x, int y)
    {
        return Color.FromArgb(PixBytes[GetArrayPos(x, y, ColorChannel.ALPHA)],
                              PixBytes[GetArrayPos(x, y, ColorChannel.RED)],
                              PixBytes[GetArrayPos(x, y, ColorChannel.GREEN)],
                              PixBytes[GetArrayPos(x, y, ColorChannel.BLUE)]);
    }

    public void SetPixel(int x, int y, Color c)
    {
        PixBytes[GetArrayPos(x, y, ColorChannel.RED)] = c.R;
        PixBytes[GetArrayPos(x, y, ColorChannel.GREEN)] = c.G;
        PixBytes[GetArrayPos(x, y, ColorChannel.BLUE)] = c.B;
        PixBytes[GetArrayPos(x, y, ColorChannel.ALPHA)] = c.A;
    }

    public int GetArrayPos(int x, int y, ColorChannel c)
    {
        return Convert.ToInt32((RowSizeBytes * y + x * 4) + c);
    } 
    #endregion
}


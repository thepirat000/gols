// Original By René Nyffenegger (http://www.adp-gmbh.ch/csharp/avi/write_avi.html)
namespace JVida_Fast_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public class AviWriter : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct AVISTREAMINFOW
        {
            public UInt32 fccType, fccHandler, dwFlags, dwCaps;

            public UInt16 wPriority, wLanguage;

            public UInt32 dwScale, dwRate,
                             dwStart, dwLength, dwInitialFrames, dwSuggestedBufferSize,
                             dwQuality, dwSampleSize, rect_left, rect_top,
                             rect_right, rect_bottom, dwEditCount, dwFormatChangeCount;

            public UInt16 szName0, szName1, szName2, szName3, szName4, szName5,
                             szName6, szName7, szName8, szName9, szName10, szName11,
                             szName12, szName13, szName14, szName15, szName16, szName17,
                             szName18, szName19, szName20, szName21, szName22, szName23,
                             szName24, szName25, szName26, szName27, szName28, szName29,
                             szName30, szName31, szName32, szName33, szName34, szName35,
                             szName36, szName37, szName38, szName39, szName40, szName41,
                             szName42, szName43, szName44, szName45, szName46, szName47,
                             szName48, szName49, szName50, szName51, szName52, szName53,
                             szName54, szName55, szName56, szName57, szName58, szName59,
                             szName60, szName61, szName62, szName63;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct AVICOMPRESSOPTIONS
        {
            public UInt32 fccType;
            public UInt32 fccHandler;
            public UInt32 dwKeyFrameEvery;  // only used with AVICOMRPESSF_KEYFRAMES
            public UInt32 dwQuality;
            public UInt32 dwBytesPerSecond; // only used with AVICOMPRESSF_DATARATE
            public UInt32 dwFlags;
            public IntPtr lpFormat;
            public UInt32 cbFormat;
            public IntPtr lpParms;
            public UInt32 cbParms;
            public UInt32 dwInterleaveEvery;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPINFOHEADER
        {
            public UInt32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public Int16 biPlanes;
            public Int16 biBitCount;
            public UInt32 biCompression;
            public UInt32 biSizeImage;
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public UInt32 biClrUsed;
            public UInt32 biClrImportant;
        }

        public class AviException : ApplicationException
        {
            public AviException(string s) : base(s) 
            { 
            }

            public AviException(string s, Int32 hr)
                : base(s)
            {

                if (hr == AVIERR_BADPARAM)
                {
                    this.err_msg = "AVIERR_BADPARAM";
                }
                else
                {
                    this.err_msg = "unknown";
                }
            }

            public string ErrMsg()
            {
                return this.err_msg;
            }
            private const Int32 AVIERR_BADPARAM = -2147205018;
            private string err_msg;
        }

        private bool closed;

        private object lockme = new object();

        public bool IsClosed
        {
            get
            {
                return this.closed;
            }
        }

        public Bitmap Open(string fileName, UInt32 frameRate, int width, int height)
        {
            this.closed = false;
            this.frameRate = frameRate;
            this.width = (UInt32)width;
            this.height = (UInt32)height;
            this.bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bmpDat = this.bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            this.stride = (UInt32)bmpDat.Stride;
            this.bmp.UnlockBits(bmpDat);
            AVIFileInit();
            int hr = AVIFileOpenW(ref this.pfile, fileName, 4097 /* OF_WRITE | OF_CREATE (winbase.h) */, 0);
            if (hr != 0)
            {
                throw new AviException("error for AVIFileOpenW");
            }

            this.CreateStream();
            this.SetOptions();

            return this.bmp;
        }

        public void AddFrame()
        {
            lock (this.lockme)
            {
                BitmapData bmpDat;
                bmpDat = this.bmp.LockBits(new Rectangle(0, 0, (int)this.width, (int)this.height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                int hr = AVIStreamWrite(this.psCompressed, this.count, 1,
                   bmpDat.Scan0, // pointer to data
                   (Int32)(this.stride * this.height),
                   0, // 16 = AVIIF_KEYFRAMe
                   0,
                   0);

                if (hr != 0)
                {
                    throw new AviException("AVIStreamWrite");
                }

                this.bmp.UnlockBits(bmpDat);

                this.count++;
            }
        }

        public void Close()
        {
            lock (this.lockme)
            {
                if (this.closed)
                {
                    return;
                }
                this.closed = true;
                AVIStreamRelease(this.ps);
                AVIStreamRelease(this.psCompressed);
                AVIFileRelease(this.pfile);
                AVIFileExit();
            }
        }

        private void CreateStream()
        {
            AVISTREAMINFOW strhdr = new AVISTREAMINFOW();
            strhdr.fccType = this.fccType;
            strhdr.fccHandler = this.fccHandler;
            strhdr.dwFlags = 0;
            strhdr.dwCaps = 0;
            strhdr.wPriority = 0;
            strhdr.wLanguage = 0;
            strhdr.dwScale = 1;
            strhdr.dwRate = this.frameRate; // Frames per Second
            strhdr.dwStart = 0;
            strhdr.dwLength = 0;
            strhdr.dwInitialFrames = 0;
            strhdr.dwSuggestedBufferSize = this.height * this.stride;
            strhdr.dwQuality = 0xffffffff; // -1;         // Use default
            strhdr.dwSampleSize = 0;
            strhdr.rect_top = 0;
            strhdr.rect_left = 0;
            strhdr.rect_bottom = this.height;
            strhdr.rect_right = this.width;
            strhdr.dwEditCount = 0;
            strhdr.dwFormatChangeCount = 0;
            strhdr.szName0 = 0;
            strhdr.szName1 = 0;

            int hr = AVIFileCreateStream(this.pfile, out this.ps, ref strhdr);

            if (hr != 0)
            {
                throw new AviException("AVIFileCreateStream");
            }
        }

        unsafe private void SetOptions()
        {
            AVICOMPRESSOPTIONS opts = new AVICOMPRESSOPTIONS();
            opts.fccType = 0;
            opts.fccHandler = 0;
            opts.dwKeyFrameEvery = 0;
            opts.dwQuality = 0;  // 0 .. 10000
            opts.dwFlags = 0;  // AVICOMRPESSF_KEYFRAMES = 4
            opts.dwBytesPerSecond = 0;
            opts.lpFormat = new IntPtr(0);
            opts.cbFormat = 0;
            opts.lpParms = new IntPtr(0);
            opts.cbParms = 0;
            opts.dwInterleaveEvery = 0;

            AVICOMPRESSOPTIONS* p = &opts;
            AVICOMPRESSOPTIONS** pp = &p;

            IntPtr x = this.ps;
            IntPtr* ptr_ps = &x;

            AVISaveOptions(0, ICMF_CHOOSE_KEYFRAME | ICMF_CHOOSE_DATARATE | ICMF_CHOOSE_ALLCOMPRESSORS, 1, ptr_ps, pp);

            int hr = AVIMakeCompressedStream(out this.psCompressed, this.ps, ref opts, 0);
            if (hr != 0)
            {
                throw new AviException("AVIMakeCompressedStream");
            }

            BITMAPINFOHEADER bi = new BITMAPINFOHEADER();
            bi.biSize = 40;
            bi.biWidth = (Int32)(this.width % 2 == 0 ? this.width : this.width+1);
            bi.biHeight = (Int32)(this.height % 2 == 0 ? this.height : this.height + 1);
            bi.biPlanes = 1;
            bi.biBitCount = 24;
            bi.biCompression = 0;  // 0 = BI_RGB
            bi.biSizeImage = this.stride * this.height;
            bi.biXPelsPerMeter = 0;
            bi.biYPelsPerMeter = 0;
            bi.biClrUsed = 0;
            bi.biClrImportant = 0;

            hr = AVIStreamSetFormat(this.psCompressed, 0, ref bi, 40);
            if (hr != 0)
            {
                throw new AviException("AVIStreamSetFormat", hr);
            }
        }

        public void Dispose()
        {
            try
            {
                this.bmp.Dispose();
                this.Close();
            }
            catch (Exception)
            {
            }
        }

        [DllImport("avifil32.dll")]
        private static extern void AVIFileInit();

        [DllImport("avifil32.dll")]
        private static extern int AVIFileOpenW(ref int ptr_pfile, [MarshalAs(UnmanagedType.LPWStr)]string fileName, int flags, int dummy);

        [DllImport("avifil32.dll")]
        private static extern int AVIFileCreateStream(
          int ptr_pfile, out IntPtr ptr_ptr_avi, ref AVISTREAMINFOW ptr_streaminfo);

        [DllImport("avifil32.dll")]
        private static extern int AVIMakeCompressedStream(
          out IntPtr ppsCompressed, IntPtr aviStream, ref AVICOMPRESSOPTIONS ao, int dummy);

        [DllImport("avifil32.dll")]
        private static extern int AVIStreamSetFormat(
          IntPtr aviStream, Int32 lPos, ref BITMAPINFOHEADER lpFormat, Int32 cbFormat);

        [DllImport("avifil32.dll")]
        private static unsafe extern int AVISaveOptions(
          int hwnd, UInt32 flags, int nStreams, IntPtr* ptr_ptr_avi, AVICOMPRESSOPTIONS** ao);

        [DllImport("avifil32.dll")]
        private static extern int AVIStreamWrite(
          IntPtr aviStream, Int32 lStart, Int32 lSamples, IntPtr lpBuffer,
          Int32 cbBuffer, Int32 dwFlags, Int32 dummy1, Int32 dummy2);

        [DllImport("avifil32.dll")]
        private static extern int AVIStreamRelease(IntPtr aviStream);

        [DllImport("avifil32.dll")]
        private static extern int AVIFileRelease(int pfile);

        [DllImport("avifil32.dll")]
        private static extern void AVIFileExit();

        private int pfile = 0;
        private IntPtr ps = new IntPtr(0);
        private IntPtr psCompressed = new IntPtr(0);
        private UInt32 frameRate = 0;
        private int count = 0;
        private UInt32 width = 0;
        private UInt32 stride = 0;
        private UInt32 height = 0;
        private UInt32 fccType = 1935960438;  // vids
        private UInt32 fccHandler = 808810089; // IV50
        private Bitmap bmp;
        private const uint ICMF_CHOOSE_KEYFRAME           = 0x1;     // show KeyFrame Every box
        private const uint ICMF_CHOOSE_DATARATE = 0x2;     // show DataRate box
        private const uint ICMF_CHOOSE_PREVIEW = 0x4;     // allow expanded preview dialog
        private const uint ICMF_CHOOSE_ALLCOMPRESSORS = 0x8;     // don't only show those that    

    }
}

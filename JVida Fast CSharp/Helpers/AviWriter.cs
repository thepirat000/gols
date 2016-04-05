// Original By René Nyffenegger (http://www.adp-gmbh.ch/csharp/avi/write_avi.html)

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace JVida_Fast_CSharp
{
    public class AviWriter : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Avistreaminfow
        {
            public UInt32 fccType, fccHandler, dwFlags, dwCaps;

            public UInt16 wPriority, wLanguage;

            public UInt32 dwScale, dwRate,
                             dwStart, dwLength, dwInitialFrames, dwSuggestedBufferSize,
                             dwQuality, dwSampleSize, rect_left, rect_top,
                             rect_right, rect_bottom, dwEditCount, dwFormatChangeCount;

            public UInt16 szName0, szName1;

            public readonly UInt16 szName2;

            public readonly UInt16 szName3;

            public readonly UInt16 szName4;

            public readonly UInt16 szName5;

            public readonly UInt16 szName6;

            public readonly UInt16 szName7;

            public readonly UInt16 szName8;

            public readonly UInt16 szName9;

            public readonly UInt16 szName10;

            public readonly UInt16 szName11;

            public readonly UInt16 szName12;

            public readonly UInt16 szName13;

            public readonly UInt16 szName14;

            public readonly UInt16 szName15;

            public readonly UInt16 szName16;

            public readonly UInt16 szName17;

            public readonly UInt16 szName18;

            public readonly UInt16 szName19;

            public readonly UInt16 szName20;

            public readonly UInt16 szName21;

            public readonly UInt16 szName22;

            public readonly UInt16 szName23;

            public readonly UInt16 szName24;

            public readonly UInt16 szName25;

            public readonly UInt16 szName26;

            public readonly UInt16 szName27;

            public readonly UInt16 szName28;

            public readonly UInt16 szName29;

            public readonly UInt16 szName30;

            public readonly UInt16 szName31;

            public readonly UInt16 szName32;

            public readonly UInt16 szName33;

            public readonly UInt16 szName34;

            public readonly UInt16 szName35;

            public readonly UInt16 szName36;

            public readonly UInt16 szName37;

            public readonly UInt16 szName38;

            public readonly UInt16 szName39;

            public readonly UInt16 szName40;

            public readonly UInt16 szName41;

            public readonly UInt16 szName42;

            public readonly UInt16 szName43;

            public readonly UInt16 szName44;

            public readonly UInt16 szName45;

            public readonly UInt16 szName46;

            public readonly UInt16 szName47;

            public readonly UInt16 szName48;

            public readonly UInt16 szName49;

            public readonly UInt16 szName50;

            public readonly UInt16 szName51;

            public readonly UInt16 szName52;

            public readonly UInt16 szName53;

            public readonly UInt16 szName54;

            public readonly UInt16 szName55;

            public readonly UInt16 szName56;

            public readonly UInt16 szName57;

            public readonly UInt16 szName58;

            public readonly UInt16 szName59;

            public readonly UInt16 szName60;
            public readonly UInt16 szName61;
            public readonly UInt16 szName62;
            public readonly UInt16 szName63;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Avicompressoptions
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
        public struct Bitmapinfoheader
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

                if (hr == AvierrBadparam)
                {
                    _errMsg = "AVIERR_BADPARAM";
                }
                else
                {
                    _errMsg = "unknown";
                }
            }

            public string ErrMsg()
            {
                return _errMsg;
            }
            private const Int32 AvierrBadparam = -2147205018;
            private readonly string _errMsg;
        }

        private bool _closed;

        private readonly object _lockme = new object();

        public bool IsClosed
        {
            get
            {
                return _closed;
            }
        }

        public Bitmap Open(string fileName, UInt32 frameRate, int width, int height)
        {
            _closed = false;
            _frameRate = frameRate;
            width = (width % 2 == 0 ? width : width - 1);
            height = (height%2 == 0 ? height : height - 1);
            _width = (UInt32)width;
            _height = (UInt32)height;
            _bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bmpDat = _bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            _stride = (UInt32)bmpDat.Stride;
            _bmp.UnlockBits(bmpDat);
            AVIFileInit();
            int hr = AVIFileOpenW(ref _pfile, fileName, 4097 /* OF_WRITE | OF_CREATE (winbase.h) */, 0);
            if (hr != 0)
            {
                throw new AviException("error for AVIFileOpenW");
            }

            CreateStream();
            SetOptions();

            return _bmp;
        }

        public void AddFrame()
        {
            lock (_lockme)
            {
                BitmapData bmpDat;
                bmpDat = _bmp.LockBits(new Rectangle(0, 0, (int)_width, (int)_height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                int hr = AVIStreamWrite(_psCompressed, _count, 1,
                   bmpDat.Scan0, // pointer to data
                   (Int32)(_stride * _height),
                   0, // 16 = AVIIF_KEYFRAMe
                   0,
                   0);

                if (hr != 0)
                {
                    throw new AviException("AVIStreamWrite");
                }

                _bmp.UnlockBits(bmpDat);

                _count++;
            }
        }

        public void Close()
        {
            lock (_lockme)
            {
                if (_closed)
                {
                    return;
                }
                _closed = true;
                AVIStreamRelease(_ps);
                AVIStreamRelease(_psCompressed);
                AVIFileRelease(_pfile);
                AVIFileExit();
            }
        }

        private void CreateStream()
        {
            Avistreaminfow strhdr = new Avistreaminfow();
            strhdr.fccType = _fccType;
            strhdr.fccHandler = _fccHandler;
            strhdr.dwFlags = 0;
            strhdr.dwCaps = 0;
            strhdr.wPriority = 0;
            strhdr.wLanguage = 0;
            strhdr.dwScale = 1;
            strhdr.dwRate = _frameRate; // Frames per Second
            strhdr.dwStart = 0;
            strhdr.dwLength = 0;
            strhdr.dwInitialFrames = 0;
            strhdr.dwSuggestedBufferSize = _height * _stride;
            strhdr.dwQuality = 0xffffffff; // -1;         // Use default
            strhdr.dwSampleSize = 0;
            strhdr.rect_top = 0;
            strhdr.rect_left = 0;
            strhdr.rect_bottom = _height;
            strhdr.rect_right = _width;
            strhdr.dwEditCount = 0;
            strhdr.dwFormatChangeCount = 0;
            strhdr.szName0 = 0;
            strhdr.szName1 = 0;

            int hr = AVIFileCreateStream(_pfile, out _ps, ref strhdr);

            if (hr != 0)
            {
                throw new AviException("AVIFileCreateStream");
            }
        }

        unsafe private void SetOptions()
        {
            Avicompressoptions opts = new Avicompressoptions();
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

            Avicompressoptions* p = &opts;
            Avicompressoptions** pp = &p;

            IntPtr x = _ps;
            IntPtr* ptrPs = &x;

            AVISaveOptions(0, IcmfChooseKeyframe | IcmfChooseDatarate | IcmfChooseAllcompressors, 1, ptrPs, pp);

            int hr = AVIMakeCompressedStream(out _psCompressed, _ps, ref opts, 0);
            if (hr != 0)
            {
                throw new AviException("AVIMakeCompressedStream");
            }

            Bitmapinfoheader bi = new Bitmapinfoheader();
            bi.biSize = 40;
            bi.biWidth = (Int32)(_width);
            bi.biHeight = (Int32)(_height);
            bi.biPlanes = 1;
            bi.biBitCount = 24;
            bi.biCompression = 0;  // 0 = BI_RGB
            bi.biSizeImage = _stride * _height;
            bi.biXPelsPerMeter = 0;
            bi.biYPelsPerMeter = 0;
            bi.biClrUsed = 0;
            bi.biClrImportant = 0;

            hr = AVIStreamSetFormat(_psCompressed, 0, ref bi, 40);
            if (hr != 0)
            {
                throw new AviException("AVIStreamSetFormat", hr);
            }
        }

        public void Dispose()
        {
            try
            {
                _bmp.Dispose();
                Close();
            }
            catch (Exception)
            {
            }
        }

        [DllImport("avifil32.dll")]
        private static extern void AVIFileInit();

        [DllImport("avifil32.dll")]
        private static extern int AVIFileOpenW(ref int ptrPfile, [MarshalAs(UnmanagedType.LPWStr)]string fileName, int flags, int dummy);

        [DllImport("avifil32.dll")]
        private static extern int AVIFileCreateStream(
          int ptrPfile, out IntPtr ptrPtrAvi, ref Avistreaminfow ptrStreaminfo);

        [DllImport("avifil32.dll")]
        private static extern int AVIMakeCompressedStream(
          out IntPtr ppsCompressed, IntPtr aviStream, ref Avicompressoptions ao, int dummy);

        [DllImport("avifil32.dll")]
        private static extern int AVIStreamSetFormat(
          IntPtr aviStream, Int32 lPos, ref Bitmapinfoheader lpFormat, Int32 cbFormat);

        [DllImport("avifil32.dll")]
        private static unsafe extern int AVISaveOptions(
          int hwnd, UInt32 flags, int nStreams, IntPtr* ptrPtrAvi, Avicompressoptions** ao);

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

        private int _pfile;
        private IntPtr _ps = new IntPtr(0);
        private IntPtr _psCompressed = new IntPtr(0);
        private UInt32 _frameRate;
        private int _count;
        private UInt32 _width;
        private UInt32 _stride;
        private UInt32 _height;
        private readonly UInt32 _fccType = 1935960438;  // vids
        private readonly UInt32 _fccHandler = 808810089; // IV50
        private Bitmap _bmp;
        private const uint IcmfChooseKeyframe           = 0x1;     // show KeyFrame Every box
        private const uint IcmfChooseDatarate = 0x2;     // show DataRate box
        private const uint IcmfChoosePreview = 0x4;     // allow expanded preview dialog
        private const uint IcmfChooseAllcompressors = 0x8;     // don't only show those that    

    }
}

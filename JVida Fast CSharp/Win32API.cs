//Llamadas a la API necesarias para el preview
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JVida_Fast_CSharp
{
    public class Win32API
    {
        public const int SWP_NOACTIVATE = 0x10;
        public const int SWP_NOZORDER = 0x4;
        public const int SWP_SHOWWINDOW = 0x40;
        public const int GWL_STYLE = -16;
        public const int WS_CHILD = 0x40000000;
        public const int GWL_HWNDPARENT = -8;

        public const int HWND_TOP = 0;
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int GetClientRect(int hwnd, ref RECT lpRect);
        [DllImport("user32", EntryPoint = "GetWindowLongA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int GetWindowLong(int hwnd, int nIndex);
        [DllImport("user32", EntryPoint = "SetWindowLongA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int SetWindowLong(int hwnd, int nIndex, int dwNewInteger);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        public static extern int SetParent(int hWndChild, int hWndNewParent);
    }
}
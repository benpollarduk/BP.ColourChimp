﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BP.ColourChimp.Classes
{
    /// <summary>
    /// A class that provides helper functions relating to capturing the desktop.
    /// </summary>
    public static class DesktopHelper
    {
        #region StaticMethods

        /// <summary>
        /// Get a bitmap of the entire desktop area.
        /// </summary>
        /// <returns>A bitmap.</returns>
        public static Bitmap GetDesktop()
        {
            var hdcScreen = GetDC(GetDesktopWindow());
            var hdcCompatible = CreateCompatibleDC(hdcScreen);

            // get screen dimensions
            var width = GetSystemMetrics(0);
            var height = GetSystemMetrics(1);

            var hBmp = CreateCompatibleBitmap(hdcScreen, width, height);

            if (hBmp == IntPtr.Zero)
                return null;

            // grab the screen
            var hOldBmp = SelectObject(hdcCompatible, hBmp);
            BitBlt(hdcCompatible, 0, 0, width, height, hdcScreen, 0, 0, 13369376);
            SelectObject(hdcCompatible, hOldBmp);
            DeleteDC(hdcCompatible);
            ReleaseDC(GetDesktopWindow(), hdcScreen);

            // create a .Net bitmap image from image in memory
            var bmp = Image.FromHbitmap(hBmp);

            // delete the gdi object and cleanup
            DeleteObject(hBmp);
            GC.Collect();

            return bmp;
        }

        #endregion

        #region Structs

        /// <summary>
        /// A rectangle that defines a windows location. See http://www.pinvoke.net/default.aspx/user32/GetWindowRect.html
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion

        #region DLLImports

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest,
            int nYDest, int nWidth, int nHeight, IntPtr hdcSrc,
            int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
            int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobjBmp);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(int hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        #endregion
    }
}
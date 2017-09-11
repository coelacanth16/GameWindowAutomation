using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace GameWindowAutomation.Core
{
    public static class Utilities
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        [Flags]
        public enum MouseEvents : long
        {
            LEFTDOWN = 0x02,
            LEFTUP = 0x04,
            RIGHTDOWN = 0x08,
            RIGHTUP = 0x10
        }

        public const long StandardLeftMouseClick = (long)(MouseEvents.LEFTDOWN | MouseEvents.LEFTUP);

        public static void DoMouseClick(Point point, long mouseFlags = StandardLeftMouseClick)
        {
            mouse_event(mouseFlags, point.X, point.Y, 0, 0);
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [ThreadStatic]
        private static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public static Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
            return screenPixel.GetPixel(0, 0);
        }
        
        public static void GetImageAt(Point topLeft, ref Bitmap areaCapture)
        {            
            using (Graphics gdest = Graphics.FromImage(areaCapture))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, areaCapture.Width, areaCapture.Height, hSrcDC, topLeft.X, topLeft.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
        }

        public static Bitmap GetImageAt(Point topLeft, Point bottomRight)
        {
            var areaCapture = new Bitmap(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y, PixelFormat.Format32bppArgb);            
            GetImageAt(topLeft, ref areaCapture);
            return areaCapture;
        }

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        //[DllImport("user32.dll")]
        //public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        //public struct Rect
        //{
        //    public int Left { get; set; }
        //    public int Top { get; set; }
        //    public int Right { get; set; }
        //    public int Bottom { get; set; }
        //}

        public static Bitmap InterpolateBitmap(Bitmap original, double scale)
        {
            var newImage = new Bitmap((int)(original.Width * scale), (int)(original.Height * scale));
            using (Graphics g = Graphics.FromImage(newImage))
            {
                // Here you set your interpolation mode
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                // Scale the image, by drawing it on the larger bitmap
                g.DrawImage(original, new Rectangle(Point.Empty, newImage.Size));
            }
            return newImage;
        }
    }
}

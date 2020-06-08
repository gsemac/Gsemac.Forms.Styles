using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Gsemac.Forms.Styles.Native {

    public static class User32 {

        // Public members

        public static IntPtr GetDC(IntPtr hwnd) {

            return GetDCNative(hwnd);

        }
        public static IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc) {

            return ReleaseDCNative(hwnd, hdc);

        }

        public static IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam) {

            return SendMessageNative(hWnd, wMsg, wParam, lParam);

        }

        // Private members

        [DllImport("user32", EntryPoint = "GetDC")]
        private static extern IntPtr GetDCNative(IntPtr hwnd);
        [DllImport("user32", EntryPoint = "ReleaseDC")]
        private static extern IntPtr ReleaseDCNative(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageNative(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

    }

}
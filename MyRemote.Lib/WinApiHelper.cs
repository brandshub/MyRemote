using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;



namespace MyRemote.Lib
{
    internal static class WinApiHelper
    {
        private const int ALT = 0xA4;
        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;
        private const uint Restore = 9;

        internal static IDictionary<HWND, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows[hWnd] = builder.ToString();
                return true;

            }, 0);

            return windows;
        }

        internal static bool FocusWindow(HWND hwnd)
        {

            if (hwnd == GetForegroundWindow()) return false;

            //check if window is minimized
            if (IsIconic(hwnd))
            {
                ShowWindow(hwnd, Restore);
            }

            // Simulate a key press
            keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);


            // Simulate a key release
            keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);

           return SetForegroundWindow(hwnd);
        }

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern HWND GetShellWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(HWND hWnd, out uint processId);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(HWND hWnd);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(HWND hWnd);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(HWND hWnd, uint Msg);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
    }
}

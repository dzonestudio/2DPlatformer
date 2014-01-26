using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hweny.WinApiHelper
{
    public sealed class WinWindow
    {
        private WinWindow() { }

        public static bool PeekMessage(out Msg lpMsg, IntPtr hwnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg)
        {
            return WinWndApi.PeekMessage(out lpMsg, hwnd, wMsgFilterMin, wMsgFilterMax, wRemoveMsg); ;
        }

        public static bool TranslateMessage(ref Msg lpMsg)
        {
            return WinWndApi.TranslateMessage(ref lpMsg);
        }

        public static bool DispatchMessage(ref Msg lpMsg)
        {
            return WinWndApi.DispatchMessage(ref lpMsg);
        }

        public static void PostQuitMessage()
        {
            WinWndApi.PostQuitMessage((uint)WindowsMessage.WM_QUIT);
        }
    }
}

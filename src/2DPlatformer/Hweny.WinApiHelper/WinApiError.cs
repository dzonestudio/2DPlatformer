using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Hweny.WinApiHelper
{
    internal sealed class WinApiError
    {
        private WinApiError() { }

        public static int GetLastWin32Error()
        {
            return Marshal.GetLastWin32Error();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hweny.WinApiHelper
{
    /// <summary>
    /// 标准控制台输入输出
    /// </summary>
    public sealed class WinConsole
    {
        private WinConsole() { }

        /// <summary>
        /// 为调用进程分配一个控制台
        /// </summary>
        public static void AllocConsole()
        {
            if (!WinConApi.AllocConsole())
            {
                throw new Exception(
                    "alloc console faild!error:" + WinApiError.GetLastWin32Error());
            }
        }

        /// <summary>
        /// 分离调用进程所挂接的控制台
        /// </summary>
        public static void FreeConsole()
        {
            WinConApi.FreeConsole();
        }

        public static void SetWindowTitle(string title)
        {
            Console.Title = title;
        }

        public static void Write(string value)
        {
            Console.Write(value);
        }

        public static void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static void SetCharacterAttribute(
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor)
        {
            SetForegroundColor(foregroundColor);
            SetBackgroundColor(backgroundColor);
        }

        public static void SetForegroundColor(ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
        }

        public static void SetBackgroundColor(ConsoleColor backgroundColor)
        {
            Console.BackgroundColor = backgroundColor;
        }

        public static void ResetColor()
        {
            Console.ResetColor();
        }

        public static void SetCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public static void SetCursorVisible(bool visible)
        {
            Console.CursorVisible = visible;
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}

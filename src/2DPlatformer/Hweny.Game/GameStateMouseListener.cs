using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hweny.Game
{
    /// <summary>
    /// 鼠标监听接口，需要监听鼠标输入的游戏状态必须实现该接口
    /// </summary>
    public interface GameStateMouseListener
    {
        /// <summary>
        /// 按压鼠标
        /// </summary>
        /// <param name="x">当前鼠标光标在游戏窗口工作区中的x坐标</param>
        /// <param name="y">当前鼠标光标在游戏窗口工作区中的y坐标</param>
        /// <param name="button">当前按下的鼠标按键</param>
        void MousePressed(int x, int y, GameMouseButtons button);
        /// <summary>
        /// 释放鼠标输入
        /// </summary>
        /// <param name="x">当前鼠标光标在游戏窗口工作区中的x坐标</param>
        /// <param name="y">当前鼠标光标在游戏窗口工作区中的y坐标</param>
        /// <param name="button">当前释放的鼠标按键</param>
        void MouseReleased(int x, int y, GameMouseButtons button);
    }

    [Flags]
    public enum GameMouseButtons
    {
        None = 0,
        Left = 1048576,
        Right = 2097152,
        Middle = 4194304,
        XButton1 = 8388608,
        XButton2 = 16777216,
    }

}

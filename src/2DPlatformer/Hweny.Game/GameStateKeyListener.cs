using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hweny.Game
{
    /// <summary>
    /// 键盘监听接口，需要监听键盘输入的游戏状态必须实现该接口
    /// </summary>
    public interface GameStateKeyListener
    {
        void KeyPressed(GameKeyEventArgs e);
        void KeyReleased(GameKeyEventArgs e);
    }

    /// <summary>
    /// 游戏键盘事件参数
    /// </summary>
    public class GameKeyEventArgs : KeyEventArgs
    {
        public GameKeyEventArgs(Keys keyData)
            : base(keyData)
        {

        }
    }
}

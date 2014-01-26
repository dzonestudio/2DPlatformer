/* 
 * ///////////////////////////////////////////////////////////////////// 
 * Filename: GameState.cs 
 * Author  : D-Zone Studio hweny(dzonestudio@163.com) 
 * Date    : 2014/1/23      
 * Resume  : 游戏状态基类，提供每一个派生的游戏状态所需要的游戏初始化、更新、绘制和清理等实现接口；
 *  
 * ///////////////////////////////////////////////////////////////////// 
 * Modifiy History 
 *  
 * Date    : 
 * Resume  : 
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Hweny.Game
{
    /// <summary>
    /// 游戏状态基类
    /// </summary>
    public abstract class GameState
    {
        internal GameStateManager gameStateManager;
        /// <summary>
        /// 游戏上下文对象
        /// </summary>
        protected GameEngineContext GameEngineContext
        {
            get
            {
                return gameStateManager.GameContext;
            }
        }

        protected GameState() { }

        /// <summary>
        /// 状态初始化
        /// </summary>
        protected internal abstract void Startup();

        /// <summary>
        /// 状态更新
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="elapsedSeconds"></param>
        protected internal abstract void Update(long gameTime, long elapsedSeconds);

        /// <summary>
        /// 状态渲染
        /// </summary>
        /// <param name="g"></param>
        protected internal abstract void Render(Graphics g);

        /// <summary>
        /// 状态清理
        /// </summary>
        protected internal abstract void Cleanup();

        /// <summary>
        /// 用指定状态替换当前状态，该状态必须为状态列表中有效的状态
        /// </summary>
        /// <param name="name"></param>
        protected void Change(string name)
        {
            gameStateManager.Change(name);
        }

        /// <summary>
        /// 指定一个状态压人状态栈，成为当前状态，该状态必须为状态列表中有效的状态
        /// </summary>
        /// <param name="name"></param>
        protected void Push(string name)
        {
            gameStateManager.Push(name);
        }

        /// <summary>
        /// 从状态栈中弹出一个状态，栈顶元素为当前状态
        /// </summary>
        protected void Pop()
        {
            gameStateManager.Pop();
        }
    }
}

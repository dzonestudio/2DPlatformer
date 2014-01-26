/* 
 * ///////////////////////////////////////////////////////////////////// 
 * Filename: GameStateManager.cs 
 * Author  : D-Zone Studio hweny(dzonestudio@163.com) 
 * Date    : 2014/1/23      
 * Resume  : 游戏状态管理类，游戏中每一个场景都属于一个状态，该类维护游戏所有状态的健值对，
 *           采用线性栈对状态进行调度，当前栈顶元素为游戏当前状态。
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
using Hweny.Utility;

namespace Hweny.Game
{
    /// <summary>
    /// 游戏状态管理类，游戏所有状态维护在该类的状态键\值对列表当中，
    /// 使用状态栈的方式对状态进行调度，栈顶元素为当前游戏运行的状态
    /// </summary>
    internal sealed class GameStateManager
    {
        private GameEngineContext gameContext;
        private Dictionary<string, GameState> states;
        private Stack<GameState> statesStack;

        /// <summary>
        /// 获取当前运行的游戏状态
        /// </summary>
        public GameState CurrentState
        {
            get
            {
                if (statesStack.Count == 0)
                {
                    return null;
                }
                return statesStack.Peek();
            }
        }

        /// <summary>
        /// 游戏上下文
        /// </summary>
        public GameEngineContext GameContext
        {
            get
            {
                return gameContext;
            }
        }

        public GameStateManager(GameEngineContext context)
        {
            gameContext = context;
            states = new Dictionary<string, GameState>();
            statesStack = new Stack<GameState>();
        }

        /// <summary>
        /// 添加一个游戏状态到游戏状态列表中
        /// </summary>
        /// <param name="name">状态名称</param>
        /// <param name="state">游戏状态对象</param>
        public void AddState(string name, GameState state)
        {
            string upperName = name.Upper();
            if (upperName == null)
            {
                throw new ArgumentNullException("state name is null!");
            }
            try
            {
                if (!states.Keys.Contains(upperName))
                {
                    state.gameStateManager = this;
                    states.Add(upperName, state);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 从游戏状态列表中移除一个游戏状态
        /// </summary>
        /// <param name="name">状态名称</param>
        public void RemoveState(string name)
        {
            string upperName = name.Upper();
            if (upperName != null)
            {
                if (states.Keys.Contains(upperName))
                {
                    states.Remove(upperName);
                }
            }
        }

        /// <summary>
        /// 清空状态栈及游戏状态列表
        /// </summary>
        public void Clear()
        {
            statesStack.Clear();
            states.Clear();
        }

        /// <summary>
        /// 用指定游戏状态替换当前运行的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        public void Change(string name)
        {
            string upperName = name.Upper();
            if (upperName == null)
            {
                throw new ArgumentNullException("state name is null!");
            }
            if (!states.Keys.Contains(upperName))
            {
                throw new ArgumentException("state '" + name + "' does not exist!");
            }
            if (CurrentState != null)
            {
                CurrentState.Cleanup();
                statesStack.Pop();
            }
            statesStack.Push(states[upperName]);
            CurrentState.Startup();
        }

        /// <summary>
        /// 往状态栈压入一个游戏状态，成为当前游戏状态
        /// </summary>
        /// <param name="name">状态名称</param>
        public void Push(string name)
        {
            string upperName = name.Upper();
            if (upperName == null)
            {
                throw new ArgumentNullException("state name is null!");
            }
            if (!states.Keys.Contains(upperName))
            {
                throw new ArgumentException("state '" + name + "' does not exist!");
            }
            if (CurrentState != null)
            {
                CurrentState.Cleanup();
            }
            statesStack.Push(states[upperName]);
            CurrentState.Startup();
        }

        /// <summary>
        /// 从状态栈弹出一个游戏状态，栈顶元素为当前游戏状态
        /// </summary>
        public void Pop()
        {
            if (CurrentState != null)
            {
                CurrentState.Cleanup();
                statesStack.Pop();
                if (CurrentState != null)
                {
                    CurrentState.Startup();
                }
            }
        }

        /// <summary>
        /// 更新当前游戏状态逻辑
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="elapsedSeconds"></param>
        public void Update(long gameTime, long elapsedSeconds)
        {
            if (CurrentState == null)
            {
                return;
            }
            CurrentState.Update(gameTime, elapsedSeconds);
        }

        /// <summary>
        /// 渲染当前游戏状态画面
        /// </summary>
        /// <param name="g"></param>
        public void Render(Graphics g)
        {
            if (CurrentState == null)
            {
                return;
            }
            CurrentState.Render(g);
        }

        /// <summary>
        /// 处理当前游戏状态键盘输入事件
        /// </summary>
        /// <param name="e"></param>
        public void KeyPressed(GameKeyEventArgs e)
        {
            if (CurrentState == null)
            {
                return;
            }
            GameStateKeyListener keyListener = CurrentState as GameStateKeyListener;
            if (keyListener != null)
            {
                keyListener.KeyPressed(e);
            }
        }

        /// <summary>
        /// 处理当前游戏状态键盘输入事件
        /// </summary>
        /// <param name="e"></param>
        public void KeyReleased(GameKeyEventArgs e)
        {
            if (CurrentState == null)
            {
                return;
            }
            GameStateKeyListener keyListener = CurrentState as GameStateKeyListener;
            if (keyListener != null)
            {
                keyListener.KeyReleased(e);
            }
        }

        /// <summary>
        /// 处理当前游戏状态鼠标输入事件
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="button"></param>
        public void MousePressed(int x, int y, GameMouseButtons button)
        {
            if (CurrentState == null)
            {
                return;
            }
            GameStateMouseListener mouseListener = CurrentState as GameStateMouseListener;
            if (mouseListener != null)
            {
                mouseListener.MousePressed(x,y,button);
            }
        }

        /// <summary>
        /// 处理当前游戏状态鼠标输入事件
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="button"></param>
        public void MouseReleased(int x, int y, GameMouseButtons button)
        {
            if (CurrentState == null)
            {
                return;
            }
            GameStateMouseListener mouseListener = CurrentState as GameStateMouseListener;
            if (mouseListener != null)
            {
                mouseListener.MouseReleased(x, y, button);
            }
        }
    }
}

/* 
 * ///////////////////////////////////////////////////////////////////// 
 * Filename: GameEngineContext.cs 
 * Author  : D-Zone Studio hweny(dzonestudio@163.com) 
 * Date    : 2014/1/23      
 * Resume  : 游戏上下文类，可以通过此类获取当前游戏引擎信息
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
    public class GameEngineContext
    {
        private GameEngine gameEngine;
        internal GameEngineContext(GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
        }

        /// <summary>
        /// 游戏窗体标题
        /// </summary>
        public string Title
        {
            get
            {
                return gameEngine.title;
            }
        }

        /// <summary>
        /// 游戏屏幕宽度
        /// </summary>
        public int ScreenWidth
        {
            get
            {
                return gameEngine.width;
            }
        }

        /// <summary>
        /// 游戏屏幕高度
        /// </summary>
        public int ScreenHeight
        {
            get
            {
                return gameEngine.height;
            }
        }

        /// <summary>
        /// 游戏最大帧速率
        /// </summary>
        public int MaxFps
        {
            get
            {
                return gameEngine.maxFps;
            }
        }

        /// <summary>
        /// 游戏当前帧速率
        /// </summary>
        public int Fps
        {
            get
            {
                return gameEngine.fps;
            }
        }

        /// <summary>
        /// 游戏引擎绘图缓冲区
        /// </summary>
        public Image BufferGraphicsSurface
        {
            get
            {
                return gameEngine.bufferImage;
            }
        }

        /// <summary>
        /// 游戏窗体句柄
        /// </summary>
        public IntPtr Hwnd
        {
            get
            {
                return gameEngine.gameWindow.Handle;
            }
        }

        /// <summary>
        /// 游戏引擎是否在运行
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return gameEngine.isRunning;
            }
        }
    }
}

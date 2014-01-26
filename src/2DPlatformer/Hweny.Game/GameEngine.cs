/* 
 * ///////////////////////////////////////////////////////////////////// 
 * Filename: GameEngine.cs 
 * Author  : D-Zone Studio hweny(dzonestudio.hweny@gmail.com) 
 * Date    : 2014/1/23      
 * Resume  : 游戏引擎类，管理游戏的创建、输入、运行与清理。
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
using System.Windows.Forms;
using Hweny.Utility;

namespace Hweny.Game
{
    public sealed class GameEngine
    {
        #region Consts

        private const string DEFAULT_GAME_TITLE = "2DPlatform game engine 1.0";
        private const int DEFAULT_SCREEN_WIDTH = 480;
        private const int DEFAULT_SCREEN_HEIGHT = 320;
        private const int DEFAULT_MAX_FPS = 60;

        #endregion

        #region fields

        /// <summary>
        /// 游戏窗体标题
        /// </summary>
        internal string title = DEFAULT_GAME_TITLE;
        /// <summary>
        /// 游戏屏幕宽度
        /// </summary>
        internal int width = DEFAULT_SCREEN_WIDTH;
        /// <summary>
        /// 游戏屏幕高度
        /// </summary>
        internal int height = DEFAULT_SCREEN_HEIGHT;
        /// <summary>
        /// 最大fps
        /// </summary>
        internal int maxFps = DEFAULT_MAX_FPS;
        /// <summary>
        /// 当前fps
        /// </summary>
        internal int fps;
        /// <summary>
        /// 双缓冲绘制画面，减少闪烁
        /// </summary>
        internal Image bufferImage;
        /// <summary>
        /// 初始游戏状态
        /// </summary>
        internal string initGameState = null;
        /// <summary>
        /// 游戏引擎是否在运行
        /// </summary>
        internal bool isRunning = false;
        /// <summary>
        /// 游戏窗体对象
        /// </summary>
        internal GameWindow gameWindow;
        /// <summary>
        /// 游戏状态管理对象
        /// </summary>
        private GameStateManager gameStateManager;


        #endregion

        #region Constructor

        public GameEngine()
        {
            Initialize(title, width, height, maxFps);
        }

        public GameEngine(string title, int width, int height, int maxFps)
        {
            Initialize(title, width, height, maxFps);
        }

        private void Initialize(string title, int width, int height, int maxFps)
        {
            GameDebuger.GetInstance().AddDebugInfo("GameEngine::init", "begin!");
            this.title = title;
            if (width > 0)
            {
                this.width = width;
            }
            if (height > 0)
            {
                this.height = height;
            }
            if (maxFps > 0)
            {
                this.maxFps = maxFps;
            }
            gameStateManager = new GameStateManager(new GameEngineContext(this));
            GameDebuger.GetInstance().AddDebugInfo("GameEngine::init", "finished!");
        }

        #endregion

        #region Game Startup Update Render Cleanup

        /// <summary>
        /// 游戏初始化
        /// </summary>
        private void Startup()
        {
            try
            {
                GameDebuger.GetInstance().AddDebugInfo("Game::startup", "begin!");
                //create buffer image
                bufferImage = new Bitmap(width, height);
                if (initGameState != null)
                {
                    gameStateManager.Push(initGameState);
                }
                GameDebuger.GetInstance().AddDebugInfo("Game::startup", "finished!");
            }
            catch (Exception e)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::startup", e.Message);
            }
        }

        /// <summary>
        /// 更新游戏逻辑
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="elapsedSeconds"></param>
        private void Update(long gameTime, long elapsedSeconds)
        {
            try
            {
                gameStateManager.Update(gameTime, elapsedSeconds);
            }
            catch (Exception e)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::update", e.Message, false);
            }
        }

        /// <summary>
        /// 渲染游戏画面
        /// </summary>
        /// <param name="g"></param>
        private void Render(Graphics g)
        {
            try
            {
                //draw frame to buffer bitmap
                Graphics bufferImageGraphics = Graphics.FromImage(bufferImage);
                bufferImageGraphics.Clear(Color.Black);
                gameStateManager.Render(bufferImageGraphics);
                bufferImageGraphics.Dispose();
                //draw buffer bitmap to screen
                g.DrawImage(bufferImage, 0, 0, bufferImage.Width, bufferImage.Height);
            }
            catch (Exception e)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::render", e.Message, false);
            }
        }

        /// <summary>
        /// 清理游戏资源
        /// </summary>
        private void Cleanup()
        {
            try
            {
                GameDebuger.GetInstance().AddDebugInfo("Game::cleanup", "begin!");
                gameStateManager.Clear();
                if (bufferImage != null)
                {
                    bufferImage.Dispose();
                    bufferImage = null;
                }
                UnRegisterWindowEvents();
                GameDebuger.GetInstance().AddDebugInfo("Game::cleanup", "finished!");
            }
            catch (Exception e)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::cleanup", e.Message);
            }
        }

        #endregion

        #region Game Input

        /// <summary>
        /// 按下键盘
        /// </summary>
        /// <param name="e"></param>
        private void KeyPressed(GameKeyEventArgs e)
        {
            try
            {
                gameStateManager.KeyPressed(e);
            }
            catch (Exception ex)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::keyPressed", ex.Message, false);
            }
        }

        /// <summary>
        /// 释放键盘
        /// </summary>
        /// <param name="e"></param>
        private void KeyReleased(GameKeyEventArgs e)
        {
            try
            {
                gameStateManager.KeyReleased(e);
            }
            catch (Exception ex)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::keyReleased", ex.Message, false);
            }
        }

        /// <summary>
        /// 按下鼠标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="button"></param>
        private void MousePressed(int x, int y, GameMouseButtons button)
        {
            try
            {
                gameStateManager.MousePressed(x, y, button);
            }
            catch (Exception ex)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::mousePressed", ex.Message, false);
            }
        }

        /// <summary>
        /// 释放鼠标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="button"></param>
        private void MouseReleased(int x, int y, GameMouseButtons button)
        {
            try
            {
                gameStateManager.MouseReleased(x, y, button);
            }
            catch (Exception ex)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::mouseReleased", ex.Message, false);
            }
        }

        #endregion

        #region Game State Manager

        /// <summary>
        /// 添加一个游戏状态对象到游戏状态列表中
        /// </summary>
        /// <param name="name">状态名</param>
        /// <param name="state">游戏状态</param>
        public void AddGameState(string name, GameState state)
        {
            try
            {
                gameStateManager.AddState(name, state);
            }
            catch (Exception e)
            {
                GameDebuger.GetInstance().AddErrorInfo("Game::addGameState", e.Message);
            }
        }

        #endregion

        #region Game Run
        
        /// <summary>
        /// 启动游戏引擎并指定初始的游戏状态
        /// </summary>
        /// <param name="stateName">游戏状态名</param>
        public void Run(string stateName = null)
        {
            if (!isRunning)
            {
                isRunning = true;
                //指定初始游戏状态
                initGameState = stateName;
                //初始化窗体
                gameWindow = new GameWindow();
                gameWindow.Text = title;
                gameWindow.ClientSize = new Size(width, height);
                gameWindow.MaxFps = maxFps;
                //注册窗体事件
                RegisterWindowEvents();
                gameWindow.Run();
            }
        }

        /// <summary>
        /// 注册窗体事件
        /// </summary>
        /// <param name="wnd"></param>
        private void RegisterWindowEvents()
        {
            GameDebuger.GetInstance().AddDebugInfo("Game::registerWindowEvents", "begin!");
            gameWindow.WndStartup += wnd_WndStartup;
            gameWindow.WndUpdate += wnd_WndUpdate;
            gameWindow.WndRender += wnd_WndRender;
            gameWindow.WndCalculateFps += wnd_WndCalculateFps;
            gameWindow.WndCleanup += wnd_WndCleanup;
            gameWindow.WndKeyDown += wnd_WndKeyDown;
            gameWindow.WndKeyUp += wnd_WndKeyUp;
            gameWindow.WndMouseDown += wnd_WndMouseDown;
            gameWindow.WndMouseMove += wnd_WndMouseMove;
            gameWindow.WndMouseUp += wnd_WndMouseUp;
            GameDebuger.GetInstance().AddDebugInfo("Game::registerWindowEvents", "finished!");
        }

        /// <summary>
        /// 注销窗体事件
        /// </summary>
        /// <param name="wnd"></param>
        private void UnRegisterWindowEvents()
        {
            if (gameWindow == null) return;
            GameDebuger.GetInstance().AddDebugInfo("Game::unRegisterWindowEvents", "begin!");
            gameWindow.WndStartup -= wnd_WndStartup;
            gameWindow.WndUpdate -= wnd_WndUpdate;
            gameWindow.WndRender -= wnd_WndRender;
            gameWindow.WndCalculateFps -= wnd_WndCalculateFps;
            gameWindow.WndCleanup -= wnd_WndCleanup;
            gameWindow.WndKeyDown -= wnd_WndKeyDown;
            gameWindow.WndKeyUp -= wnd_WndKeyUp;
            gameWindow.WndMouseDown -= wnd_WndMouseDown;
            gameWindow.WndMouseMove -= wnd_WndMouseMove;
            gameWindow.WndMouseUp -= wnd_WndMouseUp;
            GameDebuger.GetInstance().AddDebugInfo("Game::unRegisterWindowEvents", "finished!");
        }

        private void wnd_WndStartup(object sender, EventArgs e)
        {
            Startup();
        }

        private void wnd_WndUpdate(object sender, WndUpdateEventArgs e)
        {
            Update(e.GameTime, e.ElapsedSeconds);
        }

        private void wnd_WndRender(object sender, WndRenderEventArgs e)
        {
            Render(e.Graphics);
        }

        private void wnd_WndCalculateFps(object sender, WndCalculateFpsEventArgs e)
        {
            this.fps = e.CurrentFps;
        }

        private void wnd_WndCleanup(object sender, EventArgs e)
        {
            Cleanup();
        }

        private void wnd_WndKeyDown(object sender, GameKeyEventArgs e)
        {
            KeyPressed(e);
        }

        private void wnd_WndKeyUp(object sender, GameKeyEventArgs e)
        {
            KeyReleased(e);
        }

        private void wnd_WndMouseDown(object sender, MouseEventArgs e)
        {
            MousePressed(e.X, e.Y, (GameMouseButtons)e.Button);
        }

        private void wnd_WndMouseMove(object sender, MouseEventArgs e)
        {
            MousePressed(e.X, e.Y, (GameMouseButtons)e.Button);
        }

        private void wnd_WndMouseUp(object sender, MouseEventArgs e)
        {
            MouseReleased(e.X, e.Y, (GameMouseButtons)e.Button);
        }

        #endregion
    }
}

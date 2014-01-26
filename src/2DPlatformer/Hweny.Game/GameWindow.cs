/* 
 * ///////////////////////////////////////////////////////////////////// 
 * Filename: GameWindow.cs 
 * Author  : D-Zone Studio hweny(dzonestudio.hweny@gmail.com) 
 * Date    : 2014/1/23      
 * Resume  : 游戏窗体类，采用windows消息循环驱动游戏的运行。
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
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Hweny.WinApiHelper;
using Hweny.Utility;

namespace Hweny.Game
{
    /// <summary>
    /// 游戏窗体类
    /// </summary>
    internal sealed class GameWindow : Form
    {
        #region Constructor

        /// <summary>
        /// 最大fps
        /// </summary>
        public int MaxFps
        {
            get;
            set;
        }

        public GameWindow()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.White;
        }

        #endregion

        #region Window Input

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            GameKeyEventArgs keyEventArgs = new GameKeyEventArgs(e.KeyCode);
            OnWndEventHandler<GameKeyEventArgs>(keyEventArgs, WndKeyDown);
            //缺省的键盘输入事件
            //如果不指示Handled值或Handled值为false，按Escape键时将退出应用程序。
            if (!keyEventArgs.Handled && keyEventArgs.KeyCode == Keys.Escape)
            {
                WinWindow.PostQuitMessage();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            OnWndEventHandler<GameKeyEventArgs>(new GameKeyEventArgs(e.KeyCode), WndKeyUp);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            OnWndEventHandler<MouseEventArgs>(e, WndMouseDown);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            OnWndEventHandler<MouseEventArgs>(e, WndMouseMove);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            OnWndEventHandler<MouseEventArgs>(e, WndMouseUp);
        }

        #endregion

        #region Window Debug Console

        /// <summary>
        /// 为调用进程分配一个控制台，仅在调试模式下使用
        /// </summary>
        [Conditional("DEBUG")]
        private void AllocConsole()
        {
            WinConsole.AllocConsole();
            WinConsole.SetWindowTitle("Fps: calculating...");
            WinConsole.SetCursorVisible(false);
            WinConsole.SetForegroundColor(ConsoleColor.Green);
            GameDebuger.GetInstance().Update += new EventHandler(UpdateConsoleOutput);
        }

        /// <summary>
        /// 分离调用进程当前附加的控制台
        /// </summary>
        [Conditional("DEBUG")]
        private void FreeConsole()
        {
            WinConsole.FreeConsole();
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        [Conditional("DEBUG")]
        private void OutputDebugerMsg()
        {
            WinConsole.Clear();
            WinConsole.WriteLine(GameDebuger.GetInstance().OutputDebugInfo());
        }

        /// <summary>
        /// 更新控制台输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateConsoleOutput(object sender, EventArgs e)
        {
            try
            {
                if (!this.Created)
                {
                    return;
                }
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    OutputDebugerMsg();
                }));
            }
            catch { }
        }

        #endregion

        #region Window Message Loop

        /// <summary>
        /// 窗体消息循环
        /// </summary>
        private void WndMain()
        {
            AllocConsole();

            this.Show();
            IntPtr hwnd = this.Handle;

            long gameTime = 0L;
            long elapsedTime = 0L;
            long lastTime = 0L;
            long accumulatedTime = 0L;
            long timeDiff = 0L;
            long FRAME_PERIOD = 1000 / MaxFps;
            long lastFpsTime = 0L;
            int fpsTicks = 0;
            bool updateFrame = false;

            long FRAME_PERIOD_SECONDS = FRAME_PERIOD / 1000;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            //window startup
            OnWndEventHandler<EventArgs>(null, WndStartup);

            Msg msg = new Msg();
            while (msg.message != (uint)WindowsMessage.WM_QUIT)
            {
                if (!Created) break;
                if (WinWindow.PeekMessage(out msg, IntPtr.Zero, 0, 0, (uint)PM.REMOVE))
                {
                    WinWindow.TranslateMessage(ref msg);
                    WinWindow.DispatchMessage(ref msg);
                }
                else
                {
                    gameTime = sw.ElapsedMilliseconds;
                    elapsedTime = gameTime - lastTime;
                    lastTime = gameTime;

                    //更新游戏逻辑
                    accumulatedTime += elapsedTime;
                    updateFrame = false;
                    while (accumulatedTime > FRAME_PERIOD)
                    {
                        accumulatedTime -= FRAME_PERIOD;
                        //window update
                        OnWndEventHandler<WndUpdateEventArgs>(
                            new WndUpdateEventArgs(gameTime, FRAME_PERIOD_SECONDS), WndUpdate);
                        updateFrame = true;
                    }

                    //渲染游戏画面
                    if (updateFrame)
                    {
                        Graphics g = Graphics.FromHwnd(hwnd);
                        //window render
                        OnWndEventHandler<WndRenderEventArgs>(new WndRenderEventArgs(g), WndRender);
                        g.Dispose();
                    }

                    //计算fps
                    fpsTicks++;
                    if (gameTime - lastFpsTime > 1000)
                    {
                        #region for debug
#if DEBUG
                        WinConsole.SetWindowTitle("Fps:" + fpsTicks);
#endif
                        #endregion
                        //window calculate fps
                        OnWndEventHandler<WndCalculateFpsEventArgs>(
                            new WndCalculateFpsEventArgs(fpsTicks), WndCalculateFps);
                        lastFpsTime = gameTime;
                        fpsTicks = 0;
                    }

                    //锁定帧速率
                    timeDiff = sw.ElapsedMilliseconds - gameTime;
                    int sleepTime = (int)(FRAME_PERIOD - timeDiff);
                    if (sleepTime > 0)
                    {
                        System.Threading.Thread.Sleep(sleepTime);
                    }

                }
            }
            //window cleanup
            sw.Stop();
            sw = null;
            OnWndEventHandler<EventArgs>(null, WndCleanup);

            OutputDebugerMsg();
            FreeConsole();

            Application.Exit();
        }

        public void Run()
        {
         //   try
         //   {
                WndMain();
          //  }
          //  catch
          //  {
           // }
        }

        #endregion

        #region Window Events

        /// <summary>
        /// 窗体启动事件
        /// </summary>
        public event EventHandler<EventArgs> WndStartup;
        /// <summary>
        /// 窗体更新事件
        /// </summary>
        public event EventHandler<WndUpdateEventArgs> WndUpdate;
        /// <summary>
        /// 窗体绘制事件
        /// </summary>
        public event EventHandler<WndRenderEventArgs> WndRender;
        /// <summary>
        /// 更新Fps事件
        /// </summary>
        public event EventHandler<WndCalculateFpsEventArgs> WndCalculateFps;
        /// <summary>
        /// 窗体清理事件
        /// </summary>
        public event EventHandler<EventArgs> WndCleanup;
        /// <summary>
        /// 窗体键盘输入事件
        /// </summary>
        public event EventHandler<GameKeyEventArgs> WndKeyDown;
        /// <summary>
        /// 窗体键盘释放事件
        /// </summary>
        public event EventHandler<GameKeyEventArgs> WndKeyUp;
        /// <summary>
        /// 窗体鼠标输入事件
        /// </summary>
        public event EventHandler<MouseEventArgs> WndMouseDown;
        /// <summary>
        /// 窗体鼠标移动事件
        /// </summary>
        public event EventHandler<MouseEventArgs> WndMouseMove;
        /// <summary>
        /// 窗体鼠标释放事件
        /// </summary>
        public event EventHandler<MouseEventArgs> WndMouseUp;

        #endregion

        #region Window EventHandlers

        /// <summary>
        /// 响应窗体事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="handler"></param>
        private void OnWndEventHandler<T>(T t, EventHandler<T> handler) where T : EventArgs
        {
            EventHandler<T> temp = handler;
            if (temp != null)
            {
                temp(this, t);
            }
        }

        #endregion
    }

    /// <summary>
    /// 窗体更新事件参数
    /// </summary>
    internal class WndUpdateEventArgs : EventArgs
    {
        public long GameTime
        {
            get;
            private set;
        }
        public long ElapsedSeconds
        {
            get;
            private set;
        }
        public WndUpdateEventArgs(long gameTime, long elapsedSeconds)
        {
            this.GameTime = gameTime;
            this.ElapsedSeconds = elapsedSeconds;
        }
    }

    /// <summary>
    /// 窗体绘制事件参数
    /// </summary>
    internal class WndRenderEventArgs : EventArgs
    {
        public Graphics Graphics
        {
            get;
            private set;
        }
        public WndRenderEventArgs(Graphics g)
        {
            this.Graphics = g;
        }
    }

    /// <summary>
    /// 更新Fps事件参数
    /// </summary>
    internal class WndCalculateFpsEventArgs : EventArgs
    {
        public int CurrentFps
        {
            get;
            private set;
        }
        public WndCalculateFpsEventArgs(int fps)
        {
            this.CurrentFps = fps;
        }
    }
}

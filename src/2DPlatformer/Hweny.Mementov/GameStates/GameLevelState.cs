using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hweny.Game;
using Hweny.Utility;
using System.Drawing;

namespace Hweny.Mementov.GameStates
{
    public class GameLevelState : GameState, GameStateMouseListener, GameStateKeyListener
    {

        protected override void Startup()
        {
            GameDebuger.GetInstance().AddDebugInfo("GameState::Startup", "Level");
        }

        protected override void Update(long gameTime, long elapsedSeconds)
        {
            ball.Move();
        }

        class Ball
        {
            public float x { get; set; }
            public float y { get; set; }
            public float oldx { get; set; }
            public float oldy { get; set; }

            float slope = 0f;
            float b = 0f;

            float v = 0;
            public void Move()
            {

                oldx = x;
                oldy = y;

                v += 0.0001f;
                x += v;
                y += 0.1f;

                if (x - oldx == 0) return;

                slope = (y - oldy) / (x - oldx);
                b = oldy - slope * oldx;
            }

            public void Draw(Graphics g)
            {
                g.DrawEllipse(Pens.Yellow, x - 5, y - 5, 10, 10);

                if (slope == 0) return;

                PointF pt1 = new PointF();
                pt1.Y = 50;
                pt1.X = (50 - b) / slope;
                PointF pt2 = new PointF();
                pt2.Y = 200;
                pt2.X = (200 - b) / slope;

                g.DrawLine(Pens.Red, pt1, pt2);

            }
        }

        Ball ball = new Ball() { x = 100, y = 100 };
        protected override void Render(System.Drawing.Graphics g)
        {
            //TestRender(g);
            //  ball.Draw(g);
        }

        #region Game Test

        //Random r = new Random(Environment.TickCount);
        //private void TestRender(Graphics g)
        //{
        //    var gameApp=GameEngine.GetInstance();
        //    for (int i = 0; i < 200; i++)
        //    {
        //        int R = r.Next(0, 250);
        //        int G = r.Next(0, 250);
        //        int B = r.Next(0, 250);
        //        using (Pen pen = new Pen(Color.FromArgb(R, G, B),r.Next(1,5)))
        //        {
        //            g.DrawRectangle(pen, r.Next(1,gameApp.ScreenWidth), r.Next(1,gameApp.ScreenHeight), r.Next(10, 100), r.Next(10, 100));
        //        }
        //    }
        //}

        #endregion

        protected override void Cleanup()
        {

        }

        public void MousePressed(int x, int y, GameMouseButtons button)
        {
            if (button == GameMouseButtons.Left)
            {
                Pop();
            }
        }

        public void MouseReleased(int x, int y, GameMouseButtons button)
        {

        }

        public void KeyPressed(GameKeyEventArgs e)
        {

        }

        public void KeyReleased(GameKeyEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hweny.Game;
using Hweny.Utility;
using System.Drawing;

namespace Hweny.Mementov.GameStates
{
    public class GameMenuState : GameState, GameStateMouseListener, GameStateKeyListener
    {
        protected override void Startup()
        {
            GameDebuger.GetInstance().AddDebugInfo("GameState::Startup", "Menu");

            x = 0f;
            y = 32 * 7f;
            vx = vy = 0f;

            tiles = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tiles[i, j] = 0;
                }
            }
            tiles[2, 7] = 135;
            tiles[3, 6] = 135;
            tiles[4, 5] = 135;
            tiles[5, 5] = 45;
        }

        float x = 200f;
        float y = 100f;
        float vx;
        float vy;
        float g = 1.6f;
        float angle = 0;


        int[,] tiles;
        protected override void Update(long gameTime, long elapsedSeconds)
        {
          //  GameDebuger.GetInstance().AddDebugInfo("angle", (int)x / 32 + "  " + (int)y / 32 + " " + angle, false);

            if (left)
            {
                angle = tiles[(int)(x - 2) / 32, (int)(y + 16) / (32)];
                //if (angle == 0)
                //{
                //    angle = tiles[(int)(x - 2) / 32, (int)(y + 48) / (32)];
                //}
           //     GameDebuger.GetInstance().AddDebugInfo("angle", (int)x / 32 + "  " + (int)y / 32 + " " + angle, false);
                angle = angle * (float)Math.PI / 180f;

                float x1 = (float)(Math.Cos(angle) * Math.Cos(angle) * -5);
                float y1 = (float)(Math.Sin(angle) * Math.Cos(angle) * -5);
                vx = x1;
                vy = y1;

                vx += (float)(Math.Sin(angle) * Math.Cos(angle) * g);
                vy += (float)(Math.Sin(angle) * Math.Sin(angle) * g);
            }
            if (right)
            {
                angle = tiles[(int)(x + 2) / 32, (int)(y + 32) / (32)];
                if (angle == 0)
                {
                    angle = tiles[(int)(x + 30) / 32, (int)(y + 33) / (32)];
                }
                GameDebuger.GetInstance().AddDebugInfo("angle", (int)x / 32 + "  " + (int)y / 32 + " " + angle, false);
                angle = angle * (float)Math.PI / 180f;

                float x1 = (float)(Math.Cos(angle) * Math.Cos(angle) * 5);
                float y1 = (float)(Math.Sin(angle) * Math.Cos(angle) * 5);
                vx = x1;
                vy = y1;

                vx += (float)(Math.Sin(angle) * Math.Cos(angle) * g);
                vy += (float)(Math.Sin(angle) * Math.Sin(angle) * g);
            }

            x += vx;
            y += vy;

            //if (tiles[(int)x / 32, (int)y / 32+1] == 0)
            //{
            //    y = ((int)y / 32) * 32;
            //}
        }

        protected override void Render(System.Drawing.Graphics g)
        {
            g.DrawRectangle(Pens.Red, x, y, 32, 32);
            g.DrawLine(Pens.Blue, 200, 100, 0, 300);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (tiles[i, j] != 0)
                    {
                        g.DrawRectangle(Pens.Red, i * 32, j * 32, 32, 32);
                    }
                }
            }
        }

        protected override void Cleanup()
        {

        }

       void GameStateMouseListener.MousePressed(int x, int y, GameMouseButtons button)
        {
            if (button == GameMouseButtons.Left)
            {
                Push("Level");
            }
        }

       void GameStateMouseListener.MouseReleased(int x, int y, GameMouseButtons button)
       {

       }


        bool left = false;
        bool right = false;
        void GameStateKeyListener.KeyPressed(GameKeyEventArgs e)
        {
            //  float angle = 0f;
            if (e.KeyCode == System.Windows.Forms.Keys.Left)
            {
                left = true;
            }
            if (e.KeyCode == System.Windows.Forms.Keys.Right)
            {
                right = true;
            }
        }

        void GameStateKeyListener.KeyReleased(GameKeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Left)
            {
                left = false;
                vx = 0;
                vy = 0;
            }
            if (e.KeyCode == System.Windows.Forms.Keys.Right)
            {
                right = false;
                vx = 0;
                vy = 0;
            }
        }
    }
}

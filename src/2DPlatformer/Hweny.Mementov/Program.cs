using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Hweny.Game;
using Hweny.Mementov.GameStates;

namespace Hweny.Mementov
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //************GameEngine Setting**********

            var gameEngine = new GameEngine();
            gameEngine.AddGameState("Menu", new GameMenuState());
            gameEngine.AddGameState("Level", new GameLevelState());
            gameEngine.Run("Menu");
            //**************************************************
        }
    }
}

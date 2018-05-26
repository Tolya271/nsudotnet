using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NamAndBaranov.Nsudotnet.TicTacToe;

namespace NamBaranov.Nsudotnet.TicTacToe
{
    static class Programm
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameModel gameModel = new GameModel();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TicTacToe.View(gameModel));
        }
    }
}

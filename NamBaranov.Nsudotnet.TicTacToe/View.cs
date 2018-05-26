using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NamAndBaranov.Nsudotnet.TicTacToe;

namespace NamBaranov.Nsudotnet.TicTacToe
{
    partial class View : Form
    {
        GameModel _gameModel = new GameModel();
        private const int BUTTON_SIZE = 60;
        private const int SEPARATOR_SIZE = 5;
        private int _size;
        Button[,] field;
        public View(GameModel gameModel)
        {
            this._gameModel = gameModel;
            _gameModel.StartGame();
            _size = _gameModel.FieldSize * _gameModel.SubFieldSize;
            field = new Button[_size, _size];
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    field[i, j] = new Button();
                    field[i, j].Location = new Point(j * BUTTON_SIZE + SEPARATOR_SIZE * (j / _gameModel.FieldSize), 25 + i * BUTTON_SIZE + SEPARATOR_SIZE * (i / _gameModel.FieldSize));
                    field[i, j].Width = field[i, j].Height = BUTTON_SIZE;
                    field[i, j].Tag = (i * _size + j).ToString();
                    field[i, j].Font = new Font("Arial", 35);
                    field[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    field[i, j].Click += new EventHandler(Button_Click);
                    this.Controls.Add(field[i, j]);
                }
            }
            InitializeComponent();
        }
        void Button_Click(object sender, System.EventArgs e)
        {
            int tag = int.Parse((sender as Button).Tag.ToString());
            int row = tag / _size, column = tag % _size;
            
            if (_gameModel.CheckForGameOver())
            {
                MessageBox.Show("The game is already over");
                return;
            }

            if (!_gameModel.CheckForGameStart())
            {
                MessageBox.Show("You must start the game!!!");
                return;
            }

            if (!_gameModel.TryMove(row, column))
            {
                MessageBox.Show("You can't put point here");
                return;
            }

            Redraw();

            if (_gameModel.GameOver)
            {
                MessageBox.Show(String.Format("Congratulations, Player {0}, you won!!", _gameModel.СurrentTurn.ToString()));
            }
        }
        void Redraw()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Side side = _gameModel.GetSide(i, j);
                    field[i, j].Text = (side == Side.None) ? "" : side.ToString();
                    field[i, j].Enabled = _gameModel.CheckCell(i, j);
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (_gameModel.GameStart)
            {
                MessageBox.Show("Game is already started");
            }
            else
            {
                _gameModel.GameStart = true;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!_gameModel.GameStart)
            {
                MessageBox.Show("Game not started");
            }
            else
            {
                _gameModel.ResetGame();
                _gameModel.GameStart = true;
                Redraw();
            }
        }
    }
}

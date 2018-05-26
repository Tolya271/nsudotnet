using System;
using System.Collections.Generic;
using System.Text;

namespace NamBaranov.Nsudotnet.TicTacToe
{
    public class GameModel
    {
        private const int fieldSize = 3;
        public int FieldSize { get { return fieldSize; } }
        private const int subfieldSize = 3;
        public int SubFieldSize { get { return subfieldSize; } }

        public SubField[,] SubFields { get; set; }
        public Coordinates CurrentSubField { get; set; }
        public Coordinates NextSubField { get; set; }

        private Side _winner;
        private int _movesCount;
        public bool FirstMove { get; set; }

        public bool GameStart { get; set; }
        public bool GameOver { get; set; }
        public Side СurrentTurn { get; private set; }


        public GameModel()
        {
            SubFields = new SubField[fieldSize, fieldSize];
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    SubFields[i, j] = new SubField(subfieldSize);
                }
            }

            ResetGame();
        }

        public void StartGame()
        {
            ResetGame();
            GameStart = true;
        }

        public void ResetGame()
        {
            _winner = Side.None;
            _movesCount = 0;
            FirstMove = true;
            CurrentSubField = new Coordinates(0, 0);
            NextSubField = new Coordinates(0, 0);

            СurrentTurn = Side.X;
            GameStart = false;
            GameOver = false;
            ResetSubFields();
        }

        private void ResetSubFields()
        {
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    SubFields[i, j].ResetField();
                }
            }
        }

        public bool CheckSubFieldCoordinates(Coordinates coordinates)
        {
            return (NextSubField.Row == coordinates.Row && NextSubField.Column == coordinates.Column);
        }

        public Boolean CheckForGameOver()
        {
            return GameOver;
        }

        public Boolean CheckForGameStart()
        {
            return GameStart;
        }


        public Boolean TryMove(int x, int y)
        {
            if (GameOver || !GameStart)
            {
                return false;
            }

            int row = x % SubFieldSize, column = y % SubFieldSize;
            SubField selectedField = SubFields[x / SubFieldSize, y / SubFieldSize];

            if (selectedField.Field[row, column] == Side.None)
            {
                CurrentSubField.Row = row;
                CurrentSubField.Column = column;

                if (SubFields[row, column].CheckDeadHeat())
                {
                    FirstMove = true;
                }
                else
                {
                    FirstMove = false;
                    NextSubField.Row = row;
                    NextSubField.Column = column;
                }

                selectedField.SetCell(row, column, СurrentTurn);
                selectedField.IncrementMovesCount();
                selectedField.CheckDeadHeat();
                if (selectedField.Winner == Side.None)
                {
                    selectedField.CheckFieldForVictory(row, column, СurrentTurn);
                }

                _winner = CheckGameForVictory();
                _movesCount++;
                if (_winner != Side.None
                    || _movesCount == fieldSize * fieldSize * subfieldSize * subfieldSize)
                {
                    GameOver = true;
                }
                else
                {
                    СurrentTurn = СurrentTurn == Side.X ? Side.O : Side.X;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Side GetSide(int row, int column)
        {
            return SubFields[row / FieldSize, column / FieldSize].Field[row % FieldSize, column % FieldSize];
        }

        public bool CheckCell(int row, int column)
        {
            if (FirstMove)
            {
                return true;
            }
            return (NextSubField.Row == row / FieldSize && NextSubField.Column == column / FieldSize);
        }

        private Side CheckGameForVictory()
        {
            Side winner = CheckRows();
            if (winner != Side.None)
            {
                return winner;
            }
            else if ((winner = CheckColumns()) != Side.None)
            {
                return winner;
            }
            else if ((winner = CheckDiagonals()) != Side.None)
            {
                return winner;
            }

            return Side.None;
        }

        private Side CheckRows()
        {
            for (int i = 0; i < fieldSize; i++)
            {
                Side potentialWinner = SubFields[i, 0].Winner;

                for (int j = 1; j < fieldSize; j++)
                {
                    Side firstWinner = SubFields[i, j - 1].Winner;
                    Side secondWinner = SubFields[i, j].Winner;

                    if (firstWinner != secondWinner || firstWinner == Side.None || secondWinner == Side.None)
                    {
                        potentialWinner = Side.None;
                        break;
                    }
                }

                if (potentialWinner != Side.None)
                {
                    return potentialWinner;
                }
            }

            return Side.None;
        }

        private Side CheckColumns()
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Side potentialWinner = SubFields[0, j].Winner;
                for (int i = 1; i < fieldSize; i++)
                {
                    Side firstWinner = SubFields[i - 1, j].Winner;
                    Side secondWinner = SubFields[i, j].Winner;

                    if (firstWinner != secondWinner || firstWinner == Side.None || secondWinner == Side.None)
                    {
                        potentialWinner = Side.None;
                    }
                }

                if (potentialWinner != Side.None)
                {
                    return potentialWinner;
                }
            }

            return Side.None;
        }

        private Side CheckDiagonals()
        {

            Side[] leftDiagonal = new Side[fieldSize];
            Side[] rightDiagonal = new Side[fieldSize];
            int leftDiagonalPosition = 0;
            int rightDiagonalPosition = 0;

            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    if (i + j == fieldSize - 1)
                    {
                        leftDiagonal[leftDiagonalPosition] = SubFields[i, j].Winner;
                        leftDiagonalPosition++;
                    }

                    if (i == j)
                    {
                        rightDiagonal[rightDiagonalPosition] = SubFields[i, j].Winner;
                        rightDiagonalPosition++;
                    }
                }
            }

            Side leftDiagonalWinner = CheckDiagonal(leftDiagonal);
            Side rigthDiagonalWinner = CheckDiagonal(rightDiagonal);

            if (leftDiagonalWinner != Side.None)
            {
                return leftDiagonalWinner;
            }
            else
            {
                return rigthDiagonalWinner;
            }
        }

        private Side CheckDiagonal(Side[] diagonal)
        {
            Side potentialWinner = diagonal[0];
            for (int i = 1; i < diagonal.Length; i++)
            {
                if (diagonal[i - 1] != diagonal[i])
                {
                    return Side.None;
                }
            }

            return potentialWinner;
        }
    }
}

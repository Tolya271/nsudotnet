using System;
using System.Collections.Generic;
using System.Text;

namespace NamBaranov.Nsudotnet.TicTacToe
{
    public class SubField
    {
        private int fieldSize;
        public Side[,] Field { get; set; }
        public bool GameOver { get; set; }
        public Side Winner { get; set; }
        public int MovesCount { get; set; }

        public SubField(int fieldSize)
        {
            this.fieldSize = fieldSize;
            Field = new Side[this.fieldSize, this.fieldSize];
            GameOver = false;
            Winner = Side.None;
            ResetField();
        }

        public void ResetField()
        {
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    Field[i, j] = Side.None;
                }
            }

            MovesCount = 0;
        }

        public bool CheckDeadHeat()
        {
            if (MovesCount == fieldSize * fieldSize)
            {
                GameOver = true;
                Winner = Side.None;
                return true;
            }

            return false;
        }

        public bool CheckWinner()
        {
            return MovesCount == fieldSize * fieldSize;
        }

        public void IncrementMovesCount()
        {
            MovesCount++;
        }

        public void SetCell(int row, int column, Side side)
        {
            Field[row, column] = side;
        }

        public Side CheckFieldForVictory(int row, int column, Side currentTurn)
        {
            if (CheckRow(row) != Side.None)
            {
                Winner = CheckRow(row);
                GameOver = true;
            }
            else if (CheckColumn(column) != Side.None)
            {
                Winner = CheckColumn(column);
                GameOver = true;
            }
            else if (CheckLeftDiagonal(row, column) != Side.None)
            {
                Winner = CheckLeftDiagonal(row, column);
                GameOver = true;
            }
            else if (CheckRightDiagonal(row, column) != Side.None)
            {
                Winner = CheckRightDiagonal(row, column);
                GameOver = true;
            }
            else
            {
                Winner = Side.None;
                if (MovesCount == fieldSize * fieldSize)
                {
                    GameOver = true;
                }
            }

            return Winner;
        }

        private Side CheckRow(int row)
        {
            Side side = Field[row, 0];

            for (int i = 1; i < fieldSize; i++)
            {
                if (Field[row, i - 1] != Field[row, i])
                {
                    return Side.None;
                }
            }

            return side;
        }

        private Side CheckColumn(int column)
        {
            Side side = Field[0, column];

            for (int i = 1; i < fieldSize; i++)
            {
                if (Field[i - 1, column] != Field[i, column])
                {
                    return Side.None;
                }
            }

            return side;
        }

        private Side CheckLeftDiagonal(int row, int column)
        {
            if (row + column == fieldSize - 1)
            {
                Side[] diagonal = new Side[fieldSize];
                int position = 0;

                for (int i = 0; i < fieldSize; i++)
                {
                    for (int j = 0; j < fieldSize; j++)
                    {
                        if (i + j == fieldSize - 1)
                        {
                            diagonal[position] = Field[i, j];
                            position++;
                        }
                    }
                }

                return CheckDiagonal(diagonal);
            }

            return Side.None;
        }

        private Side CheckRightDiagonal(int row, int column)
        {
            if (row == column)
            {
                Side[] diagonal = new Side[fieldSize];
                int position = 0;

                for (int i = 0; i < fieldSize; i++)
                {
                    for (int j = 0; j < fieldSize; j++)
                    {
                        if (i == j)
                        {
                            diagonal[position] = Field[i, j];
                            position++;
                        }
                    }
                }

                return CheckDiagonal(diagonal);
            }

            return Side.None;
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

        private bool CheckArrayElementsEquality(Side[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] != array[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}

using System;
using Data;

namespace Editor
{
    public static class SudokuCreator
    {
        private static int[,] _currentGrid;

        public static int[,] GenerateSudoku(LevelDifficulty difficulty, out int[,] solution)
        {
            _currentGrid = new int[9, 9];
            int length = _currentGrid.GetLength(0);
            int boxLength = (int)Math.Sqrt(length);

            for (int i = 0; i < length; i += boxLength)
            {
                FillDiagonalBoxes(i, i);
            }

            FillFullGrid();

            solution = (int[,])_currentGrid.Clone();

            RemoveElements(difficulty);
            return _currentGrid;
        }

        private static void FillDiagonalBoxes(int row, int column)
        {
            Random random = new Random();
            var length = 3;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    int number;
                    do
                    {
                        number = random.Next(1, 10);
                    } while (!IsSafeBox(row, column, number));

                    _currentGrid[row + i, column + j] = number;
                }
            }
        }

        private static bool FillFullGrid()
        {
            // Get the length of the Sudoku _currentGrid
            int length = _currentGrid.GetLength(0);
            int row = -1;
            int column = -1;
            bool isFinished = true;

            // Search for an empty cell (value 0) and set its coordinates to 'row' and 'column'
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (_currentGrid[i, j] == 0)
                    {
                        row = i;
                        column = j;
                        isFinished = false;
                        break;
                    }
                }

                // Exit if no empty cells left
            }

            if (isFinished) return true;

            // Iterate through possible numbers (1 to 9) and attempt to place them in the cell
            for (int num = 1; num <= length; num++)
            {
                // Check if the number is safe to place at the current coordinates
                if (IsSafe(row, column, num))
                {
                    // Place the number and continue with the rest of the _currentGrid
                    _currentGrid[row, column] = num;
                    if (FillFullGrid())
                        return true;

                    // If placing the number doesn't lead to a solution, reset the cell value and backtrack
                    _currentGrid[row, column] = 0;
                }
            }

            // Return false if no solution is found
            return false;
        }

        private static void RemoveElements(LevelDifficulty difficulty)
        {
            //todo refine this later

            int elementsToRemoveMin = 0;
            int elementsToRemoveMax = 0;

            switch (difficulty)
            {
                case LevelDifficulty.Easy:
                    elementsToRemoveMin = 40;
                    elementsToRemoveMax = 44;
                    break;
                case LevelDifficulty.Medium:
                    elementsToRemoveMin = 45;
                    elementsToRemoveMax = 50;
                    break;
                case LevelDifficulty.Hard:
                    elementsToRemoveMin = 54;
                    elementsToRemoveMax = 58;
                    break;
                case LevelDifficulty.Extreme:
                    elementsToRemoveMin = 59;
                    elementsToRemoveMax = 63;
                    break;
            }

            Random random = new Random();
            int elementsToRemove = random.Next(elementsToRemoveMin, elementsToRemoveMax + 1);

            while (elementsToRemove > 0)
            {
                int randomX = random.Next(0, 9);
                int randomY = random.Next(0, 9);

                if (_currentGrid[randomX, randomY] != 0)
                {
                    _currentGrid[randomX, randomY] = 0;
                    elementsToRemove--;
                }
            }
        }


        private static bool IsSafe(int x, int y, int number)
        {
            return IsSafeRow(y, number) && IsSafeColumn(x, number) &&
                   IsSafeBox(x - x % 3, y - y % 3, number);
        }

        private static bool IsSafeRow(int column, int number)
        {
            var rowLenght = _currentGrid.GetLength(0);
            for (int i = 0; i < rowLenght; i++)
            {
                if (_currentGrid[i, column] == number) return false;
            }

            return true;
        }

        private static bool IsSafeColumn(int row, int number)
        {
            var columnLength = _currentGrid.GetLength(1);
            for (int i = 0; i < columnLength; i++)
            {
                if (_currentGrid[row, i] == number) return false;
            }

            return true;
        }

        private static bool IsSafeBox(int row, int column, int number)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_currentGrid[row + i, column + j] == number) return false;
                }
            }

            return true;
        }
    }
}
using Data;
using deVoid.Utils;
using UnityEngine;

namespace Game.Managers
{
    public static class ScoreManager
    {
        private static int _score;

        public static void Initialize()
        {
            Signals.Get<ScoreCheckRequested>().AddListener(OnScoreCheckRequested);
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
        }

        private static void OnLevelLoaded(LevelData levelData, bool fromSave)
        {
            if (fromSave)
            {
                _score = SaveManager.GetContinueLevelFromSave().score;
            }
            else
            {
                _score = 0;
            }

            Signals.Get<ScoreUpdated>().Dispatch(_score, true);
        }

        private static void OnScoreCheckRequested(int[,] grid, Cell cell, LevelDifficulty difficulty)
        {
            bool isRowFull = IsRowFull(grid, cell);
            bool isColumnFull = IsColumnFull(grid, cell);
            bool isBoxFull = IsBoxFull(grid, cell);

            int gainedScore = 0;

            //not completed any row, column or box. Just filled a correct cell.
            if (!isRowFull && !isColumnFull && !isBoxFull)
            {
                gainedScore = GlobalGameConfigs.GetCellPointForLevel(difficulty);
                _score += gainedScore;
                Signals.Get<ScoreUpdated>().Dispatch(_score, false);
                return;
            }

            //completed a combo of row, column or box.
            if ((isRowFull && isColumnFull) || (isRowFull && isBoxFull) || (isColumnFull && isBoxFull))
            {
                gainedScore = GlobalGameConfigs.GetSimultaneousElementCompletePoint(difficulty);
                _score += gainedScore;
                Signals.Get<ScoreUpdated>().Dispatch(_score, false);
                return;
            }

            //completed a row, column or box.
            gainedScore = GlobalGameConfigs.GetElementCompletePoint(difficulty);
            _score += gainedScore;
            Signals.Get<ScoreUpdated>().Dispatch(_score, false);
        }

        private static bool IsRowFull(int[,] grid, Cell cell)
        {
            int dimensionSize = grid.GetLength(0);

            for (int i = 0; i < dimensionSize; i++)
            {
                if (grid[cell.PositionOnGrid.x, i] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsColumnFull(int[,] grid, Cell cell)
        {
            int dimensionSize = grid.GetLength(0);

            for (int i = 0; i < dimensionSize; i++)
            {
                if (grid[i, cell.PositionOnGrid.y] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsBoxFull(int[,] grid, Cell cell)
        {
            int dimensionSize = grid.GetLength(0);
            int squareSize = Mathf.RoundToInt(Mathf.Sqrt(dimensionSize));

            int x = cell.PositionOnGrid.x / squareSize;
            int y = cell.PositionOnGrid.y / squareSize;

            for (int i = x * squareSize; i < x * squareSize + squareSize; i++)
            {
                for (int j = y * squareSize; j < y * squareSize + squareSize; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
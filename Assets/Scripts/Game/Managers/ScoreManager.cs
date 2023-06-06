using System.Collections.Generic;
using Data;
using deVoid.Utils;
using UnityEngine;

namespace Game.Managers
{
    public static class ScoreManager
    {
        private static int _score;

        private static HashSet<int> _completedRows = new HashSet<int>();
        private static HashSet<int> _completedColumns = new HashSet<int>();
        private static HashSet<int> _completedBoxes = new HashSet<int>();

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

            _completedBoxes.Clear();
            _completedColumns.Clear();
            _completedRows.Clear();
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

            List<Vector2Int> filledPositions = new List<Vector2Int>();

            // Add filled elements to filledPositions list without overlapping
            if (isRowFull && !_completedRows.Contains(cell.PositionOnGrid.x))
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    filledPositions.Add(new Vector2Int(cell.PositionOnGrid.x, i));
                }
            }

            if (isColumnFull && !_completedColumns.Contains(cell.PositionOnGrid.y))
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    Vector2Int pos = new Vector2Int(i, cell.PositionOnGrid.y);
                    if (!filledPositions.Contains(pos))
                    {
                        filledPositions.Add(pos);
                    }
                }
            }

            if (isBoxFull)
            {
                int dimensionSize = grid.GetLength(0);
                int squareSize = Mathf.RoundToInt(Mathf.Sqrt(dimensionSize));

                int x = cell.PositionOnGrid.x / squareSize;
                int y = cell.PositionOnGrid.y / squareSize;

                for (int i = x * squareSize; i < x * squareSize + squareSize; i++)
                {
                    for (int j = y * squareSize; j < y * squareSize + squareSize; j++)
                    {
                        Vector2Int pos = new Vector2Int(i, j);
                        if (!filledPositions.Contains(pos))
                        {
                            filledPositions.Add(pos);
                        }
                    }
                }
            }

            // Fire the signal with filledPositions list
            Signals.Get<ElementsFilled>().Dispatch(filledPositions, cell.PositionOnGrid);

            //completed a combo of row, column or box.
            if (filledPositions.Count > grid.GetLength(0))
            {
                gainedScore = GlobalGameConfigs.GetSimultaneousElementCompletePoint(difficulty);
                _score += gainedScore;
                Signals.Get<ScoreUpdated>().Dispatch(_score, false);

                if (isRowFull) _completedRows.Add(cell.PositionOnGrid.x);
                if (isColumnFull) _completedColumns.Add(cell.PositionOnGrid.y);
                if (isBoxFull) _completedBoxes.Add(Utils.CalculateBoxIndex(cell.PositionOnGrid));

                return;
            }

            //completed a row, column or box.
            bool newRow = isRowFull && !_completedRows.Contains(cell.PositionOnGrid.x);
            bool newColumn = isColumnFull && !_completedColumns.Contains(cell.PositionOnGrid.y);
            bool newBox = isBoxFull;

            if (newRow || newColumn || newBox)
            {
                gainedScore = GlobalGameConfigs.GetElementCompletePoint(difficulty);

                if (newRow)
                {
                    _completedRows.Add(cell.PositionOnGrid.x);
                }

                if (newColumn)
                {
                    _completedColumns.Add(cell.PositionOnGrid.y);
                }

                if (newBox)
                {
                    _completedBoxes.Add(Utils.CalculateBoxIndex(cell.PositionOnGrid));
                }

                _score += gainedScore;
                Signals.Get<ScoreUpdated>().Dispatch(_score, false);
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using deVoid.Utils;
using Managers;
using UI.Windows;
using UnityEngine;

namespace Game.Managers
{
    public static class BoardManager
    {
        private static int[,] LevelGrid { get; set; }
        private static int[,] SolutionGrid { get; set; }

        private static LevelData CurrentLevelData { get; set; }

        public static void Initialize()
        {
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
            Signals.Get<CellPointerDown>().AddListener(OnCellPointerDown);
            Signals.Get<CellPointerUp>().AddListener(OnCellPointerUp);
            Signals.Get<CellFilled>().AddListener(OnCellFilled);
            Signals.Get<ReturnToMenuRequested>().AddListener(GameplayWindow_OnReturnToMenuRequested);
        }


        private static void GameplayWindow_OnReturnToMenuRequested()
        {
            CurrentLevelData.levelGrid = Utils.GridToArray(LevelGrid);
            Signals.Get<BoardStateSaveRequested>().Dispatch(CurrentLevelData);
            Signals.Get<RequestGameStateChange>().Dispatch(GameState.Menu);
        }


        private static void OnCellFilled(Cell cell)
        {
            //means we erased or undid a cell so no validation needed
            if (cell.Number == 0) return;

            if (!IsCorrectPlacement(cell.PositionOnGrid, cell.Number))
            {
                Signals.Get<WrongNumberPlaced>().Dispatch(cell);
                return;
            }

            //doing this after return since we dont want to save the number if it is wrong
            LevelGrid[cell.PositionOnGrid.x, cell.PositionOnGrid.y] = cell.Number;

            if (IsBoardFull())
            {
                Signals.Get<LevelSuccess>()
                    .Dispatch(new LevelSuccessData(new TimeSpan(0, 0, 0), 0, LevelDifficulty.Easy));
            }
        }

        private static void OnCellEraseRequested()
        {
        }


        private static bool IsCorrectPlacement(Vector2Int position, int number)
        {
            return SolutionGrid[position.x, position.y] == number;
        }

        private static bool IsBoardFull()
        {
            int dimensionSize = LevelGrid.GetLength(0);

            for (int i = 0; i < dimensionSize; i++)
            {
                for (int j = 0; j < dimensionSize; j++)
                {
                    if (LevelGrid[i, j] == 0) return false;
                }
            }

            return true;
        }

        private static void OnLevelLoaded(LevelData levelData)
        {
            LevelGrid = Utils.ArrayToGrid(levelData.levelGrid);
            SolutionGrid = Utils.ArrayToGrid(levelData.solutionGrid);
            CurrentLevelData = levelData;
        }

        private static void OnCellPointerDown(Vector2Int position)
        {
            DispatchColorizationList(position);
        }

        private static void OnCellPointerUp(Vector2Int position)
        {
            DispatchSameNumbersOnBoard(position);
        }


        private static void DispatchColorizationList(Vector2Int position)
        {
            Vector2Int[] boxPositions = BoardHelper.GetBox(position);
            Vector2Int[] rowPositions = BoardHelper.GetRow(position);
            Vector2Int[] colPositions = BoardHelper.GetColumn(position);

            Vector2Int[] collectivePositions = boxPositions.Concat(rowPositions).Concat(colPositions).ToArray();

            HashSet<Vector2Int> positionsToDispatch = new HashSet<Vector2Int>();

            foreach (var pos in collectivePositions)
            {
                positionsToDispatch.Add(pos);
            }

            Signals.Get<ColorizationListDispatched>().Dispatch(positionsToDispatch);
        }


        private static void DispatchSameNumbersOnBoard(Vector2Int position)
        {
            int dimensionSize = LevelGrid.GetLength(0);
            int number = LevelGrid[position.x, position.y];

            List<Vector2Int> numberPositions = new List<Vector2Int>(9);

            for (int i = 0; i < dimensionSize; i++)
            {
                for (int j = 0; j < dimensionSize; j++)
                {
                    if (LevelGrid[i, j] == number)
                    {
                        numberPositions.Add(new Vector2Int(i, j));
                    }
                }
            }

            Signals.Get<SameNumberListDispatched>().Dispatch(numberPositions);
        }
    }
}
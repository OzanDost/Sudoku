using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using deVoid.Utils;
using Managers;
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
            Signals.Get<HintAuthorized>().AddListener(OnHintAuthorized);
            Signals.Get<EraseRequested>().AddListener(OnEraseRequested);
        }

        private static void OnHintAuthorized(Cell cell)
        {
            if (cell == null) return;
            if (!cell.IsEmpty && !cell.IsWrongNumber) return;

            int solution = GetSolutionForCell(cell);
            cell.GetFilled(solution, true);
            Signals.Get<HintUsed>().Dispatch();
        }


        private static void GameplayWindow_OnReturnToMenuRequested()
        {
            CurrentLevelData.levelArray = Utils.GridToArray(LevelGrid);
            Signals.Get<BoardStateSaveRequested>().Dispatch(CurrentLevelData);
            Signals.Get<RequestGameStateChange>().Dispatch(GameState.Menu);
        }


        private static void OnCellFilled(Cell cell, bool filledByPlayer)
        {
            if (!IsCorrectPlacement(cell.PositionOnGrid, cell.Number))
            {
                if (cell.Number != 0)
                {
                    Signals.Get<WrongNumberPlaced>().Dispatch(cell,filledByPlayer);
                }
            }

            LevelGrid[cell.PositionOnGrid.x, cell.PositionOnGrid.y] = cell.Number;

            if (IsBoardFull())
            {
                Signals.Get<LevelSuccess>()
                    .Dispatch(new LevelSuccessData(new TimeSpan(0, 0, 0), 0, LevelDifficulty.Easy));
            }
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

        private static int GetSolutionForCell(Cell cell)
        {
            return SolutionGrid[cell.PositionOnGrid.x, cell.PositionOnGrid.y];
        }

        private static void OnLevelLoaded(LevelData levelData, bool fromContinue)
        {
            LevelGrid = Utils.ArrayToGrid(levelData.levelArray);
            SolutionGrid = Utils.ArrayToGrid(levelData.solutionGrid);
            CurrentLevelData = levelData;
            Signals.Get<BoardReady>().Dispatch(levelData, fromContinue);
        }

        // private static void CheckForWrongNumbers()
        // {
        //     int dimensionSize = LevelGrid.GetLength(0);
        //
        //     for (int i = 0; i < dimensionSize; i++)
        //     {
        //         for (int j = 0; j < dimensionSize; j++)
        //         {
        //             if (LevelGrid[i, j] == 0) continue;
        //             if (LevelGrid[i, j] == SolutionGrid[i, j]) continue;
        //
        //             Signals.Get<WrongNumberPlaced>().Dispatch(new Vector2Int(i, j));
        //         }
        //     }
        // }

        private static void OnCellPointerDown(Vector2Int position)
        {
            DispatchColorizationList(position);
        }

        private static void OnCellPointerUp(Vector2Int position)
        {
            DispatchSameNumbersOnBoard(position);
        }

        private static void OnEraseRequested(Cell cell)
        {
            bool deletedNotes = cell.EraseCellNotes();
            bool deletedNumber = cell.EraseCellNumber();

            if (deletedNotes || deletedNumber)
            {
                cell.GetFilled(0, true);
                LevelGrid[cell.PositionOnGrid.x, cell.PositionOnGrid.y] = 0;
            }

            Signals.Get<CellEraseResponseSent>().Dispatch(deletedNotes || deletedNumber);
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

            Signals.Get<ColorizationListDispatched>().Dispatch(positionsToDispatch, position);
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
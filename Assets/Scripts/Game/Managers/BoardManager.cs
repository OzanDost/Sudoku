using System.Collections.Generic;
using Data;
using deVoid.Utils;
using UI;
using UnityEngine;

namespace Game.Managers
{
    public static class BoardManager
    {
        private static int[,] LevelGrid { get; set; }
        private static int[,] SolutionGrid { get; set; }

        private static NoteSaveData[,] Notes { get; set; }

        private static LevelData CurrentLevelData { get; set; }

        private static bool HasData { get; set; }

        public static void Initialize()
        {
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
            Signals.Get<LevelSuccess>().AddListener(OnLevelSuccess);
            Signals.Get<LevelFailed>().AddListener(OnLevelFailed);
            Signals.Get<CellPointerDown>().AddListener(OnCellPointerDown);
            Signals.Get<CellPointerUp>().AddListener(OnCellPointerUp);
            Signals.Get<CellFilled>().AddListener(OnCellFilled);
            Signals.Get<ReturnToMenuRequested>().AddListener(GameplayWindow_OnReturnToMenuRequested);
            Signals.Get<HintAuthorized>().AddListener(OnHintAuthorized);
            Signals.Get<EraseRequested>().AddListener(OnEraseRequested);
            Signals.Get<NoteUpdatedOnCell>().AddListener(OnNoteUpdatedOnCell);
        }

        private static void ResetValues()
        {
            LevelGrid = null;
            SolutionGrid = null;
            Notes = null;
            CurrentLevelData = null;
            HasData = false;
        }

        private static void OnNoteUpdatedOnCell(Cell cell, int number)
        {
            //todo add comment here
            NoteSaveData noteSaveData = Notes[cell.PositionOnGrid.x, cell.PositionOnGrid.y];
            noteSaveData.notes[number - 1] = !noteSaveData.notes[number - 1];
        }

        private static void OnLevelLoaded(LevelData levelData, bool fromContinue)
        {
            LevelGrid = Utils.ArrayToGrid(levelData.levelArray);
            SolutionGrid = Utils.ArrayToGrid(levelData.solutionGrid);
            CurrentLevelData = levelData;
            FetchNoteData(levelData, fromContinue);
            HasData = true;

            Signals.Get<BoardReady>().Dispatch(levelData, fromContinue);
        }

        private static void FetchNoteData(LevelData levelData, bool fromContinue)
        {
            if (!fromContinue)
            {
                Notes = new NoteSaveData[9, 9];
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        Notes[i, j] = new NoteSaveData(new bool[9]);
                    }
                }

                levelData.notes = Utils.GridToArray(Notes);
            }
            else
            {
                Notes = Utils.ArrayToGrid(levelData.notes);
            }
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
            Signals.Get<RequestGameStateChange>().Dispatch(GameState.Menu);
            SendLevelSaveRequest();
        }

        public static void SendLevelSaveRequest()
        {
            if (CurrentLevelData == null) return;
            CurrentLevelData.levelArray = Utils.GridToArray(LevelGrid);
            CurrentLevelData.notes = Utils.GridToArray(Notes);
            Signals.Get<BoardStateSaveRequested>().Dispatch(CurrentLevelData);
        }

        private static void OnCellFilled(Cell cell, bool filledByPlayer)
        {
            if (!HasData) return;
            bool isCorrectPlacement = IsCorrectPlacement(cell.PositionOnGrid, cell.Number);

            LevelGrid[cell.PositionOnGrid.x, cell.PositionOnGrid.y] = cell.Number;

            if (!isCorrectPlacement)
            {
                if (cell.Number != 0)
                {
                    Signals.Get<WrongNumberPlaced>().Dispatch(cell, filledByPlayer);
                    cell.OnWrongNumberPlaced();
                }
            }


            if (filledByPlayer && isCorrectPlacement)
            {
                Signals.Get<ScoreCheckRequested>().Dispatch(LevelGrid, cell, CurrentLevelData.difficulty);
            }

            if (IsBoardFull())
            {
                Signals.Get<BoardFilledSuccessfully>().Dispatch(CurrentLevelData);
            }
        }


        private static bool IsCorrectPlacement(Vector2Int position, int number)
        {
            return SolutionGrid[position.x, position.y] == number;
        }

        private static bool IsBoardFull()
        {
            if (!HasData) return false;
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

        private static void OnCellPointerDown(Vector2Int position)
        {
            DispatchColorizationList(position);
        }

        private static void OnCellPointerUp(Vector2Int position)
        {
            List<Vector2Int> sameNumberList = GetSameNumbersOnBoard(position);
            Signals.Get<SameNumberListDispatched>().Dispatch(sameNumberList);
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

            Signals.Get<CellEraseResponseSent>().Dispatch(deletedNotes || deletedNumber, cell);
        }

        private static void DispatchColorizationList(Vector2Int position)
        {
            HashSet<Vector2Int> positionsToDispatch = new HashSet<Vector2Int>();

            List<Vector2Int> boxPositions = Utils.GetBox(position);
            foreach (var boxPosition in boxPositions)
            {
                positionsToDispatch.Add(boxPosition);
            }

            List<Vector2Int> rowPositions = Utils.GetRow(position);
            foreach (var rowPosition in rowPositions)
            {
                positionsToDispatch.Add(rowPosition);
            }

            List<Vector2Int> columnPositions = Utils.GetColumn(position);
            foreach (var colPosition in columnPositions)
            {
                positionsToDispatch.Add(colPosition);
            }

            List<Vector2Int> sameNumberPositions = GetSameNumbersOnBoard(position);
            foreach (var sameNumberPosition in sameNumberPositions)
            {
                positionsToDispatch.Add(sameNumberPosition);
            }

            ColorizationData colorizationData =
                new ColorizationData(boxPositions, rowPositions, columnPositions, sameNumberPositions);

            Signals.Get<TapColorizationListDispatched>().Dispatch(colorizationData, position);
        }


        private static List<Vector2Int> GetSameNumbersOnBoard(Vector2Int position)
        {
            int dimensionSize = LevelGrid.GetLength(0);
            int number = LevelGrid[position.x, position.y];

            if (number == 0) return new List<Vector2Int>();

            List<Vector2Int> numberPositions = new List<Vector2Int>(9);

            for (int i = 0; i < dimensionSize; i++)
            {
                for (int j = 0; j < dimensionSize; j++)
                {
                    if (i == position.x && j == position.y) continue;
                    if (LevelGrid[i, j] == number)
                    {
                        numberPositions.Add(new Vector2Int(i, j));
                    }
                }
            }

            return numberPositions;
        }

        private static void OnLevelFailed()
        {
            ResetValues();
        }

        private static void OnLevelSuccess(LevelSuccessData levelSuccessData)
        {
            ResetValues();
        }
    }
}
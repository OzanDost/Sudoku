using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using deVoid.Utils;
using Managers;
using UnityEngine;

namespace Game.Managers
{
    public class BoardManager : MonoBehaviour
    {
        private int[,] LevelGrid { get; set; }
        private int[,] SolutionGrid { get; set; }

        private LevelData CurrentLevelData { get; set; }

        private void Awake()
        {
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
            Signals.Get<CellPointerDown>().AddListener(OnCellPointerDown);
            Signals.Get<CellPointerUp>().AddListener(OnCellPointerUp);
            Signals.Get<CellFilled>().AddListener(OnCellFilled);
            Signals.Get<ReturnToMenuRequested>().AddListener(GameplayWindow_OnReturnToMenuRequested);
        }

        private void GameplayWindow_OnReturnToMenuRequested()
        {
            CurrentLevelData.levelGrid = Utils.GridToArray(LevelGrid);
            Signals.Get<BoardStateSaveRequested>().Dispatch(CurrentLevelData);
            Signals.Get<RequestGameStateChange>().Dispatch(GameState.Menu);
        }


        private void OnCellFilled(Cell cell)
        {
            LevelGrid[cell.PositionOnGrid.x, cell.PositionOnGrid.y] = cell.Number;

            if (!IsCorrectPlacement(cell.PositionOnGrid, cell.Number))
            {
                Signals.Get<WrongNumberPlaced>().Dispatch(cell);
                return;
            }

            if (IsBoardFull())
            {
                Signals.Get<LevelSuccess>()
                    .Dispatch(new LevelSuccessData(new TimeSpan(0, 0, 0), 0, LevelDifficulty.Easy));
            }
        }


        private bool IsCorrectPlacement(Vector2Int position, int number)
        {
            return SolutionGrid[position.x, position.y] == number;
        }

        private bool IsBoardFull()
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

        private void OnLevelLoaded(LevelData levelData)
        {
            LevelGrid = Utils.ArrayToGrid(levelData.levelGrid);
            SolutionGrid = Utils.ArrayToGrid(levelData.solutionGrid);
            CurrentLevelData = levelData;
        }

        private void OnCellPointerDown(Vector2Int position)
        {
            DispatchColorizationList(position);
        }

        private void OnCellPointerUp(Vector2Int position)
        {
            DispatchSameNumbersOnBoard(position);
        }


        private void DispatchColorizationList(Vector2Int position)
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


        private void DispatchSameNumbersOnBoard(Vector2Int position)
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
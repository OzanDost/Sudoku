using System.Collections.Generic;
using System.Linq;
using Data;
using deVoid.Utils;
using UnityEngine;

namespace Game.Managers
{
    public class BoardManager : MonoBehaviour
    {
        private int[,] LevelGrid { get; set; }
        private int[,] SolutionGrid { get; set; }

        private void Awake()
        {
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
            Signals.Get<CellPointerDown>().AddListener(OnCellPointerDown);
            Signals.Get<CellPointerUp>().AddListener(OnCellPointerUp);
            Signals.Get<CellFilled>().AddListener(OnCellFilled);
        }

        private void OnCellFilled(Vector2Int position, int number)
        {
            LevelGrid[position.x, position.y] = number;
            //todo check true-false
            //check game finish - fail
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
            LevelGrid = levelData.levelGrid;
            SolutionGrid = levelData.solutionGrid;
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
            Vector2Int[] boxPositions = GetBox(position);
            Vector2Int[] rowPositions = GetRow(position);
            Vector2Int[] colPositions = GetColumn(position);

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

        private Vector2Int[] GetBox(Vector2Int position)
        {
            int boxRow = position.x / 3;
            int boxCol = position.y / 3;
            Vector2Int[] boxPositions = new Vector2Int[9];

            int index = 0;
            for (int row = boxRow * 3; row < boxRow * 3 + 3; row++)
            {
                for (int col = boxCol * 3; col < boxCol * 3 + 3; col++)
                {
                    boxPositions[index++] = new Vector2Int(row, col);
                }
            }

            return boxPositions;
        }

        private Vector2Int[] GetRow(Vector2Int position)
        {
            Vector2Int[] rowPositions = new Vector2Int[9];
            for (int i = 0; i < 9; i++)
            {
                rowPositions[i] = new Vector2Int(position.x, i);
            }

            return rowPositions;
        }

        private Vector2Int[] GetColumn(Vector2Int position)
        {
            Vector2Int[] colPositions = new Vector2Int[9];
            for (int i = 0; i < 9; i++)
            {
                colPositions[i] = new Vector2Int(i, position.y);
            }

            return colPositions;
        }
    }
}
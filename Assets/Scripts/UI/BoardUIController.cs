using System;
using System.Collections.Generic;
using Data;
using deVoid.Utils;
using Game;
using UnityEngine;

namespace UI
{
    public class BoardUIController : MonoBehaviour
    {
        public RectTransform board;
        public Cell[] cells;

        private Cell[,] _cellGrid;
        private Cell SelectedCell { get; set; }

        private void Awake()
        {
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
            Signals.Get<CellPointerDown>().AddListener(OnCellPointerDown);
            Signals.Get<ColorizationListDispatched>().AddListener(BoardManager_OnColorizationListDispatched);
            Signals.Get<SameNumberListDispatched>().AddListener(BoardManager_OnSameNumberListDispatched);
            Signals.Get<NumberButtonClicked>().AddListener(OnNumberButtonClicked);
        }

        private void OnCellPointerDown(Vector2Int position)
        {
            SelectedCell = _cellGrid[position.x, position.y];
        }


        //todo for testing only, remove later
        private void Update()
        {
#if UNITY_EDITOR
            //move SelectedCell with arrow keys
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (SelectedCell == null) return;
                if (SelectedCell.PositionOnGrid.y - 1 < 0) return;
                SelectedCell = _cellGrid[SelectedCell.PositionOnGrid.x, SelectedCell.PositionOnGrid.y - 1];
                SelectedCell.OnPointerDown(null);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (SelectedCell == null) return;
                if (SelectedCell.PositionOnGrid.y + 1 > 8) return;
                SelectedCell = _cellGrid[SelectedCell.PositionOnGrid.x, SelectedCell.PositionOnGrid.y + 1];
                SelectedCell.OnPointerDown(null);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (SelectedCell == null) return;
                if (SelectedCell.PositionOnGrid.x - 1 < 0) return;
                SelectedCell = _cellGrid[SelectedCell.PositionOnGrid.x - 1, SelectedCell.PositionOnGrid.y];
                SelectedCell.OnPointerDown(null);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (SelectedCell == null) return;
                if (SelectedCell.PositionOnGrid.x + 1 > 8) return;
                SelectedCell = _cellGrid[SelectedCell.PositionOnGrid.x + 1, SelectedCell.PositionOnGrid.y];
                SelectedCell.OnPointerDown(null);
            }
#endif
        }

        private void OnNumberButtonClicked(int number)
        {
            if (SelectedCell == null) return;
            if (!SelectedCell.IsEmpty) return;

            SelectedCell.GetFilled(number, true);
            Signals.Get<CellFilled>().Dispatch(SelectedCell);
        }

        private void BoardManager_OnSameNumberListDispatched(List<Vector2Int> positions)
        {
            foreach (var position in positions)
            {
                _cellGrid[position.x, position.y].PunchScale();
            }
        }

        private void ResetSelectionHighlight()
        {
            foreach (var cell in cells)
            {
                cell.SetColor(Color.white);
            }
        }

        private void BoardManager_OnColorizationListDispatched(HashSet<Vector2Int> positions)
        {
            //todo refactor this maybe add animation

            ResetSelectionHighlight();
            foreach (var position in positions)
            {
                _cellGrid[position.x, position.y].SetColor(Color.cyan);
            }
        }

        private void OnLevelLoaded(LevelData levelData)
        {
            Signals.Get<BoardGridCreationRequested>().Dispatch(board);

            AdjustCellSize();
            FillTheCells(levelData);
            ResetSelectionHighlight();
            SelectedCell = null;

            Signals.Get<LevelBoardConfigured>().Dispatch(levelData);

            _cellGrid = new Cell[9, 9];
            foreach (var cell in cells)
            {
                _cellGrid[cell.PositionOnGrid.x, cell.PositionOnGrid.y] = cell;
            }
        }

        private void FillTheCells(LevelData levelData)
        {
            int dimensionLength = (int)Mathf.Sqrt(cells.Length);
            for (int i = 0; i < cells.Length; i++)
            {
                var x = i % (dimensionLength);
                var y = i / (dimensionLength);

                // var fill = levelData.levelGrid[x, y];
                var fill = levelData.levelGrid[i];
                cells[i].GetFilled(fill, false);
                cells[i].SetPosition(x, y);
            }
        }

        private void AdjustCellSize()
        {
            float cellSize = board.rect.width / 9;
            foreach (Cell cell in cells)
            {
                cell.SetSizeDelta(cellSize * Vector2.one);
            }
        }
    }
}
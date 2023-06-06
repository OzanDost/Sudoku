using System.Collections.Generic;
using Data;
using deVoid.Utils;
using Game;
using UI.Windows;
using UnityEngine;

namespace UI
{
    public class BoardUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform board;
        [SerializeField] private Cell[] cells;

        private Cell[,] _cellGrid;
        private Cell SelectedCell { get; set; }

        private void Awake()
        {
            Signals.Get<BoardReady>().AddListener(OnLevelLoaded);
            Signals.Get<CellPointerDown>().AddListener(OnCellPointerDown);
            Signals.Get<SameNumberListDispatched>().AddListener(BoardManager_OnSameNumberListDispatched);
            Signals.Get<NumberInputMade>().AddListener(OnNumberInputMade);
            Signals.Get<EraseButtonClicked>().AddListener(OnEraseButtonClicked);
            Signals.Get<HintButtonClicked>().AddListener(HintButtonClicked);
        }

        private void HintButtonClicked()
        {
            if (SelectedCell == null) return;
            if (!SelectedCell.IsEmpty && !SelectedCell.IsWrongNumber) return;

            Signals.Get<HintRequested>().Dispatch(SelectedCell);
        }

        private void OnEraseButtonClicked()
        {
            if (SelectedCell == null)
            {
                Signals.Get<CellEraseResponseSent>().Dispatch(false, SelectedCell);
                return;
            }

            Signals.Get<EraseRequested>().Dispatch(SelectedCell);
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

        private void OnNumberInputMade(int number, NumberInputMode inputMode)
        {
            if (SelectedCell == null) return;

            if (inputMode == NumberInputMode.Normal)
            {
                if (!SelectedCell.IsEmpty) return;
                SelectedCell.GetFilled(number, true);
            }
            else
            {
                SelectedCell.AddNote(number, true);
            }
        }


        private void BoardManager_OnSameNumberListDispatched(List<Vector2Int> positions)
        {
            foreach (var position in positions)
            {
                _cellGrid[position.x, position.y].PunchScale();
            }
        }


        private void OnLevelLoaded(LevelData levelData, bool fromContinue)
        {
            Signals.Get<BoardGridCreationRequested>().Dispatch(board);

            AdjustCellSize();
            FillTheCells(levelData);
            SelectedCell = null;

            Signals.Get<LevelBoardConfigured>().Dispatch(levelData);

            _cellGrid = new Cell[9, 9];
            foreach (var cell in cells)
            {
                _cellGrid[cell.PositionOnGrid.x, cell.PositionOnGrid.y] = cell;
            }

            Signals.Get<CellsConfigured>().Dispatch(_cellGrid);
        }

        private void FillTheCells(LevelData levelData)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                var fill = levelData.levelArray[i];
                Vector2Int position = Utils.GetPositionFromArray(levelData.levelArray, i);
                cells[i].SetPosition(position.x, position.y);
                cells[i].GetFilled(fill, false);
            }
        }

        private void AdjustCellSize()
        {
            float cellSize = board.rect.height / 9;
            foreach (Cell cell in cells)
            {
                cell.SetSizeDelta(cellSize * Vector2.one);
            }
        }
    }
}
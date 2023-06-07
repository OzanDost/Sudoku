using Data;
using Game;
using Lofelt.NiceVibrations;
using ThirdParty;
using UI.Enums;
using UnityEngine;

namespace UI.Managers
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
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
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


        private void OnLevelLoaded(LevelData levelData, bool fromContinue)
        {
            Signals.Get<BoardGridCreationRequested>().Dispatch(board);

            AdjustCellSize();
            FillTheCells(levelData);
            SelectedCell = null;

            Signals.Get<LevelBoardConfiguredForUI>().Dispatch(levelData);

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
                Cell cell = cells[i];
                NoteSaveData cellNoteData = levelData.notes[i];

                int fill = levelData.levelArray[i];
                Vector2Int position = Utils.GetPositionFromArray(levelData.levelArray, i);
                cell.SetPosition(position.x, position.y);
                cell.GetFilled(fill, false);

                FillCellNotes(cell, cellNoteData);
            }
        }

        private void FillCellNotes(Cell cell, NoteSaveData noteSaveData)
        {
            cell.AddNotesInBulk(noteSaveData.notes);
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
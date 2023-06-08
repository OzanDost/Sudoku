using System.Collections.Generic;
using Data;
using DG.Tweening;
using Game;
using ThirdParty;
using UI.Data;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace UI.Managers
{
    public class BoardAnimationManager : MonoBehaviour
    {
        private Cell[,] _cells;

        #region ColorConfigs

        [SerializeField] private Color defaultCellColor;
        [SerializeField] private Color selectedCellColor;
        [SerializeField] private Color sameNumberColor;
        [SerializeField] private Color otherCellColor;
        [SerializeField] private Color elementCompletedColor;

        [SerializeField] private Color wrongNumberColor;
        [SerializeField] private Color correctNumberColor;

        #endregion

        [SerializeField] private float colorizationDuration = 0.03f;

        private Sequence _colorizationSequence;
        private Sequence _sameNumberColorizationSequence;
        private Sequence _colorClearingSequence;
        private Sequence _elementFillSequence;
        private Vector2Int _lastColorizedCellPosition;
        private List<Vector2Int> _lastColorizedCellPositions;

        private void Awake()
        {
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
            Signals.Get<TapColorizationListDispatched>().AddListener(OnTapColorizationListDispatched);
            Signals.Get<CellsConfigured>().AddListener(OnCellsConfigured);
            Signals.Get<WrongNumberPlaced>().AddListener(OnWrongNumberPlaced);
            Signals.Get<CellEraseResponseSent>().AddListener(OnCellEraseResponseSent);
            Signals.Get<SameNumberListDispatched>().AddListener(OnSameNumberListDispatched);
            Signals.Get<ElementsFilled>().AddListener(OnElementsFilled);
            Signals.Get<CellColorResetRequested>().AddListener(OnCellColorResetRequested);

            _lastColorizedCellPosition = new Vector2Int(-1, -1);
            _lastColorizedCellPositions = new List<Vector2Int>(21);
        }

        private void OnLevelLoaded(LevelData levelData, bool fromContinue)
        {
            _lastColorizedCellPosition = new Vector2Int(-1, -1);
            _lastColorizedCellPositions = new List<Vector2Int>(21);
        }

        private void OnElementsFilled(List<Vector2Int> otherCells, Vector2Int cell)
        {
            _elementFillSequence = DOTween.Sequence();
            foreach (var otherCellPosition in otherCells)
            {
                Cell otherCell = _cells[otherCellPosition.x, otherCellPosition.y];
                var innerSeq = DOTween.Sequence()
                    .Join(otherCell.PunchScale(0.2f, 0.2f, Ease.OutFlash))
                    .Join(otherCell.ColorizeCell(elementCompletedColor, null, 0.2f))
                    .Append(otherCell.ColorizeCell(null, null, 0.2f, true));
                _elementFillSequence.Join(innerSeq);
            }

            // _elementFillSequence.OnComplete(() => { _cells[cell.x, cell.y].OnPointerDown(null); });
        }

        private void OnCellsConfigured(Cell[,] cells)
        {
            _cells = cells;
            ResetSelectionHighlight();
        }

        private void OnTapColorizationListDispatched(ColorizationData colorizationData, Vector2Int mainCellPosition)
        {
            _lastColorizedCellPosition = mainCellPosition;

            List<Vector2Int> allCells = new List<Vector2Int>(14) { mainCellPosition };
            allCells.AddRange(colorizationData.sameNumberPositions);

            CreateAndAddToList(colorizationData.boxPositions, ref allCells);

            List<Vector2Int> rowCells = CreateAndAddToList(colorizationData.rowPositions, ref allCells);
            rowCells = SortCellsByDistanceToBoxCenter(rowCells, mainCellPosition);

            List<Vector2Int> columnCells = CreateAndAddToList(colorizationData.columnPositions, ref allCells);
            columnCells = SortCellsByDistanceToBoxCenter(columnCells, mainCellPosition);

            ClearLastColorizedCellPositions();

            _colorizationSequence?.Kill();
            _colorizationSequence = DOTween.Sequence();
            Sequence sameNumberSequence = GetSameNumberSequence(colorizationData.sameNumberPositions);
            _colorizationSequence.Join(_cells[mainCellPosition.x, mainCellPosition.y]
                    .ColorizeCell(selectedCellColor, null, colorizationDuration).SetEase(Ease.Linear))
                .Join(sameNumberSequence);

            AddColorizationSequenceForCells(colorizationData.boxPositions, otherCellColor);
            Sequence columnSequence = GetLinearSequence(columnCells, mainCellPosition);
            Sequence rowSequence = GetLinearSequence(rowCells, mainCellPosition);
            _colorizationSequence.Join(rowSequence).Join(columnSequence);

            _lastColorizedCellPositions = allCells;
        }

        private List<Vector2Int> CreateAndAddToList(List<Vector2Int> positions, ref List<Vector2Int> allCells)
        {
            List<Vector2Int> uniqueCells = new List<Vector2Int>();

            foreach (var position in positions)
            {
                if (!allCells.Contains(position))
                {
                    uniqueCells.Add(position);
                    allCells.Add(position);
                }
            }

            return uniqueCells;
        }

        private void ClearLastColorizedCellPositions()
        {
            _elementFillSequence?.Kill(true);
            if (_lastColorizedCellPositions.Count > 0)
            {
                foreach (var cell in _lastColorizedCellPositions)
                {
                    _cells[cell.x, cell.y].ColorizeCell(defaultCellColor, null, 0);
                }
            }
        }

        private void AddColorizationSequenceForCells(List<Vector2Int> cells, Color color)
        {
            foreach (var cellPos in cells)
            {
                _colorizationSequence.Join(_cells[cellPos.x, cellPos.y]
                    .ColorizeCell(color, null, colorizationDuration)
                    .SetEase(Ease.Linear));
            }
        }


        private void OnSameNumberListDispatched(List<Vector2Int> sameNumbersList)
        {
            foreach (var position in sameNumbersList)
            {
                _cells[position.x, position.y].PunchScale(0.3f, 0.2f, Ease.Linear);
            }
        }

        private Sequence GetSameNumberSequence(List<Vector2Int> sameNumberPositions)
        {
            Sequence sameNumberSequence = DOTween.Sequence();

            foreach (var sameNumberPosition in sameNumberPositions)
            {
                Cell cell = _cells[sameNumberPosition.x, sameNumberPosition.y];
                sameNumberSequence.Join(cell
                    .ColorizeCell(sameNumberColor, null, colorizationDuration)
                    .SetEase(Ease.Linear));
            }

            return sameNumberSequence;
        }

        private Sequence GetLinearSequence(List<Vector2Int> cellsOnLine, Vector2Int mainCellPosition)
        {
            Sequence rowColorizationSequence = DOTween.Sequence();
            for (int i = 0; i < cellsOnLine.Count; i++)
            {
                var rowCellPosition = cellsOnLine[i];

                if (i != 0)
                {
                    var previousCell = cellsOnLine[i - 1];
                    if (Utils.IsSameDistanceToBox(previousCell, rowCellPosition, mainCellPosition))
                    {
                        rowColorizationSequence.Join(_cells[rowCellPosition.x, rowCellPosition.y]
                            .ColorizeCell(otherCellColor, null, colorizationDuration)
                            .SetEase(Ease.Linear));
                    }
                    else
                    {
                        rowColorizationSequence.Append(_cells[rowCellPosition.x, rowCellPosition.y]
                            .ColorizeCell(otherCellColor, null, colorizationDuration)
                            .SetEase(Ease.Linear));
                    }
                }
                else
                {
                    rowColorizationSequence.Append(_cells[rowCellPosition.x, rowCellPosition.y]
                        .ColorizeCell(otherCellColor, null, colorizationDuration)
                        .SetEase(Ease.Linear));
                }
            }

            return rowColorizationSequence;
        }


        private List<Vector2Int> SortCellsByDistanceToBoxCenter(List<Vector2Int> otherCells, Vector2Int cellPosition)
        {
            List<Vector2Int> sortedCells = new List<Vector2Int>();
            foreach (var otherCell in otherCells)
            {
                sortedCells.Add(otherCell);
            }

            var boxCenter = Utils.GetBoxCenter(cellPosition);
            sortedCells.Sort((a, b) =>
            {
                var distanceA = Vector2Int.Distance(a, boxCenter);
                var distanceB = Vector2Int.Distance(b, boxCenter);
                return distanceA.CompareTo(distanceB);
            });

            return sortedCells;
        }

        private void OnWrongNumberPlaced(Cell cell, bool filledByPlayer)
        {
            cell.ColorizeCell(null, wrongNumberColor, 0);
        }

        private void OnCellEraseResponseSent(bool erased, Cell cell)
        {
            if (cell is null) return;
            cell.ColorizeCell(null, correctNumberColor, 0);
        }

        private void ResetSelectionHighlight()
        {
            foreach (var cell in _cells)
            {
                cell.ColorizeCell(defaultCellColor, null, 0);
            }
        }

        private void OnCellColorResetRequested(Cell cell)
        {
            cell.ColorizeCell(selectedCellColor, correctNumberColor, 0);
        }
    }
}
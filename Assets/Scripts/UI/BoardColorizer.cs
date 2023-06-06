using System.Collections.Generic;
using deVoid.Utils;
using DG.Tweening;
using Game;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace UI
{
    public class BoardColorizer : MonoBehaviour
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
        private Vector2Int _lastColorizedCellPosition;
        private List<Vector2Int> _lastColorizedCellPositions;

        private void Awake()
        {
            Signals.Get<ColorizationListDispatched>().AddListener(OnColorizationListDispatched);
            Signals.Get<CellsConfigured>().AddListener(OnCellsConfigured);
            Signals.Get<WrongNumberPlaced>().AddListener(OnWrongNumberPlaced);
            Signals.Get<CellEraseResponseSent>().AddListener(OnCellEraseResponseSent);
            Signals.Get<SameNumberListDispatched>().AddListener(OnSameNumberListDispatched);
            Signals.Get<ElementsFilled>().AddListener(OnElementsFilled);

            _lastColorizedCellPosition = new Vector2Int(-1, -1);
            _lastColorizedCellPositions = new List<Vector2Int>(21);
        }

        private void OnElementsFilled(List<Vector2Int> otherCells, Vector2Int cell)
        {
            var tempSeq = DOTween.Sequence();
            foreach (var otherCellPosition in otherCells)
            {
                Cell otherCell = _cells[otherCellPosition.x, otherCellPosition.y];
                var innerSeq = DOTween.Sequence()
                    .Join(otherCell.PunchScale(0.2f, 0.3f, Ease.OutFlash))
                    .Join(otherCell.ColorizeCell(elementCompletedColor, null, 0.3f))
                    .Append(otherCell.ColorizeCell(null, null, 0.25f, true));
                tempSeq.Join(innerSeq);
            }
        }

        private void OnCellsConfigured(Cell[,] cells)
        {
            _cells = cells;
            ResetSelectionHighlight();
        }

        private void OnColorizationListDispatched(ColorizationData colorizationData, Vector2Int mainCellPosition)
        {
            if (mainCellPosition == _lastColorizedCellPosition) return;

            _lastColorizedCellPosition = mainCellPosition;

            List<Vector2Int> allCells = new List<Vector2Int>(14) { mainCellPosition };

            List<Vector2Int> sameNumberCells = colorizationData.sameNumberPositions;
            allCells.AddRange(sameNumberCells);

            List<Vector2Int> boxCells = new List<Vector2Int>(8);
            foreach (var boxCell in colorizationData.boxPositions)
            {
                if (!allCells.Contains(boxCell))
                {
                    boxCells.Add(boxCell);
                    allCells.Add(boxCell);
                }
            }

            allCells.AddRange(boxCells);

            List<Vector2Int> rowCells = new List<Vector2Int>(6);

            foreach (var rowPosition in colorizationData.rowPositions)
            {
                if (!allCells.Contains(rowPosition))
                {
                    rowCells.Add(rowPosition);
                    allCells.Add(rowPosition);
                }
            }

            rowCells = SortCellsByDistanceToBoxCenter(rowCells, mainCellPosition);

            List<Vector2Int> columnCells = new List<Vector2Int>(6);

            foreach (var columnPosition in colorizationData.columnPositions)
            {
                if (!allCells.Contains(columnPosition))
                {
                    columnCells.Add(columnPosition);
                    allCells.Add(columnPosition);
                }
            }

            columnCells = SortCellsByDistanceToBoxCenter(columnCells, mainCellPosition);


            if (_lastColorizedCellPositions.Count > 0)
            {
                foreach (var cell in _lastColorizedCellPositions)
                {
                    _cells[cell.x, cell.y].ColorizeCell(defaultCellColor, null, 0);
                }
            }

            _colorizationSequence?.Kill();
            _colorizationSequence = DOTween.Sequence();

            Sequence sameNumberSequence = GetSameNumberSequence(sameNumberCells);

            _colorizationSequence.Join(_cells[mainCellPosition.x, mainCellPosition.y]
                    .ColorizeCell(selectedCellColor, null, colorizationDuration).SetEase(Ease.Linear))
                .Join(sameNumberSequence);

            foreach (var cellPos in boxCells)
            {
                _colorizationSequence.Join(_cells[cellPos.x, cellPos.y]
                    .ColorizeCell(otherCellColor, null, colorizationDuration)
                    .SetEase(Ease.Linear));
            }


            Sequence columnSequence = GetLinearSequence(columnCells, mainCellPosition);
            Sequence rowSequence = GetLinearSequence(rowCells, mainCellPosition);

            _colorizationSequence.Join(rowSequence);
            _colorizationSequence.Join(columnSequence);

            _lastColorizedCellPositions = allCells;
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
                cell.SetBackgroundColor(defaultCellColor);
            }
        }
    }

    public class ColorizationData
    {
        public List<Vector2Int> boxPositions;
        public List<Vector2Int> rowPositions;
        public List<Vector2Int> columnPositions;
        public List<Vector2Int> sameNumberPositions;

        public ColorizationData(List<Vector2Int> boxPositions, List<Vector2Int> rowPositions,
            List<Vector2Int> columnPositions,
            List<Vector2Int> sameNumberPositions)
        {
            this.boxPositions = boxPositions;
            this.rowPositions = rowPositions;
            this.columnPositions = columnPositions;
            this.sameNumberPositions = sameNumberPositions;
        }
    }
}
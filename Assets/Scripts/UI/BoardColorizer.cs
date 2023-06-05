using System.Collections.Generic;
using deVoid.Utils;
using DG.Tweening;
using Game;
using UnityEngine;

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

        #endregion

        private Sequence _areaColorizationSequence;
        private Sequence _sameNumberColorizationSequence;

        private void Awake()
        {
            Signals.Get<ColorizationListDispatched>().AddListener(OnColorizationListDispatched);
            Signals.Get<CellsConfigured>().AddListener(OnCellsConfigured);
            Signals.Get<WrongNumberPlaced>().AddListener(OnWrongNumberPlaced);
        }

        private void OnCellsConfigured(Cell[,] cells)
        {
            _cells = cells;
            ResetSelectionHighlight();
        }


        private void OnColorizationListDispatched(HashSet<Vector2Int> otherCells, Vector2Int cellPosition)
        {
            List<Vector2Int> cellsToColorize = SortCellsByDistance(otherCells, cellPosition);

            _areaColorizationSequence?.Kill();
            _areaColorizationSequence = DOTween.Sequence();
            _areaColorizationSequence.AppendCallback(ResetSelectionHighlight);
            _areaColorizationSequence.AppendInterval(0.1f);
            _areaColorizationSequence.AppendCallback(() =>
            {
                foreach (var cell in cellsToColorize)
                {
                    _cells[cell.x, cell.y]
                        .SetBackgroundColor(cell == cellPosition ? selectedCellColor : otherCellColor);
                }
            });
        }

        private List<Vector2Int> SortCellsByDistance(HashSet<Vector2Int> otherCells, Vector2Int cellPosition)
        {
            List<Vector2Int> sortedCells = new List<Vector2Int>();
            foreach (var otherCell in otherCells)
            {
                sortedCells.Add(otherCell);
            }

            sortedCells.Sort((a, b) =>
            {
                var distanceA = Vector2Int.Distance(a, cellPosition);
                var distanceB = Vector2Int.Distance(b, cellPosition);
                return distanceA.CompareTo(distanceB);
            });

            return sortedCells;
        }

        private void OnWrongNumberPlaced(Cell cell, bool filledByPlayer)
        {
            cell.OnWrongNumberPlaced();
        }

        private void ResetSelectionHighlight()
        {
            foreach (var cell in _cells)
            {
                cell.SetBackgroundColor(defaultCellColor);
            }
        }
    }
}
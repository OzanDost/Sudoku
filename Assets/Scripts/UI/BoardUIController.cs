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

        private void Awake()
        {
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelData levelData)
        {
            Signals.Get<BoardGridCreationRequested>().Dispatch(board);
            AdjustCellSize();
            FillTheCells(levelData);
            Signals.Get<LevelBoardConfigured>().Dispatch(levelData);
        }

        private void FillTheCells(LevelData levelData)
        {
            int dimensionLength = (int)Mathf.Sqrt(cells.Length);
            for (int i = 0; i < cells.Length; i++)
            {
                var x = i % (dimensionLength);
                var y = i / (dimensionLength);

                var fill = levelData.levelGrid[x, y] == 0 ? "" : levelData.levelGrid[x, y].ToString();
                cells[i].GetFilled(fill);
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
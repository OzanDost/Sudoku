using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class BoardHelper
    {
        public static List<Vector2Int> GetBox(Vector2Int position)
        {
            int boxRow = position.x / 3;
            int boxCol = position.y / 3;
            List<Vector2Int> boxPositions = new List<Vector2Int>(9);

            for (int row = boxRow * 3; row < boxRow * 3 + 3; row++)
            {
                for (int col = boxCol * 3; col < boxCol * 3 + 3; col++)
                {
                    if (row == position.x && col == position.y) continue;
                    boxPositions.Add(new Vector2Int(row, col));
                }
            }

            return boxPositions;
        }

        public static List<Vector2Int> GetRow(Vector2Int position)
        {
            List<Vector2Int> rowPositions = new List<Vector2Int>(8);
            for (int i = 0; i < 9; i++)
            {
                if (i == position.y) continue;
                rowPositions.Add(new Vector2Int(position.x, i));
            }

            return rowPositions;
        }

        public static List<Vector2Int> GetColumn(Vector2Int position)
        {
            List<Vector2Int> colPositions = new List<Vector2Int>(8);
            for (int i = 0; i < 9; i++)
            {
                if (i == position.x) continue;
                colPositions.Add(new Vector2Int(i, position.y));
            }

            return colPositions;
        }

        public static int GetRoundedCellDistance(Vector2Int position1, Vector2Int position2)
        {
            return Mathf.RoundToInt(Vector2.Distance(position1, position2));
        }


        public static bool IsSameDistanceToBox(Vector2Int cell1, Vector2Int cell2, Vector2Int boxCellPosition)
        {
            int boxRow = boxCellPosition.x / 3;
            int boxCol = boxCellPosition.y / 3;
            Vector2Int boxCenter = new Vector2Int(boxRow * 3 + 1, boxCol * 3 + 1);
            int distance1 = GetRoundedCellDistance(cell1, boxCenter);
            int distance2 = GetRoundedCellDistance(cell2, boxCenter);
            return distance1 == distance2;
        }

        public static Vector2Int GetBoxCenter(Vector2Int cellPosition)
        {
            int boxRow = cellPosition.x / 3;
            int boxCol = cellPosition.y / 3;
            return new Vector2Int(boxRow * 3 + 1, boxCol * 3 + 1);
        }
    }
}
using UnityEngine;

namespace Game
{
    public static class BoardHelper
    {
        public static Vector2Int[] GetBox(Vector2Int position)
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
        
        public static Vector2Int[] GetRow(Vector2Int position)
        {
            Vector2Int[] rowPositions = new Vector2Int[9];
            for (int i = 0; i < 9; i++)
            {
                rowPositions[i] = new Vector2Int(position.x, i);
            }

            return rowPositions;
        }

        public static Vector2Int[] GetColumn(Vector2Int position)
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
using System.Collections.Generic;
using UnityEngine;

namespace UI.Data
{
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
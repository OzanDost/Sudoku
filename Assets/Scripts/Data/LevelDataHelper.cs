using UnityEngine;

namespace Data
{
    public static class LevelDataHelper
    {
        public static int[,] ArrayToGrid(int[] numberArray)
        {
            var dimensionSize = (int)Mathf.Sqrt(numberArray.Length);
            int[,] grid = new int[dimensionSize, dimensionSize];

            for (int i = 0; i < numberArray.Length; i++)
            {
                int x = i % dimensionSize;
                int y = i / dimensionSize;
                grid[x, y] = numberArray[i];
            }

            return grid;
        }

        public static int[] GridToArray(int[,] grid)
        {
            int[] numberArray = new int[grid.GetLength(0) * grid.GetLength(1)];
            for (int i = 0; i < numberArray.Length; i++)
            {
                int x = i % grid.GetLength(0);
                int y = i / grid.GetLength(0);
                numberArray[i] = grid[x, y];
            }

            return numberArray;
        }
    }
}
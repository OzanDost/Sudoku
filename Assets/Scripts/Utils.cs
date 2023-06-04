using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public static class Utils
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

    public static string DifficultyEnumToStringFast(this LevelDifficulty difficulty)
    {
        return difficulty switch
        {
            LevelDifficulty.Easy => "Easy",
            LevelDifficulty.Medium => "Medium",
            LevelDifficulty.Hard => "Hard",
            LevelDifficulty.Extreme => "Extreme",
            _ => "Easy"
        };
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
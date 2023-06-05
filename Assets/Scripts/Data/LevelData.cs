using System;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class LevelData
    {
        public int id;
        public LevelDifficulty difficulty;
        [FormerlySerializedAs("levelGrid")] public int[] levelArray;
        public int[] solutionGrid;

        public LevelData(int id, LevelDifficulty difficulty, int[] levelArray, int[] solutionGrid)
        {
            this.id = id;
            this.difficulty = difficulty;
            this.levelArray = levelArray;
            this.solutionGrid = solutionGrid;
        }
    }
    
    public enum LevelDifficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme
    }
}
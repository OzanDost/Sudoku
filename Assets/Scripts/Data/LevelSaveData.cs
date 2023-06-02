using UnityEngine;

namespace Data
{
    public class LevelSaveData : ScriptableObject
    {
        public int id;
        public LevelDifficulty difficulty;
        public int[] levelGrid;
        public int[] solutionGrid;
    }
}
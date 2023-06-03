using System;

namespace Data
{
    [Serializable]
    public class LevelData
    {
        public int id;
        public LevelDifficulty difficulty;
        public int[] levelGrid;
        public int[] solutionGrid;

        public LevelData(int id, LevelDifficulty difficulty, int[] levelGrid, int[] solutionGrid)
        {
            this.id = id;
            this.difficulty = difficulty;
            this.levelGrid = levelGrid;
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
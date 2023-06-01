namespace Data
{
    public class LevelData
    {
        public int id;
        public LevelDifficulty difficulty;
        public int[,] levelGrid;
        public int[,] solutionGrid;
    }
    
    public enum LevelDifficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme
    }
}
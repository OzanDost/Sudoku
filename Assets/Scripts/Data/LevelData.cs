namespace Data
{
    public class LevelData
    {
        public int id;
        public LevelDifficulty difficulty;
        
    }
    
    public enum LevelDifficulty
    {
        Easy,
        Medium,
        Hard,
        Extreme
    }
}
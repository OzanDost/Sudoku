using System;

namespace Data
{
    [Serializable]
    public class LevelSuccessData
    {
        public string duration;
        public int score;
        public LevelDifficulty difficulty;

        public LevelSuccessData(string duration, int score, LevelDifficulty difficulty)
        {
            this.duration = duration;
            this.score = score;
            this.difficulty = difficulty;
        }
    }
}
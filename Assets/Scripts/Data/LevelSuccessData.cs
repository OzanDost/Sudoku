using System;

namespace Data
{
    public class LevelSuccessData
    {
        public TimeSpan duration;
        public int score;
        public LevelDifficulty difficulty;

        public LevelSuccessData(TimeSpan duration, int score, LevelDifficulty difficulty)
        {
            this.duration = duration;
            this.score = score;
            this.difficulty = difficulty;
        }
    }
}
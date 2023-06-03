using System;

namespace Data
{
    public class BoardSaveStateData
    {
        public int score;
        public string timeSpan;
        public int mistakes;
        public LevelData levelData;

        public BoardSaveStateData(int score, string duration, int mistakes, LevelData levelData)
        {
            this.score = score;
            this.timeSpan = duration;
            this.mistakes = mistakes;
            this.levelData = levelData;
        }
    }
}
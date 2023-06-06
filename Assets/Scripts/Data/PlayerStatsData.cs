using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerStatsData
    {
        public List<LevelSuccessData> levelSuccessDataList;

        public PlayerStatsData(List<LevelSuccessData> levelSuccessDataList)
        {
            this.levelSuccessDataList = levelSuccessDataList;
        }
    }
}
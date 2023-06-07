namespace Data
{
    public static class GlobalGameConfigs
    {
        public static int MistakeLimit = 3;
        public static int StartingHints = 3;
        public static int HintOnNewLevel = 1;

        public static int WinCountForMedium = 2;
        public static int WinCountForHard = 3;
        public static int WinCountForExtreme = 4;

        public static int CellPointForEasyLevel = 5;
        public static int CellPointForMediumLevel = 7;
        public static int CellPointForHardLevel = 10;
        public static int CellPointForExtremeLevel = 15;

        public static int GetCellPointForLevel(LevelDifficulty difficulty)
        {
            switch (difficulty)
            {
                case LevelDifficulty.Easy:
                    return CellPointForEasyLevel;
                case LevelDifficulty.Medium:
                    return CellPointForMediumLevel;
                case LevelDifficulty.Hard:
                    return CellPointForHardLevel;
                case LevelDifficulty.Extreme:
                    return CellPointForExtremeLevel;
                default:
                    return 0;
            }
        }

        public static int GetElementCompletePoint(LevelDifficulty difficulty)
        {
            switch (difficulty)
            {
                case LevelDifficulty.Easy:
                    return CellPointForEasyLevel * 5;
                case LevelDifficulty.Medium:
                    return CellPointForMediumLevel * 5;
                case LevelDifficulty.Hard:
                    return CellPointForHardLevel * 5;
                case LevelDifficulty.Extreme:
                    return CellPointForExtremeLevel * 5;
                default:
                    return 0;
            }
        }

        public static int GetSimultaneousElementCompletePoint(LevelDifficulty difficulty)
        {
            switch (difficulty)
            {
                case LevelDifficulty.Easy:
                    return CellPointForEasyLevel * 15;
                case LevelDifficulty.Medium:
                    return CellPointForMediumLevel * 15;
                case LevelDifficulty.Hard:
                    return CellPointForHardLevel * 15;
                case LevelDifficulty.Extreme:
                    return CellPointForExtremeLevel * 15;
                default:
                    return 0;
            }
        }
    }
}
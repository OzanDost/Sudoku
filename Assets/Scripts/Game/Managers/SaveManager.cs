using Data;
using UnityEngine;

namespace Game.Managers
{
    public static class SaveManager
    {
        private const string LevelJson = "levelJson";
        private const string ContinueLevelJson = "continueLevelJson";


        public static LevelSaveData GetSavedLevel()
        {
            string json = PlayerPrefs.GetString(LevelJson, "");

            if (string.IsNullOrEmpty(json))
                return null;

            return JsonUtility.FromJson<LevelSaveData>(json);
        }

        public static void SaveLevel(LevelSaveData level)
        {
            string json = JsonUtility.ToJson(level);
            PlayerPrefs.SetString(LevelJson, json);
        }

        public static bool CanContinueLevel()
        {
            return PlayerPrefs.HasKey(ContinueLevelJson);
        }

        public static void SaveContinueLevel(BoardSaveStateData level)
        {
            string json = JsonUtility.ToJson(level);
            PlayerPrefs.SetString(ContinueLevelJson, json);
        }

        public static BoardSaveStateData GetContinueLevel()
        {
            string json = PlayerPrefs.GetString(ContinueLevelJson, "");

            if (string.IsNullOrEmpty(json))
                return null;

            return JsonUtility.FromJson<BoardSaveStateData>(json);
        }

        public static void ClearContinueLevel()
        {
            PlayerPrefs.DeleteKey(ContinueLevelJson);
        }
    }
}
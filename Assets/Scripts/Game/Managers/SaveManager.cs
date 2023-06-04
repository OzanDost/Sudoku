using Data;
using UnityEngine;

namespace Game.Managers
{
    public static class SaveManager
    {
        private const string ContinueLevelJson = "continueLevelJson";
        private const string PlayerDataJson = "playerDataJson";
        private const string HintData = "hintData";


        public static bool CanContinueLevel()
        {
            return PlayerPrefs.HasKey(ContinueLevelJson);
        }

        public static void SaveContinueLevel(BoardStateSaveData level)
        {
            string json = JsonUtility.ToJson(level);
            PlayerPrefs.SetString(ContinueLevelJson, json);
        }

        public static BoardStateSaveData GetContinueLevel()
        {
            string json = PlayerPrefs.GetString(ContinueLevelJson, "");

            if (string.IsNullOrEmpty(json))
                return null;

            return JsonUtility.FromJson<BoardStateSaveData>(json);
        }

        public static void ClearContinueLevel()
        {
            PlayerPrefs.DeleteKey(ContinueLevelJson);
        }

        public static int GetHintCount()
        {
            return PlayerPrefs.GetInt(HintData, GlobalGameConfigs.StartingHints);
        }

        public static void SaveHintCount(int hintCount)
        {
            PlayerPrefs.SetInt(HintData, hintCount);
        }
    }
}
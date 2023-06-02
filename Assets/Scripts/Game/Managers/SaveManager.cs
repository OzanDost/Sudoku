using Data;
using UnityEngine;

namespace Managers
{
    public class SaveManager
    {
        private const string LevelJson = "levelJson";

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
    }
}
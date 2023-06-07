using System.Collections.Generic;
using Data;
using ThirdParty;
using UnityEngine;

namespace Game.Managers
{
    public static class SaveManager
    {
        private const string ContinueLevelJson = "continueLevelJson";
        private const string HintDataJson = "hintData";
        private const string PlayerStatDataJson = "playerStatDataJson";

        public static PlayerStatsData PlayerStatsData { get; private set; }
        public static BoardStateSaveData CurrentBoardStateSaveData { get; private set; }
        public static int RemainingHintCount { get; private set; }


        public static void Initialize()
        {
            PlayerStatsData = GetPlayerStatsDataFromSave() ?? new PlayerStatsData(new List<LevelSuccessData>());

            if (HasContinueLevel())
            {
                CurrentBoardStateSaveData = GetContinueLevelFromSave();
            }

            RemainingHintCount = GetHintCountFromSave();


            //subscribe to events
            Signals.Get<LevelSuccess>().AddListener(OnLevelSuccess);
            Signals.Get<BoardStateDispatched>().AddListener(OnBoardStateDispatched);
            Signals.Get<HintCountUpdated>().AddListener(OnHintCountUpdated);
        }

        private static void OnHintCountUpdated(int count)
        {
            RemainingHintCount = count;
        }

        private static void OnBoardStateDispatched(BoardStateSaveData boardStateSaveData)
        {
            CurrentBoardStateSaveData = boardStateSaveData;
            SaveContinueLevel();
        }

        private static void OnLevelSuccess(LevelSuccessData levelSuccessData)
        {
            ClearContinueLevelSaveData();
            PlayerStatsData.levelSuccessDataList.Add(levelSuccessData);
        }


        private static bool HasContinueLevel()
        {
            return PlayerPrefs.HasKey(ContinueLevelJson);
        }

        public static bool CanContinueLevel()
        {
            return CurrentBoardStateSaveData != null;
        }

        public static void SaveContinueLevel()
        {
            string json = JsonUtility.ToJson(CurrentBoardStateSaveData);
            PlayerPrefs.SetString(ContinueLevelJson, json);
        }

        public static BoardStateSaveData GetContinueLevelFromSave()
        {
            string json = PlayerPrefs.GetString(ContinueLevelJson, "");

            if (string.IsNullOrEmpty(json))
                return null;

            return JsonUtility.FromJson<BoardStateSaveData>(json);
        }

        public static void ClearContinueLevelSaveData()
        {
            CurrentBoardStateSaveData = null;
            PlayerPrefs.DeleteKey(ContinueLevelJson);
        }

        public static int GetHintCountFromSave()
        {
            return PlayerPrefs.GetInt(HintDataJson, GlobalGameConfigs.StartingHints);
        }

        public static void SaveHintCount()
        {
            PlayerPrefs.SetInt(HintDataJson, RemainingHintCount);
        }

        private static PlayerStatsData GetPlayerStatsDataFromSave()
        {
            string json = PlayerPrefs.GetString(PlayerStatDataJson, "");

            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                return JsonUtility.FromJson<PlayerStatsData>(json);
            }
            catch
            {
                return null;
            }
        }

        public static void SavePlayerStatsData()
        {
            string json = JsonUtility.ToJson(PlayerStatsData);
            PlayerPrefs.SetString(PlayerStatDataJson, json);
        }
    }
}
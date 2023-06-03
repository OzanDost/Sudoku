using System.Collections.Generic;
using System.Linq;
using Data;
using deVoid.Utils;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private const string LevelSaveDataPath = "LevelSaveDatas";

        [SerializeField] private bool debugTestLevel;

        [ShowIf("debugTestLevel")]
        [AssetSelector(Paths = "Assets/Resources/LevelSaveDatas")]
        [SerializeField] private LevelSaveData testLevel;

        private List<LevelSaveData> _levelList;
        private Dictionary<int, LevelSaveData> _levels;
        private LevelSaveData _lastActiveLevel;

        public void Initialize()
        {
            _levelList = Resources.LoadAll<LevelSaveData>(LevelSaveDataPath).ToList();
            _levels = new Dictionary<int, LevelSaveData>();
            foreach (var level in _levelList)
            {
                _levels.Add(level.id, level);
            }
        }

        public void CreateLevel(bool retryLevel = false)
        {
            LevelData levelData = null;

            if (retryLevel)
            {
                levelData = GetLevelData(_lastActiveLevel);
                Signals.Get<LevelLoaded>().Dispatch(levelData);
                return;
            }

            if (debugTestLevel)
            {
                levelData = GetLevelData(testLevel);
            }

            //todo
            var level = SaveManager.GetSavedLevel();
            if (level is null)
            {
            }
            else
            {
                _levels.TryGetValue(level.id, out var levelSaveData);
                if (levelSaveData is null)
                {
                }
                else
                {
                }
            }

            _lastActiveLevel = testLevel;
            Signals.Get<LevelLoaded>().Dispatch(levelData);
        }

        public void ContinueLevel()
        {
            var boardState = SaveManager.GetContinueLevel();
            Signals.Get<LevelContinued>().Dispatch(boardState);
            Signals.Get<LevelLoaded>().Dispatch(boardState.levelData);
        }

        public void RetryLevel()
        {
            if (_lastActiveLevel != null)
            {
                CreateLevel(true);
            }
        }

        private LevelData GetLevelData(LevelSaveData levelSaveData)
        {
            var levelData = new LevelData(levelSaveData.id, levelSaveData.difficulty,
                levelSaveData.levelGrid,
                levelSaveData.solutionGrid);
            return levelData;
        }
    }
}
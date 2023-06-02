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

        public void Initialize()
        {
            _levelList = Resources.LoadAll<LevelSaveData>(LevelSaveDataPath).ToList();
            _levels = new Dictionary<int, LevelSaveData>();
            foreach (var level in _levelList)
            {
                _levels.Add(level.id, level);
            }
        }

        public void CreateLevel()
        {
            if (debugTestLevel)
            {
                var levelData = GetLevelData(testLevel);
                Signals.Get<LevelLoaded>().Dispatch(levelData);
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
        }

        private LevelData GetLevelData(LevelSaveData levelSaveData)
        {
            var levelData = new LevelData();
            levelData.id = levelSaveData.id;
            levelData.difficulty = levelSaveData.difficulty;
            levelData.levelGrid = LevelDataHelper.ArrayToGrid(levelSaveData.levelGrid);
            levelData.solutionGrid = LevelDataHelper.ArrayToGrid(levelSaveData.solutionGrid);
            return levelData;
        }
    }
}
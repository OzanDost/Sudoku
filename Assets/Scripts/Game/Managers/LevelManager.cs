using System.Collections.Generic;
using System.Linq;
using Data;
using deVoid.Utils;
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
        private LevelSaveData _lastActiveLevel;

        private Dictionary<LevelDifficulty, List<LevelSaveData>> _categoryLevelDictionary;

        public void Initialize()
        {
            _categoryLevelDictionary = new Dictionary<LevelDifficulty, List<LevelSaveData>>(10);
            _levelList = Resources.LoadAll<LevelSaveData>(LevelSaveDataPath).ToList();

            foreach (var levelSaveData in _levelList)
            {
                if (!_categoryLevelDictionary.ContainsKey(levelSaveData.difficulty))
                {
                    _categoryLevelDictionary.Add(levelSaveData.difficulty, new List<LevelSaveData>());
                }

                _categoryLevelDictionary[levelSaveData.difficulty].Add(levelSaveData);
            }
        }

        //todo remove later

        private LevelData _loadedLevel;

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.S))
            {
                Signals.Get<BoardFilledSuccessfully>().Dispatch(_loadedLevel);
            }
#endif
        }

        public void CreateLevel(LevelDifficulty levelDifficulty, bool retryLevel = false)
        {
            LevelData levelData = null;
            LevelSaveData targetLevelSaveData = null;

            if (retryLevel)
            {
                levelData = GetLevelData(_lastActiveLevel);
                Signals.Get<LevelLoaded>().Dispatch(levelData, false);
                return;
            }

#if UNITY_EDITOR

            if (debugTestLevel)
            {
                targetLevelSaveData = testLevel;
                levelData = GetLevelData(targetLevelSaveData);
                Signals.Get<LevelLoaded>().Dispatch(levelData, false);
                return;
            }
#endif

            targetLevelSaveData = _categoryLevelDictionary[levelDifficulty].GetRandomElement();
            levelData = GetLevelData(targetLevelSaveData);
            Signals.Get<LevelLoaded>().Dispatch(levelData, false);
            _loadedLevel = levelData;
            _lastActiveLevel = targetLevelSaveData;
        }

        public void ContinueLevel()
        {
            var boardState = SaveManager.CurrentBoardStateSaveData;
            _lastActiveLevel = _levelList.Find(x => x.id == boardState.levelData.id);
            Signals.Get<LevelContinued>().Dispatch(boardState);
            Signals.Get<LevelLoaded>().Dispatch(boardState.levelData, true);
            _loadedLevel = boardState.levelData;
        }

        public void RetryLevel()
        {
            if (_lastActiveLevel != null)
            {
                //Difficulty is not important here since we are retrying the last level
                CreateLevel(LevelDifficulty.Easy, true);
            }
        }

        private LevelData GetLevelData(LevelSaveData levelSaveData)
        {
            var levelData = new LevelData(levelSaveData.id, levelSaveData.difficulty,
                levelSaveData.levelGrid,
                levelSaveData.solutionGrid, null);
            return levelData;
        }
    }
}
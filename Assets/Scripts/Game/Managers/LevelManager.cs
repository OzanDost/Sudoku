using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        private const string LevelSaveDataPath = "LevelSaveData";

        private Dictionary<int, LevelSaveData> _levels;

        public void Initialize()
        {
            LevelSaveData[] temp = Resources.LoadAll<LevelSaveData>(LevelSaveDataPath);
            _levels = new Dictionary<int, LevelSaveData>();
            foreach (var level in temp)
            {
                _levels.Add(level.id, level);
            }
        }
    }
}
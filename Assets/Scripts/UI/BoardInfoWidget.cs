using System;
using Data;
using deVoid.Utils;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BoardInfoWidget : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI difficultyText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI durationText;
        [SerializeField] private TextMeshProUGUI mistakesText;

        private void Awake()
        {
            Signals.Get<BoardInfoUpdated>().AddListener(OnBoardInfoUpdated);
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
        }

        private void OnLevelLoaded(LevelData levelData)
        {
            difficultyText.text = levelData.difficulty.DifficultyEnumToStringFast();
        }

        private void OnBoardInfoUpdated(TimeSpan playTime, int score, int mistakeCount)
        {
            durationText.text = playTime.TotalHours >= 1
                ? playTime.ToString(@"hh\:mm\:ss")
                : playTime.ToString(@"mm\:ss");
            scoreText.text = score.ToString();
            mistakesText.text = $"{mistakeCount}/{GlobalGameConfigs.MistakeLimit}";
        }
    }
}
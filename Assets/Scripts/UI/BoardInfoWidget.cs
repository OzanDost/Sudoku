using System;
using Data;
using deVoid.Utils;
using DG.Tweening;
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

        private Tween _scoreTween;
        private int _lastScore;

        private void Awake()
        {
            Signals.Get<BoardInfoUpdated>().AddListener(OnBoardInfoUpdated);
            Signals.Get<BoardReady>().AddListener(OnLevelLoaded);
            Signals.Get<ScoreUpdated>().AddListener(OnScoreUpdated);
        }

        private void OnScoreUpdated(int newScore, bool instant)
        {
            int valueCopyOfLastScore = _lastScore;
            if (instant)
            {
                scoreText.text = newScore.ToString();
                _lastScore = newScore;
            }
            else
            {
                _scoreTween?.Kill();
                _scoreTween = DOTween.To(() => valueCopyOfLastScore, x => scoreText.text = x.ToString(), newScore,
                    0.5f);
            }

            _lastScore = newScore;
        }

        private void OnLevelLoaded(LevelData levelData, bool fromContinue)
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
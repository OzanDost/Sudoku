using System;
using Data;
using deVoid.UIFramework;
using deVoid.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SuccessWindow : AWindowController<SuccessWindowProperties>
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI durationText;
        [SerializeField] private TextMeshProUGUI difficultyText;

        protected override void Awake()
        {
            base.Awake();
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked()
        {
            Signals.Get<SuccessContinueButtonClicked>().Dispatch();
        }
    }

    [Serializable]
    public class SuccessWindowProperties : WindowProperties
    {
        public TimeSpan duration;
        public int score;
        public LevelDifficulty difficulty;

        public SuccessWindowProperties(TimeSpan duration, int score, LevelDifficulty difficulty)
        {
            this.duration = duration;
            this.score = score;
            this.difficulty = difficulty;
        }
    }
}
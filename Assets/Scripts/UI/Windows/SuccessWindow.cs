using System;
using Coffee.UIExtensions;
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

        [SerializeField] private UIParticle leftParticle;
        [SerializeField] private UIParticle rightParticle;

        private float _targetScore;

        //todo
        protected override void Awake()
        {
            base.Awake();
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        protected override void On_UIOPen()
        {
            base.On_UIOPen();
            leftParticle.Play();
            rightParticle.Play();

            scoreText.text = $"Score: {Properties.score}";
            durationText.text = $"Duration: {Properties.duration}";
            difficultyText.text = $"Difficulty: {Properties.difficulty}";
        }

        private void OnContinueButtonClicked()
        {
            Signals.Get<SuccessContinueButtonClicked>().Dispatch();
        }
    }

    [Serializable]
    public class SuccessWindowProperties : WindowProperties
    {
        public string duration;
        public int score;
        public LevelDifficulty difficulty;

        public SuccessWindowProperties(string duration, int score, LevelDifficulty difficulty)
        {
            this.duration = duration;
            this.score = score;
            this.difficulty = difficulty;
        }
    }
}
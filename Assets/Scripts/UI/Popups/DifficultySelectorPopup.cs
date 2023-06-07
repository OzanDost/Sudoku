using System.Collections.Generic;
using Data;
using DG.Tweening;
using Game.Managers;
using ThirdParty;
using ThirdParty.uiframework.Window;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class DifficultySelectorPopup : AWindowController
    {
        [SerializeField] private RectTransform buttonContainer;
        [SerializeField] private Button closeButton;
        [SerializeField] private DifficultyButton easyButton;
        [SerializeField] private DifficultyButton mediumButton;
        [SerializeField] private DifficultyButton hardButton;
        [SerializeField] private DifficultyButton extremeButton;
        [SerializeField] private float containerTargetPosition;
        [SerializeField] private float containerAnimationSpeed;

        private Tween _visibilityTween;
        private Vector2 _containerStartPosition;
        private ASignal<LevelDifficulty> _difficultySelectedSignal;

        protected override void Awake()
        {
            base.Awake();

            easyButton.Button.onClick.AddListener(OnEasyButtonClicked);
            mediumButton.Button.onClick.AddListener(OnMediumButtonClicked);
            hardButton.Button.onClick.AddListener(OnHardButtonClicked);
            extremeButton.Button.onClick.AddListener(OnExtremeButtonClicked);
            closeButton.onClick.AddListener(OnCloseButtonClicked);

            _containerStartPosition = buttonContainer.anchoredPosition;
            _difficultySelectedSignal = Signals.Get<LevelDifficultySelected>();
        }

        private void OnCloseButtonClicked()
        {
            _visibilityTween?.Kill();
            _visibilityTween = buttonContainer.DOAnchorPosY(_containerStartPosition.y, containerAnimationSpeed)
                .SetEase(Ease.InBack)
                .SetSpeedBased()
                .SetSpeedBased().OnComplete(() => { CloseRequest?.Invoke(this); });
        }

        protected override void On_UIOPen()
        {
            _visibilityTween?.Kill();
            buttonContainer.anchoredPosition = _containerStartPosition;
            _visibilityTween = buttonContainer.DOAnchorPosY(containerTargetPosition, containerAnimationSpeed)
                .SetEase(Ease.OutBack)
                .SetSpeedBased();

            var statsData = SaveManager.PlayerStatsData;

            Dictionary<LevelDifficulty, int> winCounts = new Dictionary<LevelDifficulty, int>(4)
            {
                { LevelDifficulty.Easy, 0 }, { LevelDifficulty.Medium, 0 }, { LevelDifficulty.Hard, 0 },
                { LevelDifficulty.Extreme, 0 }
            };

            foreach (var statData in statsData.levelSuccessDataList)
            {
                winCounts[statData.difficulty]++;
            }

            easyButton.Configure(false, 0);

            mediumButton.Configure(winCounts[LevelDifficulty.Easy] < GlobalGameConfigs.WinCountForMedium,
                GlobalGameConfigs.WinCountForMedium);
            hardButton.Configure(winCounts[LevelDifficulty.Medium] < GlobalGameConfigs.WinCountForHard,
                GlobalGameConfigs.WinCountForHard);
            extremeButton.Configure(winCounts[LevelDifficulty.Hard] < GlobalGameConfigs.WinCountForExtreme,
                GlobalGameConfigs.WinCountForExtreme);

            //todo maybe show this on button
        }


        private void OnExtremeButtonClicked()
        {
            _difficultySelectedSignal.Dispatch(LevelDifficulty.Extreme);
        }

        private void OnHardButtonClicked()
        {
            _difficultySelectedSignal.Dispatch(LevelDifficulty.Hard);
        }

        private void OnMediumButtonClicked()
        {
            _difficultySelectedSignal.Dispatch(LevelDifficulty.Medium);
        }

        private void OnEasyButtonClicked()
        {
            _difficultySelectedSignal.Dispatch(LevelDifficulty.Easy);
        }
    }
}
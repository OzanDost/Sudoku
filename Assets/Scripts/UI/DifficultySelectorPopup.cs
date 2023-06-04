using Data;
using deVoid.UIFramework;
using deVoid.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DifficultySelectorPopup : AWindowController
    {
        [SerializeField] private RectTransform buttonContainer;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button easyButton;
        [SerializeField] private Button mediumButton;
        [SerializeField] private Button hardButton;
        [SerializeField] private Button extremeButton;
        [SerializeField] private float containerTargetPosition;
        [SerializeField] private float containerAnimationSpeed;

        private Tween _visibilityTween;
        private Vector2 _containerStartPosition;
        private ASignal<LevelDifficulty> _difficultySelectedSignal;

        protected override void Awake()
        {
            base.Awake();

            easyButton.onClick.AddListener(OnEasyButtonClicked);
            mediumButton.onClick.AddListener(OnMediumButtonClicked);
            hardButton.onClick.AddListener(OnHardButtonClicked);
            extremeButton.onClick.AddListener(OnExtremeButtonClicked);
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

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            _visibilityTween?.Kill();
            buttonContainer.anchoredPosition = _containerStartPosition;
            _visibilityTween = buttonContainer.DOAnchorPosY(containerTargetPosition, containerAnimationSpeed)
                .SetEase(Ease.OutBack)
                .SetSpeedBased();
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
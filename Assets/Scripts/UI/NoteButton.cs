using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class NoteButton : AWidgetButton
    {
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private RectTransform pencilIcon;
        [SerializeField] private Image stateBackgroundImage;
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;

        private Sequence _animationSequence;
        private Vector2 _originalPencilIconPosition;
        private bool _isOriginalPencilIconPositionSet;
        private bool IsEnabled { get; set; }

        protected void Awake()
        {
            button.onClick.AddListener(OnClick);
            IsEnabled = false;
            SetVisual(IsEnabled);
        }

        private void OnClick()
        {
            IsEnabled = !IsEnabled;

            SetVisual(IsEnabled);
        }

        private void SetVisual(bool isEnabled)
        {
            if (isEnabled)
            {
                stateText.SetText("ON");
                stateBackgroundImage.color = enabledColor;

                Color targetTextColor = disabledColor;
                targetTextColor.a = 1f;

                stateText.color = targetTextColor;
                Animate();
            }
            else
            {
                stateText.SetText("OFF");
                stateBackgroundImage.color = disabledColor;

                Color targetTextColor = enabledColor;
                targetTextColor.a = 1f;
                stateText.color = targetTextColor;
            }
        }

        public override void Animate()
        {
            if (!_isOriginalPencilIconPositionSet)
            {
                _originalPencilIconPosition = pencilIcon.anchoredPosition;
                _isOriginalPencilIconPositionSet = true;
            }

            float targetX = pencilIcon.anchoredPosition.x + 25;
            float targetRotation = 10;

            _animationSequence?.Kill();

            _animationSequence = DOTween.Sequence()
                .Append(pencilIcon.DOLocalMoveX(targetX, 0.15f).SetEase(Ease.Linear))
                .Join(pencilIcon.DOLocalRotate(Vector3.forward * targetRotation, 0.15f).SetEase(Ease.Linear))
                .SetLoops(4, LoopType.Yoyo)
                .OnKill(() =>
                {
                    pencilIcon.localEulerAngles = Vector3.zero;
                    pencilIcon.anchoredPosition = _originalPencilIconPosition;
                });
        }

        public override void NoFunctionAnimate()
        {
        }
    }
}
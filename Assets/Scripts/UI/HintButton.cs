using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HintButton : AWidgetButton
    {
        [SerializeField] private TextMeshProUGUI remainingHintCountText;
        [SerializeField] private GameObject adIcon;
        [SerializeField] private RectTransform hintIcon;

        private int RemainingHintCount { get; set; }

        private Sequence _animationSequence;

        [SerializeField] private float downYScale;
        [SerializeField] private float downXScale;
        [SerializeField] private float upYScale;
        [SerializeField] private float upXScale;

        private void Awake()
        {
            Button.onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            Animate();
        }

        public void SetRemainingHintCount(int remainingHintCount)
        {
            RemainingHintCount = remainingHintCount;
            remainingHintCountText.SetText(remainingHintCount.ToString());
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            bool hasHint = RemainingHintCount > 0;
            remainingHintCountText.gameObject.SetActive(hasHint);
            adIcon.SetActive(!hasHint);
        }

        [Button]
        public override void Animate()
        {
            _animationSequence?.Kill(true);
            _animationSequence = DOTween.Sequence()
                .Append(hintIcon.DOScale(new Vector3(downXScale, downYScale, 1f), 0.1f).SetEase(Ease.Linear))
                .Append(hintIcon.DOScale(new Vector3(upXScale, upYScale, 1f), 0.1f).SetEase(Ease.Linear))
                .Append(hintIcon.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear));
        }

        public override void NoFunctionAnimate()
        {
            Animate();
        }
    }
}
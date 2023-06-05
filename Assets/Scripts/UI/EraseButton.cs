using Coffee.UIExtensions;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EraseButton : AWidgetButton
    {
        [SerializeField] private RectTransform eraseIcon;
        [SerializeField] private UIParticle eraseUIParticle;
        [SerializeField] private VerticalLayoutGroup layout;

        private Sequence _animationSequence;
        private Vector2 _initialIconPosition;
        private bool _isInitialPositionSet;

        [Button]
        public override void Animate()
        {
            _animationSequence?.Kill();
            layout.enabled = false;
            _animationSequence = DOTween.Sequence()
                .Append(eraseIcon.DOScale(new Vector3(1.2f, 0.7f, 1f), 0.1f).SetEase(Ease.Linear))
                .Append(eraseIcon.DOScale(new Vector3(0.85f, 1.1f, 1f), 0.1f).SetEase(Ease.Linear))
                .AppendCallback(() => { eraseUIParticle.Play(); })
                .Append(eraseIcon.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear))
                .OnComplete(() => { layout.enabled = true; });
        }

        [Button]
        public override void NoFunctionAnimate()
        {
            if (!_isInitialPositionSet)
            {
                _initialIconPosition = eraseIcon.anchoredPosition;
                _isInitialPositionSet = true;
            }

            layout.enabled = false;

            _animationSequence?.Kill();
            _animationSequence = DOTween.Sequence()
                .Append(eraseIcon.DOShakePosition(0.2f, Vector3.right * 5f, 20, fadeOut: false))
                .OnKill(() => { eraseIcon.anchoredPosition = _initialIconPosition; })
                .OnComplete(() => { layout.enabled = true; });
        }

        private void OnDisable()
        {
            eraseUIParticle.Stop();
            layout.enabled = true;
        }
    }
}
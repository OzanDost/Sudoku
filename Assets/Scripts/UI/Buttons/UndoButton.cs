using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class UndoButton : AWidgetButton
    {
        [SerializeField] private RectTransform undoIcon;
        [SerializeField] private VerticalLayoutGroup layout;

        private Sequence _rotateSequence;
        private float _initalIconScale;
        private Tween _noFunctionAnimationTween;
        private Vector2 _initialIconPosition;
        private bool _isInitialPositionSet;

        private void Awake()
        {
            _initalIconScale = undoIcon.localScale.x;
        }

        public override void Animate()
        {
            _rotateSequence?.Kill(true);
            _rotateSequence = DOTween.Sequence();
            _rotateSequence
                .Join(undoIcon.DOScale(_initalIconScale + 0.1f, 0.1f))
                .Join(undoIcon.DORotate(new Vector3(0f, 0f, 360), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.Linear))
                .Join(undoIcon.DOScale(_initalIconScale, 0.15f));
        }

        public override void NoFunctionAnimate()
        {
            if (!_isInitialPositionSet)
            {
                _initialIconPosition = undoIcon.anchoredPosition;
                _isInitialPositionSet = true;
            }

            layout.enabled = false;

            _noFunctionAnimationTween?.Kill();
            _noFunctionAnimationTween = undoIcon.DOShakePosition(0.2f, Vector3.right * 5f, 20, fadeOut: false)
                .OnKill(() => { undoIcon.anchoredPosition = _initialIconPosition; })
                .OnComplete(() => { layout.enabled = true; });
        }

        private void OnDisable()
        {
            _rotateSequence?.Kill();
            layout.enabled = true;
        }
    }
}
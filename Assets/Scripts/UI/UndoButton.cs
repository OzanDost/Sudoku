using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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

        [Button]
        public override void Animate()
        {
            _rotateSequence = DOTween.Sequence();
            _rotateSequence
                .Append(undoIcon.DOScale(_initalIconScale + 0.1f, 0.25f))
                .Join(undoIcon.DORotate(new Vector3(0f, 0f, 360), 0.5f, RotateMode.FastBeyond360))
                .Join(undoIcon.DOScale(_initalIconScale, 0.15f));
        }

        [Button]
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
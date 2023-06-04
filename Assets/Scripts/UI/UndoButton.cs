using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class UndoButton : AWidgetButton
    {
        [SerializeField] private RectTransform undoIcon;

        private Sequence _rotateSequence;
        private float _initalIconScale;

        private void Awake()
        {
            _initalIconScale = undoIcon.localScale.x;
        }

        public override void Animate()
        {
            _rotateSequence = DOTween.Sequence();
            _rotateSequence
                .Append(undoIcon.DOScale(_initalIconScale + 0.1f, 0.25f))
                .Join(undoIcon.DORotate(new Vector3(0f, 0f, -360), 0.5f, RotateMode.FastBeyond360))
                .Join(undoIcon.DOScale(_initalIconScale, 0.15f));
        }
    }
}
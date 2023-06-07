using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Misc
{
    public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private float animationSpeed = 3f;
        [SerializeField] private float subtractedScale = 0.1f;

        private Tween _animationTween;
        private float _defaultScale;
        private bool _defaultScaleSet;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_defaultScaleSet)
            {
                _defaultScale = transform.localScale.x;
                _defaultScaleSet = true;
            }

            _animationTween?.Kill();
            _animationTween = transform.DOScale(_defaultScale - subtractedScale, animationSpeed).SetSpeedBased().SetEase(Ease.Linear);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _animationTween?.Kill();
            _animationTween = transform.DOScale(_defaultScale, animationSpeed).SetSpeedBased().SetEase(Ease.Linear);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _animationTween?.Kill();
            _animationTween = transform.DOScale(_defaultScale, animationSpeed).SetSpeedBased().SetEase(Ease.Linear);
        }
    }
}
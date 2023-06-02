using deVoid.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image cellBackground;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI numberText;

        private Tween _punchTween;
        public Vector2Int PositionOnGrid { get; private set; }
        public bool IsEmpty => string.IsNullOrEmpty(numberText.text);


        public void GetFilled(string number)
        {
            numberText.SetText(number);
        }

        public void SetSizeDelta(Vector2 sizeDelta)
        {
            rectTransform.sizeDelta = sizeDelta;
        }

        public void SetPosition(int x, int y)
        {
            var size = rectTransform.rect.size;
            var position = new Vector2(x * size.x, -y * size.y);
            rectTransform.anchoredPosition = position;
            PositionOnGrid = new Vector2Int(x, y);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Signals.Get<CellPointerDown>().Dispatch(PositionOnGrid);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //todo add some effect
            Signals.Get<CellPointerUp>().Dispatch(PositionOnGrid);
        }

        public void SetColor(Color color)
        {
            cellBackground.color = color;
        }

        public void PunchScale()
        {
            _punchTween?.Kill();
            _punchTween = numberText.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.4f, 1, 0.5f)
                .OnKill(() => numberText.rectTransform.localScale = Vector3.one);
        }
    }
}
using deVoid.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI numberText;

        private Vector2Int PositionOnGrid { get; set; }

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
        }
    }
}
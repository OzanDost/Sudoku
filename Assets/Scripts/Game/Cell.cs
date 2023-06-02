using TMPro;
using UnityEngine;

namespace Game
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI numberText;

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
        }
    }
}
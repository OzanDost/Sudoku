using UnityEngine;

namespace UI.Windows
{
    public class NumberInputWidget : MonoBehaviour
    {
        [SerializeField] private NumberButton[] buttons;
        
        public void Initialize(RectTransform boardRect)
        {
            for (var index = 0; index < buttons.Length; index++)
            {
                var numberButton = buttons[index];
                numberButton.Configure(index + 1);
            }

            AdjustNumberSizesAndPositions(boardRect);
        }

        private void AdjustNumberSizesAndPositions(RectTransform boardRect)
        {
            var boardSize = boardRect.rect.size;
            var buttonWidth = buttons[0].RectTransform.rect.size.x;
            var buttonHeight = buttons[0].RectTransform.rect.size.y;

            if (buttonWidth * 9 > boardSize.x)
            {
                var targetSize = new Vector2(boardSize.x / 9 - 10f, buttonHeight);
                foreach (var button in buttons)
                {
                    button.SetSize(targetSize);
                }
            }

            var padding = buttonWidth / 4f;
            var availableWidth = boardSize.x - padding * 2;
            var spaceBetween = (availableWidth - buttonWidth * 9f) / 8f + buttonWidth / 8f;

            var buttonTargetPosition = new Vector2(padding, 0);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].RectTransform.anchoredPosition = buttonTargetPosition;
                buttonTargetPosition.x += buttonWidth + spaceBetween;
            }
        }
    }
}
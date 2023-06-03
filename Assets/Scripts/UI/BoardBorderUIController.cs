using deVoid.Utils;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class BoardBorderUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform innerVerticalFirst;
        [SerializeField] private RectTransform innerVerticalSecond;
        [SerializeField] private RectTransform innerHorizontalFirst;
        [SerializeField] private RectTransform innerHorizontalSecond;

        [SerializeField] private RectTransform[] verticalGridLines;
        [SerializeField] private RectTransform[] horizontalGridLines;

        private RectTransform _lastActiveBoard;

        private void Awake()
        {
            Signals.Get<BoardGridCreationRequested>().AddListener(OnBoardGridCreationRequested);
        }

        private void OnBoardGridCreationRequested(RectTransform boardRect)
        {
            if (_lastActiveBoard != null && _lastActiveBoard.rect.size == boardRect.rect.size)
            {
                return;
            }

            _lastActiveBoard = boardRect;
            ConfigureBorders(4);
            ConfigureGridLines(2);
        }

        private void ConfigureBorders(float width)
        {
            float boardSize = _lastActiveBoard.rect.size.y;

            innerVerticalFirst.anchoredPosition = new Vector2(boardSize / 3f - width / 2f, 0);
            innerVerticalFirst.sizeDelta = new Vector2(width, boardSize);

            innerVerticalSecond.anchoredPosition = new Vector2(boardSize / 3f * 2 - width / 2f, 0);
            innerVerticalSecond.sizeDelta = new Vector2(width, boardSize);

            innerHorizontalFirst.anchoredPosition = new Vector2(0, -boardSize / 3f + width / 2f);
            innerHorizontalFirst.sizeDelta = new Vector2(boardSize, width);

            innerHorizontalSecond.anchoredPosition = new Vector2(0, -boardSize / 3f * 2 + width / 2f);
            innerHorizontalSecond.sizeDelta = new Vector2(boardSize, width);
        }

        private void ConfigureGridLines(float width)
        {
            float boardSize = _lastActiveBoard.rect.size.y;
            int n = 0;
            for (int i = 0; i < verticalGridLines.Length; i++)
            {
                // n++;
                if (i is 2 or 4)
                {
                    n += 2;
                }
                else
                {
                    n++;
                }


                float targetX = boardSize / 9f * n - width / 2f;

                // float x = ((2 * n) % 3 == 0 ? 2 * n - 1 : n + 1) * boardSize.x / 8f;
                verticalGridLines[i].anchoredPosition = new Vector2(targetX, 0);
                verticalGridLines[i].sizeDelta = new Vector2(width, boardSize);
            }

            n = 0;
            for (int i = 0; i < horizontalGridLines.Length; i++)
            {
                if (i is 2 or 4)
                {
                    n += 2;
                }
                else
                {
                    n++;
                }

                float targetY = -boardSize / 9f * n + width / 2f;

                horizontalGridLines[i].anchoredPosition = new Vector2(0, targetY);
                horizontalGridLines[i].sizeDelta = new Vector2(boardSize, width);
            }
        }
    }
}
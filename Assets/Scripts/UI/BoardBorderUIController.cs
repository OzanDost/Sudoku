using deVoid.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class BoardBorderUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform verticalLeft;
        [SerializeField] private RectTransform verticalRight;
        [SerializeField] private RectTransform horizontalTop;
        [SerializeField] private RectTransform horizontalBottom;
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

        public void ConfigureBorders(float width)
        {
            Vector2 boardSize = _lastActiveBoard.rect.size;

            verticalLeft.anchoredPosition = Vector2.zero;
            verticalLeft.sizeDelta = new Vector2(width, boardSize.y);

            verticalRight.anchoredPosition = new Vector2(boardSize.x - width, 0);
            verticalRight.sizeDelta = new Vector2(width, boardSize.y);

            horizontalTop.anchoredPosition = Vector2.zero;
            horizontalTop.sizeDelta = new Vector2(boardSize.x, width);

            horizontalBottom.anchoredPosition = new Vector2(0, -boardSize.y + width);
            horizontalBottom.sizeDelta = new Vector2(boardSize.x, width);

            innerVerticalFirst.anchoredPosition = new Vector2(boardSize.x / 3f - width / 2f, 0);
            innerVerticalFirst.sizeDelta = new Vector2(width, boardSize.y);

            innerVerticalSecond.anchoredPosition = new Vector2(boardSize.x / 3f * 2 - width / 2f, 0);
            innerVerticalSecond.sizeDelta = new Vector2(width, boardSize.y);

            innerHorizontalFirst.anchoredPosition = new Vector2(0, -boardSize.y / 3f + width / 2f);
            innerHorizontalFirst.sizeDelta = new Vector2(boardSize.x, width);

            innerHorizontalSecond.anchoredPosition = new Vector2(0, -boardSize.y / 3f * 2 + width / 2f);
            innerHorizontalSecond.sizeDelta = new Vector2(boardSize.x, width);
        }

        private void ConfigureGridLines(float width)
        {
            Vector2 boardSize = _lastActiveBoard.rect.size;
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


                float targetX = boardSize.x / 9f * n - width / 2f;

                // float x = ((2 * n) % 3 == 0 ? 2 * n - 1 : n + 1) * boardSize.x / 8f;
                verticalGridLines[i].anchoredPosition = new Vector2(targetX, 0);
                verticalGridLines[i].sizeDelta = new Vector2(width, boardSize.y);
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

                float targetY = -boardSize.y / 9f * n + width / 2f;

                horizontalGridLines[i].anchoredPosition = new Vector2(0, targetY);
                horizontalGridLines[i].sizeDelta = new Vector2(boardSize.x, width);
            }
        }


        [Button]
        private void Test()
        {
            Debug.Log(
                $"Anchored position: {verticalLeft.anchoredPosition}\n local position: {verticalLeft.localPosition}\n position: {verticalLeft.position}");
        }
    }
}
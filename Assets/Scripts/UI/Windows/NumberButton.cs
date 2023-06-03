using System;
using deVoid.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class NumberButton : MonoBehaviour
    {
        public RectTransform RectTransform => rectTransform;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Button button;
        [SerializeField] private int number;
        [SerializeField] private TextMeshProUGUI numberText;
        
        //todo for testing only, remove later
        public Button ButtonForTest => button;

        private void Awake()
        {
            button.onClick.AddListener(OnNumberButtonClicked);
        }

        public void Configure(int number)
        {
            this.number = number;
            numberText.SetText(number.ToString());
        }

        public void SetPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = position;
        }

        public void SetSize(Vector2 size)
        {
            rectTransform.sizeDelta = size;
        }
        private void OnNumberButtonClicked()
        {
            Signals.Get<NumberButtonClicked>().Dispatch(number);
        }
    }
}
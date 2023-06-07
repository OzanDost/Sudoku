using ThirdParty;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class NumberButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private int number;
        [SerializeField] private TextMeshProUGUI numberText;
        
        private void Awake()
        {
            button.onClick.AddListener(OnNumberButtonClicked);
        }

        public void Configure(int number)
        {
            this.number = number;
            numberText.SetText(number.ToString());
        }
        private void OnNumberButtonClicked()
        {
            Signals.Get<NumberButtonClicked>().Dispatch(number);
        }
    }
}
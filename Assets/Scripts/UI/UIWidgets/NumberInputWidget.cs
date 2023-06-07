using ThirdParty;
using UI.Buttons;
using UI.Enums;
using UnityEngine;

namespace UI.UIWidgets
{
    public class NumberInputWidget : MonoBehaviour
    {
        [SerializeField] private NumberButton[] buttons;
        private NumberInputMode inputMode = NumberInputMode.Normal;

        private void Awake()
        {
            Signals.Get<NoteModeToggleRequested>().AddListener(OnNoteModeToggleRequested);
            Signals.Get<NumberButtonClicked>().AddListener(OnNumberButtonClicked);
        }

        private void OnNumberButtonClicked(int number)
        {
            Signals.Get<NumberInputMade>().Dispatch(number, inputMode);
        }

        private void OnNoteModeToggleRequested()
        {
            inputMode = inputMode == NumberInputMode.Normal ? NumberInputMode.Note : NumberInputMode.Normal;
        }

        public void Initialize(RectTransform boardRect)
        {
            for (var index = 0; index < buttons.Length; index++)
            {
                var numberButton = buttons[index];
                numberButton.Configure(index + 1);
            }
        }
    }
}
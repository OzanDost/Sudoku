using deVoid.Utils;
using UnityEngine;

namespace UI.Windows
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

        //testing only
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                buttons[0].ButtonForTest.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buttons[1].ButtonForTest.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                buttons[2].ButtonForTest.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                buttons[3].ButtonForTest.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                buttons[4].ButtonForTest.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                buttons[5].ButtonForTest.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                buttons[6].ButtonForTest.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                buttons[7].ButtonForTest.onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                buttons[8].ButtonForTest.onClick.Invoke();
            }
#endif
        }
    }

    public enum NumberInputMode
    {
        Normal,
        Note
    }
}
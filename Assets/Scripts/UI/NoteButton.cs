using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class NoteButton : AWidgetButton
    {
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private Image stateBackgroundImage;
        [SerializeField] private Color enabledColor;
        [SerializeField] private Color disabledColor;

        private bool IsEnabled { get; set; }

        protected void Awake()
        {
            button.onClick.AddListener(OnClick);
            IsEnabled = false;
            SetVisual(IsEnabled);
        }

        private void OnClick()
        {
            IsEnabled = !IsEnabled;

            SetVisual(IsEnabled);
        }

        private void SetVisual(bool isEnabled)
        {
            if (isEnabled)
            {
                stateText.SetText("ON");
                stateBackgroundImage.color = enabledColor;

                Color targetTextColor = disabledColor;
                targetTextColor.a = 1f;

                stateText.color = targetTextColor;
            }
            else
            {
                stateText.SetText("OFF");
                stateBackgroundImage.color = disabledColor;

                Color targetTextColor = enabledColor;
                targetTextColor.a = 1f;
                stateText.color = targetTextColor;
            }
        }

        public override void Animate()
        {
        }
    }
}
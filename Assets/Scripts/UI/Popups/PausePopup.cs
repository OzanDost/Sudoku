using deVoid.UIFramework;
using deVoid.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class PausePopup : AWindowController
    {
        [SerializeField] private Button resumeButton;

        protected override void Awake()
        {
            base.Awake();
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
        }

        private void OnResumeButtonClicked()
        {
            CloseRequest?.Invoke(this);
        }

        public override void UI_Close()
        {
            base.UI_Close();
            Signals.Get<GameUnpaused>().Dispatch();
        }
    }
}
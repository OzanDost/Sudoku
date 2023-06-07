using ThirdParty;
using ThirdParty.uiframework.Window;
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

        protected override void On_UIClose()
        {
            base.On_UIClose();
            Signals.Get<GameUnpaused>().Dispatch();
            Signals.Get<PausePopupClosed>().Dispatch();
        }
    }
}
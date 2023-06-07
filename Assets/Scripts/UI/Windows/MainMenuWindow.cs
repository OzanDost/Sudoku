using Game.Managers;
using ThirdParty;
using ThirdParty.uiframework.Window;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class MainMenuWindow : AWindowController
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button continueButton;

        protected override void Awake()
        {
            base.Awake();
            playButton.onClick.AddListener(OnPlayButtonClicked);
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked()
        {
            Signals.Get<ContinueLevelRequested>().Dispatch();
        }

        protected override void On_UIOPen()
        {
            base.On_UIOPen();
            continueButton.gameObject.SetActive(false);
            if (SaveManager.CanContinueLevel())
            {
                continueButton.gameObject.SetActive(true);
            }
        }


        private void OnPlayButtonClicked()
        {
            Signals.Get<NewGameButtonClicked>().Dispatch();
        }
    }
}
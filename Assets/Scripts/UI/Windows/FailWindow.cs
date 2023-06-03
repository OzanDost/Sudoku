using deVoid.UIFramework;
using deVoid.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class FailWindow : AWindowController
    {
        [SerializeField] private Button retryButton;
        [SerializeField] private Button quitButton;

        protected override void Awake()
        {
            base.Awake();
            retryButton.onClick.AddListener(OnRetryButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnQuitButtonClicked()
        {
            Signals.Get<LevelQuit>().Dispatch();
        }

        private void OnRetryButtonClicked()
        {
            Signals.Get<LevelRetryRequested>().Dispatch();
        }
    }
}
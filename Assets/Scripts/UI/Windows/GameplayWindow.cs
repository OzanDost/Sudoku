using System;
using Data;
using ThirdParty;
using ThirdParty.uiframework.Window;
using UI.UIWidgets;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class GameplayWindow : AWindowController
    {
        [SerializeField] private RectTransform boardGridContainer;
        [SerializeField] private NumberInputWidget numberInputWidget;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button quitButton;

        protected override void Awake()
        {
            base.Awake();
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnQuitButtonClicked()
        {
            Signals.Get<ReturnToMenuRequested>().Dispatch();
        }

        private void OnPauseButtonClicked()
        {
            Signals.Get<GamePaused>().Dispatch(true);
        }

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            numberInputWidget.Initialize(boardGridContainer);
        }
    }
}
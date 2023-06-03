using System;
using Data;
using deVoid.UIFramework;
using deVoid.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class GameplayWindow : AWindowController<GameplayWindowProperties>
    {
        [SerializeField] private RectTransform boardGridContainer;
        [SerializeField] private NumberInputWidget numberInputWidget;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private BoardInfoWidget boardInfoWidget;

        protected override void Awake()
        {
            base.Awake();
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnQuitButtonClicked()
        {
            
        }

        private void OnPauseButtonClicked()
        {
        }

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            numberInputWidget.Initialize(boardGridContainer);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.U))
            {
                Signals.Get<UndoRequested>().Dispatch();
            }
#endif
        }
    }

    [Serializable]
    public class GameplayWindowProperties : WindowProperties
    {
        public LevelData levelData;

        public GameplayWindowProperties(LevelData levelData)
        {
            this.levelData = levelData;
        }
    }
}
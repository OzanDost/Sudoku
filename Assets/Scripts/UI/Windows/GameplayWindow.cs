using System;
using Data;
using deVoid.UIFramework;
using deVoid.Utils;
using Game.Managers;
using UnityEngine;

namespace UI.Windows
{
    public class GameplayWindow : AWindowController<GameplayWindowProperties>
    {
        [SerializeField] private RectTransform boardGridContainer;
        [SerializeField] private NumberInputWidget numberInputWidget;

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
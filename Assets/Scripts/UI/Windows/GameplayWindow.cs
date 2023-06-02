using System;
using Data;
using deVoid.UIFramework;
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
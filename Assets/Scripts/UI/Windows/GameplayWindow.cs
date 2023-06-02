using Data;
using deVoid.UIFramework;

namespace UI.Windows
{
    public class GameplayWindow : AWindowController
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
        }

        private void ConstructBoard()
        {
        }
    }


    public class GameplayWindowProperties : WindowProperties
    {
        public LevelData levelData;

        public GameplayWindowProperties(LevelData levelData)
        {
            this.levelData = levelData;
        }
    }
}
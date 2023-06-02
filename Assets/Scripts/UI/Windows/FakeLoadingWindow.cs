using DefaultNamespace.UI.Misc;
using deVoid.UIFramework;
using deVoid.Utils;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class FakeLoadingWindow : AWindowController
    {
        [SerializeField] private ThreeDots threeDots;

        private float _fakeLoadDuration = 5f;

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            threeDots.ToggleAnimation(true);
            Invoke(nameof(LoadingCompleted), _fakeLoadDuration);
        }

        private void LoadingCompleted()
        {
            CloseRequest?.Invoke(this);
            Signals.Get<FakeLoadingFinished>().Dispatch();
        }

        public override void UI_Close()
        {
            base.UI_Close();
            threeDots.ToggleAnimation(false);
        }
    }
}
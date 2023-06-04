using System;
using deVoid.UIFramework;
using deVoid.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class FakeRewardedPopup : AWindowController<FakeRewardedPopupProperties>
    {
        [SerializeField] private Button finishRewarded;
        [SerializeField] private Button closeRewarded;
        [SerializeField] private GameObject failedToFinishText;

        private Tween _delayedCloseTween;

        protected override void Awake()
        {
            base.Awake();
            finishRewarded.onClick.AddListener(OnFinishRewarded);
            closeRewarded.onClick.AddListener(OnCloseRewarded);
        }

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            failedToFinishText.SetActive(false);
        }

        protected override void On_UIOPen()
        {
            base.On_UIOPen();
            Signals.Get<GamePaused>().Dispatch(false);
        }

        protected override void On_UIClose()
        {
            base.On_UIClose();
            Signals.Get<GameUnpaused>().Dispatch();
        }

        private void OnCloseRewarded()
        {
            failedToFinishText.SetActive(true);
            _delayedCloseTween = DOVirtual.DelayedCall(2f, () => { CloseRequest?.Invoke(this); });
        }

        private void OnFinishRewarded()
        {
            CloseRequest?.Invoke(this);
            Properties.successActionCallBack?.Invoke();
        }
    }

    [Serializable]
    public class FakeRewardedPopupProperties : WindowProperties
    {
        public Action successActionCallBack;

        public FakeRewardedPopupProperties(Action successActionCallBack)
        {
            this.successActionCallBack = successActionCallBack;
        }
    }
}
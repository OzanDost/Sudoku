using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HintButton : AWidgetButton
    {
        [SerializeField] private TextMeshProUGUI remainingHintCountText;
        [SerializeField] private GameObject adIcon;

        private int RemainingHintCount { get; set; }

        public void SetRemainingHintCount(int remainingHintCount)
        {
            RemainingHintCount = remainingHintCount;
            remainingHintCountText.SetText(remainingHintCount.ToString());
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            bool hasHint = RemainingHintCount > 0;
            remainingHintCountText.gameObject.SetActive(hasHint);
            adIcon.SetActive(!hasHint);
        }

        public override void Animate()
        {
        }
    }
}
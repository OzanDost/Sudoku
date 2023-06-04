using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HintButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI remainingHintCountText;
        [SerializeField] private GameObject adIcon;

        public Button Button => button;
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
    }
}
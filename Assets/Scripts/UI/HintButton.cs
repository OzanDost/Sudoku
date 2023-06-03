using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HintButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI remainingHintCountText;

        public Button Button => button;
        public void SetRemainingHintCount(int remainingHintCount)
        {
            remainingHintCountText.SetText(remainingHintCount.ToString());
        }
    }
}
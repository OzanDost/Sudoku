using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class DifficultyButton : MonoBehaviour
    {
        public Button Button => button;

        [SerializeField] private Button button;
        [SerializeField] private Image lockImage;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void Configure(bool isLocked, int minWinCount)
        {
            lockImage.gameObject.SetActive(isLocked);
            button.interactable = !isLocked;
            descriptionText.SetText($"Available after {minWinCount} wins.");
            descriptionText.gameObject.SetActive(isLocked);
        }
    }
}
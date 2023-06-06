using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DifficultyButton : MonoBehaviour
    {
        public Button Button => button;

        [SerializeField] private Button button;
        [SerializeField] private Image lockImage;
        [SerializeField] private TextMeshProUGUI descriptionText;


        //TODO: Add a way to configure the button
        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        public void Configure(bool isLocked, int minWinCount)
        {
            lockImage.gameObject.SetActive(isLocked);
            button.interactable = !isLocked;
            descriptionText.SetText($"Available after {minWinCount} wins.");
            descriptionText.gameObject.SetActive(isLocked);
        }

        private void OnButtonClicked()
        {
        }
    }
}
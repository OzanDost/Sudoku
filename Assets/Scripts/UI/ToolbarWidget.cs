using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolbarWidget : MonoBehaviour
    {
        [SerializeField] private Button undoButton;
        [SerializeField] private Button eraseButton;
        [SerializeField] private Button noteButton;
        [SerializeField] private Button hintButton;

        private void Awake()
        {
            undoButton.onClick.AddListener(OnUndoButtonClicked);
            eraseButton.onClick.AddListener(OnEraseButtonClicked);
            noteButton.onClick.AddListener(OnNoteButtonClicked);
            hintButton.onClick.AddListener(OnHintButtonClicked);
        }

        private void OnHintButtonClicked()
        {
        }

        private void OnNoteButtonClicked()
        {
        }

        private void OnEraseButtonClicked()
        {
        }

        private void OnUndoButtonClicked()
        {
        }
    }
}
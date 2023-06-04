using deVoid.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolbarWidget : MonoBehaviour
    {
        [SerializeField] private Button undoButton;
        [SerializeField] private Button eraseButton;
        [SerializeField] private NoteButton noteButton;
        [SerializeField] private HintButton hintButton;

        private void Awake()
        {
            undoButton.onClick.AddListener(OnUndoButtonClicked);
            eraseButton.onClick.AddListener(OnEraseButtonClicked);
            noteButton.Button.onClick.AddListener(OnNoteButtonClicked);
            hintButton.Button.onClick.AddListener(OnHintButtonClicked);

            Signals.Get<HintCountUpdated>().AddListener(OnHintCountUpdated);
        }

        private void OnHintCountUpdated(int newHintAmount)
        {
            hintButton.SetRemainingHintCount(newHintAmount);
        }

        private void OnHintButtonClicked()
        {
            Signals.Get<HintButtonClicked>().Dispatch();
        }

        private void OnNoteButtonClicked()
        {
            Signals.Get<NoteModeToggleRequested>().Dispatch();
        }

        private void OnEraseButtonClicked()
        {
            Signals.Get<EraseRequested>().Dispatch();
        }

        private void OnUndoButtonClicked()
        {
            Signals.Get<UndoRequested>().Dispatch();
        }
    }
}
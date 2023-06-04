using deVoid.Utils;
using UnityEngine;

namespace UI
{
    public class ToolbarWidget : MonoBehaviour
    {
        [SerializeField] private UndoButton undoButton;
        [SerializeField] private EraseButton eraseButton;
        [SerializeField] private NoteButton noteButton;
        [SerializeField] private HintButton hintButton;

        private void Awake()
        {
            undoButton.Button.onClick.AddListener(OnUndoButtonClicked);
            eraseButton.Button.onClick.AddListener(OnEraseButtonClicked);
            noteButton.Button.onClick.AddListener(OnNoteButtonClicked);
            hintButton.Button.onClick.AddListener(OnHintButtonClicked);

            Signals.Get<HintCountUpdated>().AddListener(OnHintCountUpdated);
            Signals.Get<HintNotAuthorized>().AddListener(OnHintNotAuthorized);
            Signals.Get<UndoResponseSent>().AddListener(OnUndoResponseSent);
        }

        private void OnHintNotAuthorized()
        {
        }

        private void OnUndoResponseSent(bool success)
        {
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
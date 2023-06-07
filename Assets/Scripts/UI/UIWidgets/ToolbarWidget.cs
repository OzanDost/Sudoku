using Game;
using Lofelt.NiceVibrations;
using ThirdParty;
using UI.Buttons;
using UnityEngine;

namespace UI.UIWidgets
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
            Signals.Get<UndoResponseSent>().AddListener(OnUndoResponseSent);
            Signals.Get<CellEraseResponseSent>().AddListener(OnCellEraseResponseSent);
        }

        private void OnHintButtonClicked()
        {
            Signals.Get<HintButtonClicked>().Dispatch();
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        private void OnNoteButtonClicked()
        {
            Signals.Get<NoteModeToggleRequested>().Dispatch();
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        private void OnEraseButtonClicked()
        {
            Signals.Get<EraseButtonClicked>().Dispatch();
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        private void OnUndoButtonClicked()
        {
            Signals.Get<UndoRequested>().Dispatch();
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        private void OnCellEraseResponseSent(bool erased, Cell cell)
        {
            if (erased)
            {
                eraseButton.Animate();
            }
            else
            {
                eraseButton.NoFunctionAnimate();
            }
        }

        private void OnHintCountUpdated(int newHintAmount)
        {
            hintButton.SetRemainingHintCount(newHintAmount);
        }

        private void OnUndoResponseSent(bool success)
        {
            if (success)
            {
                undoButton.Animate();
            }
            else
            {
                undoButton.NoFunctionAnimate();
            }
        }
    }
}
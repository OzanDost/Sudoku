using System.Collections.Generic;
using deVoid.Utils;
using DG.Tweening;
using Game.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image cellBackground;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private GameObject[] noteNumbers;

        private Tween _punchTween;
        public Vector2Int PositionOnGrid { get; private set; }
        public int Number { get; private set; }
        public bool IsEmpty => Number == 0;
        public bool IsWrongNumber { get; set; }

        private Sequence _wrongNumberSequence;


        public void GetFilled(int number, bool filledByPlayer)
        {
            Number = number;
            string fill = number == 0 ? "" : number.ToString();
            numberText.SetText(fill);

            EraseCellNotes();

            if (filledByPlayer && number != 0)
            {
                Signals.Get<CellFilled>().Dispatch(this);

                Signals.Get<UndoableActionMade>()
                    .Dispatch(new UndoableAction(() =>
                    {
                        GetFilled(0, false);
                        OnPointerDown(null);
                    }));
            }
        }

        public void SetSizeDelta(Vector2 sizeDelta)
        {
            rectTransform.sizeDelta = sizeDelta;
        }

        public void SetPosition(int x, int y)
        {
            var size = rectTransform.rect.size;
            var position = new Vector2(x * size.x, -y * size.y);
            rectTransform.anchoredPosition = position;
            PositionOnGrid = new Vector2Int(x, y);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Signals.Get<CellPointerDown>().Dispatch(PositionOnGrid);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //todo add some effect
            Signals.Get<CellPointerUp>().Dispatch(PositionOnGrid);
        }

        public void SetBackgroundColor(Color color)
        {
            cellBackground.color = color;
        }

        public void SetNumberColor(Color color)
        {
            numberText.color = color;
        }

        public void PunchScale()
        {
            _punchTween?.Kill();
            _punchTween = numberText.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.4f, 1, 0.5f)
                .OnKill(() => numberText.rectTransform.localScale = Vector3.one);
        }

        public void AddNote(int number, bool shouldAddToUndoStack)
        {
            GameObject targetNote = noteNumbers[number - 1];
            if (targetNote.activeInHierarchy)
            {
                RemoveNote(number, shouldAddToUndoStack);
                return;
            }

            targetNote.SetActive(true);
            if (shouldAddToUndoStack)
            {
                Signals.Get<UndoableActionMade>().Dispatch(new UndoableAction(() => { RemoveNote(number, false); }));
            }
        }

        private bool RemoveNote(int number, bool shouldAddToUndoStack)
        {
            GameObject targetNote = noteNumbers[number - 1];
            if (!targetNote.activeInHierarchy) return false;
            targetNote.SetActive(false);
            if (shouldAddToUndoStack)
            {
                Signals.Get<UndoableActionMade>().Dispatch(new UndoableAction(() => { AddNote(number, false); }));
            }

            return true;
        }

        private void EraseCellNotes(bool shouldAddToUndoStack = true)
        {
            List<int> erasedNotes = new List<int>();

            for (var i = 0; i < noteNumbers.Length; i++)
            {
                if (RemoveNote(i + 1, false))
                {
                    erasedNotes.Add(i + 1);
                }
            }

            //early return to avoid unnecessary undo registration
            if (erasedNotes.Count == 0) return;

            if (shouldAddToUndoStack)
            {
                Signals.Get<UndoableActionMade>().Dispatch(new UndoableAction(() =>
                {
                    foreach (var erasedNote in erasedNotes)
                    {
                        AddNote(erasedNote, false);
                    }
                }));
            }
        }

        private void EraseCellNumber()
        {
            if (!IsWrongNumber) return;
            GetFilled(0, false);
            IsWrongNumber = false;
        }

        public void EraseCellContent()
        {
            EraseCellNotes();
            EraseCellNumber();
        }

        public void OnWrongNumberPlaced()
        {
            SetNumberColor(Color.red);
            IsWrongNumber = true;
        }
    }
}
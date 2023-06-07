using System.Collections.Generic;
using DG.Tweening;
using Game.Managers;
using ThirdParty;
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
        private Sequence _colorizationSequence;
        private Color _lastBackgroundColor;
        private Color _lastTextColor;

        private void Awake()
        {
            _lastBackgroundColor = cellBackground.color;
            _lastTextColor = numberText.color;
        }


        //this should've been split into two methods
        //it does both erasing wrong cells and regular filling
        public void GetFilled(int number, bool filledByPlayer)
        {
            int previousNumber = Number;
            Number = number;
            string fill = number == 0 ? "" : number.ToString();
            numberText.SetText(fill);

            EraseCellNotes();

            Signals.Get<CellFilled>().Dispatch(this, filledByPlayer);

            if (filledByPlayer)
            {
                Signals.Get<UndoableActionMade>()
                    .Dispatch(new UndoableAction(() =>
                    {
                        int undoNumber = number == 0 ? previousNumber : 0;
                        GetFilled(undoNumber, false);
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

            Signals.Get<NoteUpdatedOnCell>().Dispatch(this, number);
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

            Signals.Get<NoteUpdatedOnCell>().Dispatch(this, number);
            return true;
        }

        public void AddNotesInBulk(bool[] noteToggles)
        {
            for (var i = 0; i < noteToggles.Length; i++)
            {
                noteNumbers[i].SetActive(noteToggles[i]);
            }
        }

        public bool EraseCellNotes(bool shouldAddToUndoStack = true)
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
            if (erasedNotes.Count == 0) return false;

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

            return true;
        }

        public Sequence ColorizeCell(Color? backgroundColor, Color? textColor, float duration,
            bool returnToLastColor = false)
        {
            _lastBackgroundColor = cellBackground.color;
            _lastTextColor = numberText.color;

            _colorizationSequence?.Kill(true);

            if (duration == 0)
            {
                if (returnToLastColor)
                {
                    cellBackground.color = _lastBackgroundColor;
                    numberText.color = _lastTextColor;
                }
                else
                {
                    if (backgroundColor != null)
                    {
                        cellBackground.color = backgroundColor.Value;
                    }

                    numberText.color = textColor ?? numberText.color;
                }

                return null;
            }


            _colorizationSequence = DOTween.Sequence();

            if (returnToLastColor)
            {
                _colorizationSequence.Append(cellBackground.DOColor(_lastBackgroundColor, duration)
                    .SetEase(Ease.Linear));
                _colorizationSequence.Join(numberText.DOColor(_lastTextColor, duration).SetEase(Ease.Linear));
            }
            else
            {
                if (backgroundColor.HasValue)
                {
                    _colorizationSequence.Join(cellBackground.DOColor(backgroundColor.Value, duration)
                        .SetEase(Ease.Linear));
                }

                if (textColor.HasValue)
                {
                    _colorizationSequence.Join(numberText.DOColor(textColor.Value, duration).SetEase(Ease.Linear));
                }
            }

            return _colorizationSequence;
        }

        public Tween PunchScale(float punchScale, float duration, Ease ease)
        {
            _punchTween?.Kill();
            return _punchTween = numberText.rectTransform.DOPunchScale(Vector3.one * punchScale, duration, 1, 0.05f)
                .SetEase(ease)
                .OnKill(() => numberText.rectTransform.localScale = Vector3.one);
        }

        public bool CanEraseCellNumber()
        {
            if (!IsWrongNumber || Number == 0) return false;
            return true;
        }

        public void OnWrongNumberPlaced()
        {
            IsWrongNumber = true;
        }
    }
}
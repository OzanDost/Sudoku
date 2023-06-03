using System;
using System.Collections.Generic;
using deVoid.Utils;

namespace Game.Managers
{
    public class UndoManager
    {
        private static Stack<Action> _undoStack;

        public static void Initialize()
        {
            _undoStack = new Stack<Action>(30);

            Signals.Get<UndoableActionMade>().AddListener(OnUndoableActionMade);
            Signals.Get<UndoRequested>().AddListener(OnUndoRequested);
        }

        private static void OnUndoRequested()
        {
            if (_undoStack.Count == 0) return;
            //todo maybe show toaster here.

            Action undoAction = _undoStack.Pop();
            undoAction?.Invoke();
        }


        private static void OnUndoableActionMade(UndoableAction action)
        {
            Action undoableAction = ConvertToUndoAction(action);
            _undoStack.Push(undoableAction);
        }

        private static Action ConvertToUndoAction(UndoableAction action)
        {
            Action undoAction = null;
            switch (action.Type)
            {
                case UndoActionType.CellFill:
                    undoAction = () =>
                    {
                        action.Cell.GetFilled(0, false);
                        action.Cell.OnPointerDown(null);
                    };
                    break;
                case UndoActionType.CellErase:
                    break;
                case UndoActionType.NoteFill:
                    break;
                case UndoActionType.NoteErase:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return undoAction;
        }
    }

    public class UndoableAction
    {
        public UndoActionType Type { get; set; }
        public int? Number { get; set; }
        public int? AddedNote { get; set; }
        public Cell Cell { get; set; }

        public UndoableAction(UndoActionType type, int? number, int? addedNote, Cell cell)
        {
            Type = type;
            Number = number;
            AddedNote = addedNote;
            Cell = cell;
        }
    }

    public enum UndoActionType
    {
        CellFill,
        CellErase,
        NoteFill,
        NoteErase
    }
}
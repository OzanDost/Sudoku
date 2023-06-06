using System;
using System.Collections.Generic;
using Data;
using deVoid.Utils;

namespace Game.Managers
{
    public static class UndoManager
    {
        private static Stack<Action> _undoStack;

        public static void Initialize()
        {
            _undoStack = new Stack<Action>(30);

            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
            Signals.Get<UndoableActionMade>().AddListener(OnUndoableActionMade);
            Signals.Get<UndoRequested>().AddListener(OnUndoRequested);
        }

        private static void OnLevelLoaded(LevelData levelData, bool fromSave)
        {
            if (!fromSave) _undoStack.Clear();
        }

        private static void OnUndoRequested()
        {
            Signals.Get<UndoResponseSent>().Dispatch(_undoStack.Count > 0);

            if (_undoStack.Count == 0) return;

            Action undoAction = _undoStack.Pop();
            undoAction?.Invoke();
        }


        private static void OnUndoableActionMade(UndoableAction action)
        {
            _undoStack.Push(action.UndoAction);
        }
    }

    public class UndoableAction
    {
        public Action UndoAction { get; }

        public UndoableAction(Action undoAction)
        {
            UndoAction = undoAction;
        }
    }
}
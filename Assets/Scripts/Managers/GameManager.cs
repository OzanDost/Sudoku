using System;
using DefaultNamespace;
using deVoid.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameState CurrentGameState { get; private set; }

        private void Start()
        {
            Signals.Get<RequestGameStateChange>().AddListener(OnGameStateChangeRequested);
        }

      
        private void OnGameStateChangeRequested(GameState newState)
        {
            ChangeGameState(newState);
        }

        private void ChangeGameState(GameState newGameState)
        {
            var oldGameState = CurrentGameState;
            CurrentGameState = newGameState;
            Signals.Get<GameStateChanged>().Dispatch(oldGameState, newGameState);
        }
    }

    public enum GameState
    {
        None,
        Loading,
        Menu,
        Gameplay,
        Success,
        Fail,
    }
}
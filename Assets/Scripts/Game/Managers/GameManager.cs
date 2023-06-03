using deVoid.Utils;
using Game.Managers;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private BoardManager boardManager;
        public GameState CurrentGameState { get; private set; }

        private void Start()
        {
            // Subscribing to events
            Signals.Get<RequestGameStateChange>().AddListener(OnGameStateChangeRequested);
            Signals.Get<FakeLoadingFinished>().AddListener(OnFakeLoadingFinished);
            Signals.Get<PlayButtonClicked>().AddListener(MainMenu_OnPlayButtonClicked);
            Signals.Get<LevelFailed>().AddListener(OnLevelFailed);
            Signals.Get<LevelQuit>().AddListener(OnLevelQuit);
            Signals.Get<LevelRetryRequested>().AddListener(OnLevelRetryRequested);

            ChangeGameState(GameState.Loading);

            levelManager.Initialize();
            UndoManager.Initialize();
        }

        private void OnLevelQuit()
        {
            ChangeGameState(GameState.Menu);
        }

        private void OnLevelFailed()
        {
            ChangeGameState(GameState.Fail);
        }

        private void MainMenu_OnPlayButtonClicked()
        {
            levelManager.CreateLevel();
            ChangeGameState(GameState.Gameplay);
        }

        private void OnLevelRetryRequested()
        {
            levelManager.RetryLevel();
            ChangeGameState(GameState.Gameplay);
        }


        private void OnFakeLoadingFinished()
        {
            ChangeGameState(GameState.Menu);
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
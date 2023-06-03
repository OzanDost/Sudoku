using Data;
using deVoid.Utils;
using Game.Managers;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        public GameState CurrentGameState { get; private set; }

        private void Start()
        {
            // Subscribing to events
            Signals.Get<RequestGameStateChange>().AddListener(OnGameStateChangeRequested);
            Signals.Get<FakeLoadingFinished>().AddListener(OnFakeLoadingFinished);
            Signals.Get<PlayLevelRequested>().AddListener(MainMenu_OnPlayButtonClicked);
            Signals.Get<ContinueLevelRequested>().AddListener(MainMenu_OnContinueButtonClicked);
            Signals.Get<SuccessContinueButtonClicked>().AddListener(Success_OnContinueButtonClicked);
            Signals.Get<LevelFailed>().AddListener(OnLevelFailed);
            Signals.Get<LevelQuit>().AddListener(OnLevelQuit);
            Signals.Get<LevelRetryRequested>().AddListener(OnLevelRetryRequested);
            Signals.Get<LevelSuccess>().AddListener(OnLevelSuccess);


            ChangeGameState(GameState.Loading);

            levelManager.Initialize();
            UndoManager.Initialize();
            BoardManager.Initialize();
        }

        private void MainMenu_OnContinueButtonClicked()
        {
            levelManager.ContinueLevel();
            ChangeGameState(GameState.Gameplay);
        }

        private void Success_OnContinueButtonClicked()
        {
            ChangeGameState(GameState.Menu);
        }

        private void OnLevelSuccess(LevelSuccessData data)
        {
            ChangeGameState(GameState.Success);
        }

        private void OnLevelQuit()
        {
            ChangeGameState(GameState.Menu);
        }

        private void OnLevelFailed()
        {
            ChangeGameState(GameState.Fail);
            SaveManager.ClearContinueLevel();
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
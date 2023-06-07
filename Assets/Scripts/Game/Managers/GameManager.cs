using System;
using Data;
using deVoid.Utils;
using DG.Tweening;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        public GameState CurrentGameState { get; private set; }

        private void Awake()
        {
            DOTween.Init().SetCapacity(200, 150);
        }

        private void Start()
        {
            // Subscribing to events
            Signals.Get<RequestGameStateChange>().AddListener(OnGameStateChangeRequested);
            Signals.Get<FakeLoadingFinished>().AddListener(OnFakeLoadingFinished);
            Signals.Get<ContinueLevelRequested>().AddListener(MainMenu_OnContinueButtonClicked);
            Signals.Get<SuccessContinueButtonClicked>().AddListener(Success_OnContinueButtonClicked);
            Signals.Get<LevelFailed>().AddListener(OnLevelFailed);
            Signals.Get<LevelQuit>().AddListener(OnLevelQuit);
            Signals.Get<LevelRetryRequested>().AddListener(OnLevelRetryRequested);
            Signals.Get<LevelSuccess>().AddListener(OnLevelSuccess);
            Signals.Get<LevelDifficultySelected>().AddListener(OnLevelDifficultySelected);

            ChangeGameState(GameState.Loading);


            ScoreManager.Initialize();
            SaveManager.Initialize();
            levelManager.Initialize();
            UndoManager.Initialize();
            BoardManager.Initialize();

            ApplyConfigs();
        }

        private void ApplyConfigs()
        {
            Application.targetFrameRate = GlobalGameConfigs.TargetFrameRate;
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
            SaveManager.SaveContinueLevel();
        }

        private void OnLevelFailed()
        {
            ChangeGameState(GameState.Fail);
            SaveManager.ClearContinueLevelSaveData();
        }


        private void OnLevelDifficultySelected(LevelDifficulty difficulty)
        {
            levelManager.CreateLevel(difficulty);
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

        private void OnApplicationQuit()
        {
            BoardManager.SendLevelSaveRequest();
            SaveManager.SaveHintCount();
            SaveManager.SavePlayerStatsData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                // BoardManager.SendLevelSaveRequest();
                SaveManager.SaveHintCount();
                // SaveManager.SavePlayerStatsData();
            }
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
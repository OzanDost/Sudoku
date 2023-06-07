using System;
using Data;
using Game.Managers;
using ThirdParty;
using ThirdParty.uiframework;
using UI.Popups;
using UI.Windows;
using UnityEngine;

namespace UI.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")] public GameManager gameManager;

        [SerializeField] private UISettings defaultUISettings;

        private UIFrame _uiFrame;

        private void Awake()
        {
            _uiFrame = defaultUISettings.CreateUIInstance();


            // Subscribing to events
            Signals.Get<GameStateChanged>().AddListener(GameManager_OnGameStateChanged);
            Signals.Get<LevelBoardConfigured>().AddListener(OnLevelBoardConfigured);
            Signals.Get<LevelSuccess>().AddListener(OnLevelSuccess);
            Signals.Get<PausePopupRequested>().AddListener(OnPausePopupRequested);
            Signals.Get<NewGameButtonClicked>().AddListener(OnNewGameButtonClicked);
            Signals.Get<RewardedPopupRequested>().AddListener(OnRewardedPopupRequested);
        }


        private void OnRewardedPopupRequested(Action successActionCallBack, Action failedActionCallBack)
        {
            _uiFrame.OpenWindow("FakeRewardedPopup",
                new FakeRewardedPopupProperties(successActionCallBack, failedActionCallBack));
        }

        private void OnNewGameButtonClicked()
        {
            _uiFrame.OpenWindow("DifficultySelectorPopup");
        }

        private void OnPausePopupRequested()
        {
            _uiFrame.OpenWindow("PausePopup");
        }

        private void OnLevelSuccess(LevelSuccessData data)
        {
            _uiFrame.OpenWindow("SuccessPopup",
                new SuccessWindowProperties(data.duration, data.score, data.difficulty));
        }


        private void OnLevelBoardConfigured(LevelData data)
        {
            if (gameManager.CurrentGameState is GameState.Menu or GameState.Fail)
            {
                _uiFrame.CloseWindow("DifficultySelectorPopup");
                _uiFrame.CloseWindow("MainMenuWindow");
                _uiFrame.OpenWindow("GameplayWindow");
            }
        }

        private void GameManager_OnGameStateChanged(GameState oldState, GameState newState)
        {
            if (newState == GameState.Loading)
            {
                _uiFrame.OpenWindow("FakeLoadingWindow");
            }

            if (newState == GameState.Menu)
            {
                _uiFrame.CloseWindow("GameplayWindow");
                _uiFrame.OpenWindow("MainMenuWindow");
            }

            if (newState == GameState.Fail)
            {
                _uiFrame.OpenWindow("FailPopup");
            }
        }
    }
}
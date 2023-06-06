using System;
using Data;
using deVoid.UIFramework;
using deVoid.Utils;
using Game.Managers;
using UI.Popups;
using UI.Windows;
using UnityEngine;

namespace DefaultNamespace.UI
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
            _uiFrame.OpenWindow("FakeRewardedPopup", new FakeRewardedPopupProperties(successActionCallBack, failedActionCallBack));
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
            _uiFrame.OpenWindow("SuccessWindow",
                new SuccessWindowProperties(data.duration, data.score, data.difficulty));
        }


        private void OnLevelBoardConfigured(LevelData data)
        {
            if (gameManager.CurrentGameState is GameState.Menu or GameState.Fail)
            {
                _uiFrame.OpenWindow("GameplayWindow", new GameplayWindowProperties(data));
            }
        }

        private void GameManager_OnGameStateChanged(GameState oldState, GameState newState)
        {
            //todo gamestate change for UI

            if (newState == GameState.Loading)
            {
                _uiFrame.OpenWindow("FakeLoadingWindow");
            }

            if (newState == GameState.Menu)
            {
                _uiFrame.OpenWindow("MainMenuWindow");
            }

            if (newState == GameState.Fail)
            {
                _uiFrame.OpenWindow("FailWindow");
            }
        }
    }
}
using Data;
using deVoid.UIFramework;
using deVoid.Utils;
using Managers;
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

            //
        }

        private void OnGameFinished(bool isSuccessful)
        {
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
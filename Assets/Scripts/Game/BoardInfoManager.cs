using System;
using System.Collections;
using Data;
using deVoid.Utils;
using Game.Managers;
using Managers;
using UnityEngine;

namespace Game
{
    public class BoardInfoManager : MonoBehaviour
    {
        private TimeSpan _currentPlayTime;
        private int _currentScore;
        private int _mistakeCount;

        private IEnumerator _timerRoutine;
        private TimeSpan _oneSecondSpan;
        private WaitForSeconds _oneSecondWait;
        private ASignal<TimeSpan, int, int> BoardInfoUpdatedSignal;

        private void Awake()
        {
            Signals.Get<LevelContinued>().AddListener(OnLevelContinued);
            Signals.Get<LevelLoaded>().AddListener(OnLevelLoaded);
            Signals.Get<GameStateChanged>().AddListener(OnGameStateChanged);
            Signals.Get<WrongNumberPlaced>().AddListener(OnWrongNumberPlaced);
            Signals.Get<BoardStateSaveRequested>().AddListener(OnBoardStateSaveRequested);
            Signals.Get<GamePaused>().AddListener(OnGamePaused);
            Signals.Get<PausePopupClosed>().AddListener(OnPausePopupClosed);


            _oneSecondSpan = new TimeSpan(0, 0, 1);
            _oneSecondWait = new WaitForSeconds(1);
            BoardInfoUpdatedSignal = Signals.Get<BoardInfoUpdated>();
        }

        private void OnPausePopupClosed()
        {
            StartTimer();
        }

        private void OnGamePaused(bool showPausePopup)
        {
            StopTimer();
            if (showPausePopup)
            {
                Signals.Get<PausePopupRequested>().Dispatch();
            }
        }

        private void OnBoardStateSaveRequested(LevelData levelData)
        {
            BoardSaveStateData saveStateData = new BoardSaveStateData(_currentScore, _currentPlayTime.ToString(),
                _mistakeCount, levelData);
            SaveManager.SaveContinueLevel(saveStateData);
        }

        private void OnGameStateChanged(GameState oldState, GameState newState)
        {
            if (oldState == GameState.Gameplay)
            {
                ResetValues();
            }
        }

        private void ResetValues()
        {
            StopTimer();
            _currentPlayTime = new TimeSpan(0, 0, 0);


            _mistakeCount = 0;
            _currentScore = 0;
        }

        private void OnLevelLoaded(LevelData data)
        {
            StartTimer();
        }

        private void OnWrongNumberPlaced(Cell cell)
        {
            _mistakeCount++;
            BoardInfoUpdatedSignal.Dispatch(_currentPlayTime, _currentScore, _mistakeCount);
            if (_mistakeCount >= GlobalGameConfigs.MistakeLimit)
            {
                Signals.Get<LevelFailed>().Dispatch();
            }
        }

        private void OnLevelContinued(BoardSaveStateData data)
        {
            Configure(data);
        }

        private void Configure(BoardSaveStateData data)
        {
            _currentPlayTime = TimeSpan.Parse(data.timeSpan);
            _currentScore = data.score;
            _mistakeCount = data.mistakes;
        }

        private void StartTimer()
        {
            _timerRoutine = TimerRoutine();
            StartCoroutine(_timerRoutine);
        }

        private void StopTimer()
        {
            if (_timerRoutine != null)
            {
                StopCoroutine(_timerRoutine);
            }
        }

        private IEnumerator TimerRoutine()
        {
            while (true)
            {
                _currentPlayTime = _currentPlayTime.Add(_oneSecondSpan);
                BoardInfoUpdatedSignal.Dispatch(_currentPlayTime, _currentScore, _mistakeCount);
                yield return _oneSecondWait;
            }
        }
    }
}
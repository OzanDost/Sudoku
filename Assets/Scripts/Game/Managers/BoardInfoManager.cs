using System;
using System.Collections;
using Data;
using deVoid.Utils;
using UnityEngine;

namespace Game.Managers
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
            Signals.Get<BoardReady>().AddListener(OnLevelLoaded);
            Signals.Get<GameStateChanged>().AddListener(OnGameStateChanged);
            Signals.Get<WrongNumberPlaced>().AddListener(OnWrongNumberPlaced);
            Signals.Get<BoardStateSaveRequested>().AddListener(OnBoardStateSaveRequested);
            Signals.Get<GamePaused>().AddListener(OnGamePaused);
            Signals.Get<GameUnpaused>().AddListener(OnPausePopupClosed);
            Signals.Get<BoardFilledSuccessfully>().AddListener(OnBoardFilledSuccessfully);


            _oneSecondSpan = new TimeSpan(0, 0, 1);
            _oneSecondWait = new WaitForSeconds(1);
            BoardInfoUpdatedSignal = Signals.Get<BoardInfoUpdated>();
        }

        private void OnBoardFilledSuccessfully(LevelData levelData)
        {
            StopTimer();
            
            string timeSpent = _currentPlayTime.ToString();
            Signals.Get<LevelSuccess>().Dispatch(new LevelSuccessData(timeSpent, _currentScore, levelData.difficulty));
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
            BoardStateSaveData stateSaveData = new BoardStateSaveData(_currentScore, _currentPlayTime.ToString(),
                _mistakeCount, levelData);
            Signals.Get<BoardStateDispatched>().Dispatch(stateSaveData);
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

        private void OnLevelLoaded(LevelData data, bool fromContinue)
        {
            StartTimer();
        }

        private void OnWrongNumberPlaced(Cell cell, bool filledByPlayer)
        {
            if (!filledByPlayer) return;
            _mistakeCount++;
            BoardInfoUpdatedSignal.Dispatch(_currentPlayTime, _currentScore, _mistakeCount);
            if (_mistakeCount >= GlobalGameConfigs.MistakeLimit)
            {
                Signals.Get<LevelFailed>().Dispatch();
            }
        }

        private void OnLevelContinued(BoardStateSaveData data)
        {
            Configure(data);
        }

        private void Configure(BoardStateSaveData data)
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
using Data;
using deVoid.Utils;
using UnityEngine;

namespace Game.Managers
{
    public class HintManager : MonoBehaviour
    {
        private int _remainingHints;

        private void Awake()
        {
            Signals.Get<BoardReady>().AddListener(OnLevelLoaded);
            Signals.Get<GamePaused>().AddListener(OnGamePaused);
            Signals.Get<HintRequested>().AddListener(OnHintRequested);
            Signals.Get<HintUsed>().AddListener(OnHintUsed);

            _remainingHints = SaveManager.GetHintCount();
        }

        private void OnHintRequested(Cell cell)
        {
            if (_remainingHints <= 0)
            {
                void SuccessActionCallBack()
                {
                    Signals.Get<HintAuthorized>().Dispatch(cell);
                }
                
                Signals.Get<RewardedPopupRequested>().Dispatch(SuccessActionCallBack, null);
            }
            else
            {
                _remainingHints--;
                Signals.Get<HintAuthorized>().Dispatch(cell);
            }
        }


        private void OnGamePaused(bool showPausePopup)
        {
            SaveManager.SaveHintCount(_remainingHints);
        }

        private void OnLevelLoaded(LevelData levelData, bool fromContinue)
        {
            if (_remainingHints <= 0)
            {
                _remainingHints = fromContinue ? 0 : GlobalGameConfigs.HintOnNewLevel;
            }

            if (fromContinue)
            {
                if (_remainingHints <= 0) _remainingHints = 0;
            }
            else
            {
                if (_remainingHints <= 0) _remainingHints = GlobalGameConfigs.HintOnNewLevel;
            }

            Signals.Get<HintCountUpdated>().Dispatch(_remainingHints);
        }

        private void OnHintUsed()
        {
            Signals.Get<HintCountUpdated>().Dispatch(_remainingHints);
            SaveManager.SaveHintCount(_remainingHints);
        }
    }
}
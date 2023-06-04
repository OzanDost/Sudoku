
using System;
using System.Collections.Generic;
using Data;
using deVoid.Utils;
using Game;
using Game.Managers;
using Managers;
using UI.Windows;
using UnityEngine;

/// <summary>
/// First param is old state, second is new state
/// </summary>
public class GameStateChanged : ASignal<GameState, GameState>{}
public class RequestGameStateChange : ASignal<GameState>{}

//the bool is for telling whether the game is loaded from a save or not
public class LevelLoaded : ASignal<LevelData,bool>{}
public class LevelBoardConfigured : ASignal<LevelData>{}
public class LevelFailed : ASignal{}
public class LevelSuccess: ASignal<LevelSuccessData>{}
public class LevelQuit : ASignal{}
public class LevelRetryRequested : ASignal{}


public class UndoableActionMade : ASignal<UndoableAction>{}
public class UndoRequested : ASignal{}
public class EraseRequested : ASignal{}
public class NoteModeToggleRequested : ASignal{}
public class HintButtonClicked : ASignal{}
public class HintRequested : ASignal<Cell>{}
public class HintCountUpdated : ASignal<int>{}
public class HintUsed : ASignal{}
public class HintAuthorized : ASignal<Cell>{}

public class ColorizationListDispatched : ASignal<HashSet<Vector2Int>>{}
public class SameNumberListDispatched : ASignal<List<Vector2Int>>{}
public class LevelContinued : ASignal<BoardStateSaveData>{}
public class WrongNumberPlaced: ASignal<Cell>{}


public class BoardStateSaveRequested : ASignal<LevelData>{}
public class BoardInfoUpdated: ASignal<TimeSpan, int,int>{}

public class ReturnToMenuRequested : ASignal{}


//bool is for showing or not showing the pause popup
public class GamePaused : ASignal<bool>{}
public class GameUnpaused : ASignal{}
public class PausePopupClosed : ASignal{}
public class PausePopupRequested : ASignal{}


#region UI Signals
public class FakeLoadingFinished : ASignal{}
public class NewGameButtonClicked : ASignal{}
public class LevelDifficultySelected : ASignal<LevelDifficulty>{}
public class ContinueLevelRequested : ASignal{}
public class BoardGridCreationRequested : ASignal<RectTransform>{}
public class SuccessContinueButtonClicked : ASignal{}


//Action is for the action to be executed after the popup is closed (not prematurely)
public class RewardedPopupRequested : ASignal<Action>{}
#endregion

public class CellPointerDown : ASignal<Vector2Int>{}
public class CellPointerUp : ASignal<Vector2Int>{}

public class CellFilled : ASignal<Cell>{}
public class NumberButtonClicked : ASignal<int>{}
public class NumberInputMade : ASignal<int, NumberInputMode>{}
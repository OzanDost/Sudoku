
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
public class LevelLoaded : ASignal<LevelData>{}
public class LevelBoardConfigured : ASignal<LevelData>{}
public class LevelFailed : ASignal{}
public class LevelSuccess: ASignal<LevelSuccessData>{}
public class LevelQuit : ASignal{}
public class LevelRetryRequested : ASignal{}


public class UndoableActionMade : ASignal<UndoableAction>{}
public class UndoRequested : ASignal{}
public class EraseRequested : ASignal{}
public class NoteModeToggleRequested : ASignal{}
public class HintRequested : ASignal{}

public class ColorizationListDispatched : ASignal<HashSet<Vector2Int>>{}
public class SameNumberListDispatched : ASignal<List<Vector2Int>>{}
public class LevelContinued : ASignal<BoardSaveStateData>{}
public class WrongNumberPlaced: ASignal<Cell>{}

public class BoardStateSaveRequested : ASignal<LevelData>{}
public class BoardInfoUpdated: ASignal<TimeSpan, int,int>{}

public class ReturnToMenuRequested : ASignal{}


//bool is for showing or not showing the pause popup
public class GamePaused : ASignal<bool>{}

public class GameUnpaused : ASignal{}

public class PausePopupRequested : ASignal{}


#region UI Signals
public class FakeLoadingFinished : ASignal{}
public class PlayLevelRequested : ASignal{}
public class ContinueLevelRequested : ASignal{}
public class BoardGridCreationRequested : ASignal<RectTransform>{}
public class SuccessContinueButtonClicked : ASignal{}
#endregion

public class CellPointerDown : ASignal<Vector2Int>{}
public class CellPointerUp : ASignal<Vector2Int>{}

public class CellFilled : ASignal<Cell>{}
public class NumberButtonClicked : ASignal<int>{}
public class NumberInputMade : ASignal<int, NumberInputMode>{}
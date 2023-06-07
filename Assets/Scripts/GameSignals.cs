using System;
using System.Collections.Generic;
using Data;
using Game;
using Game.Managers;
using ThirdParty;
using UI.Data;
using UI.Enums;
using UnityEngine;

/// <summary>
/// First param is old state, second is new state
/// </summary>
public class GameStateChanged : ASignal<GameState, GameState>{}
public class RequestGameStateChange : ASignal<GameState>{}

public class CellsConfigured : ASignal<Cell[,]>{}

//the bool is for telling whether the game is loaded from a save or not
public class LevelLoaded : ASignal<LevelData,bool>{}
public class BoardReady : ASignal<LevelData, bool>{}
public class LevelBoardConfiguredForUI : ASignal<LevelData>{}
public class LevelContinued : ASignal<BoardStateSaveData>{}
public class LevelFailed : ASignal{}
public class LevelSuccess: ASignal<LevelSuccessData>{}
public class LevelQuit : ASignal{}
public class LevelRetryRequested : ASignal{}

public class BoardFilledSuccessfully : ASignal<LevelData>{}

//Undo
public class UndoableActionMade : ASignal<UndoableAction>{}
public class UndoRequested : ASignal{}
public class UndoResponseSent : ASignal<bool>{}

//Cell Operations
public class EraseButtonClicked : ASignal{}
public class EraseRequested : ASignal<Cell>{}
public class CellEraseResponseSent : ASignal<bool, Cell>{}
public class NoteModeToggleRequested : ASignal{}
public class WrongNumberPlaced: ASignal<Cell, bool>{}
public class CellPointerDown : ASignal<Vector2Int>{}
public class CellPointerUp : ASignal<Vector2Int>{}

//bool is for telling whether the cell is filled by player or not
public class CellFilled : ASignal<Cell,bool>{}
public class NoteUpdatedOnCell : ASignal<Cell, int>{}



//Hint Operations
public class HintButtonClicked : ASignal{}
public class HintRequested : ASignal<Cell>{}
public class HintCountUpdated : ASignal<int>{}
public class HintUsed : ASignal{}
public class HintAuthorized : ASignal<Cell>{}


//Board Animation and Colorization Operations
public class TapColorizationListDispatched : ASignal<ColorizationData, Vector2Int>{}
public class SameNumberListDispatched : ASignal<List<Vector2Int>>{}
public class ElementsFilled : ASignal<List<Vector2Int>, Vector2Int>{}
public class CellColorResetRequested : ASignal<Cell>{}


public class BoardStateDispatched : ASignal<BoardStateSaveData>{}
public class BoardStateSaveRequested : ASignal<LevelData>{}
public class BoardInfoUpdated: ASignal<TimeSpan, int,int>{}
public class ReturnToMenuRequested : ASignal{}


//Pause / Unpause Operations
//bool is for showing or not showing the pause popup

public class GamePaused : ASignal<bool>{}

public class GameUnpaused : ASignal{}

public class PausePopupClosed : ASignal{}

public class PausePopupRequested : ASignal{}


//UI 

public class FakeLoadingFinished : ASignal{}
public class NewGameButtonClicked : ASignal{}
public class LevelDifficultySelected : ASignal<LevelDifficulty>{}
public class ContinueLevelRequested : ASignal{}
public class BoardGridCreationRequested : ASignal<RectTransform>{}
public class SuccessContinueButtonClicked : ASignal{}

//First Action is for the action to be executed after the popup is closed (not prematurely),
//second is for closing the popup prematurely
public class RewardedPopupRequested : ASignal<Action, Action>{}


//List is for all filled cells, Vector2Int is for the main cell that is filled
public class NumberButtonClicked : ASignal<int>{}
public class NumberInputMade : ASignal<int, NumberInputMode>{}


public class ScoreCheckRequested : ASignal<int[,], Cell, LevelDifficulty>{}
public class ScoreUpdated : ASignal<int, bool>{}



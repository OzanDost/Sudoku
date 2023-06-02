
using Data;
using deVoid.Utils;
using Managers;
using UnityEngine;

/// <summary>
/// First param is old state, second is new state
/// </summary>
public class GameStateChanged : ASignal<GameState, GameState>{}
public class RequestGameStateChange : ASignal<GameState>{}
public class LevelLoaded : ASignal<LevelData>{}
public class LevelBoardConfigured : ASignal<LevelData>{}





#region UI Signals
public class FakeLoadingFinished : ASignal{}
public class PlayButtonClicked : ASignal{}
public class BoardGridCreationRequested : ASignal<RectTransform>{}
#endregion

public class CellPointerDown : ASignal<Vector2Int>{}
public class NumberButtonClicked : ASignal<int>{}
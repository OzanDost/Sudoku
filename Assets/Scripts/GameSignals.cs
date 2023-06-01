
using deVoid.Utils;
using Managers;

/// <summary>
/// First param is old state, second is new state
/// </summary>
public class GameStateChanged : ASignal<GameState, GameState>{}

public class RequestGameStateChange : ASignal<GameState>{}
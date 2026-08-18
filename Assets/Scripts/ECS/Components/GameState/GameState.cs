using Unity.Entities;

public enum GameStateType : byte
{
    Playing,
    GameOver,
    Win,
    Paused
}

public struct GameState : IComponentData
{
    public GameStateType CurrentState;
}

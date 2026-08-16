using Unity.Entities;

public enum GameStateType : byte
{
    Playing,
    GameOver,
    Paused
}

public struct GameState : IComponentData
{
    public GameStateType CurrentState;
}

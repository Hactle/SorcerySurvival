using Unity.Entities;

public struct GameStateChangedEvent : IComponentData
{
    public GameStateType PreviousState;
    public GameStateType NewState;
}

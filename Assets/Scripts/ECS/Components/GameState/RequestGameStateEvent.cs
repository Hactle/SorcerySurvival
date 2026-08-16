using Unity.Entities;

public struct RequestGameStateEvent : IComponentData
{
    public GameStateType NewState;
}

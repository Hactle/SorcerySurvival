using Unity.Entities;

[UpdateInGroup(typeof(EventResponseGroup))]
[UpdateBefore(typeof(GameplayGroupControlSystem))]
partial struct PauseGameSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<GameStateChangedEvent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var gameStateEvent in SystemAPI.Query<RefRO<GameStateChangedEvent>>())
        {
            if (gameStateEvent.ValueRO.PreviousState == GameStateType.Playing &&
            gameStateEvent.ValueRO.NewState == GameStateType.Paused)
            {
                GameStateUIController.Instance.ShowPause();
            }
            else if (gameStateEvent.ValueRO.PreviousState == GameStateType.Paused &&
            gameStateEvent.ValueRO.NewState == GameStateType.Playing)
            {
                GameStateUIController.Instance.HidePause();
            }
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

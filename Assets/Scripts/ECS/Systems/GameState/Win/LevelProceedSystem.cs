using Unity.Entities;

[UpdateInGroup(typeof(EventResponseGroup))]
[UpdateBefore(typeof(GameplayGroupControlSystem))]
partial struct LevelProceedSystem : ISystem
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
            if (gameStateEvent.ValueRO.NewState == GameStateType.Win)
            {
                PanelsUIController.Instance.ShowWin();
            }
        }
    }
}

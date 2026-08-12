using Unity.Entities;

[UpdateInGroup(typeof(EventResponseGroup))]
partial struct GameOverUISystem : ISystem
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
            if(gameStateEvent.ValueRO.PreviousState == GameStateType.Playing &&
            gameStateEvent.ValueRO.NewState == GameStateType.GameOver)
            {
                GameOverUI.Instance.Show();
            }
        }
    }
}

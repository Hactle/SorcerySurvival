using Unity.Entities;

[UpdateInGroup(typeof(EventResponseGroup))]
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
                var gameplayGroup =
                state.World.GetExistingSystemManaged<GamePlaySystemGroup>();
                gameplayGroup.Enabled = false;

                PauseUI.Instance.Show();
            }
            else if (gameStateEvent.ValueRO.PreviousState == GameStateType.Paused &&
            gameStateEvent.ValueRO.NewState == GameStateType.Playing)
            {
                var gameplayGroup =
                state.World.GetExistingSystemManaged<GamePlaySystemGroup>();
                gameplayGroup.Enabled = true;

                PauseUI.Instance.Hide();
            }
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

using Unity.Entities;

[UpdateInGroup(typeof(EventResponseGroup))]
partial struct GameOverStateSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameStateChangedEvent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var gameStateEvent in SystemAPI.Query<RefRO<GameStateChangedEvent>>())
        {
            if(gameStateEvent.ValueRO.PreviousState == GameStateType.Playing &&
            gameStateEvent.ValueRO.NewState == GameStateType.GameOver)
            {
                var gameplayGroup =
                state.World.GetExistingSystemManaged<GamePlaySystemGroup>();

                gameplayGroup.Enabled = false;
            }
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

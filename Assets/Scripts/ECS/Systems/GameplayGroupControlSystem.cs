using Unity.Entities;

[UpdateInGroup(typeof(EventResponseGroup))]
partial struct GameplayGroupControlSystem : ISystem
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
            if (gameStateEvent.ValueRO.PreviousState == GameStateType.Playing)
            {
                var gameplayGroup =
                state.World.GetExistingSystemManaged<GamePlaySystemGroup>();

                gameplayGroup.Enabled = false;

            }

            if (gameStateEvent.ValueRO.NewState == GameStateType.Playing)
            {
                var gameplayGroup =
                state.World.GetExistingSystemManaged<GamePlaySystemGroup>();
                gameplayGroup.Enabled = true;
            }
        }
    }
}

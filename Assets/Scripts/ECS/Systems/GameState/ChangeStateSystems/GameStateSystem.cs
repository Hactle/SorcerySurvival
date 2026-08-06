using Unity.Entities;

[UpdateInGroup(typeof(EventRequestGroup))]
partial struct GameStateSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RequestGameStateEvent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        RefRW<GameState> gameState = SystemAPI.GetSingletonRW<GameState>();

        foreach (var request
                    in SystemAPI.Query<RefRO<RequestGameStateEvent>>())
        {
            GameStateType previous = gameState.ValueRO.CurrentState;
            GameStateType next = request.ValueRO.NewState;

            if (previous == next)
                continue;

            gameState.ValueRW.CurrentState = next;

            Entity eventEntity = ecb.CreateEntity();

            ecb.AddComponent<EventTag>(eventEntity);

            ecb.AddComponent(eventEntity, new GameStateChangedEvent
            {
                PreviousState = previous,
                NewState = next
            });
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

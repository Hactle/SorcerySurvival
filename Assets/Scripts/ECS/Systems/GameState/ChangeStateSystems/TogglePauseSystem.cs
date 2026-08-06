using Unity.Entities;

[UpdateInGroup(typeof(EventResponseGroup))]
partial struct TogglePauseSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RequestTogglePauseEvent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        RefRW<GameState> gameState = SystemAPI.GetSingletonRW<GameState>();

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        GameStateType previous = gameState.ValueRO.CurrentState;

        GameStateType next = previous;

        switch (previous)
        {
            case GameStateType.Playing:
                next = GameStateType.Paused;
                break;

            case GameStateType.Paused:
                next = GameStateType.Playing;
                break;
            default:
                return;
        }

        gameState.ValueRW.CurrentState = next;
        
        Entity eventEntity = ecb.CreateEntity();

        ecb.AddComponent<EventTag>(eventEntity);

        ecb.AddComponent(eventEntity, new GameStateChangedEvent
        {
            PreviousState = previous,
            NewState = next
        });

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

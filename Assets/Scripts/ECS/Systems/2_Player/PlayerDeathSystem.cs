using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(GamePlaySystemGroup))]
partial struct PlayerDeathSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<PlayerTag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var health in
                SystemAPI.Query<RefRO<Health>>()
                         .WithAll<PlayerTag>())
        {
            if (health.ValueRO.Value > 0)
                continue;

            Entity eventEntity = ecb.CreateEntity();

            ecb.AddComponent<EventTag>(eventEntity);
            ecb.AddComponent(eventEntity, new RequestGameStateEvent()
            {
                NewState = GameStateType.GameOver
            });
            break;
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

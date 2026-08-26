using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(AbilityCleanupGroup))]
partial struct ProjectilesCleanUpSystem : ISystem
{   
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (pierce, entity) in SystemAPI.Query<
                        RefRO<Pierce>>()
                        .WithEntityAccess()
                        .WithAll<ProjectileTag>())
        {
            if (pierce.ValueRO.Value == 0)
                ecb.DestroyEntity(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}

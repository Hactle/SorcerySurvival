using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(AbilityCleanupGroup))]
partial struct ProjectilesCleanUpSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (destroyTag, entity) in SystemAPI.Query<
                        RefRO<DestroyTag>>()
                        .WithEntityAccess()
                        .WithAll<ProjectileTag>())
        {
            ecb.DestroyEntity(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}

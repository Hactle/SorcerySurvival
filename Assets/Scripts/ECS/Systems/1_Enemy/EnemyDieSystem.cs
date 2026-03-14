using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

partial struct EnemyDieSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach(var (
            deathEvent,
            entity) in SystemAPI.Query<
                    RefRW<DeathEvent>>()
                    .WithEntityAccess()
                    .WithAll<EnemyTag>())
        {
            ecb.DestroyEntity(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}

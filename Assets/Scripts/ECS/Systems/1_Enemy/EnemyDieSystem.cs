using Unity.Entities;
using Unity.Collections;

[UpdateInGroup(typeof(AbilityCleanupGroup))]
partial struct EnemyDieSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (
            enemy,
            entity) in SystemAPI.Query<
                RefRO<EnemyTag>>()
                .WithEntityAccess()
                .WithAll<DestroyTag>())
        {
            foreach (var waveState in SystemAPI.Query<RefRW<WaveState>>())
            {
                waveState.ValueRW.EnemiesRemaining--;
            }

            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}

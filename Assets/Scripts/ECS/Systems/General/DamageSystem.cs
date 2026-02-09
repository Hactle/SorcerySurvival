using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
partial struct DamageSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            health,
            damage,
            entity) in SystemAPI.Query<
                RefRW<Health>,
                RefRO<Damage>>().WithEntityAccess())
        {
            if (SystemAPI.HasComponent<PlayerTag>(entity) &&
                SystemAPI.HasComponent<Invincibility>(entity))
            {
                ecb.RemoveComponent<Damage>(entity);
                continue;
            }

            health.ValueRW.Value = math.max(
                health.ValueRW.Value - damage.ValueRO.Value,
                0);

            if (SystemAPI.HasComponent<PlayerTag>(entity))
            {
                ecb.AddComponent(entity, new Invincibility
                {
                    TimeLeft = 1.5f
                });

            }
            ecb.RemoveComponent<Damage>(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}
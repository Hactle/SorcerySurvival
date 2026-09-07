using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(DamageApplyGroup))]
partial struct DamageApplySystem : ISystem
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

        foreach (var (
            health,
            damageEvents,
            entity) in SystemAPI.Query<
                RefRW<Health>,
                DynamicBuffer<DamageEvent>>()
                .WithEntityAccess())
        {
            if (damageEvents.Length == 0)
                continue;

            float total = 0f;

            for (int i = 0; i < damageEvents.Length; i++)
                total += damageEvents[i].Value;

            health.ValueRW.Value = math.max(health.ValueRW.Value - total, 0);

            damageEvents.Clear();

            ecb.AddComponent(entity, new DamageAppliedEvent { Value = total });
        }

        ecb.Playback(state.EntityManager);
    }
}
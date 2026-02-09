using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemyAttackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var player))
            return;

        float3 playerPosition =
            SystemAPI.GetComponentRO<LocalTransform>(player).ValueRO.Position;

        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            transform,
            damage) in
                 SystemAPI.Query
                 <RefRO<LocalTransform>,
                 RefRO<AttackDamage>>().WithAll<EnemyTag>())
        {
            float distance =
                math.distance(transform.ValueRO.Position, playerPosition);

            if (distance <= 1f)
            {
                ecb.AddComponent(player, new Damage
                {
                    Value = damage.ValueRO.Value
                });
            }
        }

        ecb.Playback(state.EntityManager);
    }
}


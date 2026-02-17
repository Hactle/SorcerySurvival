using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemyToPlayerCollisionSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var player))
            return;

        float3 playerPosition =
            SystemAPI.GetComponentRO<LocalTransform>(player).ValueRO.Position;
        float playerRadius = 
            SystemAPI.GetComponentRO<CollisionRadius>(player).ValueRO.Value;

        foreach (var (
            transform,
            radius) in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<CollisionRadius>>()
                    .WithAll<EnemyTag>())
        {
            float3 difference = transform.ValueRO.Position - playerPosition;
            float distanceSq = math.lengthsq(difference);

            float minDistance = radius.ValueRO.Value + playerRadius;

            if (distanceSq < minDistance * minDistance && distanceSq > 0.0001f)
            {
                float distance = math.sqrt(distanceSq);
                float3 normal = difference / distance;

                float penetration = minDistance - distance;

                transform.ValueRW.Position += normal * penetration;

            }
        }
    }
}

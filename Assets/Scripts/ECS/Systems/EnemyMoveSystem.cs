using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemyMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
            return;

        float3 playerPosition = SystemAPI.GetComponentRO<LocalTransform>(playerEntity).ValueRO.Position;

        EntityQuery enemyQuery = SystemAPI.QueryBuilder()
            .WithAll
            <EnemyTag,
            LocalTransform>()
                .Build();

        var allTransforms = enemyQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

        foreach (var (
            transform,
            moveSpeed,
            animationIndex,
            facingDirection,
            sepStrength,
            sepRadius) in
                        SystemAPI.Query
                        <RefRW<LocalTransform>,
                        RefRO<MoveSpeed>,
                        RefRW<AnimationIndexOverride>,
                        RefRW<FacingDirectionOverride>,
                        RefRO<SeparationStrenght>,
                        RefRO<SeparationRadius>>()
                        .WithAll<EnemyTag>())
        {
            float3 selfPosition = transform.ValueRO.Position;

            float3 separationForce = float3.zero;

            for (int i = 0; i < allTransforms.Length; i++)
            {
                float3 otherPosition = allTransforms[i].Position;

                if (math.all(otherPosition == selfPosition)) continue;

                float3 difference = selfPosition - otherPosition;
                float distanceSq = math.lengthsq(difference);

                if (distanceSq < sepRadius.ValueRO.Value * sepRadius.ValueRO.Value)
                {
                    separationForce += math.normalize(difference) / math.sqrt(distanceSq);
                }
            }

            float3 directionToPlayer = playerPosition - selfPosition;
            directionToPlayer = math.normalize(directionToPlayer);

            float3 finalDirection = directionToPlayer + separationForce * sepStrength.ValueRO.Value;
            finalDirection = math.normalize(finalDirection);

            transform.ValueRW.Position += deltaTime * moveSpeed.ValueRO.Value * finalDirection;

            #region Animation
            if (math.abs(directionToPlayer.y) > math.abs(directionToPlayer.x))
            {
                animationIndex.ValueRW.Value = directionToPlayer.y > 0
                    ? (float)EnemyAnimationIndex.WalkUp
                    : (float)EnemyAnimationIndex.WalkDown;
            }
            else
            {
                animationIndex.ValueRW.Value = (float)EnemyAnimationIndex.WalkSide;
            }

            if (math.abs(directionToPlayer.x) > 0.1f) facingDirection.ValueRW.Value = math.sign(directionToPlayer.x);
            #endregion
        }

        allTransforms.Dispose();
    }
}


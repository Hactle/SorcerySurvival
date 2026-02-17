using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;


[BurstCompile]
public partial struct EnemyMoveJob : IJobEntity
{
    public float DeltaTime;
    public float3 PlayerPosition;

    [ReadOnly]
    public NativeParallelMultiHashMap<int, float3> HashMap;

    public void Execute(
        ref LocalTransform transform,
        in MoveSpeed moveSpeed,
        in SpatialHashCell cell,
        ref AnimationIndexOverride animationIndex,
        ref FacingDirectionOverride facingDirection,
        in SeparationStrenght separationStrenght,
        in SeparationRadius separationRadius)
    {
        float3 selfPosition = transform.Position;
        float3 separation = float3.zero;

        float radiusSq = separationRadius.Value * separationRadius.Value;
        int2 selfCell = cell.Value;

        for(int x = -1; x <= 1; x++)
        for(int y = -1; y <= 1; y++)
        {
            int2 neighbour = selfCell + new int2(x, y);
            int hash = Hash(neighbour);

            if (HashMap.TryGetFirstValue(hash, out float3 other, out var it))
                do
                {
                        float3 difference = selfPosition - other;
                        float distanceSq = math.lengthsq(difference);

                        if (distanceSq > 0.0001f && distanceSq < radiusSq)
                        {
                            float distance = math.sqrt(distanceSq);
                            float3 pushDir = difference / distance;

                            float strength = 1f - (distance / separationRadius.Value);

                            separation += pushDir * strength;
                        }
                    }
                while (HashMap.TryGetNextValue(out other, ref it));

        }

        float3 toPlayer = PlayerPosition - selfPosition;
        float toPlayerLenSq = math.lengthsq(toPlayer);

        float3 direction =
            toPlayerLenSq > 0.0001f
                ? toPlayer / math.sqrt(toPlayerLenSq)
                : float3.zero;

        float3 combined = direction + separation * separationStrenght.Value;
        float combinedLenSq = math.lengthsq(combined);

        if (combinedLenSq > 0.0001f)
        {
            float3 finalDirection = combined / math.sqrt(combinedLenSq);
            transform.Position += finalDirection * moveSpeed.Value * DeltaTime;
        }         

        #region Animation
        float absX = math.abs(direction.x);
        float absY = math.abs(direction.y);

        if (absY > absX)
        {
            animationIndex.Value = direction.y > 0f
                ? (float)EnemyAnimationIndex.WalkUp
                : (float)EnemyAnimationIndex.WalkDown;
        }
        else
        {
            animationIndex.Value = (float)EnemyAnimationIndex.WalkSide;
        }

        if (absX > 0.01f)
            facingDirection.Value = math.sign(direction.x);
        #endregion

    }

    static int Hash(int2 cell)
    {
        return cell.x * 73856093 ^ cell.y * 19349663;
    }
}

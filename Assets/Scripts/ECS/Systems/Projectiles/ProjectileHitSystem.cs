using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(AbilityExecuteGroup))]
[BurstCompile]
partial struct ProjectileHitSystem : ISystem
{
    const float HitRadius = 0.25f;

    public void OnUpdate(ref SystemState state)
    {
        var map = SystemAPI.GetSingleton<SpatialHashMapSingleton>().Map;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (
            transform,
            damage)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO<Damage>>()
            .WithAll<ProjectileTag>())
        {
            float3 position = transform.ValueRO.Position;

            int2 cell = new(
                (int)math.floor(position.x / 1.5f),
                (int)math.floor(position.z / 1.5f));

            for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                {
                    int2 neighbour = cell + new int2(x, y);
                    int hash = Hash(neighbour);

                    if(!map.TryGetFirstValue(hash, out Entity enemy, out var it))
                        continue;

                    do
                    {
                        var enemyTransform = 
                            SystemAPI.GetComponentRO<LocalTransform>(enemy);

                        float distance = math.distancesq(
                            position,
                            enemyTransform.ValueRO.Position);

                        if (distance < HitRadius * HitRadius)
                        {
                            ecb.AddComponent(enemy, new DamageEvent
                            {
                                Value = damage.ValueRO.Value
                            });
                            break;
                        }
                    }while(map.TryGetNextValue(out enemy, ref it));
                }
        }
        ecb.Playback(state.EntityManager);
    }
    static int Hash(int2 cell)
    {
        return cell.x * 73856093 ^ cell.y * 19349663;
    }
}

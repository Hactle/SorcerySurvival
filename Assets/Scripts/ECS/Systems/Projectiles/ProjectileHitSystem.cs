using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(AbilityExecuteGroup))]
partial struct ProjectileHitSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        const float CellSize = 1.5f;

        var map = SystemAPI.GetSingleton<SpatialHashMapSingleton>().Map;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        transformLookup.Update(ref state);

        foreach (var (
            transform,
            damage,
            hitRadius,
            pierce,
            damagedBuffer,
            entitySide,
            entity)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO<Damage>,
                RefRO<HitRadius>,
                RefRW<Pierce>,
                DynamicBuffer<DamagedEntity>,
                RefRO<EntitySide>>()
            .WithAll<ProjectileTag>()
            .WithNone<DestroyTag>()
            .WithEntityAccess())
        {
            if (entitySide.ValueRO.Value == Sides.Player)
            {
                float3 position = transform.ValueRO.Position;

                float hitRadiusSq = hitRadius.ValueRO.Value * hitRadius.ValueRO.Value;

                int2 cell = new(
                    (int)math.floor(position.x / CellSize),
                    (int)math.floor(position.y / CellSize));

                int range = (int)math.ceil(hitRadius.ValueRO.Value / CellSize);

                bool hitSomething = false;

                for (int x = -range; x <= range && !hitSomething; x++)
                    for (int y = -range; y <= range && !hitSomething; y++)
                    {
                        int2 neighbour = cell + new int2(x, y);
                        int hash = Hash(neighbour);

                        if (!map.TryGetFirstValue(hash, out Entity enemy, out var it))
                            continue;

                        do
                        {
                            if (!transformLookup.HasComponent(enemy))
                                continue;

                            float3 enemyPos = transformLookup[enemy].Position;

                            float distSq = math.distancesq(position, enemyPos);

                            if (distSq < hitRadiusSq)
                            {
                                bool alreadyHit = false;

                                for (int i = 0; i < damagedBuffer.Length; i++)
                                {
                                    if (damagedBuffer[i].Value == enemy)
                                    {
                                        alreadyHit = true;
                                        break;
                                    }
                                }

                                if (alreadyHit)
                                    continue;

                                ecb.AddComponent(enemy, new DamageEvent
                                {
                                    Value = damage.ValueRO.Value
                                });

                                damagedBuffer.Add(new DamagedEntity
                                {
                                    Value = enemy
                                });

                                pierce.ValueRW.Value = math.max(0, pierce.ValueRW.Value - 1);

                                hitSomething = true;
                                break;
                            }

                        } while (map.TryGetNextValue(out enemy, ref it));
                    }
            }
            else if (entitySide.ValueRO.Value == Sides.Enemy)
            {
                var player = SystemAPI.GetSingletonEntity<PlayerTag>();

                if (!transformLookup.HasComponent(player))
                    continue;

                float3 position = transform.ValueRO.Position;
                float3 playerPos = transformLookup[player].Position;

                float hitRadiusSq = hitRadius.ValueRO.Value * hitRadius.ValueRO.Value;
                float distanceSq = math.distancesq(position, playerPos);

                if(distanceSq < hitRadiusSq)
                {
                    bool alredyHit = false;

                    for (int i = 0; i < damagedBuffer.Length; i++)
                    {
                        if (damagedBuffer[i].Value == player)
                        {
                            alredyHit = true;
                            break;
                        }
                    }

                    if (alredyHit)
                        continue;

                    ecb.AddComponent(player, new DamageEvent
                    {
                        Value = damage.ValueRO.Value
                    });

                    damagedBuffer.Add(new DamagedEntity
                    {
                        Value = player
                    });

                    pierce.ValueRW.Value = math.max(0, pierce.ValueRW.Value - 1);

                    if (pierce.ValueRW.Value == 0)
                        ecb.AddComponent<DestroyTag>(entity);
                }
            }       
        }
        ecb.Playback(state.EntityManager);

        static int Hash(int2 cell)
        {
            return cell.x * 73856093 ^ cell.y * 19349663;
        }
    }
}

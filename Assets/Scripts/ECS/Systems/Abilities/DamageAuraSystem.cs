using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(AbilityExecuteGroup))]
partial struct DamageAuraSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        const float CellSize = 1.5f;
        float dt = SystemAPI.Time.DeltaTime;

        var map = SystemAPI.GetSingleton<SpatialHashMapSingleton>().Map;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        transformLookup.Update(ref state);

        var sideLookup = SystemAPI.GetComponentLookup<EntitySide>(true);
        sideLookup.Update(ref state);

        foreach (var (
            tick,
            radius,
            damage,
            entityOwner) in 
                    SystemAPI.Query<
                        RefRW<DamageTick>,
                        RefRO<DamageRadius>,
                        RefRO<Damage>,
                        RefRO<AbilityOwner>>())
        {
            tick.ValueRW.Timer -= dt;

            if (tick.ValueRW.Timer > 0f)
                continue;

            tick.ValueRW.Timer += tick.ValueRO.Interval;

            var ownerEntity = entityOwner.ValueRO.Owner;

            if (!transformLookup.HasComponent(ownerEntity)
                || !sideLookup.HasComponent(ownerEntity))
                continue;

            float3 ownerPosition = transformLookup[ownerEntity].Position;
            Sides ownerSide = sideLookup[ownerEntity].Value;
            float radiusSq = radius.ValueRO.Value * radius.ValueRO.Value;

            int2 centerCell = new int2(
                (int)math.floor(ownerPosition.x / CellSize),
                (int)math.floor(ownerPosition.y / CellSize));

            int range = (int)math.ceil(radius.ValueRO.Value / CellSize);

            for (int x = -range; x <= range; x++)
                for (int y = -range; y <= range; y++)
                {
                    int2 neighbour = centerCell + new int2(x, y);
                    int hash = Hash(neighbour);

                    if (!map.TryGetFirstValue(hash, out Entity target, out var it))
                        continue;

                    do
                    {
                        if (!transformLookup.HasComponent(target) || !sideLookup.HasComponent(target))
                            continue;

                        if (sideLookup[target].Value == ownerSide)
                            continue;

                        float distSq = math.distancesq(ownerPosition, transformLookup[target].Position);

                        if (distSq < radiusSq)
                        {
                            ecb.AppendToBuffer(target, new DamageEvent { Value = damage.ValueRO.Value });
                        }
                    } while (map.TryGetNextValue(out target, ref it));
                }


        }

        ecb.Playback(state.EntityManager);            

            static int Hash(int2 cell)
        {
            return cell.x * 73856093 ^ cell.y * 19349663;
        }
    }
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(SpatialHashBuildSystem))]
[BurstCompile]
public partial struct SpatialHashMapSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();

        var map = new NativeParallelMultiHashMap<int, Entity>(1024, Allocator.Persistent);

        var entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(entity, new SpatialHashMapSingleton
        {
            Map = map
        });
    }

    public void OnUpdate(ref SystemState state)
    {
        var singleton = SystemAPI.GetSingletonRW<SpatialHashMapSingleton>();
        var map = singleton.ValueRW.Map;

        map.Clear();

        foreach (var (cell, entity) in
                 SystemAPI.Query<RefRO<SpatialHashCell>>()
                          .WithEntityAccess()
                          .WithAll<EnemyTag>())
        {
            int hash = Hash(cell.ValueRO.Value);
            map.Add(hash, entity);
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        var map = SystemAPI.GetSingleton<SpatialHashMapSingleton>().Map;
        if (map.IsCreated)
            map.Dispose();
    }

    static int Hash(int2 cell)
    {
        return cell.x * 73856093 ^ cell.y * 19349663;
    }
}

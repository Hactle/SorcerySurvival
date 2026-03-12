using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(SpatialHashBuildSystem))]
[UpdateBefore(typeof(EnemyMoveSystem))]
[BurstCompile]
public partial struct SpatialHashMapSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var map = new NativeParallelMultiHashMap<int, Entity>(1024, Allocator.Persistent);

        var entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(entity, new SpatialHashMapSingleton
        {
            Map = map
        });
    }

    public void OnDestroy(ref SystemState state)
    {
        var map = SystemAPI.GetSingleton<SpatialHashMapSingleton>().Map;
        if (map.IsCreated)
            map.Dispose();
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

    static int Hash(int2 cell)
    {
        return cell.x * 73856093 ^ cell.y * 19349663;
    }
}

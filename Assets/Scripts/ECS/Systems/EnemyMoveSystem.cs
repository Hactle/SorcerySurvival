using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(SpatialHashMapSystem))]
public partial struct EnemyMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var player))
            return;

        float3 playerPosition =
            SystemAPI.GetComponentRO<LocalTransform>(player).ValueRO.Position;

        var hashMap = SystemAPI.GetSingleton<SpatialHashMapSingleton>().Map;

        var job = new EnemyMoveJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = playerPosition,
            HashMap = hashMap
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}


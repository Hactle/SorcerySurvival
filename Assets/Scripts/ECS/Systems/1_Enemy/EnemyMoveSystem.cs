using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


[UpdateInGroup(typeof(GamePlaySystemGroup))]
public partial struct EnemyMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var player))
            return;

        float3 playerPosition =
            SystemAPI.GetComponentRO<LocalTransform>(player).ValueRO.Position;

        var hashMap = SystemAPI.GetSingleton<SpatialHashMapSingleton>().Map;

        var lookUp = SystemAPI.GetComponentLookup<WorldPosition>(true);
        lookUp.Update(ref state);

        var job = new EnemyMoveJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = playerPosition,
            HashMap = hashMap,
            PositionLookup = lookUp,
        };
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}


using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(SpatialHashMapSystem))]
public partial struct SpatialHashBuildSystem : ISystem
{
    public const float CellSize = 1.5f;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (
            transform,
            cell) in
                 SystemAPI.Query
                 <RefRO<LocalTransform>,
                 RefRW<SpatialHashCell>>()
                          .WithAll<EnemyTag>())
        {
            float3 pos = transform.ValueRO.Position;

            cell.ValueRW.Value = new int2(
                (int)math.floor(pos.x / CellSize),
                (int)math.floor(pos.y / CellSize)
            );
        }
    }
}
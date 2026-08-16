using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(TransformSystemGroup))]
public partial struct PositionSyncSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, position)
            in SystemAPI.Query<RefRO<LocalTransform>, RefRW<WorldPosition>>())
        {
            position.ValueRW.Value = transform.ValueRO.Position;
        }
    }
}
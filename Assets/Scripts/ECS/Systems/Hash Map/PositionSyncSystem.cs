using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(TransformSystemGroup))]
[BurstCompile]
public partial struct PositionSyncSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, position)
            in SystemAPI.Query<RefRO<LocalTransform>, RefRW<WorldPosition>>())
        {
            position.ValueRW.Value = transform.ValueRO.Position;
        }
    }
}
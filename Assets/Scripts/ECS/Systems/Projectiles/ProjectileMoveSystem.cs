using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(AbilityExecuteGroup))]
partial struct ProjectileMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (
            target,
            speed,
            transform,
            entity) in SystemAPI.Query<
                RefRO<TargetDirection>,
                RefRO<AirSpeed>,
                RefRW<LocalTransform>>()
                .WithEntityAccess()
                .WithAll<ProjectileTag>())
        {
            transform.ValueRW.Position += speed.ValueRO.Value * SystemAPI.Time.DeltaTime * target.ValueRO.Value;
        }
    }
}

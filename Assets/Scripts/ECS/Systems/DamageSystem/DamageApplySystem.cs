using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

public partial class DamagePipelineGroup : ComponentSystemGroup { }


[UpdateInGroup(typeof(DamagePipelineGroup))]
[UpdateAfter(typeof(InvincibilityFilterSystem))]
partial struct DamageApplySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            health,
            damage,
            entity) in SystemAPI.Query<
                RefRW<Health>,
                RefRO<DamageEvent>>().WithEntityAccess())
        {
            health.ValueRW.Value = math.max(
                health.ValueRW.Value - damage.ValueRO.Value,
                0);
            
            ecb.RemoveComponent<DamageEvent>(entity);

            ecb.AddComponent(entity, new DamageAppliedEvent
            {
                Value = damage.ValueRO.Value
            });
        }
        ecb.Playback(state.EntityManager);
    }
}
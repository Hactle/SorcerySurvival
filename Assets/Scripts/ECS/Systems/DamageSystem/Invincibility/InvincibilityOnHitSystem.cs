using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(DamagePostProcessGroup))]
partial struct InvincibilityOnHitSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            canReceiveInvincibility,
            damageApply,
            entity) in SystemAPI.Query<
                RefRO<CanReceiveInvincibility>,
                RefRO<DamageAppliedEvent>>().WithEntityAccess())
        {
            ecb.AddComponent(entity, new InvincibilityTag
            {
                Value = canReceiveInvincibility.ValueRO.Value
            });
            ecb.RemoveComponent<DamageEvent>(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}

using Unity.Burst;
using Unity.Entities;


[UpdateInGroup(typeof(DamagePipelineGroup))]
[UpdateAfter(typeof(InvincibilityOnHitSystem))]
partial struct InvincibilityFilterSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach(var (
            invincibility,
            damageEvent,
            entity
            ) in SystemAPI.Query<
                RefRO<InvincibilityTag>,
                RefRO<DamageEvent>>().WithEntityAccess())
        {
            ecb.RemoveComponent<DamageEvent>(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}

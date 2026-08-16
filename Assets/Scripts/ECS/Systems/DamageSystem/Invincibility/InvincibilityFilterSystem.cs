using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(DamageFilterGroup))]
partial struct InvincibilityFilterSystem : ISystem
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

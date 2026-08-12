using Unity.Entities;

[UpdateInGroup(typeof(DamagePostProcessGroup))]
partial struct DamageAppliedCleanupSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            damageApply,
            entity) in SystemAPI.Query<
                RefRO<DamageAppliedEvent>>().WithEntityAccess())
        {
            ecb.RemoveComponent<DamageAppliedEvent>(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}
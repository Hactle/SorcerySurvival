using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct InvincibilityTimerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            invincibility,
            entity) in SystemAPI.Query<
                RefRW<InvincibilityTag>>().WithEntityAccess())
        {
            invincibility.ValueRW.Value -= deltaTime;
           
            if (invincibility.ValueRW.Value <= 0f)
            {
                ecb.RemoveComponent<InvincibilityTag>(entity);
            }
        }

        ecb.Playback(state.EntityManager);
    }
}

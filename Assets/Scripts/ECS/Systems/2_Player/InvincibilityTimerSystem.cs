using Unity.Burst;
using Unity.Entities;

partial struct InvincibilityTimerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            invincibility,
            entity) in SystemAPI.Query<
            RefRW<Invincibility>>().WithEntityAccess())
        {
            invincibility.ValueRW.TimeLeft -= deltaTime;
           
            if (invincibility.ValueRW.TimeLeft <= 0f)
            {
                ecb.RemoveComponent<Invincibility>(entity);
            }
        }

        ecb.Playback(state.EntityManager);
    }
}

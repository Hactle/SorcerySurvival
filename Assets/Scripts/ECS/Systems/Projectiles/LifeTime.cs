using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

partial struct LifeTimeCounterSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            lifeTime,
            entity) in SystemAPI.Query
                    <RefRW<LifeTime>>()
                    .WithEntityAccess())
        {
            lifeTime.ValueRW.Value = math.max(
                0,
                lifeTime.ValueRW.Value - SystemAPI.Time.DeltaTime);
                
            if (lifeTime.ValueRW.Value == 0f)
            {
                ecb.DestroyEntity(entity);
            }
        }
        ecb.Playback(state.EntityManager);
    }
}

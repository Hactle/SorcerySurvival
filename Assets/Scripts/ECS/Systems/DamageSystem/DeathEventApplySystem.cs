using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

partial struct DeathEventApplySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach(var (
            health,
            entity) in SystemAPI.Query<
                    RefRW<Health>>()
                    .WithEntityAccess())
        {
            if (health.ValueRO.Value == 0)
            {
                ecb.AddComponent<DestroyTag>(entity);
            }
        }
        ecb.Playback(state.EntityManager);
    }
}

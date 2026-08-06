using Unity.Entities;
using Unity.Collections;

partial struct EnemyDieSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (
            enemy,
            entity) in SystemAPI.Query<
                RefRO<EnemyTag>>()
                .WithEntityAccess()
                .WithAll<DestroyTag>())
        {
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}

using Unity.Entities;
using Unity.Collections;

partial struct EnemyDieSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (
            enemy,
            experienceGain,
            entity) in SystemAPI.Query<
                RefRO<EnemyTag>,
                RefRO<ExperienceGain>>()
                .WithEntityAccess()
                .WithAll<DestroyTag>())
        {
            Entity eventEntity = ecb.CreateEntity();

            ecb.AddComponent<EventTag>(eventEntity);
            ecb.AddComponent(eventEntity, new ExperienceGainRequest
            {
                Value = experienceGain.ValueRO.Value
            });

            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}

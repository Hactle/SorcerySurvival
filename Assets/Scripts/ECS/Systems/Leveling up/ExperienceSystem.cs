using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(EventRequestGroup))]
partial struct ExperienceSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<ExperienceGainRequest>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();

        ref var playerExperience = ref SystemAPI.GetComponentRW<ExperiencePool>(playerEntity).ValueRW;

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (experienceGain, entity) in SystemAPI.Query<RefRO<ExperienceGainRequest>>().WithEntityAccess())
        {
            playerExperience.CurrentValue = math.min(playerExperience.CurrentValue + experienceGain.ValueRO.Value, playerExperience.MaxValue);

            if (playerExperience.CurrentValue >= playerExperience.MaxValue)
            {
                playerExperience.CurrentValue = 0;
                playerExperience.MaxValue += 100;

                Entity eventEntity = ecb.CreateEntity();

                ecb.AddComponent<EventTag>(eventEntity);
                ecb.AddComponent<LevelUpRequest>(eventEntity);
            }
        }
        ecb.Playback(state.EntityManager);
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

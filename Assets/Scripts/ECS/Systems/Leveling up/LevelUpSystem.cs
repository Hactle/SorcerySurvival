using Unity.Entities;

[UpdateInGroup(typeof(EventRequestGroup))]
[UpdateAfter(typeof(ExperienceSystem))]
partial struct LevelUpSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<LevelUpRequest>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();

        ref var playerLevel = ref SystemAPI.GetComponentRW<Level>(playerEntity).ValueRW;

        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (levelUp, entity) in SystemAPI.Query<RefRO<LevelUpRequest>>().WithEntityAccess())
        {
            playerLevel.CurrentLevel++;

            UnityEngine.Debug.Log("New level");

            Entity eventEntity = ecb.CreateEntity();

            ecb.AddComponent<EventTag>(eventEntity);
            ecb.AddComponent(eventEntity, new LevelUpEvent
            {
                NewLevel = playerLevel.CurrentLevel
            });
        }
        ecb.Playback(state.EntityManager);
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

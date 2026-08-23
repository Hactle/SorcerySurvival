using Unity.Entities;

[UpdateInGroup(typeof(EventResponseGroup))]
partial struct LevelProgressionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<LevelCompletedEvent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var levelCompletedEvent
                in SystemAPI.Query<RefRO<LevelCompletedEvent>>())
        {
            int levelId = levelCompletedEvent.ValueRO.LevelId;

            bool changed = SaveService.Instance.CompleteLevel(levelId);

            if (changed)
                SaveService.Instance.Save();
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

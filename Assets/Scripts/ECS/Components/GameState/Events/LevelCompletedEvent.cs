using Unity.Entities;

public struct LevelCompletedEvent : IComponentData
{
    public int LevelId;
}

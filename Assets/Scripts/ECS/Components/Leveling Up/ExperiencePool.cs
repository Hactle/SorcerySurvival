using Unity.Entities;

public struct ExperiencePool : IComponentData
{
    public int MaxValue;
    public int CurrentValue;
}

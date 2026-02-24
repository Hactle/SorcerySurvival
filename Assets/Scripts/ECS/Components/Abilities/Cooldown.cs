using Unity.Entities;

public struct Cooldown : IComponentData
{
    public float CurrentValue;
    public float BaseValue;
}

using Unity.Entities;

public struct DamageTick : IComponentData
{
    public float Interval;
    public float Timer;
}

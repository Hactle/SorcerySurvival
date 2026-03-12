using Unity.Entities;

public struct EntitySide : IComponentData
{
    public Sides Value;
}

public enum Sides
{
    Player,
    Enemy
}
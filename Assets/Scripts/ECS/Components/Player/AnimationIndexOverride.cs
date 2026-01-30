using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_AnimationIndex")]
public struct AnimationIndexOverride : IComponentData
{
    public float Value;
}

public enum AnimationIndex : byte
{
    WalkUp = 0,
    IdleUp = 1,
    WalkSide = 2,
    IdleSide = 3,
    WalkDown = 4,
    IdleDown = 5,

    None = byte.MaxValue
}

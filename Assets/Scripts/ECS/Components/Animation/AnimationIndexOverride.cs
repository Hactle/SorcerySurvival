using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_AnimationIndex")]
public struct AnimationIndexOverride : IComponentData
{
    public float Value;
}

public enum PlayerAnimationIndex : byte
{
    WalkUp = 0,
    IdleUp = 1,
    WalkSide = 2,
    IdleSide = 3,
    WalkDown = 4,
    IdleDown = 5,

    None = byte.MaxValue
}

public enum EnemyAnimationIndex : byte
{
    WalkDown = 0,
    WalkSide = 1,
    WalkUp = 2,

    None = byte.MaxValue
}

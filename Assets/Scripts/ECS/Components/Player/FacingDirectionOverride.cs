using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_FacingDirection")]
public struct FacingDirectionOverride : IComponentData
{
    public float Value;
}

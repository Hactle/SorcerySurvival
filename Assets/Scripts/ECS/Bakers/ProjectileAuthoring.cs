using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjectileAuthoring : MonoBehaviour
{
    public float Speed;
    public float HitRadius;
    public float Pierce;
}

public class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new AirSpeed
        {
            Value = authoring.Speed,
        });

        AddComponent<Damage>(entity);

        AddComponent<ProjectileTag>(entity);

        AddComponent(entity, new LifeTime
        {
            Value = 6f,
        });

        AddComponent(entity, new HitRadius
        {
            Value = authoring.HitRadius,
        });

        AddComponent(entity, new Pierce
        {
            Value = authoring.Pierce,
        });

        AddBuffer<DamagedEntity>(entity);
    }
}

using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    public float Speed;
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
    }
}

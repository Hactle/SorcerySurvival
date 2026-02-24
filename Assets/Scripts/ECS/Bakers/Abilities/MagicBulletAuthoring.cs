using Unity.Entities;
using UnityEngine;

class MagicBulletAuthoring : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public float Damage;
    public float Cooldown;
}

class MagicBulletAuthoringBaker : Baker<MagicBulletAuthoring>
{
    public override void Bake(MagicBulletAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<ProjectileReleaseTag>(entity);

        AddComponent(entity, new Damage
        {
            Value = authoring.Damage,
        });

        AddComponent(entity, new Cooldown {
            CurrentValue = authoring.Cooldown,
            BaseValue = authoring.Cooldown,
        });

        AddComponent(entity, new AbilityProjectilePrefab
        {
            Prefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic)
        });
    }
}

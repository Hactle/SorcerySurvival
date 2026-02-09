using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

class EnemyAuthoring : MonoBehaviour
{
    [Header("Movement")]
    [Space(4)]
    public float MoveSpeed;
    public float SeparationRadius;
    public float SeparationStrength;
    [Space(5)]
    [Header("Attack")]
    [Space(4)]
    public float Health;
    public float Damage;


    class EnemyBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<EnemyTag>(entity);

            AddComponent(entity, new MoveSpeed
            {
                Value = authoring.MoveSpeed,
            });

            AddComponent(entity, new SeparationRadius
            {
                Value = authoring.SeparationRadius,
            });

            AddComponent(entity, new SeparationStrenght
            {
                Value = authoring.SeparationStrength,
            });
            
            AddComponent(entity, new SpatialHashCell());

            AddComponent(entity, new FacingDirectionOverride
            {
                Value = 1f
            });

            AddComponent<AnimationIndexOverride>(entity);

            AddComponent(entity, new Health
            {
                Value = authoring.Health,
            });

            AddComponent(entity, new AttackDamage
            {
                Value = authoring.Damage,
            });
        }
    }
}



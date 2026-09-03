using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Rendering;

class EnemyAuthoring : MonoBehaviour
{
    [Header("Movement")]
    [Space(4)]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private float _separationRadius;
    [SerializeField] private float _separationStrength;
    [Space(5)]
    [Header("Attack")]
    [Space(4)]
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [Space(5)]

    [SerializeField] private int _experienceGain;

    class EnemyBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<EnemyTag>(entity);

            AddComponent(entity, new EntitySide
            {
                Value = Sides.Enemy,
            });

            AddComponent(entity, new MoveSpeed
            {
                Value = authoring._moveSpeed,
            });

            AddComponent<WorldPosition>(entity);

            AddComponent(entity, new SeparationRadius
            {
                Value = authoring._separationRadius,
            });

            AddComponent(entity, new SeparationStrenght
            {
                Value = authoring._separationStrength,
            });
            
            AddComponent(entity, new SpatialHashCell());

            AddComponent(entity, new FacingDirectionOverride
            {
                Value = 1f
            });

            AddComponent<AnimationIndexOverride>(entity);

            AddComponent(entity, new Health
            {
                Value = authoring._health,
            });

            AddComponent(entity, new Damage
            {
                Value = authoring._damage,
            });

            AddComponent(entity, new CollisionRadius
            {
                Value = authoring._collisionRadius,
            });

            AddComponent(entity, new ExperienceGain
            {
                Value = authoring._experienceGain,
            });
        }
    }
}



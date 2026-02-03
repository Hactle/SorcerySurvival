using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

class EnemyAuthoring : MonoBehaviour
{
    public float MoveSpeed = 2f;
    public float SeparationRadius = 0.4f;
    public float SeparationStrength = 1.5f;

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
        }
    }
}



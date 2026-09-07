using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _collisionRadius = 0.5f;
    [SerializeField] private float _invincibilityTime;

    [SerializeField] private GameObject _startingAbilityPrefab;

    [SerializeField] private int _experiencePool = 100;

    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            var abilityPrefabEntity = GetEntity(authoring._startingAbilityPrefab, TransformUsageFlags.None);

            AddComponent<PlayerTag>(entity);

            AddComponent(entity, new EntitySide
            {
                Value = Sides.Player
            });

            AddComponent(entity, new MoveSpeed
            {
                Value = authoring._moveSpeed
            });
         
            AddComponent<InitializeCameraTargetTag>(entity);

            AddComponent<CameraTarget>(entity);

            AddComponent<PlayerMoveDirection>(entity);

            AddComponent<LastMoveDirection>(entity);

            AddComponent(entity, new FacingDirectionOverride
            {
                Value = 1f
            });

            AddComponent<AnimationIndexOverride>(entity);

            AddComponent(entity, new Health
            {
                Value = authoring._maxHealth
            });

            AddBuffer<DamageEvent>(entity);

            AddComponent(entity, new MaxHealth
            {
                Value = authoring._maxHealth
            });

            AddComponent(entity, new CollisionRadius
            {
                Value = authoring._collisionRadius,
            });

            AddComponent(entity, new CanReceiveInvincibility
            {
                Value = authoring._invincibilityTime
            });

            AddComponent(entity, new PlayerStartingAbility
            {
                Prefab = abilityPrefabEntity
            });

            AddComponent(entity, new ExperiencePool
            {
                MaxValue = authoring._experiencePool,
                CurrentValue = 0
            });

            AddComponent(entity, new Level
            {
                CurrentLevel = 1
            });
        }
    }
}
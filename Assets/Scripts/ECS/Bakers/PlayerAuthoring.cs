using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class PlayerAuthoring : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float MoveSpeed = 5f;   
    public float CollisionRadius = 0.5f;
    public float InvincibilityTime;

    public GameObject MagicBulletAbilityPrefab;

    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            var abilityPrefabEntity = GetEntity(authoring.MagicBulletAbilityPrefab, TransformUsageFlags.None);

            AddComponent<PlayerTag>(entity);

            AddComponent(entity, new MoveSpeed
            {
                Value = authoring.MoveSpeed
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
                Value = authoring.MaxHealth
            });

            AddComponent(entity, new MaxHealth
            {
                Value = authoring.MaxHealth
            });

            AddComponent(entity, new CollisionRadius
            {
                Value = authoring.CollisionRadius,
            });

            AddComponent(entity, new CanReceiveInvincibility
            {
                Value = authoring.InvincibilityTime
            });

            AddComponent(entity, new PlayerStartingAbility
            {
                Prefab = abilityPrefabEntity
            });
        }
    }
}

public partial class PlayerInputSystem : SystemBase
{
    private CharacterInput _input;

    protected override void OnCreate()
    {
        _input = new CharacterInput();
        _input.Enable();
    }

    protected override void OnUpdate()
    {
        var currentInput = (float2)_input.Player.Move.ReadValue<Vector2>();
        SystemAPI.GetSingletonRW<PlayerMoveDirection>().ValueRW.Value = currentInput;
    }
}
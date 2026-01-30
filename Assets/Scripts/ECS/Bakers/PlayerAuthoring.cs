using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed = 5f;   

    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

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
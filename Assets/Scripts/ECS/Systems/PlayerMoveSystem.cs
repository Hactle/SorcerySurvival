using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();

        ref var playerTransform = ref SystemAPI
            .GetComponentRW<LocalTransform>(playerEntity).ValueRW;
        ref var facingDirection = ref SystemAPI
            .GetComponentRW<FacingDirectionOverride>(playerEntity).ValueRW;
        ref var animationIndex = ref SystemAPI
            .GetComponentRW<AnimationIndexOverride>(playerEntity).ValueRW;
        ref var lastDirection = ref SystemAPI
                .GetComponentRW<LastMoveDirection>(playerEntity).ValueRW;

        float2 direction = SystemAPI.
            GetComponentRO<PlayerMoveDirection>(playerEntity).ValueRO.Value;

        bool isMoving = math.lengthsq(direction) > 0.1f;

        float3 moveDelta = new float3(direction, 0) * deltaTime;

        float speed = SystemAPI.GetComponentRO<MoveSpeed>(playerEntity).ValueRO.Value;
        playerTransform.Position += moveDelta * speed;

        if (isMoving)
        {
            lastDirection.Value = math.normalize(direction);
        }
        #region Animation
        float2 animationDirection = isMoving ? direction : lastDirection.Value;

        if (math.abs(animationDirection.y) > math.abs(animationDirection.x))
        {
            if (isMoving)
            {
                animationIndex.Value = animationDirection.y > 0
                    ? (float)PlayerAnimationIndex.WalkUp
                    : (float)PlayerAnimationIndex.WalkDown;
            }
            else
            {
                animationIndex.Value = animationDirection.y > 0
                    ? (float)PlayerAnimationIndex.IdleUp
                    : (float)PlayerAnimationIndex.IdleDown;
            }
        }
        else
        {
            animationIndex.Value = isMoving
                ? (float)PlayerAnimationIndex.WalkSide
                : (float)PlayerAnimationIndex.IdleSide;
        }

        if (math.abs(direction.x) > 0.1f) facingDirection.Value = math.sign(direction.x);
        #endregion
    }
}

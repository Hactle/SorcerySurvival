using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();

        ref var playerTransform = ref SystemAPI.GetComponentRW<LocalTransform>(playerEntity).ValueRW;

        float2 direction = SystemAPI.GetComponentRO<PlayerMoveDirection>(playerEntity).ValueRO.Value;

        float3 moveDelta = new float3(direction, 0) * deltaTime;

        float speed = SystemAPI.GetComponentRO<MoveSpeed>(playerEntity).ValueRO.Value;
        playerTransform.Position += moveDelta * speed;
    }
}

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(InputSystemGroup))]
partial struct InputSyncSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerMoveDirection>();
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var input = InputManager.Instance;

        SystemAPI.GetSingletonRW<PlayerMoveDirection>().ValueRW.Value =
            (float2)input.MoveDirection;

        if (!input.PausePressed)
            return;

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entity eventEntity = ecb.CreateEntity();

        ecb.AddComponent<EventTag>(eventEntity);

        ecb.AddComponent<RequestTogglePauseEvent>(eventEntity);

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        input.ConsumePause();
    }

    public void OnDestroy(ref SystemState state)
    {

    }
}

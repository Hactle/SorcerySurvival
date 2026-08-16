using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(EventCleanupGroup))]
partial struct EventCleanUpSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<EventTag>();   
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach(var (_, entity) in
                SystemAPI.Query<RefRO<EventTag>>()
                    .WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

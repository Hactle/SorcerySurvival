using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(DamageFilterGroup))]
partial struct InvincibilityFilterSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (
            invincibility,
            damageEvents) 
            in SystemAPI.Query<
                RefRO<InvincibilityTag>,
                DynamicBuffer<DamageEvent>>())
        {
            damageEvents.Clear();
        }
    }
}

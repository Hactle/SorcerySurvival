using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(AbilityCooldownGroup))]
partial struct CooldownCounterSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);  

        foreach (var (cooldown, entity) in SystemAPI.Query<RefRW<Cooldown>>().WithEntityAccess())
        {
            cooldown.ValueRW.CurrentValue = math.max(0f, cooldown.ValueRW.CurrentValue - SystemAPI.Time.DeltaTime);

            if(cooldown.ValueRW.CurrentValue == 0f)
            {
                ecb.AddComponent<AbilityReadyTag>(entity);
            }
        }
        ecb.Playback(state.EntityManager);
    }   
}
    

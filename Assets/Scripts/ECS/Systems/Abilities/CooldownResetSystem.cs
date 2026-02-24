using Unity.Entities;

[UpdateInGroup(typeof(AbilityResetGroup))]
partial struct CooldownResetSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            cooldown,
            abilityReady,
            entity) in SystemAPI.Query<
                    RefRW<Cooldown>,
                    RefRO<AbilityReadyTag>>().WithEntityAccess())
        {
            cooldown.ValueRW.CurrentValue = cooldown.ValueRO.BaseValue;

            ecb.RemoveComponent<AbilityReadyTag>(entity);
        }
        ecb.Playback(state.EntityManager);
    }
}

using Unity.Entities;

[UpdateInGroup(typeof(AbilityExecuteGroup))]
partial struct ProjectileReleaseSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (
            projectileReleaseTag,
            abilityReady,
            projectilePrefab,
            damage,
            entity) in SystemAPI.Query<
                RefRO<ProjectileReleaseTag>,
                RefRO<AbilityReadyTag>,
                RefRO<AbilityProjectilePrefab>,
                RefRO<Damage>>().WithEntityAccess())
        {
            var projectileInstance = ecb.Instantiate(projectilePrefab.ValueRO.Prefab);

            ecb.SetComponent(projectileInstance, new Damage
            {
                Value = damage.ValueRO.Value
            });
        }
        ecb.Playback(state.EntityManager);
    }
}

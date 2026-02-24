using Unity.Entities;
using Unity.Transforms;

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
            entityOwner) in SystemAPI.Query<
                RefRO<ProjectileReleaseTag>,
                RefRO<AbilityReadyTag>,
                RefRO<AbilityProjectilePrefab>,
                RefRO<Damage>,
                RefRO<AbilityOwner>>())
        {
            var projectileInstance = ecb.Instantiate(projectilePrefab.ValueRO.Prefab);

            var ownerTransform = SystemAPI.GetComponent<LocalTransform>(entityOwner.ValueRO.Owner);

            ecb.SetComponent(projectileInstance, new LocalTransform
            {
                Position = ownerTransform.Position,
                Rotation = ownerTransform.Rotation,
                Scale = 0.4f
            });

            ecb.SetComponent(projectileInstance, new Damage
            {
                Value = damage.ValueRO.Value
            });
        }
        ecb.Playback(state.EntityManager);
    }
}

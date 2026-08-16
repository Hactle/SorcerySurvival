using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(AbilityExecuteGroup))]
partial struct ProjectileReleaseSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        bool hasEnemies = !SystemAPI.QueryBuilder()
            .WithAll<EnemyTag>()
            .Build()
            .IsEmpty;

        if (!hasEnemies) return;

        foreach (var (
            projectilePrefab,
            damage,
            entityOwner) in SystemAPI.Query<
                RefRO<AbilityProjectilePrefab>,
                RefRO<Damage>,
                RefRO<AbilityOwner>>().WithAll<ProjectileReleaseTag, AbilityReadyTag>())
        {
            var projectileInstance = ecb.Instantiate(projectilePrefab.ValueRO.Prefab);

            var ownerTransform = SystemAPI.GetComponent<LocalTransform>(entityOwner.ValueRO.Owner);

            var ownerSide = SystemAPI.GetComponent<EntitySide>(entityOwner.ValueRO.Owner);

            ecb.AddComponent(projectileInstance, new EntitySide
            {
                Value = ownerSide.Value
            });

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

using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct PlayerAbilityInitializationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

        foreach (var (abilityRef, entity) in
                 SystemAPI.Query<RefRO<PlayerStartingAbility>>()
                          .WithNone<PlayerInitializedTag>()
                          .WithEntityAccess())
        {
            var abilityInstance = ecb.Instantiate(abilityRef.ValueRO.Prefab);

            ecb.AddComponent(abilityInstance, new AbilityOwner
            {
                Owner = entity
            });

            ecb.AddComponent(abilityInstance, new EntitySide
            {
                Value = Sides.Player
            });

            ecb.AddComponent<PlayerInitializedTag>(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}

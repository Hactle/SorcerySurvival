using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(AbilityExecuteGroup))]
public partial struct DamageAuraViewSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (AuraRadiusView.Instance == null)
            return;

        var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        transformLookup.Update(ref state);

        bool shown = false;

        foreach (var (radius, entityOwner) in SystemAPI.Query<
            RefRO<DamageRadius>,
            RefRO<AbilityOwner>>())
        {
            var ownerEntity = entityOwner.ValueRO.Owner;

            if (!transformLookup.HasComponent(ownerEntity))
                continue;

            Vector3 center = transformLookup[ownerEntity].Position;
            AuraRadiusView.Instance.Show(center, radius.ValueRO.Value);
            shown = true;

            break;
        }

        if (!shown)
            AuraRadiusView.Instance.Hide();
    }
}
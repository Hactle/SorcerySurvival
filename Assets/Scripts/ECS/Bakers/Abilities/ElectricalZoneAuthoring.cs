using Unity.Entities;
using UnityEngine;

class ElectricalZoneAuthoring : MonoBehaviour
{
    public float DamagePerSecond;
    public float Radius;
    public float TicksPerSecond;
}

class ElectricalZoneAuthoringBaker : Baker<ElectricalZoneAuthoring>
{
    public override void Bake(ElectricalZoneAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        float tickInterval = 1f / authoring.TicksPerSecond;
        float damagePerTick = authoring.DamagePerSecond * tickInterval;

        AddComponent(entity, new Damage
        {
            Value = damagePerTick
        });

        AddComponent(entity, new DamageRadius
        {
            Value = authoring.Radius
        });

        AddComponent(entity, new DamageTick
        {
            Interval = tickInterval,
            Timer = tickInterval
        });
    }
}

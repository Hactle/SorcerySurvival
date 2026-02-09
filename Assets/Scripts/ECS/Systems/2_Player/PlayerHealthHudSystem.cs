using Unity.Entities;

partial struct PlayerHealthHUDSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var player))
            return;

        var health = SystemAPI.GetComponentRO<Health>(player).ValueRO.Value;
        var maxHealth = SystemAPI.GetComponentRO<MaxHealth>(player).ValueRO.Value;

        float healthPercentage = health / maxHealth;

        foreach (var hud in UnityEngine.Object.FindObjectsByType<HealthView>(UnityEngine.FindObjectsSortMode.None))
        {
            hud.SetHealth(healthPercentage);
        }
    }
}

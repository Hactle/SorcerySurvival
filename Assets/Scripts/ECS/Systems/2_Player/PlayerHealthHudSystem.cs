using Unity.Entities;

partial struct PlayerHealthHUDSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (HealthView.Instance == null)
            return;

        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var player))
            return;

        var health = SystemAPI.GetComponentRO<Health>(player).ValueRO.Value;
        var maxHealth = SystemAPI.GetComponentRO<MaxHealth>(player).ValueRO.Value;

        float healthPercentage = health / maxHealth;

        HealthView.Instance.SetHealth(healthPercentage);
    }
}
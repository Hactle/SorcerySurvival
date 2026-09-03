using Unity.Entities;

partial struct ExperienceHUDSystem : ISystem
{
    private float _previousMaxValue;
    private bool _initialized;

    public void OnUpdate(ref SystemState state)
    {
        if (ExperienceView.Instance == null)
            return;

        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var player))
            return;

        var pool = SystemAPI.GetComponentRO<ExperiencePool>(player).ValueRO;

        float percentage = (float)pool.CurrentValue / pool.MaxValue;

        bool leveledUp = _initialized && pool.MaxValue != _previousMaxValue;

        if (leveledUp)
            ExperienceView.Instance.SetExperienceInstant(percentage);
        else
            ExperienceView.Instance.SetExperience(percentage);

        _previousMaxValue = pool.MaxValue;
        _initialized = true;
    }
}
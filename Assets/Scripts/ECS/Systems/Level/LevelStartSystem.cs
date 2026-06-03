using Unity.Entities;

partial struct LevelStartSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<WaveState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (waveState, waveBuffer, entity) in 
                SystemAPI.Query<
                    RefRW<WaveState>,
                    DynamicBuffer<WaveElement>>()
                .WithEntityAccess())
        {
            var firstWaveEntity = waveBuffer[0].WaveEntity;

            var waveData = SystemAPI.GetComponent<WaveData>(firstWaveEntity);

            waveState.ValueRW.CurrentWaveIndex = 0;
            waveState.ValueRW.TimeLeft = waveData.Duration;
            waveState.ValueRW.WaveInProgress = true;

            state.Enabled = false;
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

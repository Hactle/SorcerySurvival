using Unity.Entities;

partial struct WaveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<WaveState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (waveState, waveBuffer) in 
                SystemAPI.Query<
                    RefRW<WaveState>,
                    DynamicBuffer<WaveElement>>())
        {
            if (!waveState.ValueRO.WaveInProgress)
                continue;

            waveState.ValueRW.TimeLeft -= deltaTime;

            bool waveFinished = 
                waveState.ValueRO.TimeLeft <= 0 ||
                waveState.ValueRO.EnemiesRemaining <=0;

            if (!waveFinished)
                continue;
            int nextWaveIndex = waveState.ValueRO.CurrentWaveIndex + 1;

            if(nextWaveIndex >= waveBuffer.Length)
            {
                waveState.ValueRW.WaveInProgress = false;

                UnityEngine.Debug.Log("Level Completed");

                continue;
            }

            Entity nextWaveEntity = waveBuffer[nextWaveIndex].WaveEntity;

            WaveData nextWaveData = SystemAPI.GetComponent<WaveData>(nextWaveEntity);

            waveState.ValueRW.CurrentWaveIndex = nextWaveIndex;
            waveState.ValueRW.TimeLeft = nextWaveData.Duration;
            waveState.ValueRW.EnemiesRemaining = 0;
            waveState.ValueRW.WaveInProgress = true;
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

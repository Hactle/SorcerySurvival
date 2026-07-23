using Unity.Entities;

partial struct WaveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
<<<<<<< Updated upstream
        state.RequireForUpdate<WaveState>();
=======
        state.RequireForUpdate<LevelState>();
>>>>>>> Stashed changes
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

<<<<<<< Updated upstream
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
=======
        foreach (var (levelState, waveBuffer) in
                 SystemAPI.Query<
                     RefRW<LevelState>,
                     DynamicBuffer<WaveElement>>())
        {
            if (!levelState.ValueRO.WaveInProgress)
            {
                levelState.ValueRW.CurrentWaveIndex = 0;

                Entity waveEntity = waveBuffer[0].WaveEntity;
                WaveData waveData = SystemAPI.GetComponent<WaveData>(waveEntity);

                levelState.ValueRW.WaveVersion++;
                levelState.ValueRW.TimeLeft = waveData.Duration;
                levelState.ValueRW.WaveInProgress = true;
                UnityEngine.Debug.Log($"Wave {levelState.ValueRO.CurrentWaveIndex + 1} started");
>>>>>>> Stashed changes

                continue;
            }

<<<<<<< Updated upstream
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
=======
            levelState.ValueRW.TimeLeft -= deltaTime;

            if (levelState.ValueRO.TimeLeft > 0)
                continue;

            int nextWave = levelState.ValueRO.CurrentWaveIndex + 1;

            if (nextWave >= waveBuffer.Length)
            {
                levelState.ValueRW.WaveInProgress = false;

                UnityEngine.Debug.Log("Level Complete");
                continue;
            }

            Entity nextWaveEntity = waveBuffer[nextWave].WaveEntity;
            WaveData nextWaveData = SystemAPI.GetComponent<WaveData>(nextWaveEntity);

            levelState.ValueRW.WaveVersion++;
            levelState.ValueRW.CurrentWaveIndex = nextWave;
            levelState.ValueRW.TimeLeft = nextWaveData.Duration;
            levelState.ValueRW.WaveInProgress = true;
            UnityEngine.Debug.Log($"Wave {levelState.ValueRO.CurrentWaveIndex + 1} started");
        }
    }
>>>>>>> Stashed changes
}

using Unity.Entities;

[UpdateInGroup(typeof(GamePlaySystemGroup))]
public partial struct WaveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<LevelState>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);

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

                levelState.ValueRW.TimeLeft = waveData.Duration;
                levelState.ValueRW.WaveInProgress = true;

                continue;
            }

            levelState.ValueRW.TimeLeft -= deltaTime;

            if (levelState.ValueRO.TimeLeft > 0f)
                continue;

            int nextWave = levelState.ValueRO.CurrentWaveIndex + 1;

            if (nextWave >= waveBuffer.Length)
            {
                levelState.ValueRW.WaveInProgress = false;

                var levelCompletedEntity = ecb.CreateEntity();

                ecb.AddComponent<EventTag>(levelCompletedEntity);

                ecb.AddComponent(levelCompletedEntity, new LevelCompletedEvent
                {
                    LevelId = levelState.ValueRO.LevelId
                });

                var requestEntity = ecb.CreateEntity();

                ecb.AddComponent<EventTag>(requestEntity);

                ecb.AddComponent(requestEntity, new RequestGameStateEvent
                {
                    NewState = GameStateType.Win
                });

                continue;
            }

            Entity nextWaveEntity = waveBuffer[nextWave].WaveEntity;
            WaveData nextWaveData = SystemAPI.GetComponent<WaveData>(nextWaveEntity);

            levelState.ValueRW.CurrentWaveIndex = nextWave;
            levelState.ValueRW.TimeLeft = nextWaveData.Duration;
            levelState.ValueRW.WaveInProgress = true;
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
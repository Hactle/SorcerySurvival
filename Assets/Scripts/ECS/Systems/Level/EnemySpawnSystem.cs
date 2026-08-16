using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(GamePlaySystemGroup))]
[BurstCompile]
partial struct EnemySpawnSystem : ISystem
{
    private Random _random;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<LevelState>();

        _random = Random.CreateFromIndex(12345);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        float3 playerPosition = float3.zero;

        foreach (var transform in
                 SystemAPI.Query<RefRO<LocalTransform>>()
                          .WithAll<PlayerTag>())
        {
            playerPosition = transform.ValueRO.Position;
            break;
        }

        foreach (var (levelState, waveBuffer) in
                SystemAPI.Query<
                     RefRW<LevelState>,
                     DynamicBuffer<WaveElement>>())
        {
            if (!levelState.ValueRO.WaveInProgress)
                continue;

            Entity currentWave =
                waveBuffer[levelState.ValueRO.CurrentWaveIndex].WaveEntity;

            DynamicBuffer<EnemySpawnElement> spawnBuffer =
                SystemAPI.GetBuffer<EnemySpawnElement>(currentWave);

            if (levelState.ValueRO.SpawnSystemWaveVersion !=
                levelState.ValueRO.WaveVersion)
            {
                for (int i = 0; i < spawnBuffer.Length; i++)
                {
                    EnemySpawnElement spawn = spawnBuffer[i];
                    spawn.Accumulator = 0f;
                    spawnBuffer[i] = spawn;
                }

                levelState.ValueRW.SpawnSystemWaveVersion =
                    levelState.ValueRO.WaveVersion;
            }

            for (int i = 0; i < spawnBuffer.Length; i++)
            {
                EnemySpawnElement spawn = spawnBuffer[i];

                spawn.Accumulator += spawn.SpawnRate * dt;

                int spawnCount = (int)math.floor(spawn.Accumulator);

                if (spawnCount == 0)
                {
                    spawnBuffer[i] = spawn;
                    continue;
                }

                spawn.Accumulator -= spawnCount;

                for (int j = 0; j < spawnCount; j++)
                {
                    Entity enemy = ecb.Instantiate(spawn.EnemyPrefab);

                    float angle = _random.NextFloat(0f, math.PI * 2f);

                    float distance = _random.NextFloat(10f, 20f);

                    float2 offset =
                        new float2(
                        math.cos(angle),
                        math.sin(angle)
                        ) * distance;

                    float3 spawnPosition =
                        playerPosition +
                        new float3(offset.x, offset.y, 0f);

                    ecb.SetComponent(enemy, LocalTransform.FromPosition(spawnPosition));
                }

                spawnBuffer[i] = spawn;
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}

using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

class LevelAuthoring : MonoBehaviour
{
    public LevelConfig Config;
}

class LevelBaker : Baker<LevelAuthoring>
{
    public override void Bake(LevelAuthoring authoring)
    {
        Entity levelEntity = GetEntity(TransformUsageFlags.None);

        var waveBuffer = AddBuffer<WaveElement>(levelEntity);

        foreach (var wave in authoring.Config.Waves)
        {
            Entity waveEntity = CreateAdditionalEntity(TransformUsageFlags.None);

            AddComponent(waveEntity, new WaveData
            {
                Duration = wave.Duration,
                Acceleration = wave.SpawnAcceleration
            });

            var spawmBuffer = AddBuffer<EnemySpawnElement>(waveEntity);

            foreach (var enemy in wave.Enemies)
            {
                var prefabEntity = GetEntity(enemy.EnemyPrefab, TransformUsageFlags.Dynamic);

                spawmBuffer.Add(new EnemySpawnElement
                {
                    EnemyPrefab = prefabEntity,
                    Count = enemy.Count
                });
            }

            waveBuffer.Add(new WaveElement
            {
                WaveEntity = waveEntity
            });
        }

        AddComponent(levelEntity, new WaveState
        {
            CurrentWaveIndex = 0,
            EnemiesRemaining = 0,
            TimeLeft = 0,
            WaveInProgress = false
        });

    }
}

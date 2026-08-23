using Unity.Entities;
using UnityEngine;

class LevelAuthoring : MonoBehaviour
{
    public LevelConfig Config;
}

class LevelAuthoringBaker : Baker<LevelAuthoring>
{
    public override void Bake(LevelAuthoring authoring)
    {
        Entity levelEntity = GetEntity(TransformUsageFlags.None);

        AddComponent(levelEntity, new LevelState
        {
            LevelId = authoring.Config.LevelId
        });

        DynamicBuffer<WaveElement> waveBuffer = 
            AddBuffer<WaveElement>(levelEntity);

        foreach (var wave in authoring.Config.Waves)
        {
            Entity waveEntity =
                CreateAdditionalEntity(TransformUsageFlags.None);

            AddComponent(waveEntity, new WaveData
            {
                Duration = wave.Duration
            });

            DynamicBuffer<EnemySpawnElement> spawnBuffer =
                AddBuffer<EnemySpawnElement>(waveEntity);

            foreach (var enemy in wave.Enemies)
            {
                spawnBuffer.Add(new EnemySpawnElement
                {
                    EnemyPrefab = GetEntity(enemy.EnemyPrefab, TransformUsageFlags.Dynamic),
                    SpawnRate = enemy.SpawnRate,
                    Accumulator = 0f
                });
            }

            waveBuffer.Add(new WaveElement
            {
                WaveEntity = waveEntity,
            });
        }

    }
}

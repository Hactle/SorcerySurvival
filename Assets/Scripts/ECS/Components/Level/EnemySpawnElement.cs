using Unity.Entities;

public struct EnemySpawnElement : IBufferElementData
{
    public Entity EnemyPrefab;

    public float SpawnRate;

    public float Accumulator;
}

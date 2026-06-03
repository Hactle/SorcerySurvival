using Unity.Entities;

public struct EnemySpawnElement : IBufferElementData
{
    public Entity EnemyPrefab;
    public int Count;
}

using Unity.Entities;

public struct LevelState : IComponentData
{
    public int WaveVersion;
    public int SpawnSystemWaveVersion;

    public int CurrentWaveIndex;

    public float TimeLeft;

    public bool WaveInProgress;
}

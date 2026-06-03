using Unity.Entities;

public struct WaveState : IComponentData
{
    public int CurrentWaveIndex;
    public int EnemiesRemaining;
    public float TimeLeft;
    public bool WaveInProgress;
}

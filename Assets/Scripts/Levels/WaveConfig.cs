using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "Scriptable Objects/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    public EnemySpawnInfo[] Enemies;
    public float Duration;
    public float SpawnAcceleration;
}

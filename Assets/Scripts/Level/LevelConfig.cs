using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level Config")]
public class LevelConfig : ScriptableObject
{
    public List<WaveConfig> Waves = new();
}

[Serializable]
public class WaveConfig
{
    [Min(0.1f)]
    public float Duration = 30f;

    public List<EnemySpawnInfo> Enemies = new();
}

[Serializable]
public class EnemySpawnInfo
{
    public GameObject EnemyPrefab;

    [Min(0f)]
    public float SpawnRate = 1f;
}
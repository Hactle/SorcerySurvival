using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnInfo", menuName = "Scriptable Objects/EnemySpawnInfo")]
public class EnemySpawnInfo : ScriptableObject
{
    public GameObject EnemyPrefab;
    public int Count;
}

using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Level Completed")]
public class LevelCompletedRequirement : LevelRequirement
{
    [SerializeField] private int levelId;

    public override bool IsSatisfied(SaveData saveData)
    {
        return saveData.Levels.CompletedLevels.Contains(levelId);
    }
}
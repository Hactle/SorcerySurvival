using UnityEngine;

public abstract class LevelRequirement : ScriptableObject
{
    public abstract bool IsSatisfied(SaveData saveData);
}

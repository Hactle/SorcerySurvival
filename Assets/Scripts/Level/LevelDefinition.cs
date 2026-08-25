using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Level Definition")]
public class LevelDefinition : ScriptableObject
{
    public int LevelId;
    public LevelRequirement[] Requirements;
}
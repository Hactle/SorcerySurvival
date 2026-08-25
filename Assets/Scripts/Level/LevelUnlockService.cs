public class LevelUnlockService
{
    public bool IsUnlocked(LevelDefinition level)
    {
        SaveData saveData = SaveService.Instance.Data;

        foreach (LevelRequirement requirement in level.Requirements)
        {
            if (!requirement.IsSatisfied(saveData))
                return false;
        }

        return true;
    }
}
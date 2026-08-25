using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private LevelSelectItem[] levelItems;

    private LevelUnlockService _levelUnlockService;

    private void Awake()
    {
        _levelUnlockService = new LevelUnlockService();
    }

    private void OnEnable()
    {
        UpdateLevels();
    }

    private void UpdateLevels()
    {
        foreach (LevelSelectItem item in levelItems)
        {
            bool isUnlocked =
                _levelUnlockService.IsUnlocked(item.LevelDefinition);

            item.SetUnlocked(isUnlocked);
        }
    }
}
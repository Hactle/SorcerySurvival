using UnityEngine;
using UnityEngine.UI;

public class LevelSelectItem : MonoBehaviour
{
    [SerializeField] private LevelDefinition levelDefinition;
    [SerializeField] private Button playButton;

    public LevelDefinition LevelDefinition => levelDefinition;

    public void SetUnlocked(bool unlocked)
    {
        playButton.interactable = unlocked;
    }
}
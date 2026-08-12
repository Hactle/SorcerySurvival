using UnityEngine;

public enum MenuState
{
    Main,
    LevelSelect
}

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _levelSelectPanel;

    private MenuState currentState;

    private void Start()
    {
        SetState(MenuState.Main);
        ApplyState();
    }

    public void OpenMainMenu()
    {
        SetState(MenuState.Main);
    }

    public void OpenLevelSelect()
    {
        SetState(MenuState.LevelSelect);
    }

    private void SetState(MenuState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        ApplyState();
    }

    private void ApplyState()
    {
        switch (currentState)
        {
            case MenuState.Main:
                _mainPanel.SetActive(true);
                _levelSelectPanel.SetActive(false);
                break;

            case MenuState.LevelSelect:
                _mainPanel.SetActive(false);
                _levelSelectPanel.SetActive(true);
                break;
        }
    }
}
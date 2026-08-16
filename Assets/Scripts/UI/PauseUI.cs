using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance { get; private set; }

    [SerializeField] private GameObject _pausePanel;

    private void Awake()
    {
        Instance = this;
        _pausePanel.SetActive(false);
    }

    public void Show()
    {
        _pausePanel.SetActive(true);
    }

    public void Hide()
    {
        _pausePanel.SetActive(false);
    }
}

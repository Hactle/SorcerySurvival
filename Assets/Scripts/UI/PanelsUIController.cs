using UnityEngine;

public class PanelsUIController : MonoBehaviour
{
    public static PanelsUIController Instance { get; private set; }

    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _winPanel;

    private void Awake()
    {
        Instance = this;

        _gameOverPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _winPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        _gameOverPanel.SetActive(true);
    }

    public void ShowPause()
    {
        _pausePanel.SetActive(true);
    }

    public void HidePause()
    {
        _pausePanel.SetActive(false);
    }

    public void ShowWin()
    {
        _winPanel.SetActive(true);
    }
}

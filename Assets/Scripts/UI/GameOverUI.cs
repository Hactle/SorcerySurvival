using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    [SerializeField] private GameObject _gameOverPanel;

    private void Awake()
    {
        Instance = this;
        _gameOverPanel.SetActive(false);
    }

    public void Show()
    {
        _gameOverPanel.SetActive(true);
    }
}

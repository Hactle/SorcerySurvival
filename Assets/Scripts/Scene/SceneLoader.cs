using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;

public class SceneLoader : MonoBehaviour
{
    public void RestartLevel()
    {
        LoadGameplayScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel(int levelId)
    {
        LoadGameplayScene($"Level_{levelId}");
    }

    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void LoadGameplayScene(string sceneName)
    {
        SceneManager.sceneLoaded += OnGameplaySceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnGameplaySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnGameplaySceneLoaded;

        var world = World.DefaultGameObjectInjectionWorld;

        if (world == null || !world.IsCreated)
            return;

        var entityManager = world.EntityManager;

        entityManager.CreateEntity(
        typeof(LevelInitializationEvent),
        typeof(EventTag));
    }
}
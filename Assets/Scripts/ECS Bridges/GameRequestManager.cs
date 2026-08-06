using Unity.Entities;
using UnityEngine;

public class GameRequestManager : MonoBehaviour
{
    public static GameRequestManager Instance { get; private set; }

    private EntityManager _entityManager;

    private void Awake()
    {
        Instance = this;
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void ResumeGame()
    {
        if (_entityManager == null) return;

        var gameStateEventEntity = _entityManager.CreateEntity();
        _entityManager.AddComponent<EventTag>(gameStateEventEntity);
        _entityManager.AddComponentData(gameStateEventEntity, new RequestGameStateEvent()
        {
            NewState = GameStateType.Playing
        });
    }
}
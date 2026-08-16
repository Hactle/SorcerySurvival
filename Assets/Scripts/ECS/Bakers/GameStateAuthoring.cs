using Unity.Entities;
using UnityEngine;

class GameStateAuthoring : MonoBehaviour
{
    
}

class GameStateAuthoringBaker : Baker<GameStateAuthoring>
{
    public override void Bake(GameStateAuthoring authoring)
    {
        Entity gameStateEntity = GetEntity(TransformUsageFlags.None);

        AddComponent(gameStateEntity, new GameState
        {
            CurrentState = GameStateType.Playing
        });
    }
}

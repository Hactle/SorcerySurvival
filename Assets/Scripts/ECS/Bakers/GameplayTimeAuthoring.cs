using Unity.Entities;
using UnityEngine;

class GameplayTimeAuthoring : MonoBehaviour
{
    
}

class GameplayTimeAuthoringBaker : Baker<GameplayTimeAuthoring>
{
    public override void Bake(GameplayTimeAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<GameplayTime>(entity);
    }
}

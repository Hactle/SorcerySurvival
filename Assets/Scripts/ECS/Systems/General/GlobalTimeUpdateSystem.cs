using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(GamePlaySystemGroup))]
public partial struct GlobalTimeUpdateSystem : ISystem
{
    private static readonly int GlobalTimeShaderPropertyId =
        Shader.PropertyToID("_GlobalTime");

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameplayTime>();
    }

    public void OnUpdate(ref SystemState state)
    {
        RefRW<GameplayTime> gameplayTime =
            SystemAPI.GetSingletonRW<GameplayTime>();

        gameplayTime.ValueRW.Value += SystemAPI.Time.DeltaTime;

        Shader.SetGlobalFloat(
            GlobalTimeShaderPropertyId,
            gameplayTime.ValueRO.Value);
    }
}
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct CameraInitializationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InitializeCameraTargetTag>();
    }
    public void OnUpdate(ref SystemState state)
    {
        if (CameraTargetSingleton.Instance == null) return;        
        var cameraTargetTransform = CameraTargetSingleton.Instance.transform;

        var entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach ( var (cameraTarget, entity ) in SystemAPI.Query<RefRW<CameraTarget>>().WithAll<InitializeCameraTargetTag, PlayerTag>().WithEntityAccess())
        {
            cameraTarget.ValueRW.CameraTransform = cameraTargetTransform;
            entityCommandBuffer.RemoveComponent<InitializeCameraTargetTag>(entity);
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}

using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(TransformSystemGroup))]
partial struct MoveCameraSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var 
            (transform,
            cameraTarget) in
                            SystemAPI.Query
                            <LocalToWorld,
                            CameraTarget>()
                            .WithAll<PlayerTag>()
                            .WithNone<InitializeCameraTargetTag>())
        {
            cameraTarget.CameraTransform.Value.position = transform.Position;
        }
    }
}

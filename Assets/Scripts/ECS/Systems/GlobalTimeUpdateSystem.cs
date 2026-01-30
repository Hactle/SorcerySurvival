using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct GlobalTimeUpdateSystem : ISystem
{
    private static int _globalTimeShaderPropertyId;

    public void OnCreate(ref SystemState state)
    {
        _globalTimeShaderPropertyId = Shader.PropertyToID("_GlobalTime");
    }

    public void OnUpdate(ref SystemState state)
    {
        Shader.SetGlobalFloat(_globalTimeShaderPropertyId, (float)SystemAPI.Time.ElapsedTime);
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

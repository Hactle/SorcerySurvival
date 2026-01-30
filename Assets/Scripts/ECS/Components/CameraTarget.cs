using Unity.Entities;
using UnityEngine;

public struct CameraTarget : IComponentData
{
    public UnityObjectRef<Transform> CameraTransform;
}

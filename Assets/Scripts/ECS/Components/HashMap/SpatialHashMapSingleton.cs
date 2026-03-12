using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct SpatialHashMapSingleton : IComponentData
{
    public NativeParallelMultiHashMap<int, Entity> Map;
}
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(GamePlaySystemGroup))]
public partial class InitializationEventGroup : ComponentSystemGroup { }
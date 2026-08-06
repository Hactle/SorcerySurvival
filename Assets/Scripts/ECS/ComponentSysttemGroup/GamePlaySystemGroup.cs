using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(InputSystemGroup))]
public partial class GamePlaySystemGroup : ComponentSystemGroup { }

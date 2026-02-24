using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class AbilityPipelineGroup : ComponentSystemGroup { }


[UpdateInGroup(typeof(AbilityPipelineGroup))]
public partial class CooldownGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(AbilityPipelineGroup))]
[UpdateAfter(typeof(CooldownGroup))]
public partial class AbilityExecuteGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(AbilityPipelineGroup))]
[UpdateAfter(typeof(AbilityExecuteGroup))]
public partial class AbilityResetGroup : ComponentSystemGroup { }


using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class AbilityPipelineGroup : ComponentSystemGroup { }


[UpdateInGroup(typeof(AbilityPipelineGroup))]
public partial class AbilityCooldownGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(AbilityPipelineGroup))]
[UpdateAfter(typeof(AbilityCooldownGroup))]
public partial class AbilityInitializeGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(AbilityPipelineGroup))]
[UpdateAfter(typeof(AbilityInitializeGroup))]
public partial class AbilityExecuteGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(AbilityPipelineGroup))]
[UpdateAfter(typeof(AbilityExecuteGroup))]
public partial class AbilityResetGroup : ComponentSystemGroup { }


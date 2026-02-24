using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class DamagePipelineGroup : ComponentSystemGroup { }


[UpdateInGroup(typeof(DamagePipelineGroup))]
public partial class DamageFilterGroup : ComponentSystemGroup {}

[UpdateAfter(typeof(DamageFilterGroup))]
[UpdateInGroup(typeof(DamagePipelineGroup))]
public partial class DamageModificationGroup : ComponentSystemGroup {}

[UpdateAfter(typeof(DamageModificationGroup))]
[UpdateInGroup(typeof(DamagePipelineGroup))]
public partial class DamageApplyGroup : ComponentSystemGroup {}

[UpdateInGroup(typeof(DamagePipelineGroup))]
[UpdateAfter(typeof(DamageApplyGroup))]
public partial class DamagePostProcessGroup : ComponentSystemGroup { }
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(GamePlaySystemGroup))]
public partial class EventSystemGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(EventSystemGroup))]
public partial class EventRequestGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(EventSystemGroup))]
[UpdateAfter(typeof(EventRequestGroup))]
public partial class EventResponseGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(EventSystemGroup))]
[UpdateAfter(typeof(EventResponseGroup))]
public partial class EventCleanupGroup : ComponentSystemGroup { }
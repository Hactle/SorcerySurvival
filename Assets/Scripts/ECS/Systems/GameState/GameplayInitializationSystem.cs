using Unity.Entities;

[UpdateInGroup(typeof(InitializationEventGroup))]
partial struct GameplayGroupInitializationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameState>();
        state.RequireForUpdate<LevelInitializationEvent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (gameState.CurrentState != GameStateType.Playing)
            return;

        var gameplayGroup =
            state.World.GetExistingSystemManaged<GamePlaySystemGroup>();

        if (gameplayGroup != null && !gameplayGroup.Enabled)
        {
            gameplayGroup.Enabled = true;
        }
    }
}
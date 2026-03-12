using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(AbilityInitializeGroup))]
partial struct ProjectileTargetAssignmentSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        NativeList<Entity> playerProjectiles = new NativeList<Entity>(Allocator.Temp);
        NativeList<Entity> enemyProjectiles = new NativeList<Entity>(Allocator.Temp);
        NativeList<EnemyDistance> enemies = new NativeList<EnemyDistance>(Allocator.Temp);

        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

        #region FillingLists
        foreach (var (
            enemyTag,
            enemyEntity) in SystemAPI.Query<
                EnemyTag>().WithEntityAccess())
        {
            var transform = SystemAPI.GetComponent<LocalTransform>(enemyEntity);
            float dist = math.distancesq(transform.Position, playerPosition);

            enemies.Add(new EnemyDistance
            {
                Entity = enemyEntity,
                Distance = dist
            });
        }

        foreach (var (
            projectileTag,
            projectileEntity) in SystemAPI.Query<
                ProjectileTag>()
                .WithNone<TargetDirection>()
                .WithEntityAccess())
        {
            var side = SystemAPI.GetComponent<EntitySide>(projectileEntity).Value;

            if (side == Sides.Player)
                playerProjectiles.Add(projectileEntity);
            else if (side == Sides.Enemy)
                enemyProjectiles.Add(projectileEntity);
        }
        #endregion

        enemies.Sort();

        int enemyCount = enemies.Length;

        for (int i = 0; i < playerProjectiles.Length; i++)
        {
            int targetIndex;

            if(i < enemyCount)
                targetIndex = i;
            else
                targetIndex = i % enemyCount;

            var targetEnemy = enemies[targetIndex].Entity;
            var targetTransform = SystemAPI.GetComponent<LocalTransform>(targetEnemy);

            float3 direction = math.normalize(targetTransform.Position - playerPosition);

            ecb.AddComponent(playerProjectiles[i], new TargetDirection
            {
                Value = direction,
            });
        }

        for (int i = 0; i < enemyProjectiles.Length; i++)
        {
            float3 direction = math.normalize(playerPosition - SystemAPI.GetComponent<LocalTransform>(enemyProjectiles[i]).Position);

            ecb.AddComponent(enemyProjectiles[i], new TargetDirection
            {
                Value = direction,
            });
        }
        ecb.Playback(state.EntityManager);
    }
}

struct EnemyDistance : System.IComparable<EnemyDistance>
{
    public Entity Entity;
    public float Distance;

    public int CompareTo(EnemyDistance other)
    {
        return Distance.CompareTo(other.Distance);
    }
}

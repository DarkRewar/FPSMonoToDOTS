using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[RequireMatchingQueriesForUpdate]
public partial struct EnemyMovementSystem : ISystem
{
    public EntityQuery EnemyAspectQuery;

    static readonly float3 Forward = new(0, 1, 0);

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Enemy>();

        EnemyAspectQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<Enemy>()
            .WithAspect<EnemyAspect>()
            .Build(state.EntityManager);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var player = state.EntityManager.GetComponentData<LocalTransform>(SystemAPI.GetSingletonEntity<Player>());
        float3 playerPos = player.Position;
        state.Dependency = MoveEnemies(state.Dependency, ref state, playerPos);
    }

    [BurstCompile]
    private JobHandle MoveEnemies(JobHandle dependency, ref SystemState state, float3 playerPos)
    {
        var enemyMobJob = new EnemyMoveJob
        {
            Target = playerPos,
            Forward = Forward
        };
        return enemyMobJob.ScheduleParallel(EnemyAspectQuery, dependency);
    }

    [BurstCompile]
    public partial struct EnemyMoveJob : IJobEntity
    {
        public float3 Target;
        public float3 Forward;

        public readonly void Execute(EnemyAspect enemyAspect)
        {
            var pos = enemyAspect.Position;
            var dir = Target - pos;
            var dirNormalized = math.normalizesafe(dir);
            enemyAspect.Rotation = quaternion.LookRotation(dirNormalized, Forward);
            enemyAspect.Velocity = -dirNormalized;
        }
    }
}
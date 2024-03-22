using Unity.Entities;
using Unity.Transforms;

public partial struct EnemyRenderSystem : ISystem
{
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Enemy>();
        state.RequireForUpdate<EnemyAnimatedGO>();
    }

    void OnUpdate(ref SystemState state)
    {
        foreach ((var localTransform, var enemyAnimatedGO) in
            SystemAPI.Query<LocalTransform, EnemyAnimatedGO>().WithAll<Enemy>())
        {
            var pos = localTransform.Position;
            enemyAnimatedGO.EnemyController.transform.position = pos;
            enemyAnimatedGO.EnemyController.transform.rotation = localTransform.Rotation;
            enemyAnimatedGO.EnemyController.IsMoving(true);

        }
    }
}

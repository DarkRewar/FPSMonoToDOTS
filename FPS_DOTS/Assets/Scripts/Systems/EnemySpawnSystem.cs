using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemySpawnSystem : ISystem
{
    public static int EnemyCount;
    private bool _isInitialized;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        EnemyCount = 0;
        _isInitialized = false;
        state.RequireForUpdate<Config>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        if (!_isInitialized)
        {
            state.EntityManager.Instantiate(config.PlayerPrefab);
            _isInitialized = true;
        }

        foreach ((var cooldown, var entity) in SystemAPI.Query<Cooldown>().WithAll<EnemySpawn>().WithEntityAccess())
        {
            if (cooldown.TimeLeft <= 0)
            {
                SpawnWave(ref state, ecb, config);
                ecb.SetComponent(entity, new Cooldown { TimeLeft = 1f / config.EnemySpawnRate });
            }
            else
            {
                ecb.SetComponent(entity, new Cooldown { TimeLeft = cooldown.TimeLeft - SystemAPI.Time.DeltaTime });
            }
        }

        ecb.Playback(state.EntityManager);
    }

    private void SpawnWave(ref SystemState state, EntityCommandBuffer ecb, Config config)
    {
        var configEntity = SystemAPI.GetSingletonEntity<Config>();
        var configManaged = state.EntityManager.GetComponentObject<ConfigManaged>(configEntity);
        var player = state.EntityManager.GetComponentData<LocalTransform>(SystemAPI.GetSingletonEntity<Player>());

        for (var i = 0; i < config.EnemySpawnCount; i++)
        {
            if (EnemyCount >= config.EnemyMaxCount) break;

            ++EnemyCount;

            var enemyEntity = ecb.Instantiate(configManaged.EnemyEntityPrefab);

            var circle = config.EnemySpawnDistance * UnityEngine.Random.insideUnitCircle.normalized;
            var newPos = player.Position + new float3(circle.x, 0, circle.y);
            newPos.y = 0;

            var localTransform = state.EntityManager.GetComponentData<LocalTransform>(configManaged.EnemyEntityPrefab);
            localTransform.Position = newPos;
            ecb.SetComponent(enemyEntity, localTransform);
            ecb.RemoveComponent<MaterialMeshInfo>(enemyEntity);

            var enemyController = GameObject.Instantiate(configManaged.EnemyPrefab, newPos, Quaternion.identity);
            ecb.AddComponent(enemyEntity, new EnemyAnimatedGO
            {
                EnemyController = enemyController
            });
        }
    }
}

using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public float EnemySpawnDistance = 10;
    public float EnemySpawnRate = 1;
    public int EnemySpawnCount = 1;
    public int EnemyMaxCount = 500;
    public Cooldown EnemySpawnCooldown;
    public MobAuthoring EnemyPrefab;
    public PlayerAuthoring PlayerPrefab;

    public int EnemyCount = 0;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var player = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic);
            var enemy = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic);
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new Config
            {
                EnemySpawnRate = authoring.EnemySpawnRate,
                EnemySpawnCount = authoring.EnemySpawnCount,
                EnemySpawnDistance = authoring.EnemySpawnDistance,
                EnemyMaxCount = authoring.EnemyMaxCount,
                PlayerPrefab = player,
                EnemyPrefab = enemy
            });
            AddComponentObject(entity, new ConfigManaged
            {
                EnemyEntityPrefab = GetEntity(authoring.EnemyPrefab.Entity, TransformUsageFlags.Dynamic),
                EnemyPrefab = authoring.EnemyPrefab.Render,
            });

            AddComponent(entity, new Cooldown { TimeLeft = 1f / authoring.EnemySpawnRate });
            AddComponent(entity, new EnemySpawn());
        }
    }
}
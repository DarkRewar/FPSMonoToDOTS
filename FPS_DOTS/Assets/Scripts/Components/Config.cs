using Unity.Entities;

public struct Config : IComponentData
{
    public float EnemySpawnDistance;
    public float EnemySpawnRate;
    public int EnemySpawnCount;
    public int EnemyMaxCount;
    public Entity PlayerPrefab;
    public Entity EnemyPrefab;
}

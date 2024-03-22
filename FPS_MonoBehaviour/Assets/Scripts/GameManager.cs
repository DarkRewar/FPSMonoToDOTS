using BaseTool;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    protected float _enemySpawnDistance = 10;

    [SerializeField]
    protected float _enemySpawnRate = 1;

    [SerializeField]
    protected float _enemySpawnCount = 1;

    [SerializeField]
    protected float _enemyMaxCount = 500;

    [SerializeField]
    protected Cooldown _enemySpawnCooldown;

    [SerializeField]
    protected EnemyController _enemyPrefab;

    [SerializeField]
    protected PlayerController _playerController;

    public int EnemyCount = 0;
    public Text EnemyCountText;

    private void Start()
    {
        _playerController = PlayerController.Instance;
    }

    void Update()
    {
        _enemySpawnCooldown.Update();

        if (_enemySpawnCooldown.IsReady)
        {
            SpawnWave();
        }
    }

    private void LateUpdate()
    {
        EnemyCountText.text = $"Enemy Count: {EnemyCount}";
    }

    private void SpawnWave()
    {
        _enemySpawnCooldown = 1f / _enemySpawnRate;
        _enemySpawnCooldown.Reset();

        for (int i = 0; i < _enemySpawnCount; i++)
        {
            if (EnemyCount >= _enemyMaxCount) break;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var circle = _enemySpawnDistance * Random.insideUnitCircle.normalized;
        var newPos = _playerController.transform.position + new Vector3(circle.x, 0, circle.y);

        var enemy = Instantiate(_enemyPrefab, newPos.ChangeY(0), Quaternion.identity);
        enemy.Target = _playerController;

        ++EnemyCount;
    }
}

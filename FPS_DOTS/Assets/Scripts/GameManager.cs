using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int EnemyCount = 0;
    public Text EnemyCountText;

    void LateUpdate()
    {
        EnemyCount = EnemySpawnSystem.EnemyCount;
        EnemyCountText.text = $"Enemy Count: {EnemyCount}";
    }
}

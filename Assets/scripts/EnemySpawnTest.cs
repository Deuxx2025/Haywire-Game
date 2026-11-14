
using UnityEngine;

public class EnemySpawnTest : MonoBehaviour
{
    public EnemyData enemyData;
    public Transform player;

    [ContextMenu("Spawn Test Enemy")]
    public void SpawnTestEnemy()
    {
        if (enemyData == null || enemyData.prefab == null)
        {
            Debug.LogError("Assign R4LPH_Data and its prefab first.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Assign the Player Transform first.");
            return;
        }

        Vector3 spawnPosition = player.position + Vector3.right * 5f;

        GameObject instance = Instantiate(
            enemyData.prefab,
            spawnPosition,
            Quaternion.identity
        );

        Enemy enemy = instance.GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError("The spawned prefab has no Enemy component.");
            Destroy(instance);
            return;
        }

        enemy.Initialize(enemyData, player);
    }
}

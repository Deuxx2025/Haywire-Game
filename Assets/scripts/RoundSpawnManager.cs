
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSpawnManager : MonoBehaviour
{
    [Header("Setup")]
    public RoundSettings roundSettings;
    public Transform player;

    [Header("Spawn Settings")]
    [Min(0.1f)] public float spawnInterval = 3f;
    [Min(1)] public int maxAliveEnemies =10;
    public float yOffsetEnemySpawn = 0f;
    [Min(0f)] public float interRoundDelay = 3f;

    [Header("Runtime")]
    public int currentRound = 1;
    public int enemiesSpawnedThisRound = 0;

    private readonly List<GameObject> activeEnemies = new();
    private Coroutine roundRoutine;

    void Start()
    {
        if (roundSettings == null || player == null)
        {
            Debug.LogError(
                "RoundSpawnManager needs Round Settings and Player assigned."
            );
            enabled = false;
            return;
        }

        roundRoutine = StartCoroutine(RunRounds());
    }

    IEnumerator RunRounds()
    {
        while (true)
        {
            RoundSettings.RoundConfig config =
                roundSettings.GetRoundConfig(currentRound);

            if (config == null)
            {
                Debug.LogError(
                    $"No settings configured for round {currentRound}. " +
                    "Check that your RoundSettings asset contains rounds 1-30."
                );
                yield break;
            }

            int targetCount = roundSettings.GetEnemyCount(currentRound);
            enemiesSpawnedThisRound = 0;

            Debug.Log(
                $"Starting Round {currentRound}: {targetCount} enemies."
            );

            while (enemiesSpawnedThisRound < targetCount)
            {
                CleanEnemyList();

                // Wait if the arena already has enough enemies alive.
                while (ActiveEnemyCount() >= maxAliveEnemies)
                {
                    yield return null;
                    CleanEnemyList();
                }

                if (!SpawnEnemy(config))
                    yield break;

                enemiesSpawnedThisRound++;

                if (enemiesSpawnedThisRound < targetCount)
                    yield return new WaitForSeconds(spawnInterval);
            }

            // Finish the round only after every spawned enemy is defeated.
            while (ActiveEnemyCount() > 0)
            {
                CleanEnemyList();
                yield return null;
            }

            Debug.Log($"Round {currentRound} complete!");

            yield return new WaitForSeconds(interRoundDelay);

            currentRound++;
        }
    }

    bool SpawnEnemy(RoundSettings.RoundConfig config)
    {
        EnemyData selectedData = ChooseEnemy(config);

        if (selectedData == null || selectedData.prefab == null)
        {
            Debug.LogError(
                $"No valid enemy with a positive spawn weight for round " +
                $"{currentRound}. Check the Enemy Weights list."
            );
            return false;
        }

        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("RoundSpawnManager could not find Main Camera.");
            return false;
        }

        // Spawn just off either horizontal edge of the screen.
        float screenX = Random.value < 0.5f
            ? -100f
            : Screen.width + 100f;

        float worldY = player.position.y + yOffsetEnemySpawn;

        float screenY = cam.WorldToScreenPoint(
            new Vector3(player.position.x, worldY, 0f)
        ).y;

        float cameraDistance = Mathf.Abs(cam.transform.position.z);

        Vector3 spawnPosition = cam.ScreenToWorldPoint(
            new Vector3(screenX, screenY, cameraDistance)
        );

        spawnPosition.z = 0f;

        GameObject instance = Instantiate(
            selectedData.prefab,
            spawnPosition,
            Quaternion.identity
        );

        Enemy enemy = instance.GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError(
                $"{selectedData.enemyName} prefab has no Enemy component."
            );

            Destroy(instance);
            return false;
        }

        enemy.Initialize(selectedData, player);
        activeEnemies.Add(instance);

        Debug.Log($"Spawned {selectedData.enemyName} in Round {currentRound}.");

        return true;
    }

    EnemyData ChooseEnemy(RoundSettings.RoundConfig config)
    {
        int totalWeight = 0;

        foreach (RoundSettings.EnemyWeight entry in config.enemyWeights)
        {
            if (entry.enemyData != null &&
                entry.enemyData.prefab != null &&
                entry.weight > 0)
            {
                totalWeight += entry.weight;
            }
        }

        if (totalWeight <= 0)
            return null;

        int roll = Random.Range(0, totalWeight);

        foreach (RoundSettings.EnemyWeight entry in config.enemyWeights)
        {
            if (entry.enemyData == null ||
                entry.enemyData.prefab == null ||
                entry.weight <= 0)
            {
                continue;
            }

            roll -= entry.weight;

            if (roll < 0)
                return entry.enemyData;
        }

        return null;
    }

    void CleanEnemyList()
    {
        activeEnemies.RemoveAll(
            enemy => enemy == null || !enemy.activeInHierarchy
        );
    }

    int ActiveEnemyCount()
    {
        int count = 0;

        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
                count++;
        }

        return count;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject wolfPrefab;
    // Thêm các enemy khác sau
    // public GameObject goblinPrefab;
    // public GameObject skeletonPrefab;

    [Header("Spawn Settings")]
    public Transform player;
    public float spawnInterval = 3f;
    public int maxEnemies = 30;
    public float spawnDistance = 12f;
    public float spawnVariation = 3f;

    [Header("Wave Settings")]
    public bool useWaveSystem = false;
    public int enemiesPerWave = 8;
    public float timeBetweenWaves = 5f;

    [Header("Difficulty Scaling")]
    public bool enableDifficultyScaling = true;
    public float difficultyIncreaseInterval = 30f;
    public float spawnIntervalDecrease = 0.1f;
    public float minSpawnInterval = 0.8f;

    private float spawnTimer;
    private float difficultyTimer;
    private int currentWave = 0;
    private bool isSpawningWave = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        spawnTimer = spawnInterval;
        difficultyTimer = difficultyIncreaseInterval;
    }

    void Update()
    {
        if (player == null) return;

        // Xóa enemies đã bị destroy khỏi list
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (useWaveSystem)
        {
            HandleWaveSpawning();
        }
        else
        {
            HandleContinuousSpawning();
        }

        if (enableDifficultyScaling)
        {
            HandleDifficultyScaling();
        }
    }

    void HandleContinuousSpawning()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && activeEnemies.Count < maxEnemies)
        {
            SpawnWolf();
            spawnTimer = spawnInterval;
        }
    }

    void HandleWaveSpawning()
    {
        if (!isSpawningWave && activeEnemies.Count == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        isSpawningWave = true;
        currentWave++;

        Debug.Log($"🐺 Wave {currentWave} - Spawning {enemiesPerWave} wolves!");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnWolf();
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawningWave = false;
    }

    void HandleDifficultyScaling()
    {
        difficultyTimer -= Time.deltaTime;

        if (difficultyTimer <= 0)
        {
            spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - spawnIntervalDecrease);
            maxEnemies += 3;

            if (useWaveSystem)
            {
                enemiesPerWave += 2;
            }

            difficultyTimer = difficultyIncreaseInterval;
            Debug.Log($"⚡ Difficulty increased! Spawn: {spawnInterval}s, Max: {maxEnemies}");
        }
    }

    void SpawnWolf()
    {
        if (wolfPrefab == null)
        {
            Debug.LogWarning("Wolf Prefab chưa được gán!");
            return;
        }

        Vector2 spawnPos = GetRandomSpawnPosition();
        GameObject wolf = Instantiate(wolfPrefab, spawnPos, Quaternion.identity);
        wolf.tag = "Enemy";
        activeEnemies.Add(wolf);
    }

    Vector2 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = spawnDistance + Random.Range(-spawnVariation, spawnVariation);

        Vector2 offset = new Vector2(
            Mathf.Cos(angle) * distance,
            Mathf.Sin(angle) * distance
        );

        return (Vector2)player.position + offset;
    }

    public void SpawnWolfAtPosition(Vector2 position)
    {
        if (wolfPrefab == null) return;

        GameObject wolf = Instantiate(wolfPrefab, position, Quaternion.identity);
        wolf.tag = "Enemy";
        activeEnemies.Add(wolf);
    }

    public int GetActiveEnemyCount()
    {
        return activeEnemies.Count;
    }

    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, spawnDistance);

            Gizmos.color = new Color(0, 1, 1, 0.3f);
            Gizmos.DrawWireSphere(player.position, spawnDistance + spawnVariation);
            Gizmos.DrawWireSphere(player.position, spawnDistance - spawnVariation);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneEnemyConfig
    {
        public string sceneName;
        public GameObject normalEnemy1Prefab;  // Quái thường 1
        public GameObject normalEnemy2Prefab;  // Quái thường 2
        public GameObject bossPrefab;          // Boss (Tiger/Eagle/Crocodile)
        public float bossSpawnChance = 0.15f;  // 15% cơ hội spawn boss
    }

    [Header("Scene Configurations")]
    [Tooltip("Scene 1: 2 quái thường + Tiger Boss | Scene 2: 2 quái thường + Eagle Boss | Scene 3: 2 quái thường + Crocodile Boss")]
    public List<SceneEnemyConfig> sceneConfigs = new List<SceneEnemyConfig>();

    [Header("Current Scene")]
    [Tooltip("Chọn scene hiện tại (0-2)")]
    public int currentSceneIndex = 0;

    [Header("Spawn Settings")]
    public Transform player;
    public Camera mainCamera;
    public float spawnInterval = 3f;
    public int maxEnemies = 30;

    [Header("Camera Spawn Settings")]
    [Tooltip("Spawn trong vùng camera")]
    public bool spawnInCameraBounds = true;
    [Tooltip("Tỷ lệ vùng camera để spawn (0.5 = 50% kích thước camera)")]
    [Range(0.3f, 0.9f)]
    public float cameraSpawnSizeRatio = 0.7f;

    [Header("Map Bounds (Giới hạn vùng spawn)")]
    [Tooltip("Bật để giới hạn spawn trong map")]
    public bool useMapBounds = true;
    [Tooltip("Giới hạn trái của map")]
    public float mapMinX = -20f;
    [Tooltip("Giới hạn phải của map")]
    public float mapMaxX = 20f;
    [Tooltip("Giới hạn dưới của map")]
    public float mapMinY = -20f;
    [Tooltip("Giới hạn trên của map")]
    public float mapMaxY = 20f;

    [Header("Legacy Spawn Settings (nếu không dùng camera)")]
    public float spawnDistance = 12f;
    public float spawnVariation = 3f;

    [Header("Wave Settings")]
    public bool useWaveSystem = false;
    public int enemiesPerWave = 8;
    public float timeBetweenWaves = 5f;

    [Header("Boss Settings")]
    public int maxBossesAtOnce = 1;
    public float bossSpawnCooldown = 20f;
    private float bossSpawnTimer = 0f;

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
    private int currentBossCount = 0;

    void Start()
    {
        // Tìm player
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // Tìm camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("⚠️ Không tìm thấy Main Camera! Sẽ dùng spawn theo khoảng cách.");
                spawnInCameraBounds = false;
            }
        }

        spawnTimer = spawnInterval;
        difficultyTimer = difficultyIncreaseInterval;
        bossSpawnTimer = bossSpawnCooldown;

        // Validate scene config
        if (sceneConfigs.Count == 0)
        {
            Debug.LogError("⚠️ Chưa thiết lập SceneEnemyConfig! Vui lòng thêm 3 scene configs.");
        }
    }

    void Update()
    {
        if (player == null) return;

        // Xóa enemies đã bị destroy khỏi list
        CleanupDestroyedEnemies();

        // Update boss spawn cooldown
        if (bossSpawnTimer > 0)
        {
            bossSpawnTimer -= Time.deltaTime;
        }

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

    void CleanupDestroyedEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }

        // Đếm lại số boss còn sống
        currentBossCount = 0;
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null && enemy.CompareTag("Boss"))
            {
                currentBossCount++;
            }
        }
    }

    void HandleContinuousSpawning()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && activeEnemies.Count < maxEnemies)
        {
            SpawnRandomEnemy();
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

        Debug.Log($"🌊 Wave {currentWave} - Spawning {enemiesPerWave} enemies!");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnRandomEnemy();
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

            // Tăng cơ hội spawn boss theo độ khó
            if (currentSceneIndex < sceneConfigs.Count)
            {
                sceneConfigs[currentSceneIndex].bossSpawnChance =
                    Mathf.Min(0.3f, sceneConfigs[currentSceneIndex].bossSpawnChance + 0.02f);
            }

            difficultyTimer = difficultyIncreaseInterval;
            Debug.Log($"⚡ Difficulty increased! Spawn: {spawnInterval}s, Max: {maxEnemies}");
        }
    }

    void SpawnRandomEnemy()
    {
        if (currentSceneIndex >= sceneConfigs.Count)
        {
            Debug.LogError($"❌ Scene index {currentSceneIndex} không hợp lệ!");
            return;
        }

        SceneEnemyConfig config = sceneConfigs[currentSceneIndex];

        // Kiểm tra có spawn boss không
        bool shouldSpawnBoss = UnityEngine.Random.value < config.bossSpawnChance
                               && currentBossCount < maxBossesAtOnce
                               && bossSpawnTimer <= 0
                               && config.bossPrefab != null;

        if (shouldSpawnBoss)
        {
            SpawnBoss(config);
        }
        else
        {
            SpawnNormalEnemy(config);
        }
    }

    void SpawnNormalEnemy(SceneEnemyConfig config)
    {
        GameObject enemyPrefab = null;

        // Random chọn 1 trong 2 quái thường
        if (UnityEngine.Random.value < 0.5f && config.normalEnemy1Prefab != null)
        {
            enemyPrefab = config.normalEnemy1Prefab;
        }
        else if (config.normalEnemy2Prefab != null)
        {
            enemyPrefab = config.normalEnemy2Prefab;
        }
        else if (config.normalEnemy1Prefab != null)
        {
            enemyPrefab = config.normalEnemy1Prefab;
        }

        if (enemyPrefab == null)
        {
            Debug.LogWarning($"⚠️ Scene {currentSceneIndex} không có normal enemy prefab!");
            return;
        }

        Vector2 spawnPos = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.tag = "Enemy";
        activeEnemies.Add(enemy);
    }

    void SpawnBoss(SceneEnemyConfig config)
    {
        if (config.bossPrefab == null)
        {
            Debug.LogWarning($"⚠️ Scene {currentSceneIndex} không có boss prefab!");
            return;
        }

        Vector2 spawnPos = GetRandomSpawnPosition();
        GameObject boss = Instantiate(config.bossPrefab, spawnPos, Quaternion.identity);
        boss.tag = "Boss";  // Tag là "Boss" thay vì "Enemy"
        activeEnemies.Add(boss);
        currentBossCount++;
        bossSpawnTimer = bossSpawnCooldown;

        string bossName = config.bossPrefab.name;
        Debug.Log($"👑 BOSS SPAWNED: {bossName} at wave {currentWave}!");
    }

    Vector2 GetRandomSpawnPosition()
    {
        if (spawnInCameraBounds && mainCamera != null)
        {
            return GetRandomPositionInCamera();
        }
        else
        {
            return GetRandomPositionAroundPlayer();
        }
    }

    Vector2 GetRandomPositionInCamera()
    {
        // Lấy kích thước camera theo orthographic size
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Tính vùng spawn (nhỏ hơn vùng camera một chút)
        float halfSpawnWidth = (cameraWidth * cameraSpawnSizeRatio) / 2f;
        float halfSpawnHeight = (cameraHeight * cameraSpawnSizeRatio) / 2f;

        Vector3 cameraCenter = mainCamera.transform.position;

        // Vùng spawn dựa trên camera
        float camMinX = cameraCenter.x - halfSpawnWidth;
        float camMaxX = cameraCenter.x + halfSpawnWidth;
        float camMinY = cameraCenter.y - halfSpawnHeight;
        float camMaxY = cameraCenter.y + halfSpawnHeight;

        // Áp dụng map bounds: tính vùng giao (intersection)
        float spawnMinX = camMinX;
        float spawnMaxX = camMaxX;
        float spawnMinY = camMinY;
        float spawnMaxY = camMaxY;

        if (useMapBounds)
        {
            spawnMinX = Mathf.Max(camMinX, mapMinX);
            spawnMaxX = Mathf.Min(camMaxX, mapMaxX);
            spawnMinY = Mathf.Max(camMinY, mapMinY);
            spawnMaxY = Mathf.Min(camMaxY, mapMaxY);
        }

        // Kiểm tra nếu vùng giao hợp lệ
        if (spawnMinX >= spawnMaxX || spawnMinY >= spawnMaxY)
        {
            Debug.LogWarning("⚠️ Không có vùng giao giữa camera spawn và map bounds! Fallback đến trung tâm camera clamp vào map.");
            Vector2 fallback = new Vector2(cameraCenter.x, cameraCenter.y);
            if (useMapBounds)
            {
                fallback = ClampPositionToMapBounds(fallback);
            }
            return fallback;
        }

        // Random vị trí trong vùng giao
        float randomX = UnityEngine.Random.Range(spawnMinX, spawnMaxX);
        float randomY = UnityEngine.Random.Range(spawnMinY, spawnMaxY);

        return new Vector2(randomX, randomY);
    }

    Vector2 GetRandomPositionAroundPlayer()
    {
        // Spawn theo phương pháp cũ (vòng tròn xung quanh player)
        Vector2 spawnPos;
        int maxAttempts = 20;
        int attempts = 0;

        do
        {
            float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = spawnDistance + UnityEngine.Random.Range(-spawnVariation, spawnVariation);

            Vector2 offset = new Vector2(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance
            );

            spawnPos = (Vector2)player.position + offset;
            attempts++;

            // Nếu không dùng map bounds hoặc vị trí hợp lệ, break
            if (!useMapBounds || IsPositionInMapBounds(spawnPos))
            {
                break;
            }

        } while (attempts < maxAttempts);

        // Nếu thử quá nhiều lần, clamp vào trong map
        if (useMapBounds)
        {
            spawnPos = ClampPositionToMapBounds(spawnPos);
        }

        return spawnPos;
    }

    // Kiểm tra vị trí có nằm trong map bounds không
    bool IsPositionInMapBounds(Vector2 position)
    {
        return position.x >= mapMinX && position.x <= mapMaxX &&
               position.y >= mapMinY && position.y <= mapMaxY;
    }

    // Clamp vị trí vào trong map bounds
    Vector2 ClampPositionToMapBounds(Vector2 position)
    {
        return new Vector2(
            Mathf.Clamp(position.x, mapMinX, mapMaxX),
            Mathf.Clamp(position.y, mapMinY, mapMaxY)
        );
    }

    // Helper methods
    public void SpawnEnemyAtPosition(Vector2 position)
    {
        if (currentSceneIndex >= sceneConfigs.Count) return;

        SceneEnemyConfig config = sceneConfigs[currentSceneIndex];
        GameObject enemyPrefab = UnityEngine.Random.value < 0.5f ? config.normalEnemy1Prefab : config.normalEnemy2Prefab;

        if (enemyPrefab == null) return;

        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.tag = "Enemy";
        activeEnemies.Add(enemy);
    }

    public void SpawnBossAtPosition(Vector2 position)
    {
        if (currentSceneIndex >= sceneConfigs.Count) return;

        SceneEnemyConfig config = sceneConfigs[currentSceneIndex];
        if (config.bossPrefab == null) return;

        GameObject boss = Instantiate(config.bossPrefab, position, Quaternion.identity);
        boss.tag = "Boss";
        activeEnemies.Add(boss);
        currentBossCount++;
    }

    public int GetActiveEnemyCount()
    {
        return activeEnemies.Count;
    }

    public int GetActiveBossCount()
    {
        return currentBossCount;
    }

    public void SetScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < sceneConfigs.Count)
        {
            currentSceneIndex = sceneIndex;
            Debug.Log($"🗺️ Switched to Scene {sceneIndex + 1}");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ Map Bounds (vùng giới hạn map)
        if (useMapBounds)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3((mapMinX + mapMaxX) / 2f, (mapMinY + mapMaxY) / 2f, 0);
            Vector3 size = new Vector3(mapMaxX - mapMinX, mapMaxY - mapMinY, 0);
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = new Color(1, 0, 0, 0.1f);
            Gizmos.DrawCube(center, size);
        }

        if (spawnInCameraBounds && mainCamera != null)
        {
            // Vẽ vùng spawn theo camera
            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            float spawnWidth = cameraWidth * cameraSpawnSizeRatio;
            float spawnHeight = cameraHeight * cameraSpawnSizeRatio;

            Vector3 cameraCenter = mainCamera.transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cameraCenter, new Vector3(spawnWidth, spawnHeight, 0));

            Gizmos.color = new Color(0, 1, 0, 0.1f);
            Gizmos.DrawCube(cameraCenter, new Vector3(spawnWidth, spawnHeight, 0));
        }
        else if (player != null)
        {
            // Vẽ vùng spawn theo player (phương pháp cũ)
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, spawnDistance);

            Gizmos.color = new Color(0, 1, 1, 0.3f);
            Gizmos.DrawWireSphere(player.position, spawnDistance + spawnVariation);
            Gizmos.DrawWireSphere(player.position, spawnDistance - spawnVariation);
        }
    }
}
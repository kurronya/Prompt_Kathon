using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over Settings")]
    public GameObject gameOverPanel;
    public UnityEngine.UI.Text gameOverText;

    [Header("Victory Settings")]
    public GameObject victoryPanel;
    public UnityEngine.UI.Text victoryQuoteText;
    public UnityEngine.UI.Text victoryAuthorText;

    [Header("Kill Counter UI")]
    public UnityEngine.UI.Text killCounterText;

    [Header("Game Stats")]
    public int killsToWin = 50;
    private int currentKills = 0;
    private bool gameEnded = false;

    [Header("Revive Settings")]
    public Button reviveButton;
    public bool canRevive = true;
    private bool hasRevived = false;
    public Transform spawnPoint;

    // ✅ THÊM REFERENCE TỚI PLAYER (quan trọng!)
    private GameObject playerReference;

    private List<VictoryQuote> victoryQuotes = new List<VictoryQuote>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeQuotes();
    }

    void Start()
    {
        // ✅ LƯU REFERENCE TỚI PLAYER NGAY TỪ ĐẦU
        playerReference = GameObject.FindGameObjectWithTag("Player");
        if (playerReference == null)
        {
            Debug.LogError("❌ KHÔNG TÌM THẤY PLAYER trong scene!");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        UpdateKillCounterUI();
        Enemy_Health.OnMonsterDefeated += OnEnemyKilledEvent;

        if (reviveButton != null)
        {
            reviveButton.onClick.AddListener(OnReviveButtonClicked);
        }
    }

    void OnDestroy()
    {
        Enemy_Health.OnMonsterDefeated -= OnEnemyKilledEvent;
    }

    void InitializeQuotes()
    {
        victoryQuotes.Add(new VictoryQuote
        {
            quote = "Khong co gi quy hon doc lap, tu do!",
            author = "Ho Chi Minh"
        });

        victoryQuotes.Add(new VictoryQuote
        {
            quote = "Song lau bang xac khong bang chet vinh cho To quoc!",
            author = "Tran Hung Dao"
        });

        victoryQuotes.Add(new VictoryQuote
        {
            quote = "Binh phi gia khong thang!",
            author = "Tran Hung Dao"
        });

        victoryQuotes.Add(new VictoryQuote
        {
            quote = "Dat nuoc Viet Nam la cua nguoi Viet Nam!",
            author = "Ly Thuong Kiet"
        });

        victoryQuotes.Add(new VictoryQuote
        {
            quote = "Thang cuoc roi ta hat ca len cho vui!",
            author = "Quang Trung"
        });

        victoryQuotes.Add(new VictoryQuote
        {
            quote = "Giac den nha dan phai danh!",
            author = "Le Loi"
        });

        victoryQuotes.Add(new VictoryQuote
        {
            quote = "Doan ket la suc manh!",
            author = "Ho Chi Minh"
        });

        victoryQuotes.Add(new VictoryQuote
        {
            quote = "Song bao lau, chien dau bao lau!",
            author = "Vo Nguyen Giap"
        });
    }

    private void OnEnemyKilledEvent(int expAmount)
    {
        OnEnemyKilled();
    }

    public void OnEnemyKilled()
    {
        if (gameEnded) return;

        currentKills++;
        Debug.Log("Kills: " + currentKills + "/" + killsToWin);

        UpdateKillCounterUI();

        if (currentKills >= killsToWin)
        {
            Victory();
        }
    }

    private void UpdateKillCounterUI()
    {
        if (killCounterText != null)
        {
            killCounterText.text = currentKills + "/" + killsToWin;
        }
    }

    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        Time.timeScale = 0;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\n\nKills: " + currentKills + "/" + killsToWin;
        }

        if (reviveButton != null)
        {
            if (canRevive && !hasRevived)
            {
                reviveButton.gameObject.SetActive(true);
            }
            else
            {
                reviveButton.gameObject.SetActive(false);
            }
        }

        Debug.Log("Game Over!");
    }

    void OnReviveButtonClicked()
    {
        if (!canRevive || hasRevived) return;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (AdManager.Instance != null)
        {
            AdManager.Instance.PlayReviveAd(OnReviveAdComplete);
        }
        else
        {
            Debug.LogError("AdManager not found!");
        }
    }

    void OnReviveAdComplete()
    {
        Debug.Log("Ad complete! Reviving player...");

        hasRevived = true;
        gameEnded = false;

        RevivePlayer();
        Time.timeScale = 1;
    }

    // ✅ PHIÊN BẢN MỚI - SỬ DỤNG REFERENCE ĐÃ LƯU
    void RevivePlayer()
    {
        Debug.Log("=== BẮT ĐẦU HỒI SINH ===");

        // ✅ SỬ DỤNG REFERENCE ĐÃ LƯU THAY VÌ TÌM LẠI
        if (playerReference == null)
        {
            Debug.LogError("❌ PLAYER REFERENCE NULL! Thử tìm lại...");
            // Fallback: tìm trong tất cả objects (kể cả inactive)
            playerReference = FindInactiveObjectByTag("Player");
        }

        if (playerReference == null)
        {
            Debug.LogError("❌ VẪN KHÔNG TÌM THẤY PLAYER!");
            return;
        }

        Debug.Log("✅ Tìm thấy player: " + playerReference.name);

        // 1. BẬT LẠI PLAYER
        playerReference.SetActive(true);
        Debug.Log("✅ Player đã bật: " + playerReference.activeSelf);

        // 2. ĐẶT LẠI VỊ TRÍ
        if (spawnPoint != null)
        {
            playerReference.transform.position = spawnPoint.position;
            Debug.Log("✅ Spawn tại: " + spawnPoint.position);
        }
        else
        {
            playerReference.transform.position = new Vector3(0, 0, 0);
            Debug.LogWarning("⚠️ Không có SpawnPoint, spawn tại (0,0,0)");
        }

        // 3. RESET RIGIDBODY
        Rigidbody2D rb = playerReference.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            Debug.Log("✅ Reset Rigidbody");
        }

        // 4. BẬT LẠI COLLIDER
        Collider2D col = playerReference.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
            Debug.Log("✅ Bật Collider");
        }

        // 5. BẬT LẠI SPRITE RENDERER
        SpriteRenderer sprite = playerReference.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.enabled = true;
            Color color = sprite.color;
            color.a = 1f;
            sprite.color = color;
            Debug.Log("✅ Bật SpriteRenderer");
        }

        // 6. HỒI ĐẦY MÁU
        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
            Debug.Log("✅ Hồi máu: " + StatsManager.Instance.maxHealth);
        }

        // 7. UPDATE HEALTH UI
        PlayerHealth playerHealth = playerReference.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ChangeHealth(0);
            Debug.Log("✅ Cập nhật UI máu");
        }

        // 8. BẬT LẠI TẤT CẢ SCRIPTS
        MonoBehaviour[] scripts = playerReference.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
        Debug.Log("✅ Bật lại tất cả scripts");

        Debug.Log("=== HỒI SINH THÀNH CÔNG ===");
    }

    // ✅ HÀM TÌM OBJECT INACTIVE
    GameObject FindInactiveObjectByTag(string tag)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.CompareTag(tag) && t.hideFlags == HideFlags.None)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public void Victory()
    {
        if (gameEnded) return;

        gameEnded = true;
        Time.timeScale = 0;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        VictoryQuote randomQuote = victoryQuotes[UnityEngine.Random.Range(0, victoryQuotes.Count)];

        if (victoryQuoteText != null)
        {
            victoryQuoteText.text = "\"" + randomQuote.quote + "\"";
        }

        if (victoryAuthorText != null)
        {
            victoryAuthorText.text = "- " + randomQuote.author;
        }

        Debug.Log("Victory!");
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadHomeScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        UnityEngine.Application.Quit();
        Debug.Log("Game Quit!");
    }

    public void TestAddKill()
    {
        OnEnemyKilled();
    }
}

[System.Serializable]
public class VictoryQuote
{
    public string quote;
    public string author;
}
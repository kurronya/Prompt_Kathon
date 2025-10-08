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
    public int killsToWin = 1;
    private int currentKills = 0;
    private bool gameEnded = false;

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
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        UpdateKillCounterUI();

        // ✅ SUBSCRIBE TO ENEMY DEATH EVENT
        Enemy_Health.OnMonsterDefeated += OnEnemyKilledEvent;
    }

    void OnDestroy()
    {
        // ✅ UNSUBSCRIBE TO PREVENT MEMORY LEAK
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

    // ✅ HÀM MỚI - Nhận event từ Enemy_Health
    private void OnEnemyKilledEvent(int expAmount)
    {
        // Event trả về expAmount nhưng ta không cần dùng
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

        Debug.Log("Game Over!");
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;
    [Header("Level Settings")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 10;
    public float expMultiplier = 1.5f;
    [Header("UI References")]
    public Slider expBar;
    public UnityEngine.UI.Text levelText;
    public UnityEngine.UI.Text expText;

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
    }

    void Start()
    {
        UpdateUI();
        Enemy_Health.OnMonsterDefeated += AddExperience;
    }

    void OnDestroy()
    {
        // BỎ COMMENT dòng này:
        Enemy_Health.OnMonsterDefeated -= AddExperience;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddExperience(10);
            Debug.Log("Added 10 EXP!");
        }
    }

    public void AddExperience(int amount)
    {
        currentExp += amount;
        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    void LevelUp()
    {
        currentExp -= expToNextLevel;
        currentLevel++;

        expToNextLevel = Mathf.RoundToInt(expToNextLevel * expMultiplier);

        Debug.Log("LEVEL UP! Level " + currentLevel);

        Time.timeScale = 0;

        // Hiện quiz thay vì upgrade trực tiếp
        if (QuizUI.Instance != null)
        {
            QuizUI.Instance.ShowQuiz();
        }
        else
        {
            // Fallback nếu không có quiz
            if (LevelUpUI.Instance != null)
            {
                LevelUpUI.Instance.ShowUpgradeOptions();
            }
        }
    }

    void UpdateUI()
    {
        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }
        if (levelText != null)
        {
            levelText.text = "LV " + currentLevel;
        }
        if (expText != null)
        {
            expText.text = currentExp + " / " + expToNextLevel;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public Animator healthTextAnim;

    private void Start()
    {
        FindHealthUI();
        ResetHealth();
        UpdateHealthUI();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable()
    {
        if (StatsManager.Instance != null)
        {
            FindHealthUI();
            UpdateHealthUI();
            Debug.Log("✅ Player OnEnable - UI đã cập nhật!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindHealthUI();
        ResetHealth();
        UpdateHealthUI();
    }

    private void FindHealthUI()
    {
        if (healthText == null)
        {
            GameObject healthObj = GameObject.Find("HealthText");
            if (healthObj != null)
            {
                healthText = healthObj.GetComponent<TMP_Text>();
                healthTextAnim = healthObj.GetComponent<Animator>();
            }
            else
            {
                Debug.LogWarning("Khong tim thay HealthText UI trong scene!");
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null && StatsManager.Instance != null)
        {
            healthText.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
        }
    }

    public void ResetHealth()
    {
        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
            Debug.Log("Player health reset to: " + StatsManager.Instance.maxHealth);
        }
    }

    public void ChangeHealth(int amount)
    {
        if (StatsManager.Instance == null)
        {
            Debug.LogError("StatsManager is NULL!");
            return;
        }

        StatsManager.Instance.currentHealth += amount;

        if (healthTextAnim != null)
        {
            healthTextAnim.Play("TextUpdate");
        }

        UpdateHealthUI();

        if (StatsManager.Instance.currentHealth <= 0)
        {
            Debug.Log("❌ Player died!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }

            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
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
        ResetHealth(); // THÊM DÒNG NÀY - Reset máu khi start
        UpdateHealthUI();
        // Đăng ký event khi load scene mới
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindHealthUI();
        ResetHealth(); // THÊM DÒNG NÀY - Reset máu khi load scene mới
        UpdateHealthUI();
    }

    private void FindHealthUI()
    {
        // Tìm lại UI elements trong scene mới
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

    // HÀM MỚI - Reset máu về giá trị tối đa
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

        // Update UI
        if (healthTextAnim != null)
        {
            healthTextAnim.Play("TextUpdate");
        }
        UpdateHealthUI();

        // Kiểm tra chết
        if (StatsManager.Instance.currentHealth <= 0)
        {
            // Trigger Game Over
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký event để tránh memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
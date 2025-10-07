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
        UpdateHealthUI();

        // Đăng ký event khi load scene mới
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindHealthUI();
        UpdateHealthUI();
    }

    private void FindHealthUI()
    {
        // Tìm lại UI elements trong scene mới
        if (healthText == null)
        {
            GameObject healthObj = GameObject.Find("HealthText"); // Thay tên cho đúng
            if (healthObj != null)
            {
                healthText = healthObj.GetComponent<TMP_Text>();
                healthTextAnim = healthObj.GetComponent<Animator>();
            }
            else
            {
                Debug.LogWarning("Không tìm thấy HealthText UI trong scene!");
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
        }
    }

    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;

        if (healthTextAnim != null)
        {
            healthTextAnim.Play("TextUpdate");
        }

        UpdateHealthUI();

        if (StatsManager.Instance.currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký event để tránh memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Drop Settings")]
    public int expDropAmount = 10;

    // Event để thông báo khi enemy bị giết
    public static event System.Action<int> OnMonsterDefeated;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        // Debug để xem damage
        Debug.Log(gameObject.name + " took " + Mathf.Abs(amount) + " damage. HP: " + currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " defeated!");

        // Trigger event để GameManager và ExperienceManager biết
        if (OnMonsterDefeated != null)
        {
            OnMonsterDefeated.Invoke(expDropAmount);
        }

        // Destroy enemy
        Destroy(gameObject);
    }

    // Hàm để enemy nhận damage trực tiếp (alternative)
    public void TakeDamage(int damage)
    {
        ChangeHealth(-damage);
    }
}
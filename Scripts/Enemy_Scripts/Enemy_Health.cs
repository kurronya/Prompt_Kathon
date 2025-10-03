using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int expReward = 3;

    public delegate void MonsterDefeated(int exp);
    public static event MonsterDefeated OnMonsterDefeated;

    public int currentHealth;
    public int maxHealth;

    private void start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount) 
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }

        else if (currentHealth <= 0) 
        {
            OnMonsterDefeated(expReward);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [Header("Player Stats")]
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int damage = 10;
    public float speed = 5f;
    public float attackSpeed = 1f;
    public float weaponRange = 2f;
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;
    public float stunTime = 0.3f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentHealth = maxHealth;
    }
}
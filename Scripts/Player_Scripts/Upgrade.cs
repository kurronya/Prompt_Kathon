using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string upgradeName;
    public string description;
    public Sprite icon;
    public UpgradeType upgradeType;
    public float value;
    public int currentLevel;
    public int maxLevel = 5;

    public bool CanUpgrade()
    {
        return currentLevel < maxLevel;
    }

    public void ApplyUpgrade()
    {
        currentLevel++;
        switch (upgradeType)
        {
            case UpgradeType.MaxHealth:
                StatsManager.Instance.maxHealth += Mathf.RoundToInt(value);
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.ChangeHealth(Mathf.RoundToInt(value));
                    }
                }
                break;
            case UpgradeType.Damage:
                StatsManager.Instance.damage += Mathf.RoundToInt(value);
                break;
            case UpgradeType.AttackSpeed:
                // TODO: Thêm thuộc tính attack speed vào StatsManager
                // StatsManager.Instance.attackSpeed += value;
                Debug.LogWarning("AttackSpeed upgrade chưa được implement trong StatsManager");
                break;
            case UpgradeType.Speed:
                // TODO: Kiểm tra tên thuộc tính speed trong StatsManager của bạn
                // StatsManager.Instance.speed += value;
                Debug.LogWarning("Speed upgrade chưa được implement trong StatsManager");
                break;
            case UpgradeType.WeaponRange:
                StatsManager.Instance.weaponRange += value;
                break;
            case UpgradeType.KnockbackForce:
                StatsManager.Instance.knockbackForce += value;
                break;
        }
        Debug.Log("Upgraded " + upgradeName + " to Level " + currentLevel);
    }
}

public enum UpgradeType
{
    MaxHealth,
    Damage,
    AttackSpeed,
    Speed,
    WeaponRange,
    KnockbackForce
}
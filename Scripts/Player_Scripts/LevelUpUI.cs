using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public static LevelUpUI Instance;

    [Header("UI References")]
    public GameObject levelUpPanel;
    public GameObject upgradeButtonPrefab;
    public Transform upgradeButtonContainer;

    [Header("Upgrade Pool")]
    public List<Upgrade> allUpgrades = new List<Upgrade>();
    public int numberOfOptions = 3;

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
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }

        InitializeUpgrades();
    }

    void InitializeUpgrades()
    {
        allUpgrades.Add(new Upgrade
        {
            upgradeName = "Max Health",
            description = "Tang mau toi da +10",
            upgradeType = UpgradeType.MaxHealth,
            value = 10,
            maxLevel = 10
        });

        allUpgrades.Add(new Upgrade
        {
            upgradeName = "Damage",
            description = "Tang sat thuong +5",
            upgradeType = UpgradeType.Damage,
            value = 5,
            maxLevel = 10
        });

        allUpgrades.Add(new Upgrade
        {
            upgradeName = "Attack Speed",
            description = "Giam cooldown -0.2s",
            upgradeType = UpgradeType.AttackSpeed,
            value = 0.2f,
            maxLevel = 5
        });

        allUpgrades.Add(new Upgrade
        {
            upgradeName = "Movement Speed",
            description = "Tang toc do di chuyen +0.5",
            upgradeType = UpgradeType.Speed,
            value = 0.5f,
            maxLevel = 8
        });

        allUpgrades.Add(new Upgrade
        {
            upgradeName = "Weapon Range",
            description = "Tang tam vu khi +0.3",
            upgradeType = UpgradeType.WeaponRange,
            value = 0.3f,
            maxLevel = 5
        });

        allUpgrades.Add(new Upgrade
        {
            upgradeName = "Knockback Force",
            description = "Tang luc day lui +2",
            upgradeType = UpgradeType.KnockbackForce,
            value = 2f,
            maxLevel = 5
        });
    }

    public void ShowUpgradeOptions()
    {
        if (levelUpPanel == null || upgradeButtonContainer == null)
        {
            Debug.LogError("LevelUpPanel or Container is NULL!");
            return;
        }

        foreach (Transform child in upgradeButtonContainer)
        {
            Destroy(child.gameObject);
        }

        List<Upgrade> availableUpgrades = GetRandomUpgrades(numberOfOptions);

        foreach (Upgrade upgrade in availableUpgrades)
        {
            CreateUpgradeButton(upgrade);
        }

        levelUpPanel.SetActive(true);
    }

    List<Upgrade> GetRandomUpgrades(int count)
    {
        List<Upgrade> available = new List<Upgrade>();

        foreach (Upgrade upgrade in allUpgrades)
        {
            if (upgrade.CanUpgrade())
            {
                available.Add(upgrade);
            }
        }

        List<Upgrade> selected = new List<Upgrade>();
        int actualCount = Mathf.Min(count, available.Count);

        for (int i = 0; i < actualCount; i++)
        {
            int randomIndex = Random.Range(0, available.Count);
            selected.Add(available[randomIndex]);
            available.RemoveAt(randomIndex);
        }

        return selected;
    }

    void CreateUpgradeButton(Upgrade upgrade)
    {
        if (upgradeButtonPrefab == null)
        {
            Debug.LogError("Upgrade Button Prefab is NULL!");
            return;
        }

        GameObject buttonObj = Instantiate(upgradeButtonPrefab, upgradeButtonContainer);

        Transform nameTextTransform = buttonObj.transform.Find("NameText");
        if (nameTextTransform != null)
        {
            Text nameText = nameTextTransform.GetComponent<Text>();
            if (nameText != null)
            {
                nameText.text = upgrade.upgradeName + " (Lv." + (upgrade.currentLevel + 1) + ")";
            }
        }

        Transform descTextTransform = buttonObj.transform.Find("DescriptionText");
        if (descTextTransform != null)
        {
            Text descText = descTextTransform.GetComponent<Text>();
            if (descText != null)
            {
                descText.text = upgrade.description;
            }
        }

        Transform iconTransform = buttonObj.transform.Find("Icon");
        if (iconTransform != null && upgrade.icon != null)
        {
            Image icon = iconTransform.GetComponent<Image>();
            if (icon != null)
            {
                icon.sprite = upgrade.icon;
            }
        }

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnUpgradeSelected(upgrade));
        }
    }

    public void OnUpgradeSelected(Upgrade upgrade)
    {
        upgrade.ApplyUpgrade();

        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
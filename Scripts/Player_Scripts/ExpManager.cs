using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public int level;
    public int currentExp;
    public int expToNextLevel = 10;
    public float expMultiplier = 1.2f; //Add 20% more EXP to level each
    public Slider expSlider;
    public TMP_Text currentLevelText;


    private void Start() 
    {
        UpdateUI();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            GainExperience(2);
        }
    }

    private void OnEnable() 
    {
        Enemy_Health.OnMonsterDefeated += GainExperience;
    }

    private void OnDisable()
    {
        Enemy_Health.OnMonsterDefeated -= GainExperience;
    }

    public void GainExperience(int amount)
    {
        currentExp += amount;
        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }
    
    private void LevelUp() 
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * expMultiplier);
    }

    public void UpdateUI() 
    {
        expSlider.maxValue = expToNextLevel;
        expSlider.value = currentExp;
        currentLevelText.text = "Level: " + level;
    }
}

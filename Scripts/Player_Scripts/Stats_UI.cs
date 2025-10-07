using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stats_UI : MonoBehaviour
{
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;
    private bool statsOpen = false;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button exitBattleButton;

    [Header("Scene Settings")]
    [SerializeField] private string exitSceneName = "MainMenu";

    private void Start()
    {
        UpdateAllStats();

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseStatsUI);
        }
        if (exitBattleButton != null)
        {
            exitBattleButton.onClick.AddListener(OnExitBattleClicked);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleStats"))
        {
            if (statsOpen)
            {
                CloseStatsUI();
            }
            else
            {
                OpenStatsUI();
            }
        }
    }

    private void OpenStatsUI()
    {
        Time.timeScale = 0;
        UpdateAllStats();
        statsCanvas.alpha = 1;
        statsCanvas.blocksRaycasts = true;
        statsCanvas.interactable = true;
        statsOpen = true;
    }

    private void CloseStatsUI()
    {
        Time.timeScale = 1;
        UpdateAllStats();
        statsCanvas.alpha = 0;
        statsCanvas.blocksRaycasts = false;
        statsCanvas.interactable = false;
        statsOpen = false;
    }

    private void OnExitBattleClicked()
    {
        Time.timeScale = 1;
        SaveGameData();
        SceneManager.LoadScene(exitSceneName);
    }

    private void SaveGameData()
    {
        // Lưu stats hiện tại
        PlayerPrefs.SetInt("PlayerHealth", StatsManager.Instance.currentHealth);
        PlayerPrefs.SetInt("PlayerDamage", StatsManager.Instance.damage);
        PlayerPrefs.SetFloat("PlayerSpeed", StatsManager.Instance.speed); // SỬA ĐÂY
        PlayerPrefs.Save();
        Debug.Log("Game data saved!");
    }

    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "TAN CONG: " + StatsManager.Instance.damage;
    }

    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "TOC DO: " + StatsManager.Instance.speed; // Sửa text cho đúng
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
    }

    private void OnDestroy()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(CloseStatsUI);
        }
        if (exitBattleButton != null)
        {
            exitBattleButton.onClick.RemoveListener(OnExitBattleClicked);
        }
    }
}
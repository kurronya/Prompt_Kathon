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
    [SerializeField] private Button closeButton; // Nút Close (dấu X)
    [SerializeField] private Button exitBattleButton; // Nút "THOÁT CHIẾN"

    [Header("Scene Settings")]
    [SerializeField] private string exitSceneName = "MainMenu"; // Tên scene muốn chuyển đến

    private void Start()
    {
        UpdateAllStats();

        // Gán sự kiện cho các nút
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

    // Hàm mở Stats UI
    private void OpenStatsUI()
    {
        Time.timeScale = 0;
        UpdateAllStats();
        statsCanvas.alpha = 1;
        statsCanvas.blocksRaycasts = true;
        statsCanvas.interactable = true;
        statsOpen = true;
    }

    // Hàm đóng Stats UI (cho nút Close)
    private void CloseStatsUI()
    {
        Time.timeScale = 1;
        UpdateAllStats();
        statsCanvas.alpha = 0;
        statsCanvas.blocksRaycasts = false;
        statsCanvas.interactable = false;
        statsOpen = false;
    }

    // Hàm thoát chiến (cho nút THOÁT CHIẾN)
    private void OnExitBattleClicked()
    {
        // Trả lại time scale về bình thường trước khi chuyển scene
        Time.timeScale = 1;

        // Lưu dữ liệu và chuyển scene
        SaveGameData();
        SceneManager.LoadScene(exitSceneName);
    }

    // Lưu dữ liệu (tùy chọn)
    private void SaveGameData()
    {
        // Lưu stats hiện tại
        PlayerPrefs.SetInt("PlayerHealth", StatsManager.Instance.currentHealth);
        PlayerPrefs.SetInt("PlayerDamage", StatsManager.Instance.damage);
        PlayerPrefs.SetInt("PlayerSpeed", StatsManager.Instance.speed);
        PlayerPrefs.Save();

        Debug.Log("Game data saved!");
    }

    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "TAN CONG: " + StatsManager.Instance.damage;
    }

    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "TAN CONG: " + StatsManager.Instance.speed;
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
    }

    // Cleanup khi destroy
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
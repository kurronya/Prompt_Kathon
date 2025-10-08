using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public GameObject videoPanel;
    public RawImage videoDisplay;
    public Button skipButton;
    public Text skipButtonText;

    [Header("Ad Settings")]
    public float adDuration = 5f;
    private bool isPlayingAd = false;
    private System.Action onAdComplete;

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
        if (videoPanel != null)
        {
            videoPanel.SetActive(false);
        }

        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
            skipButton.onClick.AddListener(SkipAd);
        }

        // Setup video player
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    public void PlayReviveAd(System.Action callback)
    {
        if (isPlayingAd) return;

        onAdComplete = callback;
        StartCoroutine(PlayAdCoroutine());
    }

    IEnumerator PlayAdCoroutine()
    {
        isPlayingAd = true;

        if (videoPanel != null)
        {
            videoPanel.SetActive(true);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Prepare();

            // Đợi video sẵn sàng
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }

            videoPlayer.Play();
        }

        // Đếm ngược thời gian
        float timeRemaining = adDuration;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.unscaledDeltaTime;

            if (skipButtonText != null)
            {
                skipButtonText.text = "Skip (" + Mathf.Ceil(timeRemaining) + "s)";
            }

            yield return null;
        }

        // Hiện nút skip sau 5 giây
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(true);
            skipButtonText.text = "Skip Ad";
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video prepared!");
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished!");
        CompleteAd();
    }

    void SkipAd()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        CompleteAd();
    }

    void CompleteAd()
    {
        isPlayingAd = false;

        if (videoPanel != null)
        {
            videoPanel.SetActive(false);
        }

        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
        }

        // Gọi callback
        if (onAdComplete != null)
        {
            onAdComplete.Invoke();
            onAdComplete = null;
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnVideoPrepared;
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}
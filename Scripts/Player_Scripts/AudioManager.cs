using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip homeMusic;
    public AudioClip battleMusic;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private string currentSceneName;

    void Awake()
    {
        // Singleton pattern - chỉ có 1 AudioManager duy nhất
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tạo AudioSource nếu chưa có
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        // Thiết lập AudioSource
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
    }

    void Start()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Phát nhạc cho scene hiện tại
        PlayMusicForCurrentScene();
    }

    void OnDestroy()
    {
        // Unsubscribe để tránh memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    void PlayMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Nếu đang phát nhạc của scene này rồi thì không cần đổi
        if (currentSceneName == sceneName && musicSource.isPlaying)
        {
            return;
        }

        currentSceneName = sceneName;

        // Chọn nhạc dựa trên tên scene
        AudioClip clipToPlay = null;

        if (sceneName == "HomeScenes" || sceneName == "MainMenu")
        {
            clipToPlay = homeMusic;
        }
        else if (sceneName == "CombatZone" || sceneName.Contains("Battle"))
        {
            clipToPlay = battleMusic;
        }

        // Phát nhạc
        if (clipToPlay != null)
        {
            PlayMusic(clipToPlay);
        }
        else
        {
            Debug.LogWarning("No music assigned for scene: " + sceneName);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null!");
            return;
        }

        // Nếu đang phát clip này rồi thì không cần restart
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.Play();
        Debug.Log("Now playing: " + clip.name);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    // Play sound effect (SFX)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Điều chỉnh volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }

    // Toggle mute
    public void ToggleMute()
    {
        musicSource.mute = !musicSource.mute;
        sfxSource.mute = !sfxSource.mute;
    }
}
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Volume")]
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 0.85f;
    [SerializeField] [Range(0f, 1f)] private float menuMusicVolume = 0.55f;
    [SerializeField] [Range(0f, 1f)] private float gameplayMusicVolume = 0.3f;
    [SerializeField] [Range(0f, 2f)] private float paddleHitVolumeMultiplier = 1.25f;

    [Header("Clips")]
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip paddleHitClip;
    [SerializeField] private AudioClip goalClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip gameplayMusicClip;

    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<AudioManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        EnsureAudioSources();
    }

    private void EnsureAudioSources()
    {
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }

        sfxSource.volume = sfxVolume;
        musicSource.volume = menuMusicVolume;
    }

    public void PlayButtonClick()
    {
        PlaySfx(buttonClickClip);
    }

    public void PlayPaddleHit()
    {
        PlaySfx(paddleHitClip, paddleHitVolumeMultiplier);
    }

    public void PlayGoal()
    {
        PlaySfx(goalClip);
    }

    public void PlayWin()
    {
        PlaySfx(winClip);
    }

    public void PlayLose()
    {
        PlaySfx(loseClip);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusicClip, menuMusicVolume);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusicClip, gameplayMusicVolume);
    }

    private void PlaySfx(AudioClip clip)
    {
        PlaySfx(clip, 1f);
    }

    private void PlaySfx(AudioClip clip, float volumeMultiplier)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume * volumeMultiplier);
    }

    private void PlayMusic(AudioClip clip, float volume)
    {
        if (clip == null || musicSource == null)
        {
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }
}

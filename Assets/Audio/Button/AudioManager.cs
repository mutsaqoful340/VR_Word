using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    const string BGM_VOL = "BGM_VOLUME";
    const string SFX_VOL = "SFX_VOLUME";

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
            return;
        }

        LoadVolume();
    }

    void LoadVolume()
    {
        bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOL, 1f);
        sfxSource.volume = PlayerPrefs.GetFloat(SFX_VOL, 1f);
    }

    public void SetBGMVolume(float value)
    {
        bgmSource.volume = value;
        PlayerPrefs.SetFloat(BGM_VOL, value);
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat(SFX_VOL, value);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
}

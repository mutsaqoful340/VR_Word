using UnityEngine;
using UnityEngine.UI;

public class AudioSliderUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);
    }

    public void OnBGMSlider(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    public void OnSFXSlider(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
}

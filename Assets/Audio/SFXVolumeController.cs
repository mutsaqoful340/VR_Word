using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRSFXSlider : MonoBehaviour, IPointerClickHandler
{
    public AudioMixer audioMixer;
    public Slider slider;
    public float step = 0.1f;

    void Start()
    {
        SetVolume(slider.value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            slider.value = Mathf.Clamp(slider.value + step, slider.minValue, slider.maxValue);
        else
            slider.value = Mathf.Clamp(slider.value - step, slider.minValue, slider.maxValue);

        SetVolume(slider.value);
    }

    // 🔥 WAJIB PUBLIC supaya bisa dipanggil Slider
    public void SetVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f); // anti Log10(0)
        Debug.Log("SFX slider value: " + value);

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
    }
}

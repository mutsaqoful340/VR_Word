using UnityEngine;
using System.Collections;

public class AudioPanelToggle : MonoBehaviour
{
    public RectTransform settingsPanel;
    public RectTransform audioPanel;

    public float duration = 0.25f;
    public float sideRotateY = 12f;

    CanvasGroup settingsCG;
    CanvasGroup audioCG;

    Quaternion centerRot;
    Quaternion sideRot;

    void Awake()
    {
        settingsCG = settingsPanel.GetComponent<CanvasGroup>();
        audioCG = audioPanel.GetComponent<CanvasGroup>();

        centerRot = Quaternion.identity;

        // AUDIO PANEL DI KIRI → ROTATE POSITIF
        sideRot = Quaternion.Euler(0, sideRotateY, 0);

        audioCG.alpha = 0f;
        audioCG.interactable = false;
        audioCG.blocksRaycasts = false;

        audioPanel.localRotation = sideRot;
    }


    public void OpenAudio()
    {
        StopAllCoroutines();
        StartCoroutine(FadeAudio(0f, 1f));
    }

    public void CloseAudio()
    {
        StopAllCoroutines();
        StartCoroutine(FadeAudio(1f, 0f));
    }

    IEnumerator FadeAudio(float from, float to)
    {
        float t = 0f;
        audioCG.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / duration);
            audioCG.alpha = Mathf.Lerp(from, to, p);
            yield return null;
        }

        audioCG.alpha = to;
        audioCG.interactable = to > 0.9f;
        audioCG.blocksRaycasts = to > 0.9f;
    }
}

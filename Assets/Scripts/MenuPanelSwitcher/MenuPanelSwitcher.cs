using UnityEngine;
using System.Collections;

public class MenuPanelSwitcher : MonoBehaviour
{
    public RectTransform mainMenu;
    public RectTransform settingsMenu;

    [Header("Movement")]
    public float slideDistance = 0.6f;
    public float duration = 0.3f;

    [Header("Curve Toward Player")]
    public float sideRotateY = 15f;   // seberapa ngadep ke player

    Vector3 mainStartPos;
    Vector3 settingsStartPos;

    Quaternion centerRot;
    Quaternion leftRot;
    Quaternion rightRot;

    CanvasGroup mainCG;
    CanvasGroup settingsCG;

    void Awake()
    {
        mainStartPos = mainMenu.localPosition;
        settingsStartPos = settingsMenu.localPosition;

        centerRot = Quaternion.identity;
        leftRot = Quaternion.Euler(0, sideRotateY, 0);    // panel kiri ngadep kanan (player)
        rightRot = Quaternion.Euler(0, -sideRotateY, 0);  // panel kanan ngadep kiri

        mainCG = mainMenu.GetComponent<CanvasGroup>();
        settingsCG = settingsMenu.GetComponent<CanvasGroup>();

        settingsCG.alpha = 0f;
        settingsCG.interactable = false;
        settingsCG.blocksRaycasts = false;

        settingsMenu.localRotation = rightRot;
    }

    public void OpenSettings()
    {
        StopAllCoroutines();

        StartCoroutine(AnimatePanel(
            mainMenu,
            mainStartPos,
            mainStartPos + Vector3.left * slideDistance,
            centerRot,
            leftRot,
            0.6f
        ));

        StartCoroutine(AnimatePanel(
            settingsMenu,
            settingsStartPos,
            mainStartPos,
            rightRot,
            centerRot,
            1f
        ));
    }

    public void CloseSettings()
    {
        StopAllCoroutines();

        StartCoroutine(AnimatePanel(
            mainMenu,
            mainMenu.localPosition,
            mainStartPos,
            mainMenu.localRotation,
            centerRot,
            1f
        ));

        StartCoroutine(AnimatePanel(
            settingsMenu,
            settingsMenu.localPosition,
            settingsStartPos,
            settingsMenu.localRotation,
            rightRot,
            0f
        ));
    }

    IEnumerator AnimatePanel(
        RectTransform panel,
        Vector3 fromPos,
        Vector3 toPos,
        Quaternion fromRot,
        Quaternion toRot,
        float targetAlpha
    )
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        float startAlpha = cg.alpha;

        cg.interactable = false;
        cg.blocksRaycasts = false;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / duration);

            panel.localPosition = Vector3.Lerp(fromPos, toPos, p);
            panel.localRotation = Quaternion.Slerp(fromRot, toRot, p);
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, p);

            yield return null;
        }

        panel.localPosition = toPos;
        panel.localRotation = toRot;
        cg.alpha = targetAlpha;

        cg.interactable = targetAlpha > 0.9f;
        cg.blocksRaycasts = targetAlpha > 0.9f;
    }
}

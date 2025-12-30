using UnityEngine;
using TMPro;
using System.Collections;

public class NPCAutoHelpText : MonoBehaviour
{
    public PlayerIdleDetector idleDetector;
    public TextMeshProUGUI textUI;

    public float showDuration = 3f;
    public float cooldown = 10f;
    public float fadeSpeed = 4f;

    private bool isCoolingDown = false;

    [TextArea]
    public string[] helpTexts =
    {
        "Tenang… kita pelan-pelan ya",
        "Tidak apa-apa, coba lagi",
        "Lihat sekeliling dulu ya"
    };

    void Start()
    {
        if (textUI != null)
        {
            textUI.gameObject.SetActive(true);
            SetAlpha(0f);
        }
    }

    void Update()
    {
        if (idleDetector == null || textUI == null) return;

        if (idleDetector.IsPlayerIdle() && !isCoolingDown)
        {
            ShowHelp();
        }
    }

    void ShowHelp()
    {
        textUI.text = helpTexts[Random.Range(0, helpTexts.Length)];

        StopAllCoroutines();
        StartCoroutine(FadeText(1f));

        isCoolingDown = true;
        idleDetector.ResetIdle();

        Invoke(nameof(HideText), showDuration);
        Invoke(nameof(ResetCooldown), cooldown);
    }

    void HideText()
    {
        StopAllCoroutines();
        StartCoroutine(FadeText(0f));
    }

    void ResetCooldown()
    {
        isCoolingDown = false;
    }

    IEnumerator FadeText(float targetAlpha)
    {
        float startAlpha = textUI.color.a;

        while (!Mathf.Approximately(startAlpha, targetAlpha))
        {
            startAlpha = Mathf.MoveTowards(
                startAlpha,
                targetAlpha,
                fadeSpeed * Time.deltaTime
            );

            SetAlpha(startAlpha);
            yield return null;
        }
    }

    void SetAlpha(float a)
    {
        Color c = textUI.color;
        c.a = a;
        textUI.color = c;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NPCDialogController : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogPanel;
    public TextMeshProUGUI npcText;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;
    public GameObject questionButton;

    [Header("Dialog Stage")]
    public DialogStage currentStage = DialogStage.Stage1;

    [Header("Text Effect")]
    public float charPerSecond = 30f;
    public float extraReadTime = 1.5f;

    // pertanyaan hilang permanen selama game
    bool[] usedQuestionsStage1 = new bool[4];
    bool[] usedQuestionsStage2 = new bool[4];

    Coroutine typingCoroutine;

    void Start()
    {
        dialogPanel.SetActive(false);
        questionButton.SetActive(false);
        npcText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            questionButton.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questionButton.SetActive(false);
            CloseDialog();
        }
    }

    public void OpenDialog()
    {
        dialogPanel.SetActive(true);
        ResetAllOptions(); // <<< WAJ

        if (currentStage == DialogStage.Stage1)
            ShowStage1();
        else
            ShowStage2();
    }

    // ================= STAGE 1 =================
    void ShowStage1()
    {
        PlayAnswer("Ada yang ingin kamu tanyakan?");

        optionTexts[0].text = "Kok aku bisa masuk ke sini?";
        optionTexts[1].text = "Aku harus ngapain?";
        optionTexts[2].text = "Kamu siapa?";
        optionTexts[3].text = "Tidak jadi";

        SetupOption(0, usedQuestionsStage1, "Kadang dunia ini muncul saat kamu lelah.");
        SetupOption(1, usedQuestionsStage1, "Fokus satu langkah kecil dulu ya.");
        SetupOption(2, usedQuestionsStage1, "Aku Arisa. Aku akan menemanimu.");

        SetupCloseButton(3);
    }

    // ================= STAGE 2 =================
    void ShowStage2()
    {
        PlayAnswer("Kita sudah sampai. Mau lanjut?");

        optionTexts[0].text = "Apa yang harus aku lakukan?";
        optionTexts[1].text = "Bagaimana kalau aku salah?";
        optionTexts[2].text = "";
        optionTexts[3].text = "Tidak jadi";

        SetupOption(0, usedQuestionsStage2, "Susun huruf-huruf itu jadi kata yang benar.");
        SetupOption(1, usedQuestionsStage2, "Tidak apa-apa salah. Kita coba lagi.");

        optionButtons[2].gameObject.SetActive(false);
        SetupCloseButton(3);
    }

    // ================= CORE =================
    void SetupOption(int index, bool[] usedArray, string answer)
    {
        // PAKSA button aktif dulu
        optionButtons[index].gameObject.SetActive(true);
        optionButtons[index].onClick.RemoveAllListeners();

        if (usedArray[index])
        {
            optionButtons[index].gameObject.SetActive(false);
            return;
        }

        optionButtons[index].onClick.AddListener(() =>
        {
            usedArray[index] = true;   // permanen di stage ini
            optionButtons[index].gameObject.SetActive(false);
            PlayAnswer(answer);
        });
    }


    void SetupCloseButton(int index)
    {
        optionButtons[index].gameObject.SetActive(true);
        optionButtons[index].onClick.RemoveAllListeners();
        optionButtons[index].onClick.AddListener(CloseDialog);
    }

    // ================= TYPEWRITER =================
    void PlayAnswer(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeAndAutoHide(text));
    }

    IEnumerator TypeAndAutoHide(string fullText)
    {
        npcText.text = "";
        float delay = 1f / charPerSecond;

        foreach (char c in fullText)
        {
            npcText.text += c;
            yield return new WaitForSeconds(delay);
        }

        float readTime = (fullText.Length / charPerSecond) + extraReadTime;
        yield return new WaitForSeconds(readTime);

        npcText.text = "";
    }

    public void SetStage(DialogStage stage)
    {
        currentStage = stage;
    }

    void CloseDialog()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        npcText.text = "";
        dialogPanel.SetActive(false);
    }

    void ResetAllOptions()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].onClick.RemoveAllListeners();
        }
    }
}

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

    [Header("NPC Movement")]
    public NPCMoveByWaypoints npcMover;

    bool[] usedQuestionsStage1 = new bool[4];
    bool[] usedQuestionsStage2 = new bool[2];
    bool[] usedQuestionsStage3 = new bool[3];
    bool[] usedQuestionsStage4 = new bool[2];
    bool[] usedQuestionsStage5 = new bool[2];

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
        ResetAllOptions();

        switch (currentStage)
        {
            case DialogStage.Stage1: ShowStage1(); break;
            case DialogStage.Stage2: ShowStage2(); break;
            case DialogStage.Stage3: ShowStage3(); break;
            case DialogStage.Stage4: ShowStage4(); break;
            case DialogStage.Stage5: ShowStage5(); break;
        }
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

    // ================= STAGE 3 =================
    void ShowStage3()
    {
        PlayAnswer("Kamu sudah sampai sejauh ini. Mau tanya apa lagi?");

        optionTexts[0].text = "Apa aku bisa menyelesaikannya?";
        optionTexts[1].text = "Kenapa rasanya sulit?";
        optionTexts[2].text = "Apa yang terjadi setelah ini?";
        optionTexts[3].text = "Tidak jadi";

        SetupOption(0, usedQuestionsStage3, "Bisa. Pelan-pelan pun tidak apa-apa.");
        SetupOption(1, usedQuestionsStage3, "Karena otakmu bekerja dengan caranya sendiri.");
        SetupOption(2, usedQuestionsStage3, "Kita lanjut ke tantangan berikutnya.");

        SetupCloseButton(3);
    }

    // ================= STAGE 4 =================
    void ShowStage4()
    {
        PlayAnswer("Kita sudah sampai di sini.");

        optionTexts[0].text = "AMBA";
        optionTexts[1].text = "Apa yang harus aku lakukan sekarang?";
        optionTexts[2].text = "";
        optionTexts[3].text = "Diam saja";

        SetupOption(0, usedQuestionsStage4, "Bukan akhir. Ini awal yang lebih tenang.");
        SetupOption(1, usedQuestionsStage4, "Nikmati dulu. Kamu sudah berusaha.");

        optionButtons[2].gameObject.SetActive(false);
        SetupCloseButton(3);
    }

    // ================= STAGE 5 =================
    void ShowStage5()
    {
        PlayAnswer("Di sini ada pistol.");

        optionTexts[0].text = "Pistol ini untuk apa?";
        optionTexts[1].text = "Terus gimana cara pakainya?";
        optionTexts[2].text = "";
        optionTexts[3].text = "Diam saja";

        SetupOption(0, usedQuestionsStage5, "Pistol ini akan membantumu fokus.");
        SetupOption(1, usedQuestionsStage5, "Arahkan, tarik napas, lalu tekan.");

        optionButtons[2].gameObject.SetActive(false);
        SetupCloseButton(3);
    }

    // ================= CORE =================
    void SetupOption(int index, bool[] usedArray, string answer)
    {
        optionButtons[index].gameObject.SetActive(true);
        optionButtons[index].onClick.RemoveAllListeners();

        if (index >= usedArray.Length)
            return;

        if (usedArray[index])
        {
            optionButtons[index].gameObject.SetActive(false);
            return;
        }

        optionButtons[index].onClick.AddListener(() =>
        {
            usedArray[index] = true;
            optionButtons[index].gameObject.SetActive(false);
            PlayAnswer(answer);

            // === SISTEM LAMA TETAP ===
            if (currentStage == DialogStage.Stage2 && AllStage2QuestionsUsed())
                StartCoroutine(MoveNpcAfterStage2());

            // === TAMBAHAN STAGE 4 ===
            if (currentStage == DialogStage.Stage4 && AllStage4QuestionsUsed())
                StartCoroutine(MoveNpcAfterStage4());
        });
    }

    void SetupCloseButton(int index)
    {
        optionButtons[index].gameObject.SetActive(true);
        optionButtons[index].onClick.RemoveAllListeners();
        optionButtons[index].onClick.AddListener(CloseDialog);
    }

    bool AllStage2QuestionsUsed()
    {
        foreach (bool used in usedQuestionsStage2)
            if (!used) return false;
        return true;
    }

    bool AllStage4QuestionsUsed()
    {
        foreach (bool used in usedQuestionsStage4)
            if (!used) return false;
        return true;
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

        yield return new WaitForSeconds(extraReadTime);
        npcText.text = "";
    }

    IEnumerator MoveNpcAfterStage2()
    {
        yield return new WaitForSeconds(extraReadTime + 0.3f);

        CloseDialog();
        questionButton.SetActive(false);

        if (npcMover != null)
            npcMover.MoveStage2ToStage3();
    }

    IEnumerator MoveNpcAfterStage4()
    {
        yield return new WaitForSeconds(extraReadTime + 0.3f);

        CloseDialog();
        questionButton.SetActive(false);

        if (npcMover != null)
            npcMover.MoveStage4ToStage5();
    }

    // ================= UTIL =================
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

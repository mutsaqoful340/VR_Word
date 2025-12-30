using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogController : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogPanel;
    public TextMeshProUGUI npcText;

    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;

    [Header("Hint Button")]
    public GameObject questionButton; // tombol ?

    private bool playerInRange = false;

    void Start()
    {
        dialogPanel.SetActive(false);
        questionButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player MASUK trigger NPC");
            playerInRange = true;
            questionButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player KELUAR trigger NPC");
            playerInRange = false;
            questionButton.SetActive(false);
            CloseDialog();
        }
    }

    // DIPANGGIL OLEH BUTTON ?
    public void OpenDialog()
    {
        dialogPanel.SetActive(true);

        npcText.text = "Ada yang ingin kamu tanyakan?";

        optionTexts[0].text = "Kok aku bisa masuk ke dalam game?";
        optionTexts[1].text = "Aku harus apa kalau ingin kembali?";
        optionTexts[2].text = "Apa kamu Mita?";
        optionTexts[3].text = "Tidak jadi.";

        optionButtons[0].onClick.RemoveAllListeners();
        optionButtons[0].onClick.AddListener(() => Answer(0));

        optionButtons[1].onClick.RemoveAllListeners();
        optionButtons[1].onClick.AddListener(() => Answer(1));

        optionButtons[2].onClick.RemoveAllListeners();
        optionButtons[2].onClick.AddListener(() => Answer(2));

        optionButtons[3].onClick.RemoveAllListeners();
        optionButtons[3].onClick.AddListener(CloseDialog);
    }


    void Answer(int index)
    {
        switch (index)
        {
            case 0:
                npcText.text = "Kadang game ini muncul saat kamu sedang lelah. Tidak apa-apa.";
                break;
            case 1:
                npcText.text = "Tenang… fokus satu hal kecil dulu ya.";
                break;
            case 2:
                npcText.text = "Aku Arisa. Aku akan menemanimu di sini.";
                break;
        }
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
    }
}

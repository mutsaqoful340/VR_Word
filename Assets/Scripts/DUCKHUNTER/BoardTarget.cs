using UnityEngine;

public class BoardTarget : MonoBehaviour
{
    [Header("Board Info")]
    public int boardIndex;
    public string hurufTarget;

    [Header("Audio")]
    public AudioClip suaraHuruf;

    private AudioSource audioSource;
    private bool isHit;
    private int missCount;

    private Round1Manager roundManager;
    private TaskUIManager taskUI;

    void Awake()
    {
        roundManager = FindObjectOfType<Round1Manager>();
        taskUI = FindObjectOfType<TaskUIManager>();

        // 🔊 AudioSource otomatis
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ActivateBoard()
    {
        missCount = 0;
        isHit = false;
        gameObject.SetActive(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;
        if (!collision.gameObject.CompareTag("Bullet")) return;

        isHit = true;

        // 📝 UI
        taskUI.ShowShootLetterTask(hurufTarget);

        // 🔊 SUARA SESUAI BOARD
        if (suaraHuruf != null)
            audioSource.PlayOneShot(suaraHuruf);

        roundManager.OnBoardCompleted(boardIndex, missCount);

        Destroy(collision.gameObject);
        gameObject.SetActive(false);
    }

    public void AddMiss()
    {
        if (isHit) return;
        missCount++;
    }
}

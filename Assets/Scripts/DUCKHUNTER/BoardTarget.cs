using UnityEngine;

public class BoardTarget : MonoBehaviour
{
    [Header("Board Info")]
    public int boardIndex;
    public string hurufTarget;   // "BA", "BB", "DA", dll

    private int missCount;
    private bool isHit;

    private Round1Manager roundManager;
    private TaskUIManager taskUI;

    void Awake()
    {
        // ✅ Ambil referensi sekali (lebih aman dari Start)
        roundManager = FindObjectOfType<Round1Manager>();
        taskUI = FindObjectOfType<TaskUIManager>();
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

        // 🔥 SATU-SATUNYA TEMPAT UPDATE UI HURUF
        taskUI.ShowShootLetterTask(hurufTarget);

        // (Optional) Debug buat bukti
        Debug.Log($"BOARD DITEMBAK: {gameObject.name} | HURUF: {hurufTarget}");

        // Lanjutkan logic ronde
        if (roundManager != null)
        {
            roundManager.OnBoardCompleted(boardIndex, missCount);
        }

        Destroy(collision.gameObject);
        gameObject.SetActive(false);
    }

    public void AddMiss()
    {
        if (isHit) return;
        missCount++;
    }
}

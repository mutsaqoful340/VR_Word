using UnityEngine;

public class BoardTarget : MonoBehaviour
{
    [Header("Board Info")]
    public int boardIndex;
    public string hurufTarget;   // "AB", "CD", dll

    private int missCount = 0;
    private bool isHit = false;
    private Round1Manager roundManager;
    private TaskUIManager taskUI;   // 🔹 TAMBAH

    void Start()
    {
        roundManager = FindObjectOfType<Round1Manager>();
        taskUI = FindObjectOfType<TaskUIManager>(); // 🔹 AMBIL UI
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

        if (collision.gameObject.CompareTag("Bullet"))
        {
            isHit = true;

            // 🔥 INI INTINYA
            taskUI.ShowShootLetterTask(hurufTarget);

            roundManager.OnBoardCompleted(boardIndex, missCount);

            Destroy(collision.gameObject);
            gameObject.SetActive(false);
        }
    }

    public void AddMiss()
    {
        if (isHit) return;
        missCount++;
    }
}

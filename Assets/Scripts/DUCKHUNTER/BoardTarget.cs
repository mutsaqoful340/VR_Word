using UnityEngine;

public class BoardTarget : MonoBehaviour
{
    public int boardIndex; // 0,1,2

    private int missCount = 0;
    private bool isHit = false;
    private Round1Manager roundManager;

    void Start()
    {
        roundManager = FindObjectOfType<Round1Manager>();
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

            roundManager.OnBoardCompleted(boardIndex, missCount);

            Destroy(collision.gameObject); // hancurkan peluru
            gameObject.SetActive(false);
        }
    }

    // Dipanggil dari Bullet kalau MISS
    public void AddMiss()
    {
        if (isHit) return;
        missCount++;
    }
}

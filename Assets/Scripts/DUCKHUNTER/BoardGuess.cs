using UnityEngine;

public class BoardGuess : MonoBehaviour
{
    public bool isCorrect;

    private bool isHit = false;
    private Round2Manager roundManager;

    void Start()
    {
        roundManager = FindObjectOfType<Round2Manager>();
    }

    public void ActivateBoard()
    {
        isHit = false;
        gameObject.SetActive(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;
        if (!collision.gameObject.CompareTag("Bullet")) return;

        isHit = true;

        roundManager.OnBoardSelected(isCorrect);

        Destroy(collision.gameObject);
        gameObject.SetActive(false); // board hilang walau salah
    }
}

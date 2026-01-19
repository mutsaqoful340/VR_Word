using UnityEngine;
using System.Collections;

public class GameRoundManager : MonoBehaviour
{
    public Round1Manager round1;
    public Round2Manager round2;
    public Round2Manager round3;

    public ResultPanel resultPanel;
    public TaskUIManager taskUI;

    [Header("End Game Prefab")]
    public GameObject endGamePrefab;
    public Transform spawnPoint;

    [Header("Audio")]
    public AudioClip endGameSFX;
    public float audioVolume = 1f;

    [Header("End Game Idle Effect")]
    public float idleSpeed = 2f;
    public float idleHeight = 0.05f;

    // 🔒 status grab
    private bool isGrabbed = false;

    void Start()
    {
        round1.gameObject.SetActive(true);
        round2.gameObject.SetActive(false);
        round3.gameObject.SetActive(false);

        round3.isLastRound = true;
        taskUI.ShowRound1Start();
    }

    public void OnRound1Finished(float finalScore)
    {
        StartCoroutine(TransitionToRound2(finalScore));
    }

    IEnumerator TransitionToRound2(float score)
    {
        resultPanel.ShowResult(score);
        taskUI.ShowRound1Complete();

        yield return new WaitForSeconds(3f);

        resultPanel.gameObject.SetActive(false);
        round1.gameObject.SetActive(false);

        round2.gameObject.SetActive(true);
        round2.StartRound();
    }

    public void OnRound2Finished()
    {
        StartCoroutine(TransitionToRound3());
    }

    IEnumerator TransitionToRound3()
    {
        yield return new WaitForSeconds(2f);

        round2.gameObject.SetActive(false);

        round3.gameObject.SetActive(true);
        round3.StartRound();
    }

    public void OnRound3Finished()
    {
        Debug.Log("🎉 GAME SELESAI TOTAL");

        taskUI.ShowAllRoundsComplete();

        if (endGameSFX != null)
        {
            Vector3 pos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
            AudioSource.PlayClipAtPoint(endGameSFX, pos, audioVolume);
        }

        round1.gameObject.SetActive(false);
        round2.gameObject.SetActive(false);
        round3.gameObject.SetActive(false);

        SpawnEndGamePrefab();
    }

    void SpawnEndGamePrefab()
    {
        if (endGamePrefab == null)
        {
            Debug.LogWarning("End Game Prefab belum di-assign!");
            return;
        }

        Vector3 pos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        GameObject obj = Instantiate(endGamePrefab, pos, rot);

        // ▶ Play Particle
        ParticleSystem ps = obj.GetComponentInChildren<ParticleSystem>();
        if (ps != null) ps.Play();

        // ▶ Idle Float
        StartCoroutine(IdleFloat(obj.transform));
    }

    IEnumerator IdleFloat(Transform obj)
    {
        Vector3 lastLocalPos = obj.localPosition;

        while (obj != null)
        {
            float yOffset = Mathf.Sin(Time.time * idleSpeed) * idleHeight;
            obj.localPosition = lastLocalPos + Vector3.up * yOffset;
            yield return null;
        }
    }


    // 🔗 PANGGIL INI DARI SCRIPT GRAB
    public void OnGrab()
    {
        isGrabbed = true;
    }

    public void OnRelease()
    {
        isGrabbed = false;
    }
}

using UnityEngine;
using System.Collections;

public class GameRoundManager : MonoBehaviour
{
    public Round1Manager round1;
    public Round2Manager round2;
    public ResultPanel resultPanel;
    public TaskUIManager taskUI;

    void Start()
    {
        round1.gameObject.SetActive(true);
        round2.gameObject.SetActive(false);

        taskUI.ShowRound1Start();
    }

    public void OnRound1Finished(float finalScore)
    {
        StartCoroutine(TransitionToRound2(finalScore));
    }

    IEnumerator TransitionToRound2(float score)
    {
        // Tampilkan hasil ronde 1
        resultPanel.ShowResult(score);
        taskUI.ShowRound1Complete();

        yield return new WaitForSeconds(3f);

        resultPanel.gameObject.SetActive(false);

        // Matikan round 1
        round1.gameObject.SetActive(false);

        // Aktifkan round 2
        round2.gameObject.SetActive(true);
        round2.StartRound();

        taskUI.ShowRound2Start();
    }

    public void OnRound2Finished()
    {
        Debug.Log("SEMUA RONDE SELESAI");
        // Bisa:
        // - Load scene ending
        // - Tampilkan final score
        // - Tampilkan menu
    }

}

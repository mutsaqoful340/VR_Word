using UnityEngine;
using System.Collections;

public class GameRoundManager : MonoBehaviour
{
    public Round1Manager round1;
    public Round2Manager round2;
    public Round2Manager round3;

    public ResultPanel resultPanel;
    public TaskUIManager taskUI;

    void Start()
    {
        round1.gameObject.SetActive(true);
        round2.gameObject.SetActive(false);
        round3.gameObject.SetActive(false);

        // 🔥 TANDAI ROUND 3 SEBAGAI TERAKHIR
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

        // 🔒 MATIKAN SEMUA ROUND
        round1.gameObject.SetActive(false);
        round2.gameObject.SetActive(false);
        round3.gameObject.SetActive(false);
    }
}

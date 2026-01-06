using UnityEngine;
using System.Collections;

public class GameRoundManager : MonoBehaviour
{
    public Round1Manager round1;
    public Round2Manager round2;
    public ResultPanel resultPanel;

    void Start()
    {
        round1.gameObject.SetActive(true);
        round2.gameObject.SetActive(false);
    }

    public void OnRound1Finished(float finalScore)
    {
        StartCoroutine(TransitionToRound2(finalScore));
    }

    IEnumerator TransitionToRound2(float score)
    {
        // tampilkan hasil round 1
        resultPanel.ShowResult(score);

        yield return new WaitForSeconds(3f);

        resultPanel.gameObject.SetActive(false);

        // matikan round 1
        round1.gameObject.SetActive(false);

        // aktifkan round 2
        round2.gameObject.SetActive(true);
        round2.StartRound();
    }
}

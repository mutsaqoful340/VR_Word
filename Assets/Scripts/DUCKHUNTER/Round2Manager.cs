using UnityEngine;
using System.Collections;

public class Round2Manager : MonoBehaviour
{
    public BoardGuess[] boards;
    public ResultPanel resultPanel;
    public GameRoundManager gameRoundManager;
    public TaskUIManager taskUI;

    private bool roundFinished = false;

    [Header("Round Config")]
    public bool isLastRound = false;   // 🔥 FLAG ROUND TERAKHIR


    public void StartRound()
    {
        roundFinished = false;
        StopAllCoroutines();
        resultPanel.ResetPanel();

        taskUI.ShowRound2Start();

        foreach (var board in boards)
        {
            board.gameObject.SetActive(true);
            board.ActivateBoard();
        }
    }

    public void OnBoardSelected(bool isCorrect)
    {
        if (roundFinished) return;

        StopAllCoroutines();

        if (isCorrect)
        {
            roundFinished = true;
            resultPanel.ShowMessageOnly("BENAR");
            taskUI.ShowAllRoundsComplete();
            StartCoroutine(EndRoundAfterDelay(2f));
        }
        else
        {
            resultPanel.ShowMessageOnly("SALAH");
            StartCoroutine(HidePanelAfterDelay(1.5f));
        }
    }

    IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultPanel.ResetPanel();
    }

    IEnumerator EndRoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var board in boards)
            board.gameObject.SetActive(false);

        // 🔥 Kuncinya
        if (isLastRound)
            gameRoundManager.OnRound3Finished(); // STOP total
        else
            gameRoundManager.OnRound2Finished(); // lanjut normal
    }

}

using UnityEngine;
using System.Collections;

public class Round2Manager : MonoBehaviour
{
    public BoardGuess[] boards;
    public ResultPanel resultPanel;
    public GameRoundManager gameRoundManager;
    public TaskUIManager taskUI;

    private bool roundFinished = false;

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
        {
            board.gameObject.SetActive(false);
        }

        // 🔔 BERITAHU GAME ROUND MANAGER
        gameRoundManager.OnRound2Finished();
    }
}

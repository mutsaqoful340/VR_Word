using UnityEngine;
using System.Collections;

public class Round2Manager : MonoBehaviour
{
    public BoardGuess[] boards; // 3 board
    public ResultPanel resultPanel;

    private bool roundFinished = false;

    public void StartRound()
    {
        roundFinished = false;
        resultPanel.ResetPanel();

        foreach (var board in boards)
        {
            board.ActivateBoard();
        }
    }

    public void OnBoardSelected(bool isCorrect)
    {
        if (roundFinished) return;

        if (isCorrect)
        {
            roundFinished = true;
            resultPanel.ShowMessageOnly("BENAR");
            StartCoroutine(EndRoundAfterDelay(2f));
        }
        else
        {
            // SALAH → tampil → reset → LANJUT MAIN
            resultPanel.ShowMessageOnly("SALAH");
            StartCoroutine(ResetPanelAfterDelay(1.5f));
        }
    }

    IEnumerator ResetPanelAfterDelay(float delay)
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

        // lanjut ke ending / scene lain di sini
    }
}

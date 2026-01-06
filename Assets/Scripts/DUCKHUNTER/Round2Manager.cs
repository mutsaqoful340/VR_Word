using UnityEngine;
using System.Collections;

public class Round2Manager : MonoBehaviour
{
    public BoardGuess[] boards;
    public ResultPanel resultPanel;

    private bool roundFinished = false;

    public void StartRound()
    {
        roundFinished = false;
        StopAllCoroutines();
        resultPanel.ResetPanel();

        foreach (var board in boards)
        {
            board.ActivateBoard();
        }
    }

    public void OnBoardSelected(bool isCorrect)
    {
        if (roundFinished) return;

        // 🔥 PENTING: hentikan coroutine lama
        StopAllCoroutines();

        if (isCorrect)
        {
            roundFinished = true;
            resultPanel.ShowMessageOnly("BENAR");
            StartCoroutine(EndRoundAfterDelay(2f));
        }
        else
        {
            // SALAH → PASTI MUNCUL
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

        // lanjut ending / scene
    }
}

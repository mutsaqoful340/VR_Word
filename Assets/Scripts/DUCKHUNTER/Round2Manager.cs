using UnityEngine;
using System.Collections;

public class Round2Manager : MonoBehaviour
{
    public BoardGuess[] boards;
    public ResultPanel resultPanel;
    public GameRoundManager gameRoundManager;
    public TaskUIManager taskUI;

    private bool roundFinished = false;
    private Coroutine hidePanelCoroutine; // untuk delay SALAH

    [Header("Round Config")]
    public bool isLastRound = false;

    [Header("UI Task Text")]
    public string startTaskText;
    public string completeTaskText;

    public void StartRound()
    {
        roundFinished = false;

        StopAllCoroutines(); // aman karena ronde baru
        hidePanelCoroutine = null;

        resultPanel.ResetPanel();

        // Tampilkan teks start ronde
        taskUI.ShowTask(startTaskText);

        foreach (var board in boards)
        {
            board.gameObject.SetActive(true);
            board.ActivateBoard();
        }
    }

    public void OnBoardSelected(bool isCorrect)
    {
        if (roundFinished) return;

        if (isCorrect)
        {
            roundFinished = true;

            // Hentikan semua coroutine saat ronde selesai
            StopAllCoroutines();
            hidePanelCoroutine = null;

            resultPanel.ShowMessageOnly("BENAR");

            if (isLastRound)
                taskUI.ShowAllRoundsComplete();
            else
                taskUI.taskText.text = completeTaskText;

            StartCoroutine(EndRoundAfterDelay(2f));
        }
        else
        {
            // Jangan hentikan semua coroutine, cukup hidePanelCoroutine lama
            if (hidePanelCoroutine != null)
                StopCoroutine(hidePanelCoroutine);

            // Tampilkan panel SALAH seketika
            resultPanel.ShowMessageOnly("SALAH");

            // Jalankan coroutine untuk reset panel
            hidePanelCoroutine = StartCoroutine(HidePanelAfterDelay(1.5f));
        }
    }

    IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultPanel.ResetPanel();
        hidePanelCoroutine = null;
    }

    IEnumerator EndRoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var board in boards)
            board.gameObject.SetActive(false);

        if (isLastRound)
            gameRoundManager.OnRound3Finished();
        else
            gameRoundManager.OnRound2Finished();
    }
}

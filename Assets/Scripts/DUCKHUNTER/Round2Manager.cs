using UnityEngine;

public class Round2Manager : MonoBehaviour
{
    public BoardGuess[] boards; // size = 3
    public ResultPanel resultPanel;

    private bool roundFinished = false;

    public void StartRound()
    {
        roundFinished = false;

        foreach (var board in boards)
        {
            board.ActivateBoard();
        }
    }

    public void OnBoardSelected(bool isCorrect)
    {
        if (roundFinished) return;
        roundFinished = true;

        if (isCorrect)
            resultPanel.ShowMessageOnly("BENAR");
        else
            resultPanel.ShowMessageOnly("SALAH");

        Invoke(nameof(EndRound), 2f);
    }

    void EndRound()
    {
        foreach (var board in boards)
        {
            board.gameObject.SetActive(false);
        }

        // kalau mau lanjut scene / ending, di sini
    }
}

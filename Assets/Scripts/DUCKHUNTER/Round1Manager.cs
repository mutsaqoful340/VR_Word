using UnityEngine;
using System.Collections;

public class Round1Manager : MonoBehaviour
{
    public BoardTarget[] boards;
    public ResultPanel resultPanel;
    public GameRoundManager gameRoundManager;
    public TaskUIManager taskUI;

    private int currentBoard = 0;
    private int[] boardScores;

    void Start()
    {
        boardScores = new int[boards.Length];
        StartRound();
    }

    public void StartRound()
    {
        currentBoard = 0;
        taskUI.ShowRound1Start();   // ✅ instruksi awal
        ActivateCurrentBoard();
    }

    void ActivateCurrentBoard()
    {
        if (currentBoard < boards.Length)
        {
            boards[currentBoard].ActivateBoard();
            // ❌ UI huruf TETAP dari BoardTarget (benar)
        }
    }

    public void OnBoardCompleted(int boardIndex, int missCount)
    {
        int score = ScoreCalculator.CalculateBoardScore(missCount);
        boardScores[boardIndex] = score;

        currentBoard++;

        if (currentBoard < boards.Length)
        {
            ActivateCurrentBoard();
        }
        else
        {
            // 🔥 JEDA SEBELUM END ROUND
            StartCoroutine(EndRoundAfterDelay(1.5f));
        }
    }

    IEnumerator EndRoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float finalScore = ScoreCalculator.CalculateFinalScore(boardScores);

        taskUI.ShowRound1Complete();
        gameRoundManager.OnRound1Finished(finalScore);
    }
}

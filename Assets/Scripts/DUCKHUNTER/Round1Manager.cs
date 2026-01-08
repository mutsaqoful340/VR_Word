using UnityEngine;

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
        taskUI.ShowRound1Start();
        ActivateCurrentBoard();
    }

    void ActivateCurrentBoard()
    {
        if (currentBoard < boards.Length)
        {
            // Aktifkan board (SISTEM LAMA TETAP)
            boards[currentBoard].ActivateBoard();

            // 🔥 UPDATE UTAMA:
            // UI ambil huruf langsung dari board
            taskUI.ShowShootLetterTask(boards[currentBoard].hurufTarget);
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
            EndRound();
        }
    }

    void EndRound()
    {
        float finalScore = ScoreCalculator.CalculateFinalScore(boardScores);

        taskUI.ShowRound1Complete();

        gameRoundManager.OnRound1Finished(finalScore);
    }
}

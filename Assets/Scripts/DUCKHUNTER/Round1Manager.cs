using UnityEngine;

public class Round1Manager : MonoBehaviour
{
    public BoardTarget[] boards; // size = 3
    public ResultPanel resultPanel;

    private int currentBoard = 0;
    private int[] boardScores = new int[3];

    void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        currentBoard = 0;
        ActivateCurrentBoard();
    }

    void ActivateCurrentBoard()
    {
        if (currentBoard < boards.Length)
        {
            boards[currentBoard].ActivateBoard();
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
        resultPanel.ShowResult(finalScore);
    }
}

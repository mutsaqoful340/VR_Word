using UnityEngine;

public static class ScoreCalculator
{
    public static int CalculateBoardScore(int missCount)
    {
        if (missCount == 0) return 100;
        if (missCount == 1) return 80;
        return 60;
    }

    public static float CalculateFinalScore(int[] boardScores)
    {
        // Bobot: 30% - 30% - 40%
        return (boardScores[0] * 0.3f) +
               (boardScores[1] * 0.3f) +
               (boardScores[2] * 0.4f);
    }
}

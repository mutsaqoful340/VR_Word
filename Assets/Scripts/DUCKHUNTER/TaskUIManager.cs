using TMPro;
using UnityEngine;

public class TaskUIManager : MonoBehaviour
{
    public TextMeshProUGUI taskText;

    // ======================
    // ROUND 1
    // ======================

    public void ShowRound1Start()
    {
        taskText.text =
            "RONDE 1\n" +
            "Selesaikan semua papan target";
    }

    public void ShowRound1Board(int boardIndex)
    {
        taskText.text =
            "RONDE 1\n" +
            $"TARGET {boardIndex + 1}\n" +
            "TEMBAK HURUF YANG BENAR";
    }

    public void ShowRound1Complete()
    {
        taskText.text =
            "<color=green>RONDE 1 SELESAI</color>\n" +
            "Menyiapkan ronde berikutnya...";
    }

    // ======================
    // ROUND 2
    // ======================

    public void ShowRound2Start()
    {
        taskText.text =
            "RONDE 2\n" +
            "Kesulitan meningkat";
    }

    // ======================
    // END GAME
    // ======================

    public void ShowAllRoundsComplete()
    {
        taskText.text =
            "<color=cyan>SEMUA RONDE SELESAI</color>";
    }
}

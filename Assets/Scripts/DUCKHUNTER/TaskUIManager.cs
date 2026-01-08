using UnityEngine;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public TextMeshProUGUI taskText;

    public void ShowRound1Start()
    {
        taskText.text = "Tembak semua papan dengan huruf didepan";
    }

    public void ShowRound1Complete()
    {
        taskText.text = "Round 1 Selesai";
    }

    public void ShowRound2Start()
    {
        taskText.text = "Tembak papan huruf BA";
    }

    public void ShowRound2Complete()
    {
        taskText.text = "Round 2 Selesai";
    }

    public void ShowRound3Start()
    {
        taskText.text = "Tembak bebek huruf bq";
    }

    public void ShowRound3Complete()
    {
        taskText.text = "Round 3 Selesai";
    }

    // 🔽 TAMBAHAN UNTUK FIX ERROR
    public void ShowRound1Board(int boardIndex)
    {
        taskText.text = $"Round 1 - Board {boardIndex + 1}";
    }

    public void ShowAllRoundsComplete()
    {
        taskText.text = "Semua ronde selesai!";
    }

    public void ShowShootLetterTask(string huruf)
    {
        taskText.text = $"Kamu menembak huruf {huruf}";
    }

}

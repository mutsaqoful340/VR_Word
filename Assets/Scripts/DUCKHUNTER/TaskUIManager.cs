using UnityEngine;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public TextMeshProUGUI taskText;

    public void ShowRound1Start()
    {
        taskText.text = "Shoot all boards with the letters in front of you";
    }

    public void ShowRound1Complete()
    {
        taskText.text = "Round 1 completed";
    }

    public void ShowRound2Start()
    {
        taskText.text = "Shoot the BA letter board";
    }

    public void ShowRound2Complete()
    {
        taskText.text = "Round 2 completed";
    }

    public void ShowRound3Start()
    {
        taskText.text = "Shoot the letter LI duck";
    }

    public void ShowRound3Complete()
    {
        taskText.text = "Round 3 completed";
    }

    public void ShowRound1Board(int boardIndex)
    {
        taskText.text = $"Round 1 - Board {boardIndex + 1}";
    }

    public void ShowAllRoundsComplete()
    {
        taskText.text = "All rounds completed";
    }

    public void ShowShootLetterTask(string letter)
    {
        Debug.Log("SHOW SHOOT LETTER CALLED WITH: " + letter);
        taskText.text = $"You shot the letter {letter}";
    }

    public void ShowTask(string text)
    {
        taskText.text = text;
    }
}

using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI messageText;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowResult(float finalScore)
    {
        gameObject.SetActive(true);

        scoreText.text = GetScoreText(finalScore);
        messageText.text = GetMessage(finalScore);
    }

    string GetScoreText(float score)
    {
        if (score >= 85) return "11111";
        if (score >= 70) return "11110";
        if (score >= 55) return "11100";
        if (score >= 40) return "11000";
        return "10000";
    }

    string GetMessage(float score)
    {
        if (score >= 85) return "Fokus kamu sangat baik!";
        if (score >= 70) return "Bagus, lanjutkan!";
        if (score >= 55) return "Kamu sudah mencoba";
        if (score >= 40) return "Pelan-pelan ya";
        return "Tidak apa-apa, coba lagi";
    }
}

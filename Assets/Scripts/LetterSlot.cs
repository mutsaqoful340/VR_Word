using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public int slotIndex;          // 0=A, 1=B, 2=C
    public string currentLetter = "";
    public Transform snapPoint;

    [Header("Indicator")]
    public GameObject dotIndicator; // 🔵 objek titik "..."

    public bool isFilled => !string.IsNullOrEmpty(currentLetter);

    void Start()
    {
        UpdateDotIndicator();
    }

    public bool PlaceLetter(string letter, GameObject letterObject)
    {
        if (isFilled)
            return false;

        WordPuzzleManager manager = FindObjectOfType<WordPuzzleManager>();
        if (manager == null)
            return false;

        string correct = manager.correctWord[slotIndex].ToString();

        if (letter != correct)
            return false;

        // ✅ BENAR
        currentLetter = letter;

        letterObject.transform.SetParent(snapPoint);
        letterObject.transform.localPosition = Vector3.zero;
        letterObject.transform.localRotation = Quaternion.identity;

        UpdateDotIndicator(); // 🔥 HILANGKAN TITIK

        return true;
    }

    public void ClearSlot()
    {
        currentLetter = "";
        UpdateDotIndicator(); // 🔥 MUNCULKAN TITIK LAGI
    }

    void UpdateDotIndicator()
    {
        if (dotIndicator)
            dotIndicator.SetActive(!isFilled);
    }
}

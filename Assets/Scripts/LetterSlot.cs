using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public int slotIndex;          // 0=A, 1=B, 2=C
    public string currentLetter = "";
    public Transform snapPoint;

    public bool isFilled => !string.IsNullOrEmpty(currentLetter);

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

        return true;
    }

    public void ClearSlot()
    {
        currentLetter = "";
    }
}

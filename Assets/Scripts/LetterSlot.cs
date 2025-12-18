using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public string currentLetter = "";
    public Transform snapPoint;

    public bool isFilled => !string.IsNullOrEmpty(currentLetter);

    public void PlaceLetter(string letter, GameObject letterObject)
    {
        currentLetter = letter;

        // snap
        letterObject.transform.SetParent(snapPoint);
        letterObject.transform.localPosition = Vector3.zero;
        letterObject.transform.localRotation = Quaternion.identity;

        FindObjectOfType<WordPuzzleManager>().CheckWord();
    }
}

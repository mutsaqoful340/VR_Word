using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public string expectedLetter;   // huruf yang BENAR untuk slot ini
    public Transform snapPoint;

    public string currentLetter = "";
    public GameObject currentObject;   // 🔥 WAJIB ADA & PUBLIC

    public bool isFilled => currentObject != null;

    public bool TryPlaceLetter(LetterObject letterObj)
    {
        // ❌ SALAH → tolak
        if (letterObj.letter != expectedLetter)
            return false;

        // ✅ BENAR → terima
        currentLetter = letterObj.letter;
        currentObject = letterObj.gameObject;

        letterObj.transform.SetParent(snapPoint);
        letterObj.transform.localPosition = Vector3.zero;
        letterObj.transform.localRotation = Quaternion.identity;

        letterObj.Lock();
        return true;
    }

    public void ClearSlot()
    {
        currentLetter = "";
        currentObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFilled) return;

        LetterObject letterObj = other.GetComponent<LetterObject>();
        if (letterObj == null) return;

        bool success = TryPlaceLetter(letterObj);

        if (success)
            FindObjectOfType<WordPuzzleManager>().CheckWord();
    }
}

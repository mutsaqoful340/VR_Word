using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public string currentLetter = "";
    public Transform snapPoint;

    public bool isFilled => !string.IsNullOrEmpty(currentLetter);

    private void OnTriggerEnter(Collider other)
    {
        if (isFilled) return;

        LetterObject letterObj = other.GetComponent<LetterObject>();
        if (letterObj == null) return;

        PlaceLetter(letterObj.letter, other.transform);
    }

    public void PlaceLetter(string letter, Transform letterTransform)
    {
        currentLetter = letter;

        letterTransform.position = snapPoint.position;
        letterTransform.rotation = snapPoint.rotation;

        Rigidbody rb = letterTransform.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // ⬇️ hanya memanggil manager
        FindObjectOfType<WordPuzzleManager>().CheckWord();
    }
}

using UnityEngine;

public class LetterObject : MonoBehaviour
{
    public string letter;

    private void OnTriggerEnter(Collider other)
    {
        LetterSlot slot = other.GetComponent<LetterSlot>();
        if (slot != null && !slot.isFilled)
        {
            slot.PlaceLetter(letter, transform);
        }
    }
}

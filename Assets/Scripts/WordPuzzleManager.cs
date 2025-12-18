using UnityEngine;

public class WordPuzzleManager : MonoBehaviour
{
    public string correctWord = "ABC";
    public LetterSlot[] slots;

    public GameObject keyPrefab;
    public Transform keySpawnPoint;
    public Animator doorAnimator;

    private bool solved = false;

    public void CheckWord()
    {
        if (solved)
        {
            Debug.Log("Puzzle sudah solved, diabaikan");
            return;
        }

        string word = "";
        bool allFilled = true;

        Debug.Log("=== CEK WORD PUZZLE ===");

        for (int i = 0; i < slots.Length; i++)
        {
            LetterSlot slot = slots[i];

            Debug.Log($"Slot {i} ({slot.name}) = '{slot.currentLetter}'");

            if (!slot.isFilled)
            {
                allFilled = false;
            }

            word += slot.currentLetter;
        }

        if (!allFilled)
        {
            Debug.Log("Belum semua slot terisi ❌");
            return;
        }

        Debug.Log("HASIL KATA: [" + word + "]");
        Debug.Log("KATA BENAR: [" + correctWord + "]");

        if (word == correctWord)
        {
            Debug.Log("PUZZLE BENAR ✅");

            solved = true;

            if (doorAnimator)
                doorAnimator.SetTrigger("Open");
            else
                Debug.LogWarning("Door Animator BELUM di-assign!");

            if (keyPrefab && keySpawnPoint)
                Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
            else
                Debug.LogWarning("Key prefab / spawn point belum di-set!");
        }
        else
        {
            Debug.Log("KATA SALAH ❌");
        }
    }
}

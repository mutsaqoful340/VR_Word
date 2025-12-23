using UnityEngine;

public class WordPuzzleManager : MonoBehaviour
{
    public string correctWord = "ABC";
    public LetterSlot[] slots;

    public GameObject keyPrefab;
    public Transform keySpawnPoint;

    private bool solved = false;

    // 🔑 SATU-SATUNYA FUNGSI CEK PUZZLE
    public bool CheckPuzzle()
    {
        if (solved)
            return true;

        string word = "";
        bool allFilled = true;

        Debug.Log("=== CEK WORD PUZZLE ===");

        for (int i = 0; i < slots.Length; i++)
        {
            LetterSlot slot = slots[i];

            Debug.Log($"Slot {i} ({slot.name}) = '{slot.currentLetter}'");

            if (!slot.isFilled)
                allFilled = false;

            word += slot.currentLetter;
        }

        if (!allFilled)
        {
            Debug.Log("Belum semua slot terisi ❌");
            return false;
        }

        Debug.Log($"HASIL KATA: [{word}]");
        Debug.Log($"KATA BENAR: [{correctWord}]");

        if (word == correctWord)
        {
            Debug.Log("PUZZLE BENAR ✅");
            solved = true;

            if (keyPrefab && keySpawnPoint)
                Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
            else
                Debug.LogWarning("Key prefab / spawn point belum di-set!");

            return true;
        }

        Debug.Log("KATA SALAH ❌");
        return false;
    }

    public void TrySolve()
    {
        if (solved) return;

        string word = "";

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].isFilled)
                return; // belum lengkap

            word += slots[i].currentLetter;
        }

        if (word == correctWord)
        {
            solved = true;
            Debug.Log("PUZZLE BENAR ✅");

            if (keyPrefab && keySpawnPoint)
                Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
        }
        else
        {
            Debug.Log("KATA SALAH ❌");
        }
    }
}

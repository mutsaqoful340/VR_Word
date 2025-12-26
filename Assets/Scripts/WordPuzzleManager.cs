using UnityEngine;

public class WordPuzzleManager : MonoBehaviour
{
    public string correctWord = "ABC";
    public LetterSlot[] slots;

    [Header("Puzzle Visual")]
    public GameObject puzzleRoot; // parent semua slot & huruf

    [Header("Key Reward")]
    public GameObject keyPrefab;
    public Transform keySpawnPoint;

    private bool solved = false;

    // 🔑 SATU-SATUNYA LOGIC CEK
    public bool CheckPuzzle()
    {
        if (solved)
            return true;

        string word = "";
        bool allFilled = true;

        Debug.Log("=== CheckPuzzle called ===");
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log($"Slot {i}: isFilled={slots[i].isFilled}, currentLetter='{slots[i].currentLetter}'");
            
            if (!slots[i].isFilled)
                allFilled = false;

            word += slots[i].currentLetter;
        }
        Debug.Log($"AllFilled: {allFilled}, Word formed so far: '{word}'");

        if (!allFilled)
            return false;

        Debug.Log($"All slots filled! Formed word: '{word}' | Correct word: '{correctWord}'");

        if (word == correctWord)
        {
            SolvePuzzle();
            return true;
        }

        return false;
    }

    // 🔁 COMPATIBILITY UNTUK SCRIPT LAMA
    public void TrySolve()
    {
        CheckPuzzle();
    }

    void SolvePuzzle()
    {
        if (solved) return;

        solved = true;
        Debug.Log("PUZZLE SELESAI ✅");

        // 🔥 Hilangkan puzzle
        if (puzzleRoot)
            puzzleRoot.SetActive(false);

        // 🔑 Spawn key di posisi puzzle
        if (keyPrefab && keySpawnPoint)
            Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
        else
            Debug.LogWarning("Key prefab / spawn point belum di-set!");
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldPuzzle : MonoBehaviour
{
    public static WorldPuzzle Instance { get; private set; }

    public string correctWord = "ABC";
    private List<LetterSlot> registeredSlots = new List<LetterSlot>();

    [Header("Puzzle Visual")]
    public GameObject puzzleRoot; // parent semua slot & huruf

    [Header("Key Reward")]
    public GameObject keyPrefab;
    public Transform keySpawnPoint;
    public LetterBox_Anim letterBoxAnim;

    private bool solved = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterSlot(LetterSlot slot)
    {
        if (!registeredSlots.Contains(slot))
        {
            registeredSlots.Add(slot);
            // Sort by index to maintain correct order
            registeredSlots = registeredSlots.OrderBy(s => s.slotIndex).ToList();
            Debug.Log($"LetterSlot {slot.slotIndex} registered. Total slots: {registeredSlots.Count}");
        }
    }

    public void UnregisterSlot(LetterSlot slot)
    {
        registeredSlots.Remove(slot);
    }

    // 🔑 SATU-SATUNYA LOGIC CEK
    public bool CheckPuzzle()
    {
        if (solved)
            return true;

        string word = "";
        bool allFilled = true;

        Debug.Log("=== CheckPuzzle called ===");
        Debug.Log($"Total registered slots: {registeredSlots.Count}");
        
        for (int i = 0; i < registeredSlots.Count; i++)
        {
            Debug.Log($"Slot {i}: isFilled={registeredSlots[i].isFilled}, currentLetter='{registeredSlots[i].currentLetter}'");
            
            if (!registeredSlots[i].isFilled)
                allFilled = false;

            word += registeredSlots[i].currentLetter;
        }
        Debug.Log($"AllFilled: {allFilled}, Word formed so far: '{word}'");

        if (!allFilled)
            return false;

        Debug.Log($"All slots filled! Formed word: '{word}' | Correct word: '{correctWord}'");

        if (word == correctWord)
        {
            Debug.Log("Word is correct! Waiting for arrow movement to solve puzzle...");
            return true;
        }

        return false;
    }

    // 🔁 COMPATIBILITY UNTUK SCRIPT LAMA
    public void TrySolve()
    {
        CheckPuzzle();
    }

    public void SolvePuzzle()
    {
        if (solved) return;

        solved = true;
        Debug.Log("PUZZLE SELESAI ✅");

        // Make LS and LB fall
        if (letterBoxAnim != null)
        {
            letterBoxAnim.OnPuzzleSolved();
        }
        else
        {
            Debug.LogWarning("LetterBox_Anim not assigned in Inspector!");
        }

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

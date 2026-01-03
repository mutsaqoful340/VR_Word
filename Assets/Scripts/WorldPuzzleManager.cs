using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPuzzleManager : MonoBehaviour
{
    public static WorldPuzzleManager Instance { get; private set; }

    [Header("Puzzle List")]
    public WorldPuzzle[] puzzles; // Assign your 4 WorldPuzzles here

    [Header("Transition Settings")]
    public float transitionDelay = 2f; // Wait time before activating next puzzle

    private int currentPuzzleIndex = 0;
    private bool isTransitioning = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        InitializePuzzles();
    }

    void InitializePuzzles()
    {
        // Deactivate all puzzles first
        for (int i = 0; i < puzzles.Length; i++)
        {
            if (puzzles[i] != null)
            {
                puzzles[i].gameObject.SetActive(false);
            }
        }

        // Activate only the first puzzle
        if (puzzles.Length > 0 && puzzles[0] != null)
        {
            puzzles[0].gameObject.SetActive(true);
            Debug.Log($"🎮 Puzzle 1/{puzzles.Length} activated: {puzzles[0].correctWord}");
        }
    }

    /// <summary>
    /// Called by WorldPuzzle when it's solved
    /// </summary>
    public void OnPuzzleSolved()
    {
        if (isTransitioning) return;

        Debug.Log($"✅ Puzzle {currentPuzzleIndex + 1} solved!");
        StartCoroutine(TransitionToNextPuzzle());
    }

    IEnumerator TransitionToNextPuzzle()
    {
        isTransitioning = true;

        // Wait for animations/despawn to finish
        yield return new WaitForSeconds(transitionDelay);

        // Deactivate current puzzle
        if (puzzles[currentPuzzleIndex] != null)
        {
            puzzles[currentPuzzleIndex].gameObject.SetActive(false);
            Debug.Log($"Puzzle {currentPuzzleIndex + 1} deactivated");
        }

        // Move to next
        currentPuzzleIndex++;

        // Check if more puzzles remain
        if (currentPuzzleIndex < puzzles.Length)
        {
            if (puzzles[currentPuzzleIndex] != null)
            {
                puzzles[currentPuzzleIndex].gameObject.SetActive(true);
                // OnEnable will automatically set this as the new Instance
                Debug.Log($"🎮 Puzzle {currentPuzzleIndex + 1}/{puzzles.Length} activated: {puzzles[currentPuzzleIndex].correctWord}");
            }
        }
        else
        {
            Debug.Log("🎉 ALL PUZZLES COMPLETED!");
            OnAllPuzzlesCompleted();
        }

        isTransitioning = false;
    }

    void OnAllPuzzlesCompleted()
    {
        // Add your victory logic here
        // e.g., show victory screen, unlock final door, etc.
    }

    /// <summary>
    /// Get current progress as string (for UI)
    /// </summary>
    public string GetProgress()
    {
        return $"{currentPuzzleIndex + 1}/{puzzles.Length}";
    }

    /// <summary>
    /// Get current puzzle index (0-based)
    /// </summary>
    public int GetCurrentIndex()
    {
        return currentPuzzleIndex;
    }
}


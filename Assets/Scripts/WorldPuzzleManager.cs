using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleStageEntry
{
    public DialogStage stage;
    public WorldPuzzle puzzle;
}

public class WorldPuzzleManager : MonoBehaviour
{
    public static WorldPuzzleManager Instance { get; private set; }

    [Header("Puzzle List")]
    public PuzzleStageEntry[] puzzleStages; // Assign puzzles with their stages

    [Header("Dialog Controller")]
    public NPCDialogController dialogController; // Reference to track stage changes

    [Header("Transition Settings")]
    public float transitionDelay = 2f; // Wait time before activating next puzzle

    private DialogStage lastCheckedStage = DialogStage.Stage1;
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

    void Update()
    {
        // Monitor stage changes from DialogController
        if (dialogController != null)
        {
            DialogStage currentStage = dialogController.currentStage;
            
            if (currentStage != lastCheckedStage)
            {
                Debug.Log($"📢 Stage changed from {lastCheckedStage} to {currentStage}");
                OnStageChanged(currentStage);
                lastCheckedStage = currentStage;
            }
        }
    }

    void InitializePuzzles()
    {
        // Deactivate all puzzles first
        foreach (var entry in puzzleStages)
        {
            if (entry.puzzle != null)
            {
                entry.puzzle.gameObject.SetActive(false);
            }
        }

        // Activate puzzle for the starting stage
        lastCheckedStage = DialogStage.Stage1;
        
        if (dialogController != null)
        {
            DialogStage startStage = dialogController.currentStage;
            lastCheckedStage = startStage;
            ActivatePuzzleForStage(startStage);
        }
        else
        {
            Debug.LogWarning("⚠️ No DialogController assigned! Please assign it in Inspector.");
            ActivatePuzzleForStage(DialogStage.Stage1);
        }
    }

    /// <summary>
    /// Called when DialogController stage changes
    /// </summary>
    void OnStageChanged(DialogStage newStage)
    {
        Debug.Log($"🔄 Handling stage change to {newStage}");
        
        // Deactivate all puzzles
        foreach (var entry in puzzleStages)
        {
            if (entry.puzzle != null && entry.puzzle.gameObject.activeSelf)
            {
                entry.puzzle.gameObject.SetActive(false);
                Debug.Log($"🔴 Deactivated: {entry.puzzle.name}");
            }
        }
        
        // Activate puzzle for new stage
        ActivatePuzzleForStage(newStage);
    }

    /// <summary>
    /// Activate puzzle for specific stage
    /// </summary>
    void ActivatePuzzleForStage(DialogStage stage)
    {
        WorldPuzzle puzzle = GetPuzzleForStage(stage);
        
        if (puzzle != null)
        {
            puzzle.gameObject.SetActive(true);
            Debug.Log($"✅ Puzzle activated for {stage}: {puzzle.name}");
        }
        else
        {
            Debug.Log($"ℹ️ No puzzle assigned to {stage}");
        }
    }

    /// <summary>
    /// Called by WorldPuzzle when it's solved (optional - for advancing stages)
    /// </summary>
    public void OnPuzzleSolved()
    {
        if (dialogController == null)
        {
            Debug.LogWarning("Cannot advance stage - no DialogController assigned!");
            return;
        }

        DialogStage currentStage = dialogController.currentStage;
        Debug.Log($"✅ Puzzle solved for stage {currentStage}!");
        
        // Optional: Auto-advance to next stage
        // DialogStage nextStage = GetNextStage(currentStage);
        // dialogController.SetStage(nextStage);
    }

    /// <summary>
    /// Get current progress as string (for UI)
    /// </summary>
    public string GetProgress()
    {
        if (dialogController != null)
        {
            return $"Stage {dialogController.currentStage}";
        }
        return $"Stage {lastCheckedStage}";
    }

    /// <summary>
    /// Get current stage
    /// </summary>
    public DialogStage GetCurrentStage()
    {
        if (dialogController != null)
        {
            return dialogController.currentStage;
        }
        return lastCheckedStage;
    }

    /// <summary>
    /// Find puzzle assigned to a specific stage
    /// </summary>
    private WorldPuzzle GetPuzzleForStage(DialogStage stage)
    {
        foreach (var entry in puzzleStages)
        {
            if (entry.stage == stage && entry.puzzle != null)
            {
                return entry.puzzle;
            }
        }
        return null;
    }
}


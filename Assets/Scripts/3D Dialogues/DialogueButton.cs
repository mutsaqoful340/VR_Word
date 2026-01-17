using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueSystem dialogueSystem;
    public NPCMoveByWaypoints npcMover;
    
    [Header("Dialogue Options")]
    [Tooltip("Use ScriptableObject for easy editing")]
    [SerializeField] private DialogueSequenceSO dialogueSequence;
    
    [Header("Setup")]
    [SerializeField] private SubtitleSystem subtitleSystem;
    
    [Header("Manager Integration")]
    [Tooltip("If false, this button won't notify DialogueButtonManager when played (useful for auto-play stage intros)")]
    [SerializeField] private bool notifyManager = true;

    private bool hasPlayed = false;

    [ContextMenu("Play Dialogue")]
    public void OnPlay_Dialogue()
    {
        Debug.Log($"DialogueButton: OnPlay_Dialogue called on {gameObject.name}");
        
        // Prevent playing if already played
        if (hasPlayed)
        {
            Debug.LogWarning("DialogueButton: Dialogue already played once, ignoring.");
            return;
        }

        if (dialogueSystem == null)
        {
            Debug.LogError("DialogueButton: DialogueSystem not assigned!");
            return;
        }

        // Failsafe: Don't start new dialogue if one is already playing
        if (dialogueSystem.IsPlaying)
        {
            Debug.LogWarning("DialogueButton: Dialogue already playing, ignoring button press.");
            return;
        }

        // Use ScriptableObject (recommended)
        if (dialogueSequence != null)
        {
            dialogueSystem.PlayDialogue(dialogueSequence);
            hasPlayed = true;
            Debug.Log("DialogueButton: Dialogue started, marking as played.");
            DisablseSelf();
        }
        else
        {
            Debug.LogError("DialogueButton: No dialogue configured! Assign a DialogueSequence or enable useManualDialogue with a SubtitleSystem.");
            return;
        }

        // Only notify manager if this button is meant to be tracked
        if (notifyManager)
        {
            Debug.Log("DialogueButton: Looking for DialogueButtonManager...");
            DialogueButtonManager dialogueButtonManager = FindObjectOfType<DialogueButtonManager>();
            if (dialogueButtonManager != null)
            {
                Debug.Log($"DialogueButton: Found manager! Notifying with instance: {this.gameObject.name}");
                dialogueButtonManager.isClicked(this);
            }
            else
            {
                Debug.LogError("DialogueButton: DialogueButtonManager not found in scene!");
            }
        }
        else
        {
            Debug.Log("DialogueButton: Notify Manager is disabled, skipping manager notification.");
        }
    }

    public void OnClose_Dialogue(GameObject dialoguePanel)
    {
        Debug.Log($"DialogueButton: OnClose_Dialogue called on {gameObject.name}");
        
        if (dialogueSystem == null)
        {
            Debug.LogWarning("DialogueButton: DialogueSystem not assigned, but closing panel anyway.");
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
                Debug.Log("DialogueButton: Panel closed.");
            }
            return;
        }
        
        if (dialogueSystem.IsPlaying)
        {
            Debug.LogWarning("DialogueButton: Dialogue is still playing, but closing panel anyway.");
        }

        // Close the panel
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
            Debug.Log("DialogueButton: Panel closed successfully.");
        }
        else
        {
            Debug.LogError("DialogueButton: dialoguePanel parameter is null!");
        }

        // Notify DialogueButtonManager that this button was clicked
        Debug.Log("DialogueButton: Looking for DialogueButtonManager...");
        DialogueButtonManager dialogueButtonManager = FindObjectOfType<DialogueButtonManager>();
        if (dialogueButtonManager != null)
        {
            Debug.Log($"DialogueButton: Found manager! Notifying with instance: {this.gameObject.name}");
            dialogueButtonManager.isClicked(this);
        }
        else
        {
            Debug.LogError("DialogueButton: DialogueButtonManager not found in scene!");
        }
    }

    public void DisablseSelf()
    {
        Debug.Log($"DialogueButton: Attempting to disable GameObject: {gameObject.name}");
        gameObject.SetActive(false);
        Debug.Log("DialogueButton: This line should never appear if SetActive worked!");
    }

    public void OnClick_1To2()
    {
        npcMover.MoveStage1ToStage2();
    }
    public void OnClick_2To3()
    {
        npcMover.MoveStage2ToStage3();
    }

    public void OnClick_3To4()
    {
        npcMover.MoveStage3ToStage4();
    }

    public void OnClick_4To5()
    {
        npcMover.MoveStage4ToStage5();
    }
}


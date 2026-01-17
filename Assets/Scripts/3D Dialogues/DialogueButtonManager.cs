using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueButtonManager : MonoBehaviour
{
    [Serializable]
    public class DialogueButtonEvent
    {
        public DialogueButton dialogueButton;
        public bool isClicked;
    }


    [Header("Dialogue Buttons")]
    public List<DialogueButtonEvent> dialogueButtons = new List<DialogueButtonEvent>();
    
    [Header("Puzzle Prerequisite (Optional)")]
    [Tooltip("If assigned, puzzle must be solved before AllButtonsClicked event fires")]
    public WorldPuzzle requiredPuzzle;
    
    [Header("Dialogue System (Optional)")]
    [Tooltip("If assigned, will wait for dialogue to finish before invoking AllButtonsClicked")]
    public DialogueSystem dialogueSystem;
    
    public UnityEvent AllButtonsClicked;

    public UnityEvent DialogueOnStageStart;

    private bool hasInvokedEvent = false;
    private bool hasInvokedStageStart = false;

    void OnEnable()
    {
        Debug.Log($"⚠️ DialogueButtonManager.OnEnable() called on '{gameObject.name}' - Active: {gameObject.activeInHierarchy}");
        
        // Use coroutine to check if we're still active after one frame (avoids initialization flicker)
        StartCoroutine(CheckAndInvokeStageStart());
    }
    
    private IEnumerator CheckAndInvokeStageStart()
    {
        // Wait one frame to let initialization complete
        yield return null;
        
        // Only invoke if we're still active and haven't invoked yet
        if (gameObject.activeInHierarchy && !hasInvokedStageStart)
        {
            hasInvokedStageStart = true;
            Debug.Log($"✅ DialogueButtonManager: Stage REALLY started on '{gameObject.name}', invoking DialogueOnStageStart");
            DialogueOnStageStart?.Invoke();
        }
        else
        {
            Debug.Log($"⏭️ DialogueButtonManager: Skipping DialogueOnStageStart on '{gameObject.name}' (inactive or already invoked)");
        }
    }
    
    void OnDisable()
    {
        Debug.Log($"❌ DialogueButtonManager.OnDisable() called on '{gameObject.name}'");
        // Reset flag when disabled so it can play again if re-enabled
        hasInvokedStageStart = false;
    }

    // void Start()
    // {
    //     if (gameObject.activeInHierarchy)
    //     {
    //         DialogueOnStageStart?.Invoke();
    //     }
    // }

    public void isClicked(DialogueButton button)
    {
        Debug.Log($"DialogueButtonManager: isClicked called with button: {button.gameObject.name} [ID: {button.GetInstanceID()}]");
        Debug.Log($"DialogueButtonManager: List has {dialogueButtons.Count} buttons");
        
        // Find the button in the list and mark it as clicked
        bool found = false;
        foreach (var dialogueButtonEvent in dialogueButtons)
        {
            string listButtonName = dialogueButtonEvent.dialogueButton != null ? dialogueButtonEvent.dialogueButton.gameObject.name : "NULL";
            int listButtonID = dialogueButtonEvent.dialogueButton != null ? dialogueButtonEvent.dialogueButton.GetInstanceID() : 0;
            
            Debug.Log($"Checking button in list: {listButtonName} [ID: {listButtonID}] == Clicked: {button.gameObject.name} [ID: {button.GetInstanceID()}]");
            
            if (dialogueButtonEvent.dialogueButton == button)
            {
                dialogueButtonEvent.isClicked = true;
                Debug.Log($"DialogueButtonManager: ✓ MATCHED! Button {button.gameObject.name} marked as clicked.");
                found = true;
                break;
            }
        }
        
        if (!found)
        {
            Debug.LogError($"DialogueButtonManager: Button {button.gameObject.name} [ID: {button.GetInstanceID()}] NOT FOUND in list! The button you clicked is a DIFFERENT instance than the ones in the list.");
        }

        // Check if all buttons have been clicked
        CheckAllButtonsClicked();
    }

    private void CheckAllButtonsClicked()
    {
        bool allClicked = true;

        foreach (var dialogueButtonEvent in dialogueButtons)
        {
            if (!dialogueButtonEvent.isClicked)
            {
                allClicked = false;
                break;
            }
        }

        if (allClicked && dialogueButtons.Count > 0 && !hasInvokedEvent)
        {
            Debug.Log("DialogueButtonManager: All buttons clicked!");
            
            // Check if puzzle prerequisite is met
            if (requiredPuzzle != null)
            {
                // Use reflection to access private 'solved' field
                var solvedField = requiredPuzzle.GetType().GetField("solved", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                bool isSolved = solvedField != null && (bool)solvedField.GetValue(requiredPuzzle);
                
                if (!isSolved)
                {
                    Debug.LogWarning("DialogueButtonManager: Required puzzle is NOT solved yet. Cannot invoke AllButtonsClicked.");
                    return;
                }
                
                Debug.Log("DialogueButtonManager: Required puzzle is SOLVED ✓");
            }
            else
            {
                Debug.Log("DialogueButtonManager: No puzzle prerequisite required.");
            }
            
            // Start coroutine to wait for dialogue if needed
            StartCoroutine(WaitForDialogueAndInvoke());
        }
    }

    private IEnumerator WaitForDialogueAndInvoke()
    {
        // If DialogueSystem is assigned and dialogue is playing, wait for it to finish
        if (dialogueSystem != null && dialogueSystem.IsPlaying)
        {
            Debug.Log("DialogueButtonManager: Dialogue is still playing, waiting for it to finish...");
            
            while (dialogueSystem.IsPlaying)
            {
                yield return null; // Wait one frame
            }
            
            Debug.Log("DialogueButtonManager: Dialogue finished!");
        }
        else
        {
            Debug.Log("DialogueButtonManager: No dialogue playing or DialogueSystem not assigned.");
        }
        
        // Now invoke the event
        hasInvokedEvent = true;
        Debug.Log("DialogueButtonManager: Invoking AllButtonsClicked event.");
        AllButtonsClicked?.Invoke();
    }

}

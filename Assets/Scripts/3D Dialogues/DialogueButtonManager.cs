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
    
    public UnityEvent AllButtonsClicked;

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

        if (allClicked && dialogueButtons.Count > 0)
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
            
            Debug.Log("DialogueButtonManager: Invoking AllButtonsClicked event.");
            AllButtonsClicked?.Invoke();
        }
    }

}

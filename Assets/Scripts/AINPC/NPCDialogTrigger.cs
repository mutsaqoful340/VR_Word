using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NPCDialogTrigger : MonoBehaviour
{
    [Serializable]
    public class DialogUIStage
    {
        public GameObject dialogPanel;
        public DialogStage stage;
    }

    [Header("References")]
    public NPCDialogController dialogController;
    public NPCMoveByWaypoints npcMover;
    
    [Header("Dialog Stages")]
    public List<DialogUIStage> dialogUIList = new List<DialogUIStage>();

    [Header("Trigger Settings")]
    public string targetTag = "PlayerHand";

    void Start()
    {
        // Disable all dialog panels at start
        foreach (var dialogUI in dialogUIList)
        {
            if (dialogUI.dialogPanel != null)
            {
                dialogUI.dialogPanel.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Automatically deactivate panels if NPC starts moving
        if (npcMover.isMoving)
        {
            DeactivateAllPanels();
        }
        
        // Debug: Check if this GameObject is active
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("NPCDialogTrigger: This GameObject is disabled!");
        }
    }

    public void PanelActivate(DialogStage stage)
    {
        foreach (var dialogUI in dialogUIList)
        {
            if (dialogUI.dialogPanel != null)
            {
                dialogUI.dialogPanel.SetActive(dialogUI.stage == stage);
            }
        }
    }

    // Single button method - activates panel based on current stage
    public void OnClick_ActivateCurrentStagePanel()
    {
        Debug.Log("NPCDialogTrigger: OnClick_ActivateCurrentStagePanel called");
        
        if (dialogController == null)
        {
            Debug.LogError("NPCDialogTrigger: dialogController is null!");
            return;
        }
        
        DialogStage currentStage = dialogController.currentStage;
        Debug.Log($"NPCDialogTrigger: Current stage is {currentStage}, activating panel...");
        PanelActivate(currentStage);
    }

    public void DeactivateAllPanels()
    {
        foreach (var dialogUI in dialogUIList)
        {
            if (dialogUI.dialogPanel != null)
            {
                dialogUI.dialogPanel.SetActive(false);
            }
        }
    }
}

// ========== OLD CODE ==========

// using UnityEngine;

// public class NPCDialogTrigger : MonoBehaviour
// {
//     public NPCDialogController dialogController;
//     public void OnQuestionButtonPressed()
//     {
//         if (dialogController != null)
//         {
//             Debug.Log("Tombol ? ditekan - buka dialog NPC");
//             dialogController.OpenDialog();
//         }
//     }
// }
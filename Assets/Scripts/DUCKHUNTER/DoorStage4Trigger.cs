using UnityEngine;

public class DoorStage4Trigger : MonoBehaviour
{
    [Header("References")]
    public NPCMoveByWaypoints npcMover;
    public NPCDialogController dialogController;

    private bool hasTriggered = false;

    // 🔹 DIPANGGIL SAAT PINTU STAGE 4 TERBUKA
    public void OnDoorOpened()
    {
        Debug.Log("OnDoorOpened TERPANGGIL");
        if (hasTriggered)
            return;

        if (npcMover == null || dialogController == null)
            return;

        // 🔒 Pastikan NPC sedang di Stage 3
        if (dialogController.currentStage != DialogStage.Stage3)
            return;

        hasTriggered = true;

        npcMover.MoveStage3ToStage4();

        Debug.Log("Door Stage 4 terbuka → NPC jalan ke posisi Stage 4");
    }
}

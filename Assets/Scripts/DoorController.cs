using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;
    public NPCMoveByWaypoints npcMover;

    // 🔹 TAMBAHAN (AMAN)
    public DoorStage4Trigger doorStage4Trigger;

    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;

        if (other.CompareTag("Key"))
        {
            doorAnimator.SetTrigger("Open");
            isOpen = true;

            Debug.Log("Pintu terbuka pakai kunci");

            // SISTEM LAMA (TETAP)
            if (npcMover != null)
                npcMover.MoveStage1ToStage2();

            // 🔥 SISTEM BARU (OPTIONAL)
            if (doorStage4Trigger != null)
                doorStage4Trigger.OnDoorOpened();

            Destroy(other.gameObject);
        }
    }
}

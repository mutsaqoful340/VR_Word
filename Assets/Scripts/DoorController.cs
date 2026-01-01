using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;
    public NPCMoveByWaypoints npcMover;

    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;

        if (other.CompareTag("Key"))
        {
            doorAnimator.SetTrigger("Open");
            isOpen = true;

            Debug.Log("Pintu terbuka pakai kunci");

            // 🔥 NPC mulai jalan lewat waypoint
            if (npcMover != null)
                npcMover.StartMoving();

            Destroy(other.gameObject);
        }
    }
}

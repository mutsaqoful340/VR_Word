using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public NPCGuideToDoor npcGuide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcGuide.StopGuiding();
            Debug.Log("Player sampai di pintu, mulai puzzle kata");

            // TODO: aktifkan puzzle nyusun kata
        }
    }
}

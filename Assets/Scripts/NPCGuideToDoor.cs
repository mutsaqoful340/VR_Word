using UnityEngine;

public class NPCGuideToDoor : MonoBehaviour
{
    public Transform npc;
    public Transform door;

    public float rotateSpeed = 3f;
    public float activateDistance = 5f;

    private bool guiding = false;

    void Update()
    {
        if (!guiding) return;

        // NPC menghadap pintu
        Vector3 dir = door.position - npc.position;
        dir.y = 0;

        Quaternion lookRot = Quaternion.LookRotation(dir);
        npc.rotation = Quaternion.Slerp(npc.rotation, lookRot, Time.deltaTime * rotateSpeed);
    }

    public void StartGuiding()
    {
        guiding = true;
    }

    public void StopGuiding()
    {
        guiding = false;
    }
}

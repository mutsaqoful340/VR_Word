using UnityEngine;

public class NPCMoveByWaypoints : MonoBehaviour
{
    [Header("Waypoint Sets")]
    public Transform[] stage1ToStage2;
    public Transform[] stage2ToStage3;

    [Header("Movement")]
    public float speed = 1.5f;
    public float rotateSpeed = 6f;
    public Animator animator;

    [Header("Dialog Ref")]
    public NPCDialogController dialogController;

    Transform[] currentWaypoints;
    int currentIndex;
    bool isMoving;

    void Update()
    {
        if (!isMoving || currentWaypoints == null || currentWaypoints.Length == 0)
            return;

        Transform target = currentWaypoints[currentIndex];
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        // ✔ Sampai waypoint
        if (dir.magnitude < 0.1f)
        {
            currentIndex++;

            if (currentIndex >= currentWaypoints.Length)
            {
                StopMoving();
            }
            return;
        }

        // ✔ Rotasi halus
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                rotateSpeed * 100f * Time.deltaTime
            );
        }

        // ✔ Jalan maju
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // ================== MOVE API ==================

    // 🔹 DIPANGGIL SAAT STAGE 1 SELESAI
    public void MoveStage1ToStage2()
    {
        if (dialogController.currentStage != DialogStage.Stage1)
            return;

        currentWaypoints = stage1ToStage2;
        StartMove();
    }

    // 🔹 DIPANGGIL SAAT STAGE 2 SELESAI
    public void MoveStage2ToStage3()
    {
        if (dialogController.currentStage != DialogStage.Stage2)
            return;

        currentWaypoints = stage2ToStage3;
        StartMove();
    }

    void StartMove()
    {
        currentIndex = 0;
        isMoving = true;

        if (animator != null)
            animator.SetBool("isWalking", true);
    }

    void StopMoving()
    {
        isMoving = false;

        if (animator != null)
            animator.SetBool("isWalking", false);

        AdvanceStage();
    }

    // ================== STAGE CONTROL ==================

    void AdvanceStage()
    {
        if (dialogController == null)
            return;

        if (dialogController.currentStage == DialogStage.Stage1)
        {
            dialogController.SetStage(DialogStage.Stage2);
        }
        else if (dialogController.currentStage == DialogStage.Stage2)
        {
            dialogController.SetStage(DialogStage.Stage3);
        }

        Debug.Log("NPC berhenti & lanjut ke stage berikutnya");
    }
}
